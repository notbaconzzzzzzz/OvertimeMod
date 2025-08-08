/*
private void Transform(Nothing.NothingForm form) // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class Nothing : CreatureBase
{
	// Token: 0x060025D9 RID: 9689 RVA: 0x00110CF4 File Offset: 0x0010EEF4
	public Nothing()
	{
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060025DA RID: 9690 RVA: 0x0002629C File Offset: 0x0002449C
	private static int spearDamage
	{
		get
		{
			return UnityEngine.Random.Range(50, 61);
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060025DB RID: 9691 RVA: 0x00025F05 File Offset: 0x00024105
	private static int strikeDamage
	{
		get
		{
			return UnityEngine.Random.Range(300, 301);
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060025DC RID: 9692 RVA: 0x00025317 File Offset: 0x00023517
	private static int normalDamage_Lv2
	{
		get
		{
			return UnityEngine.Random.Range(25, 36);
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060025DD RID: 9693 RVA: 0x000262A7 File Offset: 0x000244A7
	private static float HealValue_1st
	{
		get
		{
			return UnityEngine.Random.Range(17f, 17f);
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x060025DE RID: 9694 RVA: 0x000262B8 File Offset: 0x000244B8
	private static float HealValue_2nd
	{
		get
		{
			return UnityEngine.Random.Range(33f, 33f);
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x060025DF RID: 9695 RVA: 0x000262C9 File Offset: 0x000244C9
	public NothingAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as NothingAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x000262F8 File Offset: 0x000244F8
	public override void OnInit()
	{
		base.OnInit();
		this.currentForm = Nothing.NothingForm.LV1;
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x00026307 File Offset: 0x00024507
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetModel(this.model);
		this.moveWaitTimer.StopTimer();
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x00110D98 File Offset: 0x0010EF98
	public override void ParamInit()
	{
		this.spearBindTargets.Clear();
		this.copiedWorker = null;
		this.nothingWorker = null;
		this.snipe = null;
		this.attackTarget = null;
		this.targetChangeWorker = null;
		this.changeWorkerRoomTimer.StopTimer();
		this.changeWorkerEscapeTimer.StopTimer();
		this.snipingTimer.StopTimer();
		this.evolveTimer.StopTimer();
		this.hatchTimer.StopTimer();
		this.spearCastingTimer.StopTimer();
		this.moveWaitTimer.StopTimer();
		this.passiveCoolTimer.StopTimer();
		this.waitTime = 0f;
		this.normalAttackStack = 0;
		this.remainAttackDelay = 0f;
		this.spearAttack = false;
		this.gonnaEscape = false;
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x000225FF File Offset: 0x000207FF
	public override void ActivateQliphothCounter()
	{
		this.Escape();
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x0002632C File Offset: 0x0002452C
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		if (this.currentForm == Nothing.NothingForm.LV2 && value > 1f)
		{
			this.CancelPassive();
		}
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x00026354 File Offset: 0x00024554
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.Transform(Nothing.NothingForm.LV1);
		this.ParamInit();
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x0001F1C4 File Offset: 0x0001D3C4
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ParamInit();
	}

	// Token: 0x060025E7 RID: 9703 RVA: 0x00110E58 File Offset: 0x0010F058
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (this.defSound.RunTimer())
		{
			this.defSound.StartTimer(this.currentDefSoundFrq);
			if (this.currentDefSoundKey != string.Empty)
			{
				base.Unit.PlaySound(this.currentDefSoundKey);
			}
		}
		if (this.changeWorkerRoomTimer.RunTimer())
		{
			this.TransformToWorkerRoom(this.targetChangeWorker);
		}
		if (this.changeWorkerEscapeTimer.RunTimer())
		{
			this.TransformToWorkerEscape(this.targetChangeWorker);
			this.model.Escape();
			this.snipingTimer.StartTimer(1f);
			return;
		}
		if (this.snipingTimer.RunTimer())
		{
			this.snipe.gameObject.SetActive(true);
			this.snipe.StartSnipe(this);
			return;
		}
	}

	// Token: 0x060025E8 RID: 9704 RVA: 0x00110F38 File Offset: 0x0010F138
	public override void OnEnterRoom(UseSkill skill)
	{
		this.currentAgent = skill.agent;
		if (this.currentForm == Nothing.NothingForm.WORKER_ROOM && this.currentAgent.fortitudeLevel < 4 && !this.currentAgent.HasUnitBuf(UnitBufType.DEATH_ANGEL_BETRAYER))
		{
			this.currentAgent.mental = 0f;
			this.currentAgent.Panic();
			try
			{
				skill.CancelWork();
			}
			catch (Exception ex)
			{
				Debug.Log("skill is null");
			}
		}
	}

	// Token: 0x060025E9 RID: 9705 RVA: 0x00110FC4 File Offset: 0x0010F1C4
	public override void OnReleaseWork(UseSkill skill)
	{
		if (GameManager.currentGameManager.state == GameState.STOP)
		{
			return;
		}
		if (this.gonnaEscape)
		{
			this.currentAgent = null;
			return;
		}
		if (this.currentForm == Nothing.NothingForm.LV1)
		{
			if (this.currentAgent.justiceLevel <= 3)
			{
				this.model.SubQliphothCounter();
			}
			else if (skill.agent.IsPanic() || skill.agent.IsDead())
			{
				if (!skill.agent.IsDead())
				{
					skill.agent.Die();
				}
				this.targetChangeWorker = skill.agent;
				this.changeWorkerRoomTimer.StartTimer(2f);
				this.animScript.PlayChangeEffectAnimation();
			}
			else if (this.model.feelingState == CreatureFeelingState.BAD && !skill.agent.invincible)
			{
				skill.agent.Die();
				this.targetChangeWorker = skill.agent;
				this.changeWorkerRoomTimer.StartTimer(2f);
				this.animScript.PlayChangeEffectAnimation();
			}
		}
		else if (this.currentForm == Nothing.NothingForm.WORKER_ROOM && (skill.agent.IsPanic() || skill.agent.mental <= 0f))
		{
			skill.agent.Die();
			this.targetChangeWorker = skill.agent;
			this.model.SubQliphothCounter();
		}
		this.currentAgent = null;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x0011113C File Offset: 0x0010F33C
	public override void Escape()
	{
		if (this.model.currentSkill != null)
		{
			this.model.currentSkill.agent.StopAction();
		}
		if (this.currentForm == Nothing.NothingForm.LV1)
		{
			this.EscapeLv1();
		}
		else if (this.currentForm == Nothing.NothingForm.WORKER_ROOM)
		{
			this.EscapeWorkerForm();
		}
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x00026369 File Offset: 0x00024569
	public override void OnSuppressed()
	{
		base.OnSuppressed();
		this.defSound.StopTimer();
		this.CancelPassive();
		this.passiveCoolTimer.StopTimer();
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x00111198 File Offset: 0x0010F398
	public override void OnReturn()
	{
		if (this.nothingWorker != null)
		{
			UnityEngine.Object.Destroy(this.nothingWorker.gameObject);
			this.nothingWorker = null;
		}
		this.model.ResetQliphothCounter();
		this.Transform(Nothing.NothingForm.LV1);
		this.ParamInit();
		this.defSound.StartTimer(this.currentDefSoundFrq);
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x001111F8 File Offset: 0x0010F3F8
	public override void UniqueEscape()
	{
		if (this.currentForm == Nothing.NothingForm.LV1)
		{
			this.ProcessLv1();
		}
		else if (this.currentForm == Nothing.NothingForm.EGG)
		{
			this.model.CheckNearWorkerEncounting();
			if (this.hatchTimer.started)
			{
				this.heartBeatElap += Time.deltaTime;
				if (this.heartBeatElap >= this.heartBeatFreq)
				{
					float num = Mathf.Lerp(0f, this.hatchTimer.maxTime, this.hatchTimer.elapsed) / this.hatchTimer.maxTime;
					this.heartBeatElap = 0f;
					this.heartBeatFreq = 2.5f - num * 2.5f + 2.5f;
					base.Unit.PlaySound("heartBeat");
				}
			}
			if (this.hatchTimer.RunTimer())
			{
				this.Transform(Nothing.NothingForm.LV2);
				this.model.ClearWorkerEncounting();
				base.Unit.PlaySound("meet");
				base.Unit.PlaySound("hatch");
			}
			else
			{
				this.model.ClearCommand();
				this.model.GetMovableNode().StopMoving();
			}
		}
		else if (this.currentForm == Nothing.NothingForm.LV2)
		{
			this.ProcessLv2();
		}
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x00111344 File Offset: 0x0010F544
	private void ProcessLv1()
	{
		this.model.CheckNearWorkerEncounting();
		if (this.evolveTimer.RunTimer())
		{
			this.StartEvolve();
			return;
		}
		if ((this.waitTime <= 0f && this.attackTarget == null) || this.model.GetCreatureCurrentCmd() == null)
		{
			if (this.model.sefira.CheckAgentControll())
			{
				this.model.MoveToNode(MapGraph.instance.GetRoamingNodeByRandom(this.model.sefira.indexString));
			}
			else
			{
				this.model.MoveToNode(MapGraph.instance.GetRoamingNodeByRandom());
			}
			this.waitTime = 20f + UnityEngine.Random.value * 10f;
		}
		if (!(this.model.GetCreatureCurrentCmd() is AttackCommand_nothing))
		{
			List<UnitModel> list = new List<UnitModel>();
			MovableObjectNode movableNode = this.model.GetMovableNode();
			PassageObjectModel passage = movableNode.GetPassage();
			if (passage != null)
			{
				foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets(movableNode))
				{
					UnitModel unit = movableObjectNode.GetUnit();
					if (unit != null)
					{
						if (unit != this.model)
						{
							if (this.model.IsHostile(unit))
							{
								if (unit.IsAttackTargetable())
								{
									if (unit.hp > 0f)
									{
										list.Add(unit);
									}
								}
							}
						}
					}
				}
				if (list.Count > 0)
				{
					UnitModel unitModel = null;
					float num = 100000f;
					foreach (UnitModel unitModel2 in list)
					{
						float magnitude = (unitModel2.GetCurrentViewPosition() - this.model.GetCurrentViewPosition()).magnitude;
						if (num > magnitude)
						{
							num = magnitude;
							unitModel = unitModel2;
						}
					}
					if (unitModel != null)
					{
						this.attackTarget = unitModel;
						this.model.AttackTarget(new AttackCommand_nothing(unitModel));
					}
				}
			}
		}
	}

	// Token: 0x060025EF RID: 9711 RVA: 0x00111580 File Offset: 0x0010F780
	private void ProcessLv2()
	{
		this.model.CheckNearWorkerEncounting();
		if (this.passiveCoolTimer.RunTimer())
		{
			this.GetPassive();
		}
		this.waitTime -= Time.deltaTime;
		if (this.remainAttackDelay > 0f)
		{
			this.remainAttackDelay -= Time.deltaTime;
		}
		if (this.IsAttacking())
		{
			return;
		}
		if (this.currentPassage != null && this.currentPassage.GetPassageType() == PassageType.ISOLATEROOM)
		{
			this.model.ClearCommand();
			this.model.MoveToNode(MapGraph.instance.GetRoamingNodeByRandom());
			return;
		}
		if (this._currentMovementTargetAgent != null && this._currentMovementTargetAgent.GetState() == AgentAIState.MANAGE && this._currentMovementTargetAgent.target != null && this._currentMovementTargetAgent.target.state == CreatureState.WORKING)
		{
			this.model.ClearCommand();
			this.model.MoveToNode(MapGraph.instance.GetRoamingNodeByRandom());
			this._currentMovementTargetAgent = null;
			return;
		}
		if (this.moveWaitTimer.started && this.animScript.CanMove())
		{
			this.moveWaitTimer.StopTimer();
			this.model.MoveToNode(MapGraph.instance.GetRoamingNodeByRandom());
		}
		if (this.spearCastingTimer.RunTimer())
		{
			this.StartSpearAttack();
		}
		if (this.spearCastingTimer.started)
		{
			this.model.ClearCommand();
			return;
		}
		if (this.spearAttack)
		{
			this.model.ClearCommand();
			return;
		}
		List<UnitModel> list = new List<UnitModel>();
		List<UnitModel> list2 = new List<UnitModel>();
		List<UnitModel> list3 = new List<UnitModel>();
		this.GetAttackTargets(list, list2, list3);
		if (list.Count == 0)
		{
			if (this.waitTime <= 0f || this.model.GetCreatureCurrentCmd() == null)
			{
				List<AgentModel> list4 = new List<AgentModel>();
				List<AgentModel> list5 = new List<AgentModel>();
				foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
				{
					if (agentModel.IsAttackTargetable())
					{
						if (!agentModel.IsDead())
						{
							if (agentModel.currentSkill == null)
							{
								if (!agentModel.cannotBeAttackTargetable)
								{
									if (agentModel.GetState() != AgentAIState.MANAGE || agentModel.target == null || agentModel.target.state != CreatureState.WORKING)
									{
										list4.Add(agentModel);
										if (agentModel.GetCurrentSefira() == this.model.sefira)
										{
											list5.Add(agentModel);
										}
									}
								}
							}
						}
					}
				}
				if (list5.Count > 0)
				{
					AgentModel agentModel2 = list5[UnityEngine.Random.Range(0, list5.Count)];
					this._currentMovementTargetAgent = agentModel2;
					this.model.MoveToMovable(agentModel2.GetMovableNode());
				}
				else if (list4.Count > 0)
				{
					AgentModel agentModel3 = list4[UnityEngine.Random.Range(0, list4.Count)];
					this.model.MoveToMovable(agentModel3.GetMovableNode());
					this._currentMovementTargetAgent = agentModel3;
				}
				else if (this.animScript.CanMove())
				{
					this.model.MoveToNode(MapGraph.instance.GetRoamingNodeByRandom());
				}
				else
				{
					this.moveWaitTimer.StartTimer();
				}
				this.waitTime = 20f + UnityEngine.Random.value * 10f;
			}
		}
		else
		{
			if (this.remainAttackDelay <= 0f)
			{
				if (UnityEngine.Random.value <= 0.4f + (float)this.normalAttackStack * 0.15f)
				{
					bool flag = list2.Count > 0;
					bool flag2 = list3.Count > 0;
					if (flag && UnityEngine.Random.value < 0.3f)
					{
						this.movable.StopMoving();
						this.animScript.AttackLv2Strike();
					}
					else if (flag2)
					{
						this.movable.StopMoving();
						this.CastSpearAttack();
						this.model.ClearCommand();
						this._currentMovementTargetAgent = null;
					}
					this.normalAttackStack = 0;
				}
				else if (list2.Count > 0)
				{
					this.movable.StopMoving();
					this._currentMovementTargetAgent = null;
					this.normalAttackStack++;
					this.animScript.AttackLv2();
				}
				this.remainAttackDelay = 2.5f;
			}
			if (list2.Count > 0)
			{
				this._currentMovementTargetAgent = null;
				this.model.ClearCommand();
			}
			else if (this.waitTime <= 0f || this.model.GetCreatureCurrentCmd() == null)
			{
				UnitModel unitModel = list[UnityEngine.Random.Range(0, list.Count)];
				this.model.MoveToMovable(unitModel.GetMovableNode());
				this.waitTime = 20f + UnityEngine.Random.value * 10f;
				if (unitModel is AgentModel)
				{
					this._currentMovementTargetAgent = (unitModel as AgentModel);
				}
			}
		}
	}

	// Token: 0x060025F0 RID: 9712 RVA: 0x00111AB4 File Offset: 0x0010FCB4
	public void GiveDamageLv2NormalAttack()
	{
		List<UnitModel> list = new List<UnitModel>();
		this.GetAttackTargets(null, list, null);
		foreach (UnitModel unitModel in list)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)Nothing.normalDamage_Lv2));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
			WorkerModel workerModel = unitModel as WorkerModel;
			if (workerModel != null && workerModel.IsDead())
			{
				UnitDirection dir = UnitDirection.LEFT;
				if (workerModel.GetCurrentViewPosition().x >= this.movable.GetCurrentViewPosition().x)
				{
					dir = UnitDirection.RIGHT;
				}
				this.MakeExplodeEffect(dir, workerModel, 1f);
				if (workerModel is AgentModel)
				{
					AngelaConversation.instance.MakeMessage(AngelaMessageState.AGENT_DEAD_NORMAL, new object[]
					{
						workerModel as AgentModel,
						this.model
					});
				}
			}
		}
	}

	// Token: 0x060025F1 RID: 9713 RVA: 0x00111BC4 File Offset: 0x0010FDC4
	public void GiveDamageLv2StrikeAttack()
	{
		List<UnitModel> list = new List<UnitModel>();
		this.GetAttackTargets(null, list, null);
		foreach (UnitModel unitModel in list)
		{
			unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)Nothing.strikeDamage));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
			WorkerModel workerModel = unitModel as WorkerModel;
			if (workerModel != null && workerModel.IsDead())
			{
				UnitDirection dir = UnitDirection.LEFT;
				if (workerModel.GetCurrentViewPosition().x >= this.movable.GetCurrentViewPosition().x)
				{
					dir = UnitDirection.RIGHT;
				}
				this.MakeExplodeEffect(dir, workerModel, 1f);
				if (workerModel is AgentModel)
				{
					AngelaConversation.instance.MakeMessage(AngelaMessageState.AGENT_DEAD_NORMAL, new object[]
					{
						workerModel as AgentModel,
						this.model
					});
				}
			}
		}
	}

	// Token: 0x060025F2 RID: 9714 RVA: 0x0002638D File Offset: 0x0002458D
	public void SetLv2SpearBindTargets()
	{
		this.spearBindTargets = new List<UnitModel>();
		this.GetAttackTargets(null, null, this.spearBindTargets);
	}

	// Token: 0x060025F3 RID: 9715 RVA: 0x00111CD4 File Offset: 0x0010FED4
	public void GiveDamageLv2SpearAttack()
	{
		List<UnitModel> list = new List<UnitModel>();
		foreach (UnitModel unitModel in this.spearBindTargets)
		{
			if (!list.Contains(unitModel))
			{
				list.Add(unitModel);
				unitModel.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)Nothing.spearDamage));
				DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, RwbpType.R, this.model);
				WorkerModel workerModel = unitModel as WorkerModel;
				if (workerModel != null && workerModel.IsDead())
				{
					UnitDirection dir = UnitDirection.LEFT;
					if (workerModel.GetCurrentViewPosition().x >= this.movable.GetCurrentViewPosition().x)
					{
						dir = UnitDirection.RIGHT;
					}
					this.MakeExplodeEffect(dir, workerModel, 1f);
					if (workerModel is AgentModel)
					{
						AngelaConversation.instance.MakeMessage(AngelaMessageState.AGENT_DEAD_NORMAL, new object[]
						{
							workerModel as AgentModel,
							this.model
						});
					}
				}
			}
		}
		base.Unit.PlaySound("sk1shang");
		PassageObjectModel currentPassage = this.model.GetMovableNode().currentPassage;
		if (currentPassage != null)
		{
			Vector3 currentViewPosition = this.model.GetMovableNode().GetCurrentViewPosition();
			UnitDirection direction = this.model.GetMovableNode().GetDirection();
			MapNode left = currentPassage.GetLeft();
			MapNode right = currentPassage.GetRight();
			MapNode mapNode = (direction != UnitDirection.RIGHT) ? left : right;
			if (mapNode != null)
			{
				GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/Nullthing/WallEffect");
				Vector3 localScale = new Vector3(1f, 1f, 1f);
				if (direction == UnitDirection.LEFT)
				{
					localScale.x = -1f;
				}
				gameObject.transform.localRotation = Quaternion.identity;
				Vector3 position = mapNode.GetPosition();
				position.y += 2f;
				gameObject.transform.position = position;
				gameObject.transform.localScale = localScale;
			}
		}
		this.spearBindTargets.Clear();
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x000263A8 File Offset: 0x000245A8
	private void GetPassive()
	{
		if (!this.model.HasUnitBuf(UnitBufType.NOTHING_PASSIVE))
		{
			this.model.AddUnitBuf(new NothingPassive(this));
		}
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x00111F00 File Offset: 0x00110100
	private void CancelPassive()
	{
		UnitBuf unitBufByType = this.model.GetUnitBufByType(UnitBufType.NOTHING_PASSIVE);
		if (unitBufByType != null)
		{
			unitBufByType.Destroy();
		}
		this.passiveCoolTimer.StartTimer(10f);
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x00111F38 File Offset: 0x00110138
	public void Heal()
	{
		float num = this.model.hp / (float)this.model.maxHp;
		float num2 = (float)this.model.maxHp - this.model.hp;
		float num3 = Nothing.HealValue_1st;
		if (num < 0.3f)
		{
			num3 = Nothing.HealValue_2nd;
		}
		if (num2 >= num3)
		{
			this.model.hp += num3;
		}
		else
		{
			if (num2 <= 0f)
			{
				return;
			}
			this.model.hp = (float)this.model.maxHp;
		}
		this.MakeEffect("Effect/RecoverHP");
	}

	// Token: 0x060025F7 RID: 9719 RVA: 0x00111FE4 File Offset: 0x001101E4
	private GameObject MakeEffect(string src)
	{
		GameObject gameObject = Prefab.LoadPrefab(src);
		gameObject.transform.SetParent(this.animScript.gameObject.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		return gameObject;
	}

	// Token: 0x060025F8 RID: 9720 RVA: 0x00112044 File Offset: 0x00110244
	private void GetAttackTargets(List<UnitModel> outTargetsInPassage, List<UnitModel> outAttackTargets, List<UnitModel> outSpearTargets)
	{
		MovableObjectNode movableNode = this.model.GetMovableNode();
		PassageObjectModel passage = this.model.GetMovableNode().GetPassage();
		float x = movableNode.GetCurrentViewPosition().x;
		if (passage != null)
		{
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets(movableNode))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit != null)
				{
					if (movableObjectNode.GetPassage() == passage)
					{
						if (unit != this.model)
						{
							if (unit.hp > 0f)
							{
								if (unit.IsAttackTargetable())
								{
									if (this.model.IsHostile(unit))
									{
										if (outTargetsInPassage != null)
										{
											outTargetsInPassage.Add(unit);
										}
										float x2 = movableObjectNode.GetCurrentViewPosition().x;
										if (movableNode.GetDirection() == UnitDirection.RIGHT)
										{
											if (x <= x2)
											{
												if (Mathf.Abs(x - x2) < 2f && outAttackTargets != null)
												{
													outAttackTargets.Add(unit);
												}
												if (outSpearTargets != null)
												{
													outSpearTargets.Add(unit);
												}
											}
										}
										else if (x >= x2)
										{
											if (Mathf.Abs(x - x2) < 2f && outAttackTargets != null)
											{
												outAttackTargets.Add(unit);
											}
											if (outSpearTargets != null)
											{
												outSpearTargets.Add(unit);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x000263CE File Offset: 0x000245CE
	private void CastSpearAttack()
	{
		this.spearCastingTimer.StartTimer(2f);
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x001121C4 File Offset: 0x001103C4
	private void StartSpearAttack()
	{
		List<UnitModel> list = new List<UnitModel>();
		this.GetAttackTargets(null, null, list);
		if (list.Count > 0)
		{
			this.animScript.PrepareSpear();
			this.spearAttack = true;
		}
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x000263E0 File Offset: 0x000245E0
	public void EndSpearAttack()
	{
		this.spearAttack = false;
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x000263E9 File Offset: 0x000245E9
	private void EscapeWorkerForm()
	{
		if (this.changeWorkerEscapeTimer.started)
		{
			return;
		}
		this.animScript.StartSnipingEffect();
		this.changeWorkerEscapeTimer.StartTimer(3f);
		base.Unit.PlaySound("escape2");
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x00026428 File Offset: 0x00024628
	private void EscapeLv1()
	{
		this.TransformToLv1Escape();
		this.model.Escape();
		base.Unit.PlaySound("escape1");
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x00112200 File Offset: 0x00110400
	private void TransformToWorkerRoom(WorkerModel worker)
	{
		this.TransformToWorker(worker);
		this.nothingWorker.model.GetMovableNode().SetCurrentNode(this.model.GetRoomNode());
		this.nothingWorker.model.GetMovableNode().SetDirection(UnitDirection.RIGHT);
		this.Transform(Nothing.NothingForm.WORKER_ROOM);
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x00112254 File Offset: 0x00110454
	private void TransformToWorkerEscape(WorkerModel worker)
	{
		this.snipe = this.animScript.ShowSnipingUI();
		this.snipe.gameObject.SetActive(false);
		this.TransformToWorker(worker);
		this.nothingWorker.model.GetMovableNode().SetCurrentNode(MapGraph.instance.GetRoamingNodeByRandom());
		this.Transform(Nothing.NothingForm.WORKER_ESCAPE);
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x001122B0 File Offset: 0x001104B0
	private void Transform(Nothing.NothingForm form)
	{ // <Mod>
		this.animScript.ChangeForm(form);
		this.currentForm = form;
		this.SetDefenseType();
		switch (form)
		{
		case Nothing.NothingForm.WORKER_ROOM:
			this.defSound.StopTimer();
			this.currentDefSoundFrq = 5f;
			this.defSound.StartTimer(this.currentDefSoundFrq);
			this.currentDefSoundKey = "humanDefault";
			base.Unit.PlaySound(this.currentDefSoundKey);
			break;
		case Nothing.NothingForm.LV1:
			this.model.hp = (float)(this.model.baseMaxHp = 2000);
			this.model.movementScale = 2f;
			this.defSound.StopTimer();
			this.currentDefSoundFrq = 12f;
			this.defSound.StartTimer(this.currentDefSoundFrq);
			this.currentDefSoundKey = "stand1";
			base.Unit.PlaySound(this.currentDefSoundKey);
			break;
		case Nothing.NothingForm.EGG:
			this.model.hp = (float)(this.model.baseMaxHp = 2000);
			break;
		case Nothing.NothingForm.LV2:
			this.model.hp = (float)(this.model.baseMaxHp = 2000);
			this.model.movementScale = 1.2f;
            model.movementScale *= 1f / model.GetMovementScaleByBuf();
			this.MakeEffectGlobalPos("Effect/Creature/Nullthing/NullthingEggBreak", base.Unit.transform.position);
			this.defSound.StopTimer();
			this.currentDefSoundFrq = 25f;
			this.defSound.StartTimer(this.currentDefSoundFrq);
			this.currentDefSoundKey = "transform";
			base.Unit.PlaySound(this.currentDefSoundKey);
			this.passiveCoolTimer.StartTimer(10f);
			break;
		}
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x00112480 File Offset: 0x00110680
	private void TransformToWorker(WorkerModel worker)
	{
		if (this.nothingWorker != null)
		{
			UnityEngine.Object.Destroy(this.nothingWorker.gameObject);
			this.nothingWorker = null;
		}
		if (worker is AgentModel)
		{
			AgentModel agentModel = worker as AgentModel;
			Dictionary<string, object> saveData = agentModel.GetSaveData();
			AgentModel agentModel2 = new AgentModel(-1L);
			agentModel2.LoadData(saveData);
			agentModel2.hp = (float)agentModel2.maxHp;
			agentModel2.mental = (float)agentModel2.maxMental;
			agentModel2.isRealWorker = false;
			agentModel2.currentSefira = agentModel.currentSefira;
			this.copiedWorker = agentModel2;
		}
		else if (worker is OfficerModel)
		{
			OfficerModel officerModel = worker as OfficerModel;
			OfficerModel officerModel2 = new OfficerModel(-1L, officerModel.currentSefira);
			officerModel2.baseMaxHp = officerModel.baseMaxHp;
			officerModel2.baseMaxMental = officerModel.baseMaxMental;
			officerModel2.hp = (float)officerModel2.maxHp;
			officerModel2.mental = (float)officerModel2.maxMental;
			officerModel2.isRealWorker = false;
			this.copiedWorker = officerModel2;
		}
		this.copiedWorker.GetMovableNode().SetActive(false);
		GameObject gameObject = Prefab.LoadPrefab("NothingWorker");
		gameObject.transform.SetParent(OfficerLayer.currentLayer.transform);
		gameObject.transform.localPosition = Vector3.zero;
		this.nothingWorker = gameObject.GetComponent<NothingWorker>();
		this.nothingWorker.Init(this.copiedWorker);
		worker.GetWorkerUnit().gameObject.SetActive(false);
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x0002644C File Offset: 0x0002464C
	private void TransformToLv1Escape()
	{
		this.evolveTimer.StartTimer(30f);
		this.Transform(Nothing.NothingForm.LV1);
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x001125F4 File Offset: 0x001107F4
	private void StartEvolve()
	{
		this.hatchTimer.StartTimer(30f);
		base.Unit.PlaySound("heartBeat");
		this.model.ClearWorkerEncounting();
		this.Transform(Nothing.NothingForm.EGG);
		this.heartBeatElap = 0f;
		this.heartBeatFreq = 5f;
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x0011264C File Offset: 0x0011084C
	public void FinishSniping()
	{
		this.moveWaitTimer.StopTimer();
		if (this.nothingWorker.model.IsDead())
		{
			this.model.Suppressed();
		}
		else
		{
			this.TransformToLv1Escape();
			this.model.GetMovableNode().Assign(this.nothingWorker.model.GetMovableNode());
			UnityEngine.Object.Destroy(this.nothingWorker.gameObject);
			this.nothingWorker = null;
		}
		UnityEngine.Object.Destroy(this.snipe.gameObject);
		this.snipe = null;
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x001126E0 File Offset: 0x001108E0
	private void SetDefenseType()
	{
		Nothing.NothingForm nothingForm = this.currentForm;
		if (nothingForm != Nothing.NothingForm.LV1)
		{
			if (nothingForm != Nothing.NothingForm.EGG)
			{
				if (nothingForm == Nothing.NothingForm.LV2)
				{
					this.model.SetDefenseId("3");
				}
			}
			else
			{
				this.model.SetDefenseId("2");
			}
		}
		else
		{
			this.model.SetDefenseId("1");
		}
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x00026465 File Offset: 0x00024665
	public override float TranformWorkProb(float originWorkProb)
	{
		if (this.currentAgent != null)
		{
			return (float)this.currentAgent.fortitudeLevel * 0.2f * base.TranformWorkProb(originWorkProb);
		}
		return base.TranformWorkProb(originWorkProb);
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x00112750 File Offset: 0x00110950
	public bool IsAttacking()
	{
		return this.spearAttack || this.animScript.animator.GetBool("Strike") || this.animScript.animator.GetBool("Attack");
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x00107224 File Offset: 0x00105424
	public void MakeExplodeEffect(UnitDirection dir, WorkerModel target, float size)
	{
		ExplodeGutEffect explodeGutEffect = null;
		WorkerUnit workerUnit = target.GetWorkerUnit();
		if (ExplodeGutManager.instance.MakeEffects(workerUnit.gameObject.transform.position, ref explodeGutEffect))
		{
			explodeGutEffect.particleCount = UnityEngine.Random.Range(3, 9);
			explodeGutEffect.ground = target.GetMovableNode().GetCurrentViewPosition().y;
			float num = 0f;
			float num2 = 0f;
			explodeGutEffect.SetEffectSize(size);
			explodeGutEffect.Shoot(ExplodeGutEffect.Directional.DIRECTION, dir);
			if (target.GetMovableNode().GetPassage() != null)
			{
				target.GetMovableNode().GetPassage().GetVerticalRange(ref num, ref num2);
				explodeGutEffect.SetCurrentPassage(target.GetMovableNode().GetPassage());
			}
		}
		workerUnit.gameObject.SetActive(false);
	}

	// Token: 0x040024F7 RID: 9463
	private const int maxHp_lv1 = 2000;

	// Token: 0x040024F8 RID: 9464
	private const float movement_lv1 = 2f;

	// Token: 0x040024F9 RID: 9465
	private const int maxHp_lv2 = 2000;

	// Token: 0x040024FA RID: 9466
	private const float movement_lv2 = 1.2f;

	// Token: 0x040024FB RID: 9467
	private const int maxHp_egg = 2000;

	// Token: 0x040024FC RID: 9468
	private Timer evolveTimer = new Timer();

	// Token: 0x040024FD RID: 9469
	private const float evolveTime = 30f;

	// Token: 0x040024FE RID: 9470
	private Timer hatchTimer = new Timer();

	// Token: 0x040024FF RID: 9471
	private const float hatchTime = 30f;

	// Token: 0x04002500 RID: 9472
	private Timer changeWorkerRoomTimer = new Timer();

	// Token: 0x04002501 RID: 9473
	private const float changeWorkerRoomTime = 2f;

	// Token: 0x04002502 RID: 9474
	private Timer changeWorkerEscapeTimer = new Timer();

	// Token: 0x04002503 RID: 9475
	private const float changeWorkerEscapeTime = 3f;

	// Token: 0x04002504 RID: 9476
	private Timer snipingTimer = new Timer();

	// Token: 0x04002505 RID: 9477
	private const float snipingTime = 1f;

	// Token: 0x04002506 RID: 9478
	private int normalAttackStack;

	// Token: 0x04002507 RID: 9479
	private const float skillProbPerNormalAttackStack = 0.15f;

	// Token: 0x04002508 RID: 9480
	private const float defaultSkillProb = 0.4f;

	// Token: 0x04002509 RID: 9481
	private float remainAttackDelay;

	// Token: 0x0400250A RID: 9482
	private const float attackDelay = 2.5f;

	// Token: 0x0400250B RID: 9483
	private bool spearAttack;

	// Token: 0x0400250C RID: 9484
	private List<UnitModel> spearBindTargets = new List<UnitModel>();

	// Token: 0x0400250D RID: 9485
	private Timer spearCastingTimer = new Timer();

	// Token: 0x0400250E RID: 9486
	private const float spearCastingTime = 2f;

	// Token: 0x0400250F RID: 9487
	private const int spearDamageMin = 50;

	// Token: 0x04002510 RID: 9488
	private const int spearDamageMax = 60;

	// Token: 0x04002511 RID: 9489
	private const float strikeProb = 0.3f;

	// Token: 0x04002512 RID: 9490
	private const int strikeDamageMin = 300;

	// Token: 0x04002513 RID: 9491
	private const int strikeDamageMax = 300;

	// Token: 0x04002514 RID: 9492
	private const int normalDamageMin_Lv2 = 25;

	// Token: 0x04002515 RID: 9493
	private const int normalDamageMax_Lv2 = 35;

	// Token: 0x04002516 RID: 9494
	private Timer passiveCoolTimer = new Timer();

	// Token: 0x04002517 RID: 9495
	private const float passiveCoolTime = 10f;

	// Token: 0x04002518 RID: 9496
	private const float passiveCancelDmg = 1f;

	// Token: 0x04002519 RID: 9497
	private const float passivePhaseOver = 0.3f;

	// Token: 0x0400251A RID: 9498
	private const float _healValueMin_1st = 17f;

	// Token: 0x0400251B RID: 9499
	private const float _healValueMax_1st = 17f;

	// Token: 0x0400251C RID: 9500
	private const float _healValueMin_2nd = 33f;

	// Token: 0x0400251D RID: 9501
	private const float _healValueMax_2nd = 33f;

	// Token: 0x0400251E RID: 9502
	private const string _healEffectSrc = "Effect/RecoverHP";

	// Token: 0x0400251F RID: 9503
	public Timer moveWaitTimer = new Timer();

	// Token: 0x04002520 RID: 9504
	public Timer defSound = new Timer();

	// Token: 0x04002521 RID: 9505
	private const float humanDefFrq = 5f;

	// Token: 0x04002522 RID: 9506
	private const float level1DefFrq = 12f;

	// Token: 0x04002523 RID: 9507
	private const float level2DefFrq = 25f;

	// Token: 0x04002524 RID: 9508
	private float currentDefSoundFrq = 10f;

	// Token: 0x04002525 RID: 9509
	private string currentDefSoundKey = string.Empty;

	// Token: 0x04002526 RID: 9510
	private const float heartBeatMax = 2.5f;

	// Token: 0x04002527 RID: 9511
	private float heartBeatElap;

	// Token: 0x04002528 RID: 9512
	private float heartBeatFreq = 2.5f;

	// Token: 0x04002529 RID: 9513
	public const float attackRange_lv1 = 2f;

	// Token: 0x0400252A RID: 9514
	public const float attackRange_lv2 = 2f;

	// Token: 0x0400252B RID: 9515
	private bool gonnaEscape;

	// Token: 0x0400252C RID: 9516
	private NothingSnipingUI snipe;

	// Token: 0x0400252D RID: 9517
	private float waitTime;

	// Token: 0x0400252E RID: 9518
	private UnitModel attackTarget;

	// Token: 0x0400252F RID: 9519
	public Nothing.NothingForm currentForm;

	// Token: 0x04002530 RID: 9520
	private NothingWorker nothingWorker;

	// Token: 0x04002531 RID: 9521
	public WorkerModel copiedWorker;

	// Token: 0x04002532 RID: 9522
	private WorkerModel targetChangeWorker;

	// Token: 0x04002533 RID: 9523
	private AgentModel currentAgent;

	// Token: 0x04002534 RID: 9524
	private AgentModel _currentMovementTargetAgent;

	// Token: 0x04002535 RID: 9525
	private NothingAnim _animScript;

	// Token: 0x02000431 RID: 1073
	public class SoundTableKey
	{
		// Token: 0x06002609 RID: 9737 RVA: 0x000042F0 File Offset: 0x000024F0
		public SoundTableKey()
		{
		}

		// Token: 0x04002536 RID: 9526
		public const string attack1 = "attack1";

		// Token: 0x04002537 RID: 9527
		public const string attack2 = "attack2";

		// Token: 0x04002538 RID: 9528
		public const string change2 = "change2";

		// Token: 0x04002539 RID: 9529
		public const string change1 = "change1";

		// Token: 0x0400253A RID: 9530
		public const string heartBeat = "heartBeat";

		// Token: 0x0400253B RID: 9531
		public const string humanDefault = "humanDefault";

		// Token: 0x0400253C RID: 9532
		public const string sk1cast = "sk1cast";

		// Token: 0x0400253D RID: 9533
		public const string sk1ching = "sk1ching";

		// Token: 0x0400253E RID: 9534
		public const string sk1dmg = "sk1dmg";

		// Token: 0x0400253F RID: 9535
		public const string sk1shang = "sk1shang";

		// Token: 0x04002540 RID: 9536
		public const string sk2cast = "sk2cast";

		// Token: 0x04002541 RID: 9537
		public const string sk2hook = "sk2hook";

		// Token: 0x04002542 RID: 9538
		public const string sk3cast = "sk3cast";

		// Token: 0x04002543 RID: 9539
		public const string sk3finish = "sk3finish";

		// Token: 0x04002544 RID: 9540
		public const string stand1 = "stand1";

		// Token: 0x04002545 RID: 9541
		public const string stand2 = "stand2";

		// Token: 0x04002546 RID: 9542
		public const string transform = "transform";

		// Token: 0x04002547 RID: 9543
		public const string escape3 = "escape3";

		// Token: 0x04002548 RID: 9544
		public const string escape1 = "escape1";

		// Token: 0x04002549 RID: 9545
		public const string escape2 = "escape2";

		// Token: 0x0400254A RID: 9546
		public const string hit1 = "hit1";

		// Token: 0x0400254B RID: 9547
		public const string meet = "meet";

		// Token: 0x0400254C RID: 9548
		public const string run2 = "run2";

		// Token: 0x0400254D RID: 9549
		public const string walk = "walk";

		// Token: 0x0400254E RID: 9550
		public const string run1 = "run1";

		// Token: 0x0400254F RID: 9551
		public const string hatch = "hatch";
	}

	// Token: 0x02000432 RID: 1074
	public enum NothingForm
	{
		// Token: 0x04002551 RID: 9553
		WORKER_ROOM,
		// Token: 0x04002552 RID: 9554
		LV1,
		// Token: 0x04002553 RID: 9555
		WORKER_ESCAPE,
		// Token: 0x04002554 RID: 9556
		EGG,
		// Token: 0x04002555 RID: 9557
		LV2
	}
}
