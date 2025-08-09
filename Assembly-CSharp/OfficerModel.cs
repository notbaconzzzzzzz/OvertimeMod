/*
public override DefenseInfo defense // 
-public override void RecoverMental(float amount) // 
+public override float RecoverMentalv2(float amount) // 
public override void InitialEncounteredCreature(UnitModel encountered) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using WorkerSprite;

// Token: 0x02000788 RID: 1928
[Serializable]
public class OfficerModel : WorkerModel
{
	// Token: 0x06003B7E RID: 15230 RVA: 0x00176280 File Offset: 0x00174480
	public OfficerModel(long id, string area)
	{
		this.lookingDir = LOOKINGDIR.NOCARE;
		this._panicImmuneTimer = new Timer();
		this.screamMax = 5f;
		this.workerClass = WorkerClass.OFFICER;
		this.commandQueue = new WorkerCommandQueue(this);
		this.instanceId = id;
		base.currentSefira = area;
		this.movableNode = new MovableObjectNode(this);
		this.movableNode.SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(area));
		this.recoveryRate = 1;
		this.elapsedTime = 0f;
		this.OnWorkEndFlag = true;
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.Worker);
		this.spriteData = new WorkerSprite.WorkerSprite();
		base.SetWeapon(WeaponModel.GetOfficerWeapon());
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06003B7F RID: 15231 RVA: 0x00034A50 File Offset: 0x00032C50
	// (set) Token: 0x06003B80 RID: 15232 RVA: 0x00034A58 File Offset: 0x00032C58
	public int deptNum { get; set; }

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06003B81 RID: 15233 RVA: 0x00034A61 File Offset: 0x00032C61
	// (set) Token: 0x06003B82 RID: 15234 RVA: 0x00034A69 File Offset: 0x00032C69
	public OfficerAIState state
	{
		get
		{
			return this._state;
		}
		set
		{
			this._state = value;
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06003B83 RID: 15235 RVA: 0x00176338 File Offset: 0x00174538
	public override DefenseInfo defense
	{ // <Mod>
		get
		{
			DefenseInfo zero = DefenseInfo.zero;
			if (!ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_defense"))
			{
				zero.B = 1.5f;
			}
			zero.P = 2f;
			return zero;
		}
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x00034A72 File Offset: 0x00032C72
	public override void OnStageEnd()
	{
		if (this.unconAction != null)
		{
			this.unconAction.OnStageEnd();
		}
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x00176364 File Offset: 0x00174564
	public override void OnStageRelease()
	{
		foreach (UnitBuf unitBuf in this._bufList.ToArray())
		{
			unitBuf.OnStageRelease();
		}
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0002F756 File Offset: 0x0002D956
	public override string GetUnitName()
	{
		return this.name;
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x00034A8A File Offset: 0x00032C8A
	public override void ChangeHairSprite(SpriteInfo spriteInfo)
	{
		this.hairSprite = spriteInfo.sprite;
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x0017639C File Offset: 0x0017459C
	public override void OnFixedUpdate()
	{
		if (this.IsDead())
		{
			return;
		}
		this._panicImmuneTimer.RunTimer();
		this.UpdateBufState();
		this.elapsedTime += Time.deltaTime;
		this.recoverElapsed += Time.deltaTime;
		if (this.remainMoveDelay > 0f)
		{
			this.remainMoveDelay -= Time.deltaTime;
		}
		if (this.remainAttackDelay > 0f)
		{
			this.remainAttackDelay -= Time.deltaTime;
		}
		if (this.stunTime > 0f)
		{
			this.stunTime -= Time.deltaTime;
			return;
		}
		if (this.haltUpdate)
		{
			return;
		}
		if (this.isDebugger)
		{
			if (Input.GetKey(KeyCode.J))
			{
				this.GetMovableNode().MoveBy(UnitDirection.LEFT, 0.2f);
			}
			else if (Input.GetKey(KeyCode.L))
			{
				this.GetMovableNode().MoveBy(UnitDirection.RIGHT, 0.2f);
			}
		}
		else
		{
			this.ProcessAction();
		}
		if (this.remainMoveDelay > 0f)
		{
			this.movableNode.ProcessMoveNode(0f);
		}
		else if (this.state == OfficerAIState.DOCUMENT)
		{
			this.movableNode.ProcessMoveNode((this.baseMovement + this.movement / 25f) / 2f * this.movementMul * base.GetMovementScaleByBuf());
		}
		else
		{
			this.movableNode.ProcessMoveNode((this.baseMovement + this.movement / 25f) * this.movementMul * base.GetMovementScaleByBuf());
		}
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x00176548 File Offset: 0x00174748
	public IEnumerator<WaitForSeconds> StartAction()
	{
		yield return new WaitForSeconds(5f);
		this.state = OfficerAIState.IDLE;
		yield break;
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x00034A98 File Offset: 0x00032C98
	public override void ProcessAction()
	{
		this.commandQueue.Execute(this);
		this.ProcessActionNormalOfficer();
		this.waitTimer -= Time.deltaTime;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x00176564 File Offset: 0x00174764
	private void ProcessActionNormalOfficer()
	{
		if (!this.OnWorkEndFlag)
		{
			this.SpecialActionUpdate();
			return;
		}
		if (this._readyToSuicide && !this.CannotControll())
		{
			this.Suicide();
			return;
		}
		if (base.CurrentPanicAction != null)
		{
			this.screamElapsed += Time.deltaTime;
			if (this.screamElapsed >= this.screamMax)
			{
				if (UnityEngine.Random.Range(0, 3) == 0)
				{
					int num = UnityEngine.Random.Range(1, 13);
					this._unit.PlaySound("Agent/Scream/" + num, null, false);
				}
				this.screamElapsed = 0f;
			}
			base.CurrentPanicAction.Execute();
		}
		else if (this.unconAction != null)
		{
			this.unconAction.Execute();
		}
		else
		{
			PassageObjectModel passage = this.movableNode.GetPassage();
			if (passage != null)
			{
				UnitModel unitModel = null;
				foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
				{
					UnitModel unit = movableObjectNode.GetUnit();
					if (unit.IsAttackTargetable() && this.IsHostile(unit))
					{
						unitModel = unit;
						break;
					}
				}
				if (unitModel != null)
				{
					if (!(unitModel is CreatureModel) || (unitModel as CreatureModel).script.IsAutoSuppressable())
					{
						WorkerCommand currentCommand = base.GetCurrentCommand();
						if (!(currentCommand is AttackOfficerCommand) || ((AttackOfficerCommand)currentCommand).GetTarget() != unitModel)
						{
							base.SetAgentCommand(new AttackOfficerCommand(unitModel));
						}
					}
					if (!this._panicImmuneTimer.started && this.mental <= 0f)
					{
						this.Panic();
					}
					return;
				}
			}
			if (this.state == OfficerAIState.IDLE && this.waitTimer <= 0f)
			{
				switch (UnityEngine.Random.Range(0, 5))
				{
				case 0:
					this.state = OfficerAIState.IDLE;
					this.waitTimer = 1.5f + 5f * UnityEngine.Random.value;
					break;
				case 1:
					this.state = OfficerAIState.MEMO_MOVE;
					this.target = SefiraManager.instance.GetSefira(base.currentSefira).GetIdleCreature();
					if (this.target == null)
					{
						this.waitTimer = 1.5f + 5f * UnityEngine.Random.value;
						this.state = OfficerAIState.IDLE;
					}
					else
					{
						base.MoveToNode(this.target.GetEntryNode());
						this.isMoving = true;
						this.waitTimer = 90f;
					}
					break;
				case 2:
				case 4:
					this.state = OfficerAIState.WANDER;
					this.waitTimer = 12f + 5f * UnityEngine.Random.value;
					this.isMoving = true;
					base.MoveToNode(SefiraManager.instance.GetSefira(base.currentSefira).GetRandomWayPoint());
					break;
				case 3:
					this.state = OfficerAIState.DOCUMENT;
					this.waitTimer = 90f;
					base.MoveToNode(SefiraManager.instance.GetSefira(base.currentSefira).GetOtherDepartNode(this.deptNum));
					this.isMoving = true;
					break;
				}
			}
			else if (this.state == OfficerAIState.DOCUMENT)
			{
				if (!this.movableNode.IsMoving() && this.isMoving)
				{
					this.isMoving = false;
					this.waitTimer = 5f;
				}
				else if (!this.isMoving && this.waitTimer <= 0f)
				{
					this.ReturnToSefiraFromWork();
					this.state = OfficerAIState.RETURN;
					this.isMoving = true;
				}
			}
			else if (this.state == OfficerAIState.MEMO_MOVE || this.state == OfficerAIState.MEMO_STAY)
			{
				if (this.state == OfficerAIState.MEMO_MOVE)
				{
					if (!this.movableNode.IsMoving() && this.isMoving && this.movableNode.GetCurrentNode() == this.target.GetEntryNode())
					{
						this.isMoving = false;
						this.waitTimer = 10f;
						if (this._unit.memoObject != null)
						{
							this._unit.memoObject.SetActive(true);
						}
						this.state = OfficerAIState.MEMO_STAY;
					}
				}
				else if (!this.isMoving && this.waitTimer <= 0f)
				{
					this.ReturnToSefiraFromWork();
					if (this._unit.memoObject != null)
					{
						this._unit.memoObject.SetActive(false);
					}
					SefiraManager.instance.GetSefira(base.currentSefira).EndCreatureWork(this.target);
					this.target = null;
					this.state = OfficerAIState.RETURN;
					this.isMoving = true;
				}
			}
			else if (this.state == OfficerAIState.RETURN)
			{
				if (!this.movableNode.IsMoving() && this.isMoving)
				{
					this.isMoving = false;
					this.state = OfficerAIState.IDLE;
					this.waitTimer = 1.5f + 5f * UnityEngine.Random.value;
				}
			}
			else if (this.state != OfficerAIState.SPECIALACTION)
			{
				if (this.state == OfficerAIState.WANDER && !this.movableNode.IsMoving() && this.isMoving)
				{
					this.ReturnToSefiraFromWork();
					this.state = OfficerAIState.RETURN;
				}
			}
		}
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x00034ABE File Offset: 0x00032CBE
	protected override void OnSetWeapon()
	{
		base.OnSetWeapon();
		if (this._unit != null)
		{
			this._unit.OnChangeWeapon();
		}
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x00034AE2 File Offset: 0x00032CE2
	public override void PrepareWeapon()
	{
		base.PrepareWeapon();
		WeaponSetter.SetWeaponAnimParam(this);
		this._unit.PrepareWeapon();
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x00034AFB File Offset: 0x00032CFB
	public override void CancelWeapon()
	{
		base.CancelWeapon();
		this._unit.CancelWeapon();
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x00034B0E File Offset: 0x00032D0E
	public void SpecialAction(OfficerSpecialAction action)
	{
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeOfficerSpecialAction(action));
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x00176AD4 File Offset: 0x00174CD4
	public void SpecialActionUpdate()
	{
		if (this.currentSpecialAction == null)
		{
			this.OnWorkEndFlag = true;
			return;
		}
		if (this._unit.CheckMannualMovingEnd())
		{
			AnimatorManager.instance.ResetAnimatorTransform(this.instanceId);
			if (!this.startSpecialAction)
			{
				this._unit.isMovingByMannually = false;
				Sefira sefira = SefiraManager.instance.GetSefira(base.currentSefira);
				this.startSpecialAction = true;
			}
			else
			{
				this.EndSpecialAction();
				this._unit.isMovingByMannually = false;
				this.startSpecialAction = false;
				if (this.shouldPanic)
				{
					this.Panic();
				}
			}
		}
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x00176B74 File Offset: 0x00174D74
	public void ReturnSpecialAction()
	{
		if (this.currentSpecialAction == null)
		{
			return;
		}
		Vector3 position = this.currentSpecialAction.GetNode().GetPosition();
		position.z = this._unit.zValue;
		this._unit.MannualMovingCall(position, false, true, true, true, false, 0.01f);
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x00176BC8 File Offset: 0x00174DC8
	public void HaltSpecialAction()
	{
		if (this.currentSpecialAction == null)
		{
			return;
		}
		if (this.isMoving || !this.startSpecialAction)
		{
			this.currentSpecialAction = null;
			return;
		}
		AnimatorManager.instance.ResetAnimatorTransform(this.instanceId);
		Vector3 position = this.currentSpecialAction.GetNode().GetPosition();
		position.z = this._unit.zValue;
		this.OnWorkEndFlag = false;
		if (!this.isMoving)
		{
			this._unit.MannualMovingCall(position, false, true, true, true, false, 0f);
		}
		this.OnWorkEndFlag = true;
		this._unit.ReleaseUpdatePosition();
		this.waitTimer = 15f;
		this.lookingDir = LOOKINGDIR.NOCARE;
		SefiraManager.instance.GetSefira(base.currentSefira).ResetSpecaialAction(this.currentSpecialAction);
		this._unit.isMovingByMannually = false;
		this.startSpecialAction = false;
		this.currentSpecialAction = null;
		if (this.shouldPanic)
		{
			this.Panic();
		}
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x00176CC8 File Offset: 0x00174EC8
	public void EndSpecialAction()
	{
		this.OnWorkEndFlag = true;
		this._unit.ReleaseUpdatePosition();
		this.waitTimer = 15f;
		this.lookingDir = LOOKINGDIR.NOCARE;
		this.ReturnToSefiraFromWork();
		this.state = OfficerAIState.RETURN;
		this.isMoving = true;
		SefiraManager.instance.GetSefira(base.currentSefira).ResetSpecaialAction(this.currentSpecialAction);
		this.currentSpecialAction = null;
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x00176B74 File Offset: 0x00174D74
	public void SpecialActionReturn()
	{
		if (this.currentSpecialAction == null)
		{
			return;
		}
		Vector3 position = this.currentSpecialAction.GetNode().GetPosition();
		position.z = this._unit.zValue;
		this._unit.MannualMovingCall(position, false, true, true, true, false, 0.01f);
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0002F3CC File Offset: 0x0002D5CC
	public override void PursueUnconAgent(UnitModel target)
	{
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeUnconPursueAgent(target));
	}

	// Token: 0x06003B96 RID: 15254 RVA: 0x00034B21 File Offset: 0x00032D21
	public void ReturnToSefiraFromWork()
	{
		this.ReturnToSefira();
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x00034B29 File Offset: 0x00032D29
	public OfficerAIState GetState()
	{
		return this.state;
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x00176D30 File Offset: 0x00174F30
	public override void StopAction()
	{
		if (this.state != OfficerAIState.CANNOT_CONTROLL && this.state != OfficerAIState.PANIC && base.CurrentPanicAction == null)
		{
			this.state = OfficerAIState.IDLE;
		}
		this.commandQueue.Clear();
		if (this.isMoving)
		{
			this.isMoving = false;
		}
		if ((this.state == OfficerAIState.SPECIALACTION || this.currentSpecialAction != null) && this.currentSpecialAction != null)
		{
			this.HaltSpecialAction();
			return;
		}
		if (this.lookingDir != LOOKINGDIR.NOCARE)
		{
			this.lookingDir = LOOKINGDIR.NOCARE;
		}
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x00176DC4 File Offset: 0x00174FC4
	public override void ShowCreatureActionSpeech(long creatureID, string key)
	{
		AgentLyrics.CreatureReactionList creatureReaction = AgentLyrics.instance.GetCreatureReaction(creatureID);
		if (creatureReaction == null)
		{
			return;
		}
		this._unit.showSpeech.ShowCreatureActionLyric(creatureReaction.action, key);
	}

	// <Mod>
	public override float RecoverMentalv2(float amount, bool canBeModified = true)
	{
		if (this.IsDead())
		{
			return 0f;
		}
		float num = base.RecoverMentalv2(amount, canBeModified);
		if (this.mental >= (float)this.mentalReturn && this.IsPanic())
		{
			this.StopPanic();
		}
		return num;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x00176DFC File Offset: 0x00174FFC
	public override void ReturnToSefira()
	{
		string id = SefiraManager.instance.GetSefira(base.currentSefira).GetDepartNodeByRandom(this.deptNum).GetId();
		base.MoveToNode(id);
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x00034B69 File Offset: 0x00032D69
	public override void EncounterCreature(UnitModel encounteredCreature)
	{
		if (!this.IsPanic() && this.shouldPanic)
		{
			return;
		}
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x00034B82 File Offset: 0x00032D82
	public override void EncounterStandingItem(StandingItemModel standing)
	{
		if (!this.IsPanic())
		{
			if (this.shouldPanic)
			{
				return;
			}
			base.TakeDamage(new DamageInfo(RwbpType.W, (float)this.maxMental));
		}
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x00176E34 File Offset: 0x00175034
	public override void LoseControl()
	{
		if (this.state == OfficerAIState.MEMO_MOVE || this.state == OfficerAIState.MEMO_STAY)
		{
			SefiraManager.instance.GetSefira(base.currentSefira).EndCreatureWork(this.target);
		}
		this.state = OfficerAIState.CANNOT_CONTROLL;
		this.commandQueue.Clear();
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x00034BAE File Offset: 0x00032DAE
	public override void GetControl()
	{
		if (this.state == OfficerAIState.CANNOT_CONTROLL)
		{
			this.state = OfficerAIState.IDLE;
			this.commandQueue.Clear();
			if (this.unconAction != null)
			{
				this.unconAction.OnDestroy();
				this.unconAction = null;
			}
		}
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x00034BEC File Offset: 0x00032DEC
	public override bool CannotControll()
	{
		return this.state == OfficerAIState.CANNOT_CONTROLL;
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x00034BF8 File Offset: 0x00032DF8
	public void OnMemoEnd()
	{
		this.waitTimer = 0f;
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x00034C05 File Offset: 0x00032E05
	public override void ClearUnconCommand()
	{
		if (this.state == OfficerAIState.CANNOT_CONTROLL)
		{
			this.commandQueue.Clear();
		}
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x00034C1F File Offset: 0x00032E1F
	public override void SetUncontrollableAction(UncontrollableAction uncon)
	{
		if (this.unconAction != null)
		{
			this.unconAction.OnDestroy();
		}
		this.unconAction = uncon;
		if (this.unconAction != null)
		{
			this.unconAction.Init();
		}
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x00176E88 File Offset: 0x00175088
	public void PanicOfficer(bool force)
	{
		if (this.IsDead())
		{
			return;
		}
		if (this.IsPanic() && !this.shouldPanic)
		{
			return;
		}
		if (this.state == OfficerAIState.CANNOT_CONTROLL)
		{
			return;
		}
		if (this.invincible)
		{
			return;
		}
		if (this.state == OfficerAIState.SPECIALACTION && this.currentSpecialAction != null && !this.isMoving && !this.shouldPanic)
		{
			this.shouldPanic = true;
			this.HaltSpecialAction();
			return;
		}
		if (this.returnPanic)
		{
			return;
		}
		if (this.mentalReturn == 0)
		{
			this.mentalReturn = (int)((float)this.maxMental * 0.8f);
		}
		this.state = OfficerAIState.PANIC;
		this.shouldPanic = false;
		this.ResetAnimator();
		base.CurrentPanicAction = new PanicReady(this);
		Notice.instance.Send(NoticeName.OnOfficerPanic, new object[]
		{
			this
		});
		foreach (UnitBuf unitBuf in this._bufList.ToArray())
		{
			unitBuf.OnUnitPanic();
		}
		this.SetFaction(FactionTypeList.StandardFaction.PanicWorker);
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x00034C54 File Offset: 0x00032E54
	public override void Panic()
	{
		this.PanicOfficer(false);
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x00034C5D File Offset: 0x00032E5D
	private void Suicide()
	{
		this.LoseControl();
		this.SetUncontrollableAction(new Uncontrollable_OfficerSuicide(this));
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x00034C71 File Offset: 0x00032E71
	public void PrepareToSuicide()
	{
		this._readyToSuicide = true;
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x00034C7A File Offset: 0x00032E7A
	public override void PanicReadyComplete()
	{
		if (base.CurrentPanicAction != null && !(base.CurrentPanicAction is PanicReady))
		{
			return;
		}
		base.CurrentPanicAction = new PanicOfficer(this);
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00176FA8 File Offset: 0x001751A8
	public override void StopPanic()
	{
		this.state = OfficerAIState.IDLE;
		if (base.CurrentPanicAction != null)
		{
			base.CurrentPanicAction = null;
		}
		this._panicData = null;
		this.mental = (float)this.maxMental;
		base.SetWorkerFaceType(WorkerFaceType.DEFAULT);
		base.SetPanicAnim(false);
		this.Stun(1f);
		this.SetFaction(FactionTypeList.StandardFaction.Worker);
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00034CA4 File Offset: 0x00032EA4
	public override void StopPanicWithoutStun()
	{
		this.state = OfficerAIState.IDLE;
		base.CurrentPanicAction = null;
		this.SetFaction(FactionTypeList.StandardFaction.Worker);
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x00034CBF File Offset: 0x00032EBF
	public override bool IsPanic()
	{
		return base.CurrentPanicAction != null;
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x00034CCD File Offset: 0x00032ECD
	public void SetUnit(OfficerUnit unit)
	{
		this._unit = unit;
		unit.spriteSetter.Init(this);
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x00034CE2 File Offset: 0x00032EE2
	public OfficerUnit GetUnit()
	{
		return this._unit;
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x00034CEA File Offset: 0x00032EEA
	public void OnClick()
	{
		if (this.unconAction != null)
		{
			this.unconAction.OnClick();
		}
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x00177008 File Offset: 0x00175208
	public override void OnDie()
	{
		base.OnDie();
		if (!this.deadInit)
		{
			this.deadInit = true;
			Notice.instance.Send(NoticeName.OnOfficerDie, new object[]
			{
				this
			});
		}
		if (this.state == OfficerAIState.MEMO_MOVE || this.state == OfficerAIState.MEMO_STAY)
		{
			SefiraManager.instance.GetSefira(base.currentSefira).EndCreatureWork(this.target);
		}
		if (this.unconAction != null)
		{
			this.unconAction.OnDie();
		}
		if (base.specialDeadScene)
		{
			base.SetDeadType(DeadType.SPECIAL);
			try
			{
				if (this.hasUniqueFace)
				{
					this.GetUnit().animChanger.ChangeAnimatorWithUniqueFace(this.deadSceneName, this.seperator);
				}
				else
				{
					this.GetUnit().SetWorkerFaceType(WorkerFaceType.PANIC);
					this.GetUnit().animChanger.ChangeAnimator(this.deadSceneName, this.seperator);
				}
			}
			catch (Exception ex)
			{
			}
		}
		this.state = OfficerAIState.DEAD;
		this._unit.OnDie();
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x0000425D File Offset: 0x0000245D
	public override void ResetAnimator()
	{
	}

	// Token: 0x06003BB1 RID: 15281 RVA: 0x00034D02 File Offset: 0x00032F02
	public override bool IsCrazy()
	{
		return this.state == OfficerAIState.CANNOT_CONTROLL || this.IsPanic();
	}

	// Token: 0x06003BB2 RID: 15282 RVA: 0x00034D21 File Offset: 0x00032F21
	public override void OnResurrect()
	{
		this._unit.OnResurrect();
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x00177124 File Offset: 0x00175324
	public override void InitialEncounteredCreature(UnitModel encountered)
	{ // <Mod>
		if (this.IsDead())
		{
			return;
		}
		base.InitialEncounteredCreature(encountered);
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_defense"))
		{
			if (encountered.GetRiskLevel() >= 5)
			{
				TakeDamageWithoutEffect(encountered, new DamageInfo(RwbpType.W, (float)(maxMental * 2)));
			}
		}
		else if (ResearchDataModel.instance.IsUpgradedAbility("resist_horror"))
		{
			if (encountered.GetRiskLevel() >= 4)
			{
				this.TakeDamageWithoutEffect(encountered, new DamageInfo(RwbpType.W, (float)(this.maxMental * 2)));
			}
		}
		else if (encountered.GetRiskLevel() >= 2)
		{
			this.TakeDamageWithoutEffect(encountered, new DamageInfo(RwbpType.W, (float)(this.maxMental * 2)));
		}
		if (this.unconAction != null && this.unconAction is Uncontrollable_LightsHammer)
		{
			return;
		}
		try
		{
		}
		catch (NullReferenceException message)
		{
			Debug.Log(message);
		}
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x00034D2E File Offset: 0x00032F2E
	public override void InitialEncounteredCreature(RiskLevel level)
	{
		base.InitialEncounteredCreature(level);
		this.TakeDamageWithoutEffect(null, new DamageInfo(RwbpType.W, (float)level * 10));
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x00034D49 File Offset: 0x00032F49
	public override GameObject MakeCreatureEffectHead(CreatureModel creature)
	{
		return this._unit.MakeCreatureEffect(creature);
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x00034D57 File Offset: 0x00032F57
	public override GameObject MakeCreatureEffectHead(CreatureModel model, bool addlist)
	{
		return this._unit.MakeCreatureEffect(model, addlist);
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x00034D66 File Offset: 0x00032F66
	public override GameObject MakeCreatureEffect(long id)
	{
		return this._unit.MakeCreatureEffect(id);
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x00034D74 File Offset: 0x00032F74
	public override void ClearEffect()
	{
		this._unit.ClearEffect();
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x00034D81 File Offset: 0x00032F81
	public override Sefira GetCurrentSefira()
	{
		if (base.currentSefira.Equals("0"))
		{
			return null;
		}
		return SefiraManager.instance.GetSefira(base.currentSefira);
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x00034DAA File Offset: 0x00032FAA
	public override WorkerDeadScript ChangePuppetAnimToDie(string src)
	{
		if (this.puppetChanged)
		{
			return null;
		}
		this.puppetChanged = true;
		return null;
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x00034DC1 File Offset: 0x00032FC1
	public override void ChangePuppetAnimToUncon(string src)
	{
		if (this.puppetChanged)
		{
			return;
		}
		this.puppetChanged = true;
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x00034CE2 File Offset: 0x00032EE2
	public override WorkerUnit GetWorkerUnit()
	{
		return this._unit;
	}

	// <Mod>
	public void FeignDeathScene(string specialDeadSceneName)
	{
		try
		{
			if (this.hasUniqueFace)
			{
				this.GetUnit().animChanger.ChangeAnimatorWithUniqueFace(specialDeadSceneName, this.seperator);
			}
			else
			{
				this.GetUnit().SetWorkerFaceType(WorkerFaceType.PANIC);
				this.GetUnit().animChanger.ChangeAnimator(specialDeadSceneName, this.seperator);
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x040036C4 RID: 14020
	public bool isDebugger;

	// Token: 0x040036C6 RID: 14022
	private bool isMoving;

	// Token: 0x040036C7 RID: 14023
	private float elapsedTime;

	// Token: 0x040036C8 RID: 14024
	private float recoverElapsed;

	// Token: 0x040036C9 RID: 14025
	public int mentalReturn;

	// Token: 0x040036CA RID: 14026
	public int recoveryRate;

	// Token: 0x040036CB RID: 14027
	public LOOKINGDIR lookingDir;

	// Token: 0x040036CC RID: 14028
	public bool startSpecialAction;

	// Token: 0x040036CD RID: 14029
	private OfficerSpecialAction currentSpecialAction;

	// Token: 0x040036CE RID: 14030
	private OfficerAIState _state;

	// Token: 0x040036CF RID: 14031
	private Timer _panicImmuneTimer;

	// Token: 0x040036D0 RID: 14032
	private float screamElapsed;

	// Token: 0x040036D1 RID: 14033
	private float screamMax;

	// Token: 0x040036D2 RID: 14034
	private bool shouldPanic;

	// Token: 0x040036D3 RID: 14035
	private bool deadInit;

	// Token: 0x040036D4 RID: 14036
	private bool _readyToSuicide;

	// Token: 0x040036D5 RID: 14037
	private OfficerUnit _unit;
}
