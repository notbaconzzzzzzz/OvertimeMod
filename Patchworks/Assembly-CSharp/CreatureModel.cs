/*
public virtual void OnStageStart() // 
public virtual void OnFixedUpdate() // 
public int GetRedusedWorkProbByCounter() // 
public void ActivateOverload(int level, float iOverloadTime = 60f, OverloadType overloadType = OverloadType.DEFAULT) // 
public void ExplodeOverload() // 
public override void TakeDamage(UnitModel actor, DamageInfo dmg) // 
public virtual void Suppressed() // 
public void SetFeelingStateInWork(CreatureFeelingState state) // 
public void FinishReturn() // 
public void AddCreatureSuccessCube(int count) // 
+public void RecoverHP(float amount) //
+buncha stuff // Tranquilizer Bullets
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005FE RID: 1534
[Serializable]
public class CreatureModel : UnitModel, IObserver, ISerializablePlayData, IMouseCommandTargetModel
{
	// Token: 0x06003439 RID: 13369 RVA: 0x0015AD64 File Offset: 0x00158F64
	public CreatureModel(long instanceId)
	{
		this.InstanceID = Guid.NewGuid();
		this.movableNode = new MovableObjectNode(this);
		this.movableNode.SetPassageChangedParam(this);
		this.commandQueue = new CreatureCommandQueue(this);
		this.movableNode.AddUnpassableType(PassType.SHIELDBEARER);
		this.instanceId = instanceId;
		this.narrationList = new List<string>();
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.IdleCreature);
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x0600343A RID: 13370 RVA: 0x0002FA2C File Offset: 0x0002DC2C
	// (set) Token: 0x0600343B RID: 13371 RVA: 0x0002FA34 File Offset: 0x0002DC34
	public CreatureState state
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

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x0600343C RID: 13372 RVA: 0x0015AE30 File Offset: 0x00159030
	public override float radius
	{
		get
		{
			float result;
			try
			{
				result = this.script.GetRadius();
			}
			catch (Exception)
			{
				result = 0f;
			}
			return result;
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x0600343D RID: 13373 RVA: 0x0002FA3D File Offset: 0x0002DC3D
	// (set) Token: 0x0600343E RID: 13374 RVA: 0x0002FA4C File Offset: 0x0002DC4C
	public UseSkill currentSkill
	{
		get
		{
			UseSkill currentSkill = this._currentSkill;
			return this._currentSkill;
		}
		set
		{
			this._currentSkill = value;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x0600343F RID: 13375 RVA: 0x0002FA55 File Offset: 0x0002DC55
	public CreatureUnit Unit
	{
		get
		{
			return this._unit;
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06003440 RID: 13376 RVA: 0x0002FA5D File Offset: 0x0002DC5D
	// (set) Token: 0x06003441 RID: 13377 RVA: 0x0002FA65 File Offset: 0x0002DC65
	public Guid InstanceID { get; private set; }

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06003442 RID: 13378 RVA: 0x0002FA6E File Offset: 0x0002DC6E
	public override DefenseInfo defense
	{
		get
		{
			return this.metaInfo.defenseTable.GetDefenseInfo(this.defenseId);
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06003443 RID: 13379 RVA: 0x0002FA86 File Offset: 0x0002DC86
	public int qliphothCounter
	{
		get
		{
			return this._qliphothCounter;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06003444 RID: 13380 RVA: 0x0002FA8E File Offset: 0x0002DC8E
	public int probReductionCounter
	{
		get
		{
			return this._probReductionCounter;
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06003445 RID: 13381 RVA: 0x0002FA96 File Offset: 0x0002DC96
	// (set) Token: 0x06003446 RID: 13382 RVA: 0x0002FA9E File Offset: 0x0002DC9E
	public int ProbReductionValue
	{
		get
		{
			return this._probReductionValue;
		}
		set
		{
			this._probReductionValue = value;
			this.OnChangeProbReduectionCounter();
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06003447 RID: 13383 RVA: 0x0002FAAD File Offset: 0x0002DCAD
	public bool activated
	{
		get
		{
			return this._activated;
		}
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x0015AE68 File Offset: 0x00159068
	public Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"instanceId",
				this.instanceId
			},
			{
				"metadataId",
				this.metadataId
			},
			{
				"entryNodeId",
				this.entryNodeId
			},
			{
				"sefiraNum",
				this.sefiraNum
			},
			{
				"basePosition",
				new Vector2Serializer(this.basePosition)
			}
		};
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x0015AEE0 File Offset: 0x001590E0
	public void LoadData(Dictionary<string, object> dic)
	{
		GameUtil.TryGetValue<long>(dic, "instanceId", ref this.instanceId);
		GameUtil.TryGetValue<long>(dic, "metadataId", ref this.metadataId);
		if (this.metadataId == 100015L)
		{
			this.metadataId = 100014L;
		}
		GameUtil.TryGetValue<string>(dic, "entryNodeId", ref this.entryNodeId);
		GameUtil.TryGetValue<string>(dic, "sefiraNum", ref this.sefiraNum);
		Vector2Serializer vector2Serializer = new Vector2Serializer();
		GameUtil.TryGetValue<Vector2Serializer>(dic, "basePosition", ref vector2Serializer);
		this.basePosition = vector2Serializer.V2;
		if (this.script.HasScriptSaveData())
		{
			this.script.LoadScriptData();
		}
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x0002FAB5 File Offset: 0x0002DCB5
	public void WorkParamInit()
	{
		this.currentSkill = null;
		this.script.OnReleaseWorkAllocated();
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x0002FAC9 File Offset: 0x0002DCC9
	public virtual void OnGameInit()
	{
		this.movableNode.SetCurrentNode(this.roomNode);
	}

	// Token: 0x0600344C RID: 13388 RVA: 0x0015AF8C File Offset: 0x0015918C
	public virtual void OnStageStart()
	{ // <Mod>
		if (this.state != CreatureState.WAIT)
		{
			if (this.state == CreatureState.ESCAPE)
			{
				this.script.OnReturn();
			}
			this.state = CreatureState.WAIT;
			Debug.LogError("Not expected state");
		}
		this.movableNode.SetCurrentNode(this.roomNode);
		Sefira sefira = SefiraManager.instance.GetSefira(this.roomNode.GetAttachedPassage().GetSefiraName());
		this.sefira = sefira;
		this.sefiraNum = this.sefira.indexString;
		this.workCount = 0;
		this.suppressReturnTimer = 0f;
		this.feelingStateRemainTime = 0f;
		this.feelingState = CreatureFeelingState.NONE;
		this.WorkParamInit();
		this.ResetProbReductionCounter();
		this.ResetQliphothCounter();
		this.childs.Clear();
		this.childInst = 1L;
		this._probReductionValue = 0;
		this.script.OnStageStart();
		overloadReduction = 0;
		_isTranquilized = false;
		tranqRemainTime = 0f;
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x0015B068 File Offset: 0x00159268
	public virtual void OnStageEnd()
	{
		foreach (ChildCreatureModel childCreatureModel in this.childs)
		{
			childCreatureModel.SelfDestroy();
		}
		for (int i = 0; i < this.childs.Count; i++)
		{
			this.childs[i] = null;
		}
		this.childs.Clear();
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x0015B0E8 File Offset: 0x001592E8
	public virtual void OnStageRelease()
	{
		this.WorkParamInit();
		this.CancelOverload();
		this.ClearProbReduction();
		this.remainAttackDelay = 0f;
		this.feelingStateRemainTime = 0f;
		this.feelingState = CreatureFeelingState.NONE;
		this.overloadType = OverloadType.DEFAULT;
		if (this.state == CreatureState.ESCAPE || this.state == CreatureState.SUPPRESSED || this.state == CreatureState.SUPPRESSED_RETURN)
		{
			this.state = CreatureState.WAIT;
			this.script.OnReturn();
			this.SetCurrentNode(this.roomNode);
			Sefira sefira = SefiraManager.instance.GetSefira(this.roomNode.GetAttachedPassage().GetSefiraName());
			this.sefira = sefira;
			this.sefiraNum = this.sefira.indexString;
			CreatureLayer.currentLayer.GetCreature(this.instanceId).room.OnReturn();
		}
		UnitBuf[] array = this._bufList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnStageRelease();
		}
		this.script.OnStageRelease();
		this.state = CreatureState.WAIT;
		this.ClearCommand();
		foreach (ChildCreatureModel childCreatureModel in this.childs)
		{
			childCreatureModel.SelfDestroy();
		}
		this.childs.Clear();
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x0015B23C File Offset: 0x0015943C
	public static string GetUnitName(CreatureTypeInfo metaInfo, CreatureObserveInfoModel observeInfo)
	{
		if (metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
		{
			if (observeInfo.GetObserveState(CreatureModel.regionName[0]))
			{
				return metaInfo.collectionName;
			}
			return metaInfo.codeId;
		}
		else
		{
			if (metaInfo.creatureWorkType == CreatureWorkType.KIT)
			{
				int observeCost = observeInfo.GetObserveCost(CreatureModel.regionName[0]);
				if (metaInfo.creatureKitType == CreatureKitType.ONESHOT)
				{
					if (observeInfo.totalKitUseCount >= observeCost)
					{
						return metaInfo.name;
					}
				}
				else if (observeInfo.totalKitUseTime >= (float)observeCost)
				{
					return metaInfo.name;
				}
				return metaInfo.codeId;
			}
			return metaInfo.name;
		}
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x0002FADC File Offset: 0x0002DCDC
	public override string GetUnitName()
	{
		return CreatureModel.GetUnitName(this.metaInfo, this.observeInfo);
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x0002FAEF File Offset: 0x0002DCEF
	public CreatureFeelingState GetFeelingState()
	{
		return this.feelingState;
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x0002FAF7 File Offset: 0x0002DCF7
	public void CopyNodeData(CreatureModel target)
	{
		target.SetRoomNode(this.roomNode);
		target.SetCustomNode(this.customNode);
		target.SetWorkspaceNode(this.workspaceNode);
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x0002FB1D File Offset: 0x0002DD1D
	public void SetRoomNode(MapNode node)
	{
		this.roomNode = node;
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x0002FB26 File Offset: 0x0002DD26
	public void SetCustomNode(MapNode node)
	{
		this.customNode = node;
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x0002FB2F File Offset: 0x0002DD2F
	public virtual void SetCurrentNode(MapNode node)
	{
		this.movableNode.SetCurrentNode(node);
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x0002FB3D File Offset: 0x0002DD3D
	public virtual MapNode GetCurrentNode()
	{
		return this.movableNode.GetCurrentNode();
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x0002FB4A File Offset: 0x0002DD4A
	public virtual MapEdge GetCurrentEdge()
	{
		return this.movableNode.GetCurrentEdge();
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x0002FB57 File Offset: 0x0002DD57
	public void SetWorkspaceNode(MapNode node)
	{
		this.workspaceNode = node;
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x0002FB60 File Offset: 0x0002DD60
	public MapNode GetWorkspaceNode()
	{
		return this.workspaceNode;
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x0002FB68 File Offset: 0x0002DD68
	public MapNode GetEntryNode()
	{
		return this.entryNode;
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x0002FB70 File Offset: 0x0002DD70
	public MapNode GetCustomNode()
	{
		return this.customNode;
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x0002FB78 File Offset: 0x0002DD78
	public MapNode GetRoomNode()
	{
		return this.roomNode;
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x0002FB80 File Offset: 0x0002DD80
	public UnitDirection GetDirection()
	{
		return this.movableNode.GetDirection();
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x0002FB8D File Offset: 0x0002DD8D
	public bool IsWorkingState()
	{
		return this.state == CreatureState.WORKING || this.state == CreatureState.WORKING_SCENE || this.state == CreatureState.OBSERVE || this.state == CreatureState.WORKING_AUTO;
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x0015B2BC File Offset: 0x001594BC
	public bool IsEscaped()
	{
		return this.state == CreatureState.ESCAPE || this.state == CreatureState.SUPPRESSED || this.state == CreatureState.SUPPRESSED_RETURN || (this.script is RedShoes && (this.script.skill as RedShoesSkill).attractTargetAgent != null && (this.script.skill as RedShoesSkill).isAcquired);
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x0015B324 File Offset: 0x00159524
	public bool IsEscapedOnlyEscape()
	{
		return this.state == CreatureState.ESCAPE || (this.script is RedShoes && (this.script.skill as RedShoesSkill).attractTargetAgent != null && (this.script.skill as RedShoesSkill).isAcquired);
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x0015B378 File Offset: 0x00159578
	public UnitModel GetConnectedUnitModel()
	{
		if (this.script is RedShoes && (this.script.skill as RedShoesSkill).attractTargetAgent != null && (this.script.skill as RedShoesSkill).isAcquired)
		{
			return (this.script.skill as RedShoesSkill).attractTargetAgent;
		}
		return this;
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x0015B3D8 File Offset: 0x001595D8
	public virtual void OnFixedUpdate()
	{ // <Mod> Tranquilizer Bullets
		if (this.remainMoveDelay > 0f)
		{
			this.remainMoveDelay -= Time.deltaTime;
		}
		if (this.remainAttackDelay > 0f)
		{
			this.remainAttackDelay -= Time.deltaTime;
		}
		this.UpdateBufState();
		if (this.feelingState != CreatureFeelingState.NONE && !isTranquilized)
		{
			this.script.OnAllocatedWork(null);
			this.feelingStateRemainTime -= Time.deltaTime;
			if (this.feelingStateRemainTime <= 0f)
			{
				this.script.OnReleaseWorkAllocated();
				this.script.OnWorkCoolTimeEnd(this.feelingState);
				Notice.instance.Send(NoticeName.OnWorkCoolTimeEnd, new object[]
				{
					this,
					this.feelingState
				});
				this.Unit.room.OnFeelingStateDisplayEnd();
				this.feelingState = CreatureFeelingState.NONE;
			}
		}
		if (this.GetCurrentNode() == this.roomNode)
		{
			this.movableNode.SetDirection(UnitDirection.RIGHT);
		}
		this.tempAnim.OnFixedUpdate();
		if (this._equipment.weapon != null)
		{
			this._equipment.weapon.OnFixedUpdate();
		}
		this.commandQueue.Execute(this);
		if (!this.script.UniqueMoveControl())
		{
			if (this.GetMovableNode().IsMoving())
			{
				this.SetMoveAnimState(true);
			}
			else if (this._unit != null && this._unit.animTarget != null)
			{
				this._unit.animTarget.StopMoving();
			}
		}
		if (GameManager.currentGameManager.ManageStarted)
		{
			this.script.OnFixedUpdate(this);
		}
		if (this.state == CreatureState.ESCAPE)
		{
			this.OnEscapeUpdate();
		}
		else if (this.state == CreatureState.SUPPRESSED)
		{
			this.suppressReturnTimer += Time.deltaTime;
			if (this.suppressReturnTimer >= 10f)
			{
				this.suppressReturnTimer = 0f;
				this.state = CreatureState.SUPPRESSED_RETURN;
				this.FinishReturn();
			}
		}
		else if (this.state == CreatureState.WAIT && this.isOverloaded && this.feelingState == CreatureFeelingState.NONE && !isTranquilized)
		{
			this.overloadTimer += Time.deltaTime;
			if (this.overloadTimer >= this.currentOverloadMaxTime)
			{
				this.ExplodeOverload();
			}
		}
		try
		{
			if (this.remainMoveDelay > 0f)
			{
				this.movableNode.ProcessMoveNode(0f);
			}
			else
			{
				this.movableNode.ProcessMoveNode(this.metaInfo.speed * this.movementScale * base.GetMovementScaleByBuf());
			}
		}
		catch (MovableObjectNode.MovableElevatorStuckException)
		{
			this.script.OnElevatorStuck();
		}
		if (isTranquilized)
		{
			tranqRemainTime -= Time.deltaTime;
			if (tranqRemainTime <= 0f)
			{
				OnTranqEnd();
			}
		}
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x0002FBB5 File Offset: 0x0002DDB5
	protected override void PlayAttackAnimation(string animationName)
	{
		base.PlayAttackAnimation(animationName);
		this.SendAnimMessage(animationName);
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x00003FDD File Offset: 0x000021DD
	public void AddWorkCount()
	{
	}

	// Token: 0x06003465 RID: 13413 RVA: 0x0002FBC5 File Offset: 0x0002DDC5
	public void ResetWorkCount()
	{
		this.workCount = 0;
	}

	// Token: 0x06003466 RID: 13414 RVA: 0x0002FBCE File Offset: 0x0002DDCE
	public int GetMaxWorkCount()
	{
		return this.metaInfo.maxWorkCount;
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x0002FBDB File Offset: 0x0002DDDB
	public int GetMaxWorkCountView()
	{
		return this.script.GetMaxWorkCountView();
	}

	// Token: 0x06003468 RID: 13416 RVA: 0x000040E7 File Offset: 0x000022E7
	public bool IsWorkCountFull()
	{
		return false;
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x0015B66C File Offset: 0x0015986C
	public void AddProbReductionCounter()
	{
		int num = this.metaInfo.maxProbReductionCounter;
		if (num == -1)
		{
			if (this.metaInfo.creatureWorkType == CreatureWorkType.KIT)
			{
				num = 0;
			}
			else
			{
				switch (this.metaInfo.GetRiskLevel())
				{
				case RiskLevel.ZAYIN:
				case RiskLevel.TETH:
				case RiskLevel.HE:
					num = 0;
					break;
				case RiskLevel.WAW:
					num = 8;
					break;
				case RiskLevel.ALEPH:
					num = 5;
					break;
				}
			}
			if (this.metaInfo.id == 100064L)
			{
				num = 5;
			}
		}
		if (this._probReductionCounter < num)
		{
			this._probReductionCounter++;
		}
		this.OnChangeProbReduectionCounter();
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x0002FBE8 File Offset: 0x0002DDE8
	public void ResetProbReductionCounter()
	{
		this._probReductionCounter = 0;
		this.OnChangeProbReduectionCounter();
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x0015B700 File Offset: 0x00159900
	public int GetRedusedWorkProbByCounter()
	{ // <Mod>
		int num = 0;
		switch (this.metaInfo.GetRiskLevel())
		{
		case RiskLevel.ZAYIN:
		case RiskLevel.TETH:
		case RiskLevel.HE:
			num = 0;
			if (this.metaInfo.id == 100064L)
			{
				num = 6;
			}
			break;
		case RiskLevel.WAW:
			num = 4;
			break;
		case RiskLevel.ALEPH:
			num = 6;
			break;
		}
		if (num > 0 && MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.YESOD))
		{
			num -= 1;
		}
		return this.probReductionCounter * num;
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x0002FBF7 File Offset: 0x0002DDF7
	private void OnChangeProbReduectionCounter()
	{
		if (this._unit != null && this._unit.room != null)
		{
			this._unit.room.OnChangeProbReduction();
		}
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x0002FBF7 File Offset: 0x0002DDF7
	public void OnActivateAgentDeadPenalty()
	{
		if (this._unit != null && this._unit.room != null)
		{
			this._unit.room.OnChangeProbReduction();
		}
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x0015B760 File Offset: 0x00159960
	public void ActivateOverload(int level, float iOverloadTime = 60f, OverloadType overloadType = OverloadType.DEFAULT)
	{ // <Mod>
		ActivateOverload(level, iOverloadTime, overloadType, false);
	}

	// <Mod>
	public void ActivateOverload(int level, float iOverloadTime = 60f, OverloadType overloadType = OverloadType.DEFAULT, bool isNatural = false)
	{
		this.isOverloaded = true;
		isNaturalOverload = isNatural;
		this.overloadLevel = level;
		this.overloadTimer = 0f;
		this.overloadType = overloadType;
		int num = 0;
		num += SefiraAbilityValueInfo.tipherethOfficerAliveValues[SefiraManager.instance.GetOfficerAliveLevel(SefiraEnum.TIPERERTH1)];
		if (ResearchDataModel.instance.IsUpgradedAbility("upgrade_officer_bonuses"))
		{
			num *= 2;
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("officer_department_bonus"))
		{
			switch (sefira.GetOfficerAliveLevel())
			{
				case 1:
					num += 3;
					break;
				case 2:
					num += 7;
					break;
				case 3:
					num += 10;
					break;
			}
		}
		if (num >= 60)
		{
			num -= overloadReduction;
		}
		else
		{
			num -= overloadReduction * num / 60;
		}
		this.currentOverloadMaxTime = iOverloadTime + (float)num;
		if (overloadType != OverloadType.DEFAULT)
		{
			this.Unit.room.SetOverloadAlarmColor(overloadType);
		}
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x0015B7C4 File Offset: 0x001599C4
	public void ExplodeOverload()
	{ // <Mod> Capped energy loss at 50 PE; Secondary Qliphoth Overload
		Notice.instance.Send(NoticeName.OnIsolateOverloaded, new object[]
		{
			this,
			this.overloadType
		});
		if (this.overloadType != OverloadType.DEFAULT && this.ProbReductionValue > 0)
		{
			this.ProbReductionValue = 0;
		}
		this.isOverloaded = false;
		if (overloadType == OverloadType.DEFAULT)
		{
			if (qliphothCounter <= 0)
			{
				int num2 = 3 + (int)metaInfo.GetRiskLevel();
				if (num2 == 7) num2 = 9;
				CreatureOverloadManager.instance.AddSecondaryGague(num2);
			}
			else
			{
				CreatureOverloadManager.instance.AddSecondaryGague(1);
			}
		}
		int num = 5 * this.overloadLevel;
		if (num > 50)
		{
			num = 50;
		}
		EnergyModel.instance.SubEnergy((float)num);
		if (this.qliphothCounter > 0)
		{
			this.SetQliphothCounter(0);
		}
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x0002FC2A File Offset: 0x0002DE2A
	public void ClearProbReduction()
	{
		if (this.ProbReductionValue > 0)
		{
			this.ProbReductionValue = 0;
		}
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x0002FC3C File Offset: 0x0002DE3C
	public void CancelOverload()
	{
		Notice.instance.Send(NoticeName.OnIsolateOverloadCanceled, new object[]
		{
			this,
			this.overloadType
		});
		this.isOverloaded = false;
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x0015B83C File Offset: 0x00159A3C
	public List<WorkerModel> GetNearWorkerEncounted()
	{
		List<WorkerModel> list = new List<WorkerModel>();
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (passage != null)
		{
			foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit is WorkerModel)
				{
					WorkerModel workerModel = (WorkerModel)unit;
					if ((!(workerModel is AgentModel) || (workerModel as AgentModel).activated) && !workerModel.IsDead())
					{
						if (!list.Contains(workerModel))
						{
							list.Add(workerModel);
						}
						if (!this.encounteredWorker.Contains(workerModel))
						{
							this.encounteredWorker.Add(workerModel);
							workerModel.InitialEncounteredCreature(this);
							if (!workerModel.returnPanic)
							{
								workerModel.EncounterCreature(this);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x0002F7BF File Offset: 0x0002D9BF
	public virtual void OnEscapeUpdate()
	{
		this.script.UniqueEscape();
	}

	// Token: 0x06003474 RID: 13428 RVA: 0x00003FDD File Offset: 0x000021DD
	public void OnNotice(string notice, params object[] param)
	{
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x00003FDD File Offset: 0x000021DD
	public void ShowTextOutside(CreatureOutsideTextLayout layoutType, string textKey)
	{
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x0015B920 File Offset: 0x00159B20
	public string ShowNarrationText(string narrationKey, params string[] param)
	{
		string format_text;
		if (this.metaInfo.narrationTable.TryGetValue(narrationKey, out format_text))
		{
			string textFromFormatText = TextConverter.GetTextFromFormatText(format_text, this, param);
			this.narrationList.Add(textFromFormatText);
			Notice.instance.Send(NoticeName.AddNarrationLog, new object[]
			{
				textFromFormatText,
				this
			});
			return textFromFormatText;
		}
		return string.Empty;
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x0015B97C File Offset: 0x00159B7C
	public string ShowNarrationText(string narrationKey, bool roomEffect, params string[] param)
	{
		string format_text;
		if (this.metaInfo.narrationTable.TryGetValue(narrationKey, out format_text))
		{
			string textFromFormatText = TextConverter.GetTextFromFormatText(format_text, this, param);
			this.narrationList.Add(textFromFormatText);
			Notice.instance.Send(NoticeName.AddNarrationLog, new object[]
			{
				textFromFormatText,
				this
			});
			if (roomEffect)
			{
				this._unit.room.ProcessNarration(textFromFormatText);
			}
			return textFromFormatText;
		}
		return string.Empty;
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x0002FC6C File Offset: 0x0002DE6C
	public void ShowNarrationForcely(string desc)
	{
		this.narrationList.Add(desc);
		Notice.instance.Send(NoticeName.AddNarrationLog, new object[]
		{
			desc,
			this
		});
		this._unit.room.ProcessNarration(desc);
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x0015B9EC File Offset: 0x00159BEC
	public void ShowProcessNarrationText(string narrationKey, params string[] param)
	{
		string format_text;
		if (this.metaInfo.narrationTable.TryGetValue(narrationKey, out format_text))
		{
			string[] textFromFormatProcessText = TextConverter.GetTextFromFormatProcessText(format_text, this, param);
			string text = string.Empty;
			if (this.observeInfo.observeProgress < 1)
			{
				text = textFromFormatProcessText[0];
			}
			else if (1 <= this.observeInfo.observeProgress && this.observeInfo.observeProgress < 2)
			{
				text = textFromFormatProcessText[1];
			}
			else if (2 <= this.observeInfo.observeProgress && this.observeInfo.observeProgress < 3)
			{
				text = textFromFormatProcessText[2];
			}
			else if (this.observeInfo.observeProgress <= 4)
			{
				text = textFromFormatProcessText[3];
			}
			this.narrationList.Add(text);
			Notice.instance.Send(NoticeName.AddNarrationLog, new object[]
			{
				text,
				this
			});
		}
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x0015BAB4 File Offset: 0x00159CB4
	public virtual void EscapeWithoutIsolateRoom()
	{
		this.state = CreatureState.ESCAPE;
		this.baseMaxHp = this.metaInfo.maxHp;
		this.hp = (float)this.metaInfo.maxHp;
		PlayerModel.emergencyController.AddScore(this);
		this.SetFaction(FactionTypeList.StandardFaction.EscapedCreature);
		Notice.instance.Send(NoticeName.OnEscape, new object[]
		{
			this
		});
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x0015BB1C File Offset: 0x00159D1C
	public virtual void Escape()
	{
		if (this.state == CreatureState.WORKING && this.currentSkill != null)
		{
			this.currentSkill.CancelWork();
		}
		this.WorkParamInit();
		this.isOverloaded = false;
		this.state = CreatureState.ESCAPE;
		this.baseMaxHp = this.metaInfo.maxHp;
		this.hp = (float)this.metaInfo.maxHp;
		CreatureLayer.currentLayer.GetCreature(this.instanceId).room.OnEscape();
		this.sefira.OnEscapeCreature(this);
		PlayerModel.emergencyController.AddScore(this);
		AngelaConversation.instance.MakeMessage(AngelaMessageState.CREATURE_ESCAPE, new object[]
		{
			this
		});
		this.SetFaction(FactionTypeList.StandardFaction.EscapedCreature);
		Notice.instance.Send(NoticeName.OnEscape, new object[]
		{
			this
		});
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x0002FCA8 File Offset: 0x0002DEA8
	public void ResetAttackDelay()
	{
		this.remainAttackDelay = 4f;
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x0002FCB5 File Offset: 0x0002DEB5
	public void ResetAttackDelay(float value)
	{
		this.remainAttackDelay = value;
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x0015BBE8 File Offset: 0x00159DE8
	public override void TakeDamage(UnitModel actor, DamageInfo dmg)
	{ // <Mod>
		if (dmg.result.activated)
		{
			dmg.result.activated = false;
			dmg = dmg.Copy();
			dmg.result.activated = true;
		}
		else
		{
			dmg.result.activated = true;
			dmg = dmg.Copy();
		}
		DamageResult result = dmg.result;
		result.ResetValues(dmg);
		if (!this.script.CanTakeDamage(actor, dmg))
		{
			return;
		}
		base.TakeDamage(actor, dmg);
		if (base.Equipment.armor != null)
		{
			base.Equipment.armor.OnTakeDamage(actor, ref dmg);
		}
		if (base.Equipment.weapon != null)
		{
			base.Equipment.weapon.OnTakeDamage(actor, ref dmg);
		}
		base.Equipment.gifts.OnTakeDamage(actor, ref dmg);
		float num = 0f;
		float num2 = 1f;
		if (actor != null)
		{
			num2 = UnitModel.GetDmgMultiplierByEgoLevel(actor.GetAttackLevel(), this.GetDefenseLevel());
		}
		num2 *= this.GetBufDamageMultiplier(actor, dmg);
		num2 *= this.script.GetDamageFactor(actor, dmg);
		if (dmg.type == RwbpType.R || dmg.type == RwbpType.W)
		{
			num = dmg.GetDamageWithDefenseInfo(this.defense) * num2;
		}
		else if (dmg.type == RwbpType.B)
		{
			num = dmg.GetDamageWithDefenseInfo(this.defense) * num2;
		}
		else if (dmg.type == RwbpType.P)
		{
			num = dmg.GetDamageWithDefenseInfo(this.defense) * num2;
		}
		else if (dmg.type == RwbpType.N)
		{
			num = dmg.GetDamageWithDefenseInfo(this.defense) * num2;
		}
		result.beforeShield = num;
		float originalDamage = num;
		originalDamage /= num2;
		result.byResist = originalDamage;
		originalDamage /= defense.GetMultiplier(dmg.type);
		result.originDamage = originalDamage;
		if (this.hp > 0f)
		{
			result.resultDamage = num;
			result.hpDamage = num;
			if (num >= 0f)
			{
				float num3 = (float)((int)this.hp);
				this.hp -= num;
				float num4 = num3 - (float)((int)this.hp);
				result.resultNumber = (int)num4;
				this.MakeDamageEffect(dmg.type, num4, this.defense.GetDefenseType(dmg.type));
				if (dmg.type == RwbpType.R || dmg.type == RwbpType.B || num4 > 1f)
				{
					this.MakeSpatteredBlood();
				}
				this.script.OnTakeDamage(actor, dmg, num);
			}
			else if (num < 0f)
			{
				float num3 = (float)((int)this.hp);
				float num5 = -num;
				this.hp += num5;
				float num4 = num3 - (float)((int)this.hp);
				result.resultNumber = (int)num4;
				GameObject gameObject = Prefab.LoadPrefab("Effect/RecoverHP");
				gameObject.transform.SetParent(this.Unit.animTarget.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		}
		this.hp = Mathf.Clamp(this.hp, 0f, (float)this.maxHp);
		if (this.state == CreatureState.ESCAPE && this.hp <= 0f)
		{
			this.Suppressed();
		}
		script.OnTakeDamage_After(actor, dmg);
		if (base.Equipment.armor != null)
		{
			base.Equipment.armor.OnTakeDamage_After(num, dmg.type);
			base.Equipment.armor.OnTakeDamage_After(actor, dmg);
		}
		if (base.Equipment.weapon != null)
		{
			base.Equipment.weapon.OnTakeDamage_After(num, dmg.type);
			base.Equipment.weapon.OnTakeDamage_After(actor, dmg);
		}
		base.Equipment.gifts.OnTakeDamage_After(num, dmg.type);
		base.Equipment.gifts.OnTakeDamage_After(actor, dmg);
		foreach (UnitBuf unitBuf in _bufList)
		{
			unitBuf.OnTakeDamage_After(actor, dmg);
		}
		if (actor != null && actor is AgentModel)
		{
            Notice.instance.Send(NoticeName.CreatureDamagedByAgent, new object[]
            {
				this,
				num,
                dmg
            });
		}
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x0015BEC4 File Offset: 0x0015A0C4
	public virtual void Suppressed()
	{ // <Mod> Overtime Yesod Suppression; Suppress for energy
		Notice.instance.Send(NoticeName.OnCreatureSuppressed, new object[]
		{
			this
		});
		this.state = CreatureState.SUPPRESSED;
		this.script.OnSuppressed();
		if (ResearchDataModel.instance.IsUpgradedAbility("suppress_for_energy"))
		{
			EnergyModel.instance.AddEnergy(script.SuppressionEnergy);
		}
		this.commandQueue.Clear();
		base.ClearWorkerEncounting();
		if (this.roomNode != null)
		{
			try
			{
				if (sefiraOrigin == null)
				{
					sefiraOrigin = SefiraManager.instance.GetSefira(this.roomNode.GetAttachedPassage().GetSefiraName());
				}
				this.sefira = sefiraOrigin;
				this.sefiraNum = this.sefira.indexString;
			}
			catch (Exception)
			{
			}
		}
		if (this.sefira != null)
		{
			this.sefira.OnSuppressedCreature(this);
		}
		this.SetFaction(FactionTypeList.StandardFaction.IdleCreature);
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x0002FCBE File Offset: 0x0002DEBE
	public override void OnSuperArmorBreak()
	{
		this.script.OnSuperArmorBreak();
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x0002FCCB File Offset: 0x0002DECB
	public override bool IsAttackTargetable()
	{
		return this.script.IsAttackTargetable();
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x0002FCD8 File Offset: 0x0002DED8
	public override bool IsHostile(UnitModel target)
	{
		if (target is WorkerModel)
		{
			return true;
		}
		if (target is OrdealCreatureModel)
		{
			return true;
		}
		if (target is CreatureModel)
		{
			CreatureModel creatureModel = (CreatureModel)target;
			return (target as CreatureModel).script.IsAutoSuppressable();
		}
		return target is RabbitModel;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x0002FD17 File Offset: 0x0002DF17
	public override int GetRiskLevel()
	{
		return (int)(this.metaInfo.GetRiskLevel() + 1);
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0002FD26 File Offset: 0x0002DF26
	public override int GetAttackLevel()
	{
		if (this.metaInfo.id == 100064L)
		{
			return 5;
		}
		return this.GetRiskLevel();
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x0002FD43 File Offset: 0x0002DF43
	public override int GetDefenseLevel()
	{
		return this.GetRiskLevel();
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x0002FD4B File Offset: 0x0002DF4B
	public void SetFeelingStateInWork(CreatureFeelingState state)
	{ // <Mod>
		this.feelingState = state;
		float num = 1f;
		if (currentSkill != null && currentSkill._isOverloadedCreature && currentSkill._overloadType == OverloadType.GRIEF)
		{
			num = 0.5f;
		}
		this.feelingStateRemainTime = (float)this.metaInfo.workCooltime * num;
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x0002FD66 File Offset: 0x0002DF66
	public void ClearCommand()
	{
		this.commandQueue.Clear();
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x0002FD73 File Offset: 0x0002DF73
	public void MoveToNode(MapNode mapNode)
	{
		this.commandQueue.SetAgentCommand(CreatureCommand.MakeMove(mapNode));
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x0002FD86 File Offset: 0x0002DF86
	public void MoveToMovable(MovableObjectNode movable)
	{
		this.commandQueue.SetAgentCommand(CreatureCommand.MakeMove(movable));
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x0002FD99 File Offset: 0x0002DF99
	public virtual void PursueWorker(WorkerModel target)
	{
		if (!this.script.IsAttackTargetable())
		{
			return;
		}
		Debug.Log("Start pursue .. " + this.state);
		this.commandQueue.SetAgentCommand(CreatureCommand.MakePursue(target));
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x0002FDD4 File Offset: 0x0002DFD4
	public void AttackTarget(AttackCommand_creature cmd)
	{
		if (!this.IsAttackTargetable())
		{
			return;
		}
		this.commandQueue.SetAgentCommand(cmd);
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0002FDEB File Offset: 0x0002DFEB
	public virtual void PursueWorkerAlter(WorkerModel target, float damage)
	{
		if (!this.script.IsAttackTargetable())
		{
			return;
		}
		this.commandQueue.SetAgentCommand(CreatureCommand.MakePursueAlter(target, damage));
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x0002FE0D File Offset: 0x0002E00D
	public virtual void PursueWorkerAlter(WorkerModel target, RwbpType dmgType, int dmgMin, int dmgMax)
	{
		if (!this.script.IsAttackTargetable())
		{
			return;
		}
		this.commandQueue.SetAgentCommand(CreatureCommand.MakePursueAlter(target, dmgType, dmgMin, dmgMax));
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x0002FE32 File Offset: 0x0002E032
	public virtual void SetPursueWorkerCommand(WorkerModel target, CreatureCommand pursueCommand)
	{
		if (!(pursueCommand is PursueCreatureCommand) && !(pursueCommand is PursueCreatureCommandAlter) && !(pursueCommand is PursueCreatureCommandMultipleAttack))
		{
			return;
		}
		this.commandQueue.SetAgentCommand(pursueCommand);
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0015BF7C File Offset: 0x0015A17C
	public void FinishReturn()
	{ // <Mod> Overtime Yesod Suppression
		if (this.state == CreatureState.SUPPRESSED_RETURN)
		{
			this.state = CreatureState.WAIT;
			this.script.OnReturn();
			this.SetCurrentNode(this.roomNode);
			if (sefiraOrigin == null)
			{
				sefiraOrigin = SefiraManager.instance.GetSefira(this.roomNode.GetAttachedPassage().GetSefiraName());
			}
			this.sefira = sefiraOrigin;
			this.sefiraNum = this.sefira.indexString;
			CreatureLayer.currentLayer.GetCreature(this.instanceId).room.OnReturn();
			this.Unit.animTarget.ResetAnimator();
		}
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0015C010 File Offset: 0x0015A210
	public virtual void SendAnimMessage(string name)
	{
		if (this.Unit == null)
		{
			return;
		}
		CreatureAnimScript animTarget = this.Unit.animTarget;
		if (animTarget != null && animTarget.gameObject.activeInHierarchy)
		{
			animTarget.SendMessage(name, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x0015C058 File Offset: 0x0015A258
	public virtual void SetMoveAnimState(bool b)
	{
		if (this.Unit == null)
		{
			return;
		}
		CreatureAnimScript animTarget = this.Unit.animTarget;
		if (animTarget != null && animTarget.gameObject.activeInHierarchy)
		{
			if (b)
			{
				animTarget.Move();
				return;
			}
			animTarget.Stop();
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x0015C0A8 File Offset: 0x0015A2A8
	public CreatureAnimScript GetAnimScript()
	{
		CreatureUnit creature = CreatureLayer.currentLayer.GetCreature(this.instanceId);
		if (creature != null)
		{
			return creature.animTarget;
		}
		return null;
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x0002FE59 File Offset: 0x0002E059
	public override void InteractWithDoor(DoorObjectModel door)
	{
		base.InteractWithDoor(door);
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x00003FDD File Offset: 0x000021DD
	public override void OnStopMovableByShield(AgentModel shielder)
	{
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x00021CEF File Offset: 0x0001FEEF
	public float GetFeelingPercent()
	{
		return 0f;
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x0002FE62 File Offset: 0x0002E062
	public void AddObservationLevel()
	{
		this.observeInfo.observeProgress++;
		this.OnObservationLevelChanged();
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x0015C0D8 File Offset: 0x0015A2D8
	public void OnObservationLevelChanged()
	{
		if (this._unit != null)
		{
			this._unit.room.OnObservationLevelChanged();
		}
		this.metaInfo.specialSkillTable.OnCreatureNameRevealed(this);
		this.ConvertNarrationCodeIDToName();
		Notice.instance.Send(NoticeName.CreatureObserveLevelAdded, new object[]
		{
			this
		});
		if (this.script != null)
		{
			this.script.OnObserveLevelChanged();
			this.script.ObserveLevelChangeForSpecialSkillTip();
			if (this.Unit != null && this.Unit.room != null)
			{
				this.script.RoomCounterInit();
			}
		}
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x0015C180 File Offset: 0x0015A380
	public void AddCreatureSuccessCube(int count)
	{ // <Mod> increased max cube number to 9999
		int num = count;
		if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.YESOD))
		{
			int num2 = (int)Mathf.Max(1f, (float)num * (1f / 3f) + 0.5f);
			num += num2;
		}
		else if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.YESOD))
		{
			int num2 = (int)Mathf.Max(1f, (float)num * 0.25f + 0.5f);
			num += num2;
		}
		this.observeInfo.cubeNum += num;
		if (this.observeInfo.cubeNum > 9999)
		{
			this.observeInfo.cubeNum = 9999;
		}
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x0002FE7D File Offset: 0x0002E07D
	public void SubCreatureSuccessCube(int count)
	{
		this.observeInfo.cubeNum -= count;
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x000040E7 File Offset: 0x000022E7
	public int GetObservationConditionPoint()
	{
		return 0;
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x0015C1F0 File Offset: 0x0015A3F0
	public int GetNeedObservePoint()
	{
		switch (this.observeInfo.observeProgress)
		{
		case 0:
			return 10;
		case 1:
			return 40;
		case 2:
			return 90;
		case 3:
			return 170;
		case 4:
			return 290;
		default:
			return 99999999;
		}
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x0002FE92 File Offset: 0x0002E092
	public CreatureCommand GetCreatureCurrentCmd()
	{
		return this.commandQueue.GetCurrentCmd();
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0002FE9F File Offset: 0x0002E09F
	public void SetUnit(CreatureUnit unit)
	{
		this._unit = unit;
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x0015C240 File Offset: 0x0015A440
	public void ShowCreatureSpeech(string key)
	{
		string randomLyricByKey = CreatureLyrics.instnace.GetRandomLyricByKey(this.metadataId, key);
		if (randomLyricByKey == string.Empty)
		{
			Debug.Log(key + " not founded");
			return;
		}
		this._unit.speech.showSpeech(randomLyricByKey);
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x0015C290 File Offset: 0x0015A490
	public void ShowCreatureSpeech(string key, float time)
	{
		string randomLyricByKey = CreatureLyrics.instnace.GetRandomLyricByKey(this.metadataId, key);
		if (randomLyricByKey == string.Empty)
		{
			Debug.Log(key + " not founded");
			return;
		}
		this._unit.speech.showSpeech(randomLyricByKey, time);
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x0002FEA8 File Offset: 0x0002E0A8
	public void ShowCreatureSpeechDirect(string desc)
	{
		this._unit.speech.showSpeech(desc);
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x0002FEBB File Offset: 0x0002E0BB
	public void ShowCreatureSpeechDirect(string desc, float time)
	{
		this._unit.speech.showSpeech(desc, time);
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x0002FECF File Offset: 0x0002E0CF
	public void SpecialSkillActivated()
	{
		this.metaInfo.ActivatedSpecialSkill();
		this._unit.SpecialSkill();
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x0002FEE7 File Offset: 0x0002E0E7
	public void OnPhysicsAttackInWork()
	{
		Notice.instance.Send(NoticeName.MakeName(NoticeName.OnCreaturePhysicsAttack, new string[]
		{
			this.instanceId.ToString()
		}), new object[0]);
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x0002FF17 File Offset: 0x0002E117
	public void OnMentalAttackInWork()
	{
		Notice.instance.Send(NoticeName.MakeName(NoticeName.OnCreatureMentalAttack, new string[]
		{
			this.instanceId.ToString()
		}), new object[0]);
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x0002FF47 File Offset: 0x0002E147
	public void OnComplexAttackInWork()
	{
		Notice.instance.Send(NoticeName.MakeName(NoticeName.OnCreatureComplexAttack, new string[]
		{
			this.instanceId.ToString()
		}), new object[0]);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x0015C2E0 File Offset: 0x0015A4E0
	public void ConvertNarrationCodeIDToName()
	{
		for (int i = 0; i < this.narrationList.Count; i++)
		{
			this.narrationList[i] = this.ConvertCodeIDToName(this.narrationList[i]);
		}
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x0002FF77 File Offset: 0x0002E177
	public string ConvertCodeIDToName(string origin)
	{
		origin.Replace("$0", this.metaInfo.name);
		return origin.Replace(this.metaInfo.codeId, this.metaInfo.collectionName);
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x0002FFAC File Offset: 0x0002E1AC
	public string ConvertCodeIDToNameForcely(string origin)
	{
		return origin.Replace(this.metaInfo.codeId, this.metaInfo.collectionName);
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x0015C324 File Offset: 0x0015A524
	public void OnRevealSpecialSkillTip(string key, params object[] param)
	{
		CreatureSpecialSkillDesc desc = this.metaInfo.specialSkillTable.GetDesc(key);
		if (desc != null)
		{
			desc.ActivateDesc(this, param);
		}
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x0015C350 File Offset: 0x0015A550
	public void MakeSpatteredBlood()
	{
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (passage != null)
		{
			passage.AttachBloodObjectAlter(this.GetCurrentViewPosition().x);
		}
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x0015C380 File Offset: 0x0015A580
	public float GetWorkSuccessProb(AgentModel actor, SkillTypeInfo skill)
	{
		float result = 0f;
		switch (skill.rwbpType)
		{
		case RwbpType.N:
			result = 1f;
			break;
		case RwbpType.R:
			result = this.metaInfo.workProbTable.GetWorkProb(skill.rwbpType, actor.fortitudeLevel);
			break;
		case RwbpType.W:
			result = this.metaInfo.workProbTable.GetWorkProb(skill.rwbpType, actor.prudenceLevel);
			break;
		case RwbpType.B:
			result = this.metaInfo.workProbTable.GetWorkProb(skill.rwbpType, actor.temperanceLevel);
			break;
		case RwbpType.P:
			result = this.metaInfo.workProbTable.GetWorkProb(skill.rwbpType, actor.justiceLevel);
			break;
		}
		return result;
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x0002FFCA File Offset: 0x0002E1CA
	public float GetCubeSpeed()
	{
		return this.metaInfo.cubeSpeed;
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x0015C43C File Offset: 0x0015A63C
	public bool isPrevMaxObserve()
	{
		int observeProgress = this.observeInfo.observeProgress;
		return this.metaInfo.MaxObserveLevel - 1 == observeProgress;
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x0002FFD7 File Offset: 0x0002E1D7
	public void ChangePos(CreatureModel changed)
	{
		SefiraIsolate sefiraIsolate = this.isolateRoomData;
		SefiraIsolate sefiraIsolate2 = changed.isolateRoomData;
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x0015C468 File Offset: 0x0015A668
	public long AddChildCreatureModel()
	{
		long num = this.childInst;
		this.childInst += 1L;
		ChildCreatureModel childCreatureModel = new ChildCreatureModel(num);
		childCreatureModel.SetParent(this);
		childCreatureModel.GetMovableNode().SetActive(true);
		this.childs.Add(childCreatureModel);
		return num;
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0015C4B0 File Offset: 0x0015A6B0
	public long AddChildCreatureModel(string childScriptBase, string childPrefab)
	{
		long num = this.childInst;
		this.childInst += 1L;
		ChildCreatureModel childCreatureModel = new ChildCreatureModel(num);
		childCreatureModel.SetParent(this, childScriptBase, childPrefab);
		childCreatureModel.GetMovableNode().SetActive(true);
		this.childs.Add(childCreatureModel);
		return num;
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x0015C4FC File Offset: 0x0015A6FC
	public ChildCreatureModel GetChildCreatureModel(long instID)
	{
		ChildCreatureModel result = null;
		foreach (ChildCreatureModel childCreatureModel in this.childs)
		{
			if (childCreatureModel.instanceId == instID)
			{
				result = childCreatureModel;
				break;
			}
		}
		return result;
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x0002FFE7 File Offset: 0x0002E1E7
	public List<ChildCreatureModel> GetAliveChilds()
	{
		return this.childs;
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x0002FFEF File Offset: 0x0002E1EF
	public void DeleteChildCreatureModel(long instId)
	{
		this.GetChildCreatureModel(instId).OnDeleted();
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x0002FE92 File Offset: 0x0002E092
	public virtual CreatureCommand GetCurrentCommand()
	{
		return this.commandQueue.GetCurrentCmd();
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x0002FFFD File Offset: 0x0002E1FD
	public override void SetFaction(FactionTypeInfo type)
	{
		if (this.script.HasUniqueFaction())
		{
			return;
		}
		base.SetFaction(type);
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x00030014 File Offset: 0x0002E214
	public override void SetFaction(string factionCode)
	{
		if (this.script.HasUniqueFaction())
		{
			return;
		}
		base.SetFaction(factionCode);
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x0003002B File Offset: 0x0002E22B
	public void SetActivatedState(bool state)
	{
		this._activated = state;
		bool activated = this._activated;
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x0003003B File Offset: 0x0002E23B
	public void ForcelyCancelSuppress()
	{
		Notice.instance.Send(NoticeName.CreatureSuppressCancel, new object[]
		{
			this
		});
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x00030056 File Offset: 0x0002E256
	public void SetDefenseId(string id)
	{
		this.defenseId = id;
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x0003005F File Offset: 0x0002E25F
	public void ResetQliphothCounter()
	{
		this.script.ResetQliphothCounter();
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x0003006C File Offset: 0x0002E26C
	public void SubQliphothCounter()
	{
		if (this._qliphothCounter <= 0)
		{
			return;
		}
		this._qliphothCounter--;
		if (this._qliphothCounter < 0)
		{
			this._qliphothCounter = 0;
		}
		this.script.ReducedQliphothCounter();
		this.UpdateQliphothCounter();
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x0015C558 File Offset: 0x0015A758
	public void AddQliphothCounter()
	{
		if (this._qliphothCounter >= this.script.GetQliphothCounterMax())
		{
			return;
		}
		this._qliphothCounter++;
		if (this._qliphothCounter > this.script.GetQliphothCounterMax())
		{
			this._qliphothCounter = this.script.GetQliphothCounterMax();
		}
		this.script.AddedQliphothCounter();
		this.UpdateQliphothCounter();
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x0015C5BC File Offset: 0x0015A7BC
	public void SetQliphothCounter(int value)
	{
		if (this.script.GetQliphothCounterMax() <= 0)
		{
			return;
		}
		if (value < 0)
		{
			value = 0;
		}
		else if (value > this.script.GetQliphothCounterMax())
		{
			value = this.script.GetQliphothCounterMax();
		}
		int num = Math.Abs(this._qliphothCounter - value);
		if (this._qliphothCounter > value)
		{
			for (int i = 0; i < num; i++)
			{
				if (this._qliphothCounter <= 0)
				{
					this._qliphothCounter = 0;
					return;
				}
				this.SubQliphothCounter();
			}
			return;
		}
		if (this._qliphothCounter < value)
		{
			for (int j = 0; j < num; j++)
			{
				if (this._qliphothCounter >= this.script.GetQliphothCounterMax())
				{
					this._qliphothCounter = this.script.GetQliphothCounterMax();
					return;
				}
				this.AddQliphothCounter();
			}
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0015C678 File Offset: 0x0015A878
	private void UpdateQliphothCounter()
	{
		if (this.script.GetQliphothCounterMax() <= 0)
		{
			return;
		}
		if (this.qliphothCounter == 0)
		{
			this.isOverloaded = false;
			this.script.ActivateQliphothCounter();
			Debug.Log(this.script.model.GetUnitName() + "'s qliphothCounter activated");
		}
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x000300A7 File Offset: 0x0002E2A7
	public int GetCurrentCumlatvieCube()
	{
		return this.observeInfo.cubeNum;
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x000300B4 File Offset: 0x0002E2B4
	public bool CanPurhcase(int cost)
	{
		return this.observeInfo.cubeNum >= cost;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x000300C7 File Offset: 0x0002E2C7
	public bool TransactionCube(int cost)
	{
		if (this.observeInfo.cubeNum < cost)
		{
			return false;
		}
		this.observeInfo.cubeNum -= cost;
		return true;
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x000300ED File Offset: 0x0002E2ED
	public int GetObservationLevel()
	{
		return this.observeInfo.GetObservationLevel();
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0015C6D0 File Offset: 0x0015A8D0
	public void OnFixedUpdateInKitEquip(AgentModel actor)
	{
		bool flag = false;
		int observeCost = this.observeInfo.GetObserveCost(CreatureModel.regionName[0]);
		if (this.observeInfo.totalKitUseTime >= (float)observeCost)
		{
			flag = true;
		}
		this.observeInfo.totalKitUseTime += Time.deltaTime;
		this.script.kitEvent.OnFixedUpdateInKitEquip(actor);
		if (!flag && this.observeInfo.totalKitUseTime >= (float)observeCost)
		{
			this.OnObservationLevelChanged();
		}
		this._unit.room.SetKitObserveLevel();
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x000300FA File Offset: 0x0002E2FA
	public int GetObserveBonusProb()
	{
		if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
		{
			return 0;
		}
		return this.metaInfo.observeBonus.GetProbBonus(this.GetObservationLevel());
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x00030121 File Offset: 0x0002E321
	public int GetObserveBonusSpeed()
	{
		if (GlobalGameManager.instance.gameMode == GameMode.UNLIMIT_MODE)
		{
			return 0;
		}
		return this.metaInfo.observeBonus.GetSpeedBonus(this.GetObservationLevel());
	}

	//> <Mod>
	public void RecoverHP(float amount)
	{
		this.hp += amount;
		this.hp = ((this.hp <= (float)this.maxHp) ? this.hp : ((float)this.maxHp));
	}

	public bool isTranquilized
	{
		get
		{
			return _isTranquilized;
		}
	}

	public bool TryTranquilize(float _time)
	{
		if (Unit.room == null || !script.IsTranqable()) return false;
		if (isTranquilized)
		{
			tranqRemainTime += _time;
			return true;
		}
		_isTranquilized = true;
		tranqRemainTime = _time;
		OnTranquilized();
		return true;
	}

	public void OnTranquilized()
	{
		script.OnTranquilized();
	}

	public void EndTranq()
	{
		_isTranquilized = false;
		script.OnTranqEnd();
	}
	//<

	// Token: 0x060034C6 RID: 13510 RVA: 0x0015C754 File Offset: 0x0015A954
	static CreatureModel()
	{
	}

	// Token: 0x040030EC RID: 12524
	public static string[] regionName = new string[]
	{
		"stat",
		"defense",
		"work_r",
		"work_w",
		"work_b",
		"work_p"
	};

	// Token: 0x040030ED RID: 12525
	public static string[] careTakingRegion = new string[]
	{
		"care_0",
		"care_1",
		"care_2",
		"care_3",
		"care_4",
		"care_5",
		"care_6"
	};

	// Token: 0x040030EE RID: 12526
	public CreatureCommandQueue commandQueue;

	// Token: 0x040030EF RID: 12527
	public bool canBeSuppressed = true;

	// Token: 0x040030F0 RID: 12528
	public CreatureTypeInfo metaInfo;

	// Token: 0x040030F1 RID: 12529
	public long metadataId;

	// Token: 0x040030F2 RID: 12530
	public CreatureBase script = new CreatureBase();

	// Token: 0x040030F3 RID: 12531
	public CreatureObserveInfoModel observeInfo;

	// Token: 0x040030F4 RID: 12532
	public List<string> narrationList;

	// Token: 0x040030F5 RID: 12533
	protected CreatureState _state;

	// Token: 0x040030F6 RID: 12534
	public CreatureFeelingState feelingState;

	// Token: 0x040030F7 RID: 12535
	public float feelingStateRemainTime;

	// Token: 0x040030F8 RID: 12536
	private UseSkill _currentSkill;

	// Token: 0x040030F9 RID: 12537
	public float suppressReturnTimer;

	// Token: 0x040030FA RID: 12538
	public string sefiraNum;

	// Token: 0x040030FB RID: 12539
	public Sefira sefira;

	// Token: 0x040030FC RID: 12540
	public Sefira sefiraOrigin;

	// Token: 0x040030FD RID: 12541
	public IsolatePos specialSkillPos = IsolatePos.UP;

	// Token: 0x040030FE RID: 12542
	public Vector2 basePosition;

	// Token: 0x040030FF RID: 12543
	public string entryNodeId;

	// Token: 0x04003100 RID: 12544
	public MapNode entryNode;

	// Token: 0x04003101 RID: 12545
	private MapNode workspaceNode;

	// Token: 0x04003102 RID: 12546
	public MapNode roomNode;

	// Token: 0x04003103 RID: 12547
	private MapNode customNode;

	// Token: 0x04003104 RID: 12548
	public SefiraIsolate isolateRoomData;

	// Token: 0x04003105 RID: 12549
	private string defenseId = "1";

	// Token: 0x04003106 RID: 12550
	protected CreatureUnit _unit;

	// Token: 0x04003108 RID: 12552
	public float movementScale = 1f;

	// Token: 0x04003109 RID: 12553
	public int workCount;

	// Token: 0x0400310A RID: 12554
	private int _qliphothCounter;

	// Token: 0x0400310B RID: 12555
	private int _probReductionCounter;

	// Token: 0x0400310C RID: 12556
	private int _probReductionValue;

	// Token: 0x0400310D RID: 12557
	public int overloadLevel;

	// Token: 0x0400310E RID: 12558
	public bool isOverloaded;

	// Token: 0x0400310F RID: 12559
	public OverloadType overloadType;

	// Token: 0x04003110 RID: 12560
	public float currentOverloadMaxTime = 1f;

	// Token: 0x04003111 RID: 12561
	public float overloadTimer;

	// Token: 0x04003112 RID: 12562
	public const float overloadTime = 60f;

	// Token: 0x04003113 RID: 12563
	[NonSerialized]
	public AgentModel kitEquipOwner;

	// Token: 0x04003114 RID: 12564
	private List<ChildCreatureModel> childs = new List<ChildCreatureModel>();

	// Token: 0x04003115 RID: 12565
	private long childInst = 1L;

	// Token: 0x04003116 RID: 12566
	private bool _activated = true;

	// Token: 0x04003117 RID: 12567
	private static int counter = 0;

	//> <Mod>
	public int overloadReduction
	{
		get
		{
			return _overloadReduction;
		}
		set
		{
			_overloadReduction = value;
			this.OnChangeProbReduectionCounter();
		}
	}

	private int _overloadReduction;


	public bool isNaturalOverload
	{
		get
		{
			return _isNaturalOverload;
		}
		set
		{
			_isNaturalOverload = value;
		}
	}

	private bool _isNaturalOverload;

	private bool _isTranquilized;

	private float tranqRemainTime;
}
