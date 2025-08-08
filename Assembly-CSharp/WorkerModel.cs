/*
public override void TakeDamageWithoutEffect(UnitModel actor, DamageInfo dmg) // Chesed research
public override void TakeDamage(UnitModel actor, DamageInfo dmg) // Chesed research; Notice
public virtual void RecoverHp(float amount) // Changed from void to float
+public virtual float RecoverHpv2(float amount) // Changed from void to float
public virtual void RecoverMental(float amount) // Changed from void to float
+public virtual float RecoverMentalv2(float amount) // Changed from void to float
+public UnitMouseEventTarget GetUnitMouseTarget() // 
+public void SetUnitMouseTarget(UnitMouseEventTarget target) // 
+private UnitMouseEventTarget _unitMouseTarget // 
*/
using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using WorkerSprite;

// Token: 0x02000BDD RID: 3037
public class WorkerModel : UnitModel, IObserver, IMouseCommandTargetModel, IMouseCommandTargetModelExt // public class WorkerModel : UnitModel, IObserver, IMouseCommandTargetModel
{
	// Token: 0x06005B9F RID: 23455 RVA: 0x00207AB8 File Offset: 0x00205CB8
	public WorkerModel()
	{
	}

	// Token: 0x06005BA0 RID: 23456 RVA: 0x00207B08 File Offset: 0x00205D08
	public WorkerModel(int instanceId, string area)
	{
		this.currentSefira = area;
		this.currentSefira = area;
		this.instanceId = (long)instanceId;
		this.movableNode = new MovableObjectNode(this);
		this.movableNode.SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(area));
		this._panicData = PanicDataList.instance.GetPanicData(this);
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.Worker);
		Debug.Log(this.factionTypeInfo.name);
		this.spriteData = new WorkerSprite.WorkerSprite();
	}

	// Token: 0x06005BA1 RID: 23457 RVA: 0x00207BD0 File Offset: 0x00205DD0
	public WorkerModel(int instanceId, Sefira area)
	{
		this.currentSefira = area.indexString;
		this.instanceId = (long)instanceId;
		this.movableNode = new MovableObjectNode(this);
		this.movableNode.SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(area.indexString));
		this.currentSefira = area.indexString;
		this._panicData = PanicDataList.instance.GetPanicData(this);
		this.factionTypeInfo = FactionTypeList.instance.GetFaction(FactionTypeList.StandardFaction.Worker);
		Debug.Log(this.factionTypeInfo.name);
		this.spriteData = new WorkerSprite.WorkerSprite();
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x00013F31 File Offset: 0x00012131
	public virtual int fortitudeLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x00013F31 File Offset: 0x00012131
	public virtual int prudenceLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x00013F31 File Offset: 0x00012131
	public virtual int temperanceLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x06005BA5 RID: 23461 RVA: 0x00013F31 File Offset: 0x00012131
	public virtual int justiceLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x06005BA6 RID: 23462 RVA: 0x000491CF File Offset: 0x000473CF
	// (set) Token: 0x06005BA7 RID: 23463 RVA: 0x000491D7 File Offset: 0x000473D7
	public string currentSefira
	{
		get
		{
			return this._currentSefira;
		}
		set
		{
			this._currentSefira = value;
			this.currentSefiraEnum = SefiraName.GetSefiraEnum(this._currentSefira);
		}
	}

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x000491F1 File Offset: 0x000473F1
	// (set) Token: 0x06005BA9 RID: 23465 RVA: 0x000491F9 File Offset: 0x000473F9
	public PanicAction CurrentPanicAction
	{
		get
		{
			return this.currentPanicAction;
		}
		set
		{
			this.currentPanicAction = value;
			if (value != null)
			{
				value.Init();
			}
		}
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06005BAA RID: 23466 RVA: 0x00049213 File Offset: 0x00047413
	// (set) Token: 0x06005BAB RID: 23467 RVA: 0x0004921B File Offset: 0x0004741B
	public CreatureModel recentlyAttacked
	{
		get
		{
			return this._recentlyAttacked;
		}
		set
		{
			this._recentlyAttacked = value;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06005BAC RID: 23468 RVA: 0x00049224 File Offset: 0x00047424
	public WorkerModel attackTargetWorker
	{
		get
		{
			return this._attackTargetWorker;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06005BAD RID: 23469 RVA: 0x0004922C File Offset: 0x0004742C
	// (set) Token: 0x06005BAE RID: 23470 RVA: 0x00049234 File Offset: 0x00047434
	public bool specialDeadScene
	{
		get
		{
			return this._specialDeadScene;
		}
		set
		{
			this._specialDeadScene = value;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06005BAF RID: 23471 RVA: 0x0004923D File Offset: 0x0004743D
	public Animator workerAnimator
	{
		get
		{
			return this.GetWorkerUnit().animEventHandler.GetComponent<Animator>();
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x0004924F File Offset: 0x0004744F
	public virtual PanicData panicData
	{
		get
		{
			this._panicData = PanicDataList.instance.GetPanicData(this);
			return this._panicData;
		}
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x00049268 File Offset: 0x00047468
	public DeadType DeadType
	{
		get
		{
			return this._deadType;
		}
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void OnStageStart()
	{
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void OnStageEnd()
	{
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void OnStageRelease()
	{
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ChangeHairSprite(SpriteInfo spriteInfo)
	{
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x00207CA8 File Offset: 0x00205EA8
	public virtual Dictionary<string, object> GetSaveData()
	{
		return new Dictionary<string, object>
		{
			{
				"instanceId",
				this.instanceId
			},
			{
				"currentSefira",
				this.currentSefira
			},
			{
				"name",
				this.name
			},
			{
				"baseMovement",
				this.baseMovement
			},
			{
				"baseMaxHp",
				this.baseMaxHp
			},
			{
				"baseMaxMental",
				this.baseMaxMental
			},
			{
				"sefira",
				this.currentSefira
			}
		};
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x00200178 File Offset: 0x001FE378
	public static bool TryGetValue<T>(Dictionary<string, object> dic, string name, ref T field)
	{
		object obj;
		if (dic.TryGetValue(name, out obj) && obj is T)
		{
			field = (T)((object)obj);
			return true;
		}
		return false;
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x00207D48 File Offset: 0x00205F48
	public virtual void LoadData(Dictionary<string, object> dic)
	{
		WorkerModel.TryGetValue<long>(dic, "instanceId", ref this.instanceId);
		WorkerModel.TryGetValue<string>(dic, "name", ref this.name);
		WorkerModel.TryGetValue<float>(dic, "baseMovement", ref this.baseMovement);
		WorkerModel.TryGetValue<int>(dic, "baseMaxHp", ref this.baseMaxHp);
		WorkerModel.TryGetValue<int>(dic, "baseMaxMental", ref this.baseMaxMental);
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x00049270 File Offset: 0x00047470
	public virtual void OnFixedUpdate()
	{
		if (this.haltUpdate)
		{
			return;
		}
		this.ProcessAction();
		this.movableNode.ProcessMoveNode(this.movement * this.movementMul);
	}

	// Token: 0x06005BBA RID: 23482 RVA: 0x0004929C File Offset: 0x0004749C
	public virtual void HaltUpdate()
	{
		this.haltUpdate = true;
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x000492A5 File Offset: 0x000474A5
	public virtual void ReleaseUpdate()
	{
		Debug.Log(this.name + " halt release");
		this.haltUpdate = false;
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ResetSprite()
	{
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ProcessAction()
	{
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x0002FFAD File Offset: 0x0002E1AD
	public virtual MapNode GetCurrentNode()
	{
		return this.movableNode.GetCurrentNode();
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x0002FF9F File Offset: 0x0002E19F
	public virtual void SetCurrentNode(MapNode node)
	{
		this.movableNode.SetCurrentNode(node);
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x0002FFBA File Offset: 0x0002E1BA
	public virtual MapEdge GetCurrentEdge()
	{
		return this.movableNode.GetCurrentEdge();
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x000492C3 File Offset: 0x000474C3
	public virtual EdgeDirection GetEdgeDirection()
	{
		return this.movableNode.GetEdgeDirection();
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x000492D0 File Offset: 0x000474D0
	public WorkerCommand GetCurrentCommand()
	{
		return this.commandQueue.GetCurrentCmd();
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x000492DD File Offset: 0x000474DD
	public virtual void ReturnToSefira()
	{
		this.SetCurrentNode(MapGraph.instance.GetSepiraNodeByRandom(this.currentSefira));
	}

	// Token: 0x06005BC4 RID: 23492 RVA: 0x000492F5 File Offset: 0x000474F5
	public virtual void StopAction()
	{
		this.commandQueue.Clear();
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x00049302 File Offset: 0x00047502
	public void MoveToNode(MapNode targetNode)
	{
		this.MoveToNode(targetNode, true);
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x0004930C File Offset: 0x0004750C
	public void MoveToNode(MapNode targetNode, bool resetCommand)
	{
		this.lastestMoveTarget = targetNode;
		if (resetCommand)
		{
			this.commandQueue.SetAgentCommand(WorkerCommand.MakeMove(targetNode));
		}
		else
		{
			this.commandQueue.AddFirst(WorkerCommand.MakeMove(targetNode));
		}
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x00049342 File Offset: 0x00047542
	public void MoveToMovable(MovableObjectNode targetMovable)
	{
		this.MoveToMovable(targetMovable, true);
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x0004934C File Offset: 0x0004754C
	public void MoveFromNullPassage()
	{
		this.commandQueue.SetAgentCommand(new MoveFromNullPassageCommand());
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x0004935E File Offset: 0x0004755E
	public void MoveToMovable(MovableObjectNode targetMovable, bool resetCommand)
	{
		if (resetCommand)
		{
			this.commandQueue.SetAgentCommand(WorkerCommand.MakeMove(targetMovable));
		}
		else
		{
			this.commandQueue.AddFirst(WorkerCommand.MakeMove(targetMovable));
		}
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x0004938D File Offset: 0x0004758D
	public void MoveToNode(string targetNodeID)
	{
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeMove(MapGraph.instance.GetNodeById(targetNodeID)));
	}

	// Token: 0x06005BCB RID: 23499 RVA: 0x0002F39E File Offset: 0x0002D59E
	public void FollowMovable(MovableObjectNode targetNode)
	{
		this.commandQueue.SetAgentCommand(WorkerCommand.MakeFollowAgent(targetNode));
	}

	// Token: 0x06005BCC RID: 23500 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ClearUnconCommand()
	{
	}

	// Token: 0x06005BCD RID: 23501 RVA: 0x000493AA File Offset: 0x000475AA
	public virtual bool IsDead()
	{
		return this._isDead || (!this.invincible && this.hp <= 0f);
	}

	// Token: 0x06005BCE RID: 23502 RVA: 0x00207DB0 File Offset: 0x00205FB0
	public MapNode GetConnectedNode()
	{
		MapNode currentNode = this.movableNode.GetCurrentNode();
		MapNode result = null;
		if (currentNode == null)
		{
			return null;
		}
		foreach (MapEdge mapEdge in currentNode.GetEdges())
		{
			if (!(mapEdge.type == "door"))
			{
				result = mapEdge.ConnectedNode(currentNode);
			}
		}
		return result;
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x00207E1C File Offset: 0x0020601C
	public override void TakeDamageWithoutEffect(UnitModel actor, DamageInfo dmg)
	{ // <Mod> Chesed research; Damage Result
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
		if (this.invincible && !base.HasUnitBuf(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM))
		{
			return;
		}
		if (this.IsDead())
		{
			return;
		}
		base.TakeDamageWithoutEffect(actor, dmg);
		if (base.Equipment.armor != null)
		{
			base.Equipment.armor.OnTakeDamage(actor, ref dmg);
		}
		if (base.Equipment.weapon != null)
		{
			base.Equipment.weapon.OnTakeDamage(actor, ref dmg);
		}
		base.Equipment.gifts.OnTakeDamage(actor, ref dmg);
		float num = dmg.GetDamage();
		result.beforeShield = num;
		result.byResist = num;
		result.originDamage = num;
		if (dmg.type == RwbpType.R)
		{
			result.resultDamage = num;
			result.hpDamage = num;
			float hp = this.hp;
			this.hp -= num;
			float num2 = hp - this.hp;
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num);
			}
		}
		else if (dmg.type == RwbpType.W)
		{
			result.resultDamage = num;
			float num3 = this.mental;
			if (this.CannotControll() && this.unconAction is Uncontrollable_RedShoes)
			{
				result.hpDamage = num;
				num3 = this.hp;
				this.hp -= num;
				float num4 = this.hp - num3;
				if (base.Equipment.kitCreature != null)
				{
					base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num);
				}
			}
			else if (this.IsPanic() && !(this is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
			{
				result.spDamage = -num;
				this.mental += num;
				float num4 = this.mental - num3;
			}
			else
			{
				result.spDamage = num;
				this.mental -= num;
				float num4 = num3 - this.mental;
				if (base.Equipment.kitCreature != null)
				{
					base.Equipment.kitCreature.script.kitEvent.OnTakeDamageMental(this, num);
				}
			}
		}
		else if (dmg.type == RwbpType.B)
		{
			result.resultDamage = num;
			result.hpDamage = num;
			float hp2 = this.hp;
			this.hp -= num;
			if (this.IsPanic() && !(this is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
			{
				result.spDamage = -num;
				this.mental += num;
			}
			else
			{
				result.spDamage = num;
				this.mental -= num;
				if (base.Equipment.kitCreature != null)
				{
					base.Equipment.kitCreature.script.kitEvent.OnTakeDamageMental(this, num);
				}
			}
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num);
			}
		}
		else if (dmg.type == RwbpType.P)
		{
			float num5 = num / 100f;
			num = (float)this.maxHp * num5;
			result.scaling = maxHp / 100f;
			result.resultDamage = num;
			result.hpDamage = num;
			this.hp -= num;
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, (float)this.maxHp * num5);
			}
		}
		else if (dmg.type == RwbpType.N)
		{
			result.resultDamage = num;
			result.hpDamage = num;
			float hp3 = this.hp;
			this.hp -= num;
			float num6 = hp3 - this.hp;
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num);
			}
		}
		if (num > 0f && this.defense.GetMultiplier(dmg.type) > 1f)
		{
			base.AddUnitBuf(new UnderAttackBuf());
		}
		if (this.IsPanic() && (this.CurrentPanicAction is PanicRoaming || this.CurrentPanicAction is PanicOpenIsolate) && actor is AgentModel && !((AgentModel)actor).CannotControll() && !((AgentModel)actor).IsPanic())
		{
			base.AddUnitBuf(new PanicUnderAttackBuf());
		}
		this.hp = Mathf.Clamp(this.hp, 0f, (float)this.maxHp);
		this.mental = Mathf.Clamp(this.mental, 0f, (float)this.maxMental);
		if (this.invincible)
		{
			return;
		}
		if (this.hp <= 0f)
		{
			float revivalChance = 0f;
			if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHESED))
			{
				revivalChance = 1f;
			}
			else
			{
				if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHESED))
				{
					revivalChance += 0.25f;
				}
				if (ResearchDataModel.instance.IsUpgradedAbility("resist_death_panic"))
				{
					revivalChance += 0.15f;
				}
			}
			if (UnityEngine.Random.value < revivalChance && this.isRealWorker)
			{
				this._revivalHp = true;
			}
			else
			{
				this._revivalHp = false;
			}
			if (!this._revivaledHp && this._revivalHp && this.DeadType != DeadType.EXECUTION)
			{
				this.hp = 1f;
				this._revivaledHp = true;
				this._revivalHp = false;
				this.RecoverHP((float)this.maxHp / 2f);
			}
			else
			{
				if (dmg.specialDeadSceneEnable)
				{
					this.SetSpecialDeadScene(dmg.specialDeadSceneName);
				}
				this.OnDie();
			}
		}
		else if (this.IsPanic())
		{
			if (this.mental >= (float)this.maxMental)
			{
				this.StopPanic();
			}
		}
		else
		{
			if (this.mental <= 0f)
			{
				float revivalChance = 0f;
				if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHESED))
				{
					revivalChance = 1f;
				}
				else
				{
					if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHESED))
					{
						revivalChance += 0.25f;
					}
					if (ResearchDataModel.instance.IsUpgradedAbility("resist_death_panic"))
					{
						revivalChance += 0.15f;
					}
				}
				if (UnityEngine.Random.value <= revivalChance && this.isRealWorker)
				{
					this._revivalMental = true;
				}
				else
				{
					this._revivalMental = false;
				}
				if (!this._revivaledMental && this._revivalMental)
				{
					this._revivaledMental = true;
					this._revivalMental = false;
					this.RecoverMental((float)this.maxMental / 2f);
				}
				else
				{
					this.Panic();
				}
			}
		}
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
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x002084A0 File Offset: 0x002066A0
	public override void TakeDamage(UnitModel actor, DamageInfo dmg)
	{ // <Mod> Chesed research; send notice; Damage Result
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
		if (actor is CreatureModel)
		{
			float mult = 1f;
			mult = actor.GetDamageFactorByEquipment();
			dmg.min *= mult;
			dmg.max *= mult;
		}
		DamageResult result = dmg.result;
		result.ResetValues(dmg);
		if (this.invincible && !base.HasUnitBuf(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM))
		{
			return;
		}
		if (this.IsDead())
		{
			return;
		}
		float originalMin = dmg.min;
		float originalMax = dmg.max;
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
		float num = 1f;
		if (actor != null)
		{
			num = UnitModel.GetDmgMultiplierByEgoLevel(actor.GetAttackLevel(), this.GetDefenseLevel());
		}
		num *= this.GetBufDamageMultiplier(actor, dmg);
		float num2 = dmg.GetDamageWithDefenseInfo(this.defense) * num;
		result.beforeShield = num2;
		float originalDamage = num2;
		originalDamage /= num;
		result.byResist = originalDamage;
		originalDamage /= defense.GetMultiplier(dmg.type);
		result.originDamage = originalDamage;
		if (actor is CreatureModel)
		{
            Notice.instance.Send(NoticeName.CreatureHitWorker, new object[]
            {
				actor,
				originalDamage,
                dmg,
				this
            });
		}
		if (dmg.type == RwbpType.R)
		{
			BarrierBuf[] array = this._barrierBufList.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				num2 = array[i].UseBarrier(RwbpType.R, num2);
			}
			result.resultDamage = num2;
			result.hpDamage = num2;
			float num3 = (float)((int)this.hp);
			this.hp -= num2;
			float value = num3 - (float)((int)this.hp);
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num2);
			}
			result.resultNumber = (int)value;
			this.MakeDamageEffect(RwbpType.R, value, this.defense.GetDefenseType(RwbpType.R));
			float num4 = (float)this.maxHp;
			if (num4 > 0f && UnityEngine.Random.value < num2 * 2f / num4)
			{
				this.MakeSpatteredBlood();
			}
		}
		else if (dmg.type == RwbpType.W)
		{
			BarrierBuf[] array2 = this._barrierBufList.ToArray();
			for (int j = 0; j < array2.Length; j++)
			{
				num2 = array2[j].UseBarrier(RwbpType.W, num2);
			}
			result.resultDamage = num2;
			float num5 = this.mental;
			float value2;
			if (this.CannotControll() && this.unconAction is Uncontrollable_RedShoes)
			{
				result.hpDamage = num2;
				num5 = this.hp;
				this.hp -= num2;
				value2 = (float)((int)this.hp - (int)num5);
				if (base.Equipment.kitCreature != null)
				{
					base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num2);
				}
			}
			else if (this.IsPanic() && !(this is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
			{
				result.spDamage = -num2;
				this.mental += num2;
				value2 = this.mental - num5;
			}
			else if (this is OfficerModel)
			{
				result.hpDamage = num2;
				result.spDamage = num2;
				this.mental -= num2;
				this.hp -= num2;
				value2 = num5 - this.hp;
			}
			else
			{
				result.spDamage = num2;
				this.mental -= num2;
				value2 = num5 - this.mental;
				if (base.Equipment.kitCreature != null)
				{
					base.Equipment.kitCreature.script.kitEvent.OnTakeDamageMental(this, num2);
				}
			}
			result.resultNumber = (int)value2;
			this.MakeDamageEffect(RwbpType.W, value2, this.defense.GetDefenseType(RwbpType.W));
		}
		else if (dmg.type == RwbpType.B)
		{
			BarrierBuf[] array3 = this._barrierBufList.ToArray();
			for (int k = 0; k < array3.Length; k++)
			{
				num2 = array3[k].UseBarrier(RwbpType.B, num2);
			}
			result.resultDamage = num2;
			result.hpDamage = num2;
			float num6 = (float)((int)this.hp);
			this.hp -= num2;
			float value3 = num6 - (float)((int)this.hp);
			if (this.IsPanic() && !(this is OfficerModel) && actor is WorkerModel && !((WorkerModel)actor).CannotControll() && !((WorkerModel)actor).IsPanic())
			{
				result.spDamage = -num2;
				this.mental += num2;
			}
			else
			{
				result.spDamage = num2;
				this.mental -= num2;
				if (base.Equipment.kitCreature != null)
				{
					base.Equipment.kitCreature.script.kitEvent.OnTakeDamageMental(this, num2);
				}
			}
			result.resultNumber = (int)value3;
			this.MakeDamageEffect(RwbpType.B, value3, this.defense.GetDefenseType(RwbpType.B));
			float num7 = (float)this.maxHp;
			if (num7 > 0f && UnityEngine.Random.value < num2 * 2f / num7)
			{
				this.MakeSpatteredBlood();
			}
		}
		else if (dmg.type == RwbpType.P)
		{
			float num8 = num2 / 100f;
			num2 = (float)this.maxHp * num8;
			result.scaling = maxHp / 100f;
			BarrierBuf[] array4 = this._barrierBufList.ToArray();
			for (int l = 0; l < array4.Length; l++)
			{
				num2 = array4[l].UseBarrier(RwbpType.P, num2);
			}
			result.resultDamage = num2;
			result.hpDamage = num2;
			float num9 = (float)((int)this.hp);
			this.hp -= num2;
			float value4 = num9 - (float)((int)this.hp);
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num2);
			}
			result.resultNumber = (int)value4;
			this.MakeDamageEffect(RwbpType.P, value4, this.defense.GetDefenseType(RwbpType.P));
		}
		else if (dmg.type == RwbpType.N)
		{
			result.resultDamage = num2;
			result.hpDamage = num2;
			float hp = this.hp;
			this.hp -= num2;
			float value5 = hp - this.hp;
			if (base.Equipment.kitCreature != null)
			{
				base.Equipment.kitCreature.script.kitEvent.OnTakeDamagePhysical(this, num2);
			}
			result.resultNumber = (int)value5;
			this.MakeDamageEffect(RwbpType.N, value5, DefenseInfo.Type.NONE);
		}
		if (num2 > 0f && this.defense.GetMultiplier(dmg.type) > 1f)
		{
			base.AddUnitBuf(new UnderAttackBuf());
		}
		if (this.IsPanic() && (this.CurrentPanicAction is PanicRoaming || this.CurrentPanicAction is PanicOpenIsolate) && actor is AgentModel && !((AgentModel)actor).CannotControll() && !((AgentModel)actor).IsPanic())
		{
			base.AddUnitBuf(new PanicUnderAttackBuf());
		}
		this.hp = Mathf.Clamp(this.hp, 0f, (float)this.maxHp);
		this.mental = Mathf.Clamp(this.mental, 0f, (float)this.maxMental);
		if (this.invincible)
		{
			return;
		}
		if (this.hp <= 0f)
		{
			float revivalChance = 0f;
			if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHESED))
			{
				revivalChance = 1f;
			}
			else
			{
				if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHESED))
				{
					revivalChance += 0.25f;
				}
				if (ResearchDataModel.instance.IsUpgradedAbility("resist_death_panic"))
				{
					revivalChance += 0.15f;
				}
			}
			if (UnityEngine.Random.value <= revivalChance && this.isRealWorker)
			{
				this._revivalHp = true;
			}
			else
			{
				this._revivalHp = false;
			}
			if (!this._revivaledHp && this._revivalHp && this.DeadType != DeadType.EXECUTION)
			{
				this.hp = 1f;
				this._revivaledHp = true;
				this._revivalHp = false;
				this.RecoverHP((float)this.maxHp / 2f);
			}
			else
			{
				if (dmg.specialDeadSceneEnable)
				{
					this.SetSpecialDeadScene(dmg.specialDeadSceneName);
				}
				this.OnDie();
			}
		}
		else if (this.IsPanic())
		{
			if (this.mental >= (float)this.maxMental)
			{
				this.StopPanic();
			}
		}
		else if (this.mental <= 0f)
		{
			float revivalChance = 0f;
			if (MissionManager.instance.ExistsFinishedOvertimeBossMission(SefiraEnum.CHESED))
			{
				revivalChance = 1f;
			}
			else
			{
				if (MissionManager.instance.ExistsFinishedBossMission(SefiraEnum.CHESED))
				{
					revivalChance += 0.25f;
				}
				if (ResearchDataModel.instance.IsUpgradedAbility("resist_death_panic"))
				{
					revivalChance += 0.15f;
				}
			}
			if (UnityEngine.Random.value <= revivalChance && this.isRealWorker)
			{
				this._revivalMental = true;
			}
			else
			{
				this._revivalMental = false;
			}
			if (!this._revivaledMental && this._revivalMental)
			{
				this._revivaledMental = true;
				this._revivalMental = false;
				this.RecoverMental((float)this.maxMental / 2f);
			}
			else
			{
				this.Panic();
			}
		}
		if (base.Equipment.armor != null)
		{
			base.Equipment.armor.OnTakeDamage_After(num2, dmg.type);
			base.Equipment.armor.OnTakeDamage_After(actor, dmg);
		}
		if (base.Equipment.weapon != null)
		{
			base.Equipment.weapon.OnTakeDamage_After(num2, dmg.type);
			base.Equipment.weapon.OnTakeDamage_After(actor, dmg);
		}
		base.Equipment.gifts.OnTakeDamage_After(num2, dmg.type);
		base.Equipment.gifts.OnTakeDamage_After(actor, dmg);
		foreach (UnitBuf unitBuf in _bufList)
		{
			unitBuf.OnTakeDamage_After(actor, dmg);
		}
		result.activated = false;
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x00208C2C File Offset: 0x00206E2C
	protected void CreatePhysicalDamagedEffect(float value)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/DamagedEffect");
		DamageTextEffect component = gameObject.GetComponent<DamageTextEffect>();
		gameObject.transform.position = this.GetWorkerUnit().GetHairTransform().position;
		component.PhysicalDamage(value);
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x00208C70 File Offset: 0x00206E70
	protected void CreateMentalDamagedEffect(float value)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/DamagedEffect");
		DamageTextEffect component = gameObject.GetComponent<DamageTextEffect>();
		gameObject.transform.position = this.GetWorkerUnit().GetHairTransform().position;
		component.MentalDamage(value);
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x000493D6 File Offset: 0x000475D6
	public override bool IsAttackTargetable()
	{
		return !this.IsDead() && (this.unconAction == null || this.unconAction.IsAttackTargetable());
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x00208CB4 File Offset: 0x00206EB4
	public override bool IsHostile(UnitModel target)
	{
		if (this.CannotControll() && this.unconAction != null && this.unconAction.HasUniqueHostility())
		{
			return this.unconAction.IsHostile();
		}
		if (target is WorkerModel)
		{
			WorkerModel workerModel = (WorkerModel)target;
			if (target == this)
			{
				return false;
			}
			if (workerModel.CannotControll())
			{
				if (workerModel.unconAction is Uncontrollable_RedShoes)
				{
					return true;
				}
				if (workerModel.unconAction is Uncontrollable_Machine)
				{
					return true;
				}
			}
			else if (workerModel.IsPanic() && workerModel.currentPanicAction is PanicViolence)
			{
				return true;
			}
		}
		else
		{
			if (target is CreatureModel)
			{
				return (target as CreatureModel).script.IsAutoSuppressable();
			}
			if (target is RabbitModel)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x000493FD File Offset: 0x000475FD
	public virtual void RecentlyAttackedCreature(CreatureModel creatureModel)
	{
		this.recentlyAttacked = creatureModel;
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x00208D90 File Offset: 0x00206F90
	public virtual void TakeMentalDamage(float damage, MentalDamageOption option)
	{
		if (this.IsDead())
		{
			return;
		}
		if (option != MentalDamageOption.IGNORE_DEFENSE)
		{
			damage = damage * (float)(100 - this.mentalDefense) / 100f;
		}
		if (damage <= 0f)
		{
			return;
		}
		this.mental -= damage;
		GameObject gameObject = Prefab.LoadPrefab("Effect/DamagedEffect");
		DamageTextEffect component = gameObject.GetComponent<DamageTextEffect>();
		try
		{
			gameObject.transform.position = this.GetWorkerUnit().GetHairTransform().position;
		}
		catch (MissingReferenceException message)
		{
			Debug.Log(message);
			Debug.Log(this.instanceId);
		}
		component.MentalDamage(damage);
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x0000425D File Offset: 0x0000245D
	public void TakeMentalDamage(float damage)
	{
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x00208E44 File Offset: 0x00207044
	public void MakeSpatteredBlood()
	{
		PassageObjectModel passage = this.movableNode.GetPassage();
		if (passage != null)
		{
			passage.AttachBloodObject(this.GetCurrentViewPosition().x);
		}
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x00208E78 File Offset: 0x00207078
	public virtual void RecoverHP(float amount)
	{ // <Mod>
        RecoverHPv2(amount);
    }

    // <Mod>
	public virtual float RecoverHPv2(float amount)
	{
		if (this.IsDead())
		{
			return 0f;
		}
		if (this.blockRecover)
		{
			return 0f;
		}
		float num = 1f;
		if (base.Equipment.weapon != null && base.Equipment.weapon.script != null)
		{
			num *= base.Equipment.weapon.script.RecoveryMultiplier(false, amount);
		}
		if (base.Equipment.armor != null && base.Equipment.armor.script != null)
		{
			num *= base.Equipment.armor.script.RecoveryMultiplier(false, amount);
		}
		num *= base.Equipment.gifts.RecoveryMultiplier(false, amount);
		foreach (UnitBuf unitBuf in _bufList)
		{
			num *= unitBuf.RecoveryMultiplier(false, amount);
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("regenerator_healing_boost") && (!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true) || OvertimeNetzachBossBuf.IsBullet))
		{
			PassageObjectModel passage = movableNode.GetPassage();
			if (passage != null && passage.isRegenerator && passage.GetSefira().isRecoverActivated)
			{
				float factor = 2f - hp / maxHp;
				if (factor < 1f)
				{
					factor = 1f;
				}
				if (factor > 2f)
				{
					factor = 2f;
				}
				num *= factor;
			}
		}
		amount *= num;
		if (base.Equipment.weapon != null && base.Equipment.weapon.script != null)
		{
			base.Equipment.weapon.script.OwnerHeal(false, ref amount);
		}
		if (base.Equipment.armor != null && base.Equipment.armor.script != null)
		{
			base.Equipment.armor.script.OwnerHeal(false, ref amount);
		}
		base.Equipment.gifts.OwnerHeal(false, ref amount);
		foreach (UnitBuf unitBuf in _bufList)
		{
			unitBuf.OwnerHeal(false, ref amount);
		}
		this.hp += amount;
		this.hp = ((this.hp <= (float)this.maxHp) ? this.hp : ((float)this.maxHp));
		if (hp <= 0f)
		{
			hp = 0f;
			Die();
		}
        return amount;
	}

	// Token: 0x06005BDA RID: 23514 RVA: 0x00208F6C File Offset: 0x0020716C
	public virtual void RecoverMental(float amount)
	{ // <Mod>
        RecoverMentalv2(amount);
    }

    // <Mod>
	public virtual float RecoverMentalv2(float amount)
	{
		if (this.IsDead())
		{
			return 0f;
		}
		if (this.blockRecover)
		{
			return 0f;
		}
		float num = 1f;
		if (base.Equipment.weapon != null && base.Equipment.weapon.script != null)
		{
			num *= base.Equipment.weapon.script.RecoveryMultiplier(true, amount);
		}
		if (base.Equipment.armor != null && base.Equipment.armor.script != null)
		{
			num *= base.Equipment.armor.script.RecoveryMultiplier(true, amount);
		}
		num *= base.Equipment.gifts.RecoveryMultiplier(true, amount);
		foreach (UnitBuf unitBuf in _bufList)
		{
			num *= unitBuf.RecoveryMultiplier(true, amount);
		}
		if (ResearchDataModel.instance.IsUpgradedAbility("regenerator_healing_boost") && (!SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.NETZACH, true) || OvertimeNetzachBossBuf.IsBullet))
		{
			PassageObjectModel passage = movableNode.GetPassage();
			if (passage != null && passage.isRegenerator && passage.GetSefira().isRecoverActivated)
			{
				float factor = 2f - mental / maxMental;
				if (factor < 1f)
				{
					factor = 1f;
				}
				if (factor > 2f)
				{
					factor = 2f;
				}
				num *= factor;
			}
		}
		amount *= num;
		if (base.Equipment.weapon != null && base.Equipment.weapon.script != null)
		{
			base.Equipment.weapon.script.OwnerHeal(true, ref amount);
		}
		if (base.Equipment.armor != null && base.Equipment.armor.script != null)
		{
			base.Equipment.armor.script.OwnerHeal(true, ref amount);
		}
		base.Equipment.gifts.OwnerHeal(true, ref amount);
		foreach (UnitBuf unitBuf in _bufList)
		{
			unitBuf.OwnerHeal(true, ref amount);
		}
		this.mental += amount;
		this.mental = ((this.mental <= (float)this.maxMental) ? this.mental : ((float)this.maxMental));
		if (mental <= 0f)
		{
			mental = 0f;
			Panic();
		}
        return amount;
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x00209060 File Offset: 0x00207260
	public virtual void SetInvincible(bool b)
	{
		bool flag = this.IsDead();
		this.invincible = b;
		if (!flag && this.IsDead())
		{
			this.OnDie();
		}
	}

	// Token: 0x06005BDC RID: 23516 RVA: 0x00049406 File Offset: 0x00047606
	public virtual void Stun(float time)
	{
		this.stunTime = time;
	}

	// Token: 0x06005BDD RID: 23517 RVA: 0x0004940F File Offset: 0x0004760F
	public virtual void StopStun()
	{
		this.stunTime = 0f;
	}

	// Token: 0x06005BDE RID: 23518 RVA: 0x0004941C File Offset: 0x0004761C
	public virtual void OnAttackWorker(WorkerModel target)
	{
		this._attackTargetWorker = target;
	}

	// Token: 0x06005BDF RID: 23519 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void Panic()
	{
	}

	// Token: 0x06005BE0 RID: 23520 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void PanicReadyComplete()
	{
	}

	// Token: 0x06005BE1 RID: 23521 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void StopPanic()
	{
	}

	// Token: 0x06005BE2 RID: 23522 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void StopPanicWithoutStun()
	{
	}

	// Token: 0x06005BE3 RID: 23523 RVA: 0x00004367 File Offset: 0x00002567
	public virtual bool IsPanic()
	{
		return false;
	}

	// Token: 0x06005BE4 RID: 23524 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ResetAnimator()
	{
	}

	// Token: 0x06005BE5 RID: 23525 RVA: 0x00049425 File Offset: 0x00047625
	public virtual bool IsSuppable()
	{
		return this.IsPanic() || (this.unconAction is Uncontrollable_RedShoes || this.unconAction is Uncontrollable_Machine);
	}

	// Token: 0x06005BE6 RID: 23526 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void EncounterCreature(UnitModel encounteredCreature)
	{
	}

	// Token: 0x06005BE7 RID: 23527 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void EncounterStandingItem(StandingItemModel standing)
	{
	}

	// Token: 0x06005BE8 RID: 23528 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void LoseControl()
	{
	}

	// Token: 0x06005BE9 RID: 23529 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void GetControl()
	{
	}

	// Token: 0x06005BEA RID: 23530 RVA: 0x00004367 File Offset: 0x00002567
	public virtual bool CannotControll()
	{
		return false;
	}

	// Token: 0x06005BEB RID: 23531 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void SetUncontrollableAction(UncontrollableAction uncon)
	{
	}

	// Token: 0x06005BEC RID: 23532 RVA: 0x00049457 File Offset: 0x00047657
	public virtual void SetCustsomCommand(WorkerCommand cmd)
	{
		this.commandQueue.SetAgentCommand(cmd);
	}

	// Token: 0x06005BED RID: 23533 RVA: 0x00049465 File Offset: 0x00047665
	public virtual void Die()
	{
		if (this.IsDead())
		{
			return;
		}
		if (this.invincible)
		{
			return;
		}
		this.hp = 0f;
		this.OnDie();
	}

	// Token: 0x06005BEE RID: 23534 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ShowCreatureActionSpeech(long creatureID, string key)
	{
	}

	// Token: 0x06005BEF RID: 23535 RVA: 0x00209094 File Offset: 0x00207294
	public virtual void OnDie()
	{
		if (this.invincible)
		{
			return;
		}
		if (this.isStun)
		{
			this.OnStunEnd();
		}
		this.StopAction();
		PassageObjectModel passage = this.GetMovableNode().GetPassage();
		if (passage != null)
		{
			passage.AddDeletedUnit(this.GetMovableNode());
		}
		this.GetMovableNode().SetActive(false);
		this.ClearEffect();
		foreach (UnitBuf unitBuf in this._bufList.ToArray())
		{
			unitBuf.OnUnitDie();
		}
		if (this.stunEffect != null)
		{
			UnityEngine.Object.Destroy(this.stunEffect);
			this.stunEffect = null;
		}
		if (this.GetWorkerUnit() != null)
		{
			BufStateUI bufUI = this.GetWorkerUnit().bufUI;
			if (bufUI != null)
			{
				bufUI.gameObject.SetActive(false);
			}
		}
		this._isDead = true;
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void AfterDeadAnim()
	{
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x000302AE File Offset: 0x0002E4AE
	public override void InteractWithDoor(DoorObjectModel door)
	{
		base.InteractWithDoor(door);
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x00209180 File Offset: 0x00207380
	public static int CompareByName(WorkerModel x, WorkerModel y)
	{
		if (x == null || y == null)
		{
			Debug.Log("Errror in comparison by name");
			return 0;
		}
		if (x.name == null)
		{
			if (y.name == null)
			{
				return 0;
			}
			return -1;
		}
		else
		{
			if (y.name == null)
			{
				return 1;
			}
			return x.name.CompareTo(y.name);
		}
	}

	// Token: 0x06005BF3 RID: 23539 RVA: 0x002091E0 File Offset: 0x002073E0
	public virtual bool IsInSefira()
	{
		PassageObjectModel passage;
		return (passage = this.GetMovableNode().GetPassage()) != null && (passage.type == PassageType.DEPARTMENT || passage.type == PassageType.SEFIRA);
	}

	// Token: 0x06005BF4 RID: 23540 RVA: 0x00049490 File Offset: 0x00047690
	public virtual void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.FixedUpdate)
		{
			this.OnFixedUpdate();
		}
	}

	// Token: 0x06005BF5 RID: 23541 RVA: 0x000494A8 File Offset: 0x000476A8
	public void SetSpecialDeadScene(string deadSceneName)
	{
		this.deadSceneName = deadSceneName;
		this.specialDeadScene = true;
	}

	// Token: 0x06005BF6 RID: 23542 RVA: 0x000494B8 File Offset: 0x000476B8
	public void SetSpecialDeadScene(string deadSceneName, bool seperator)
	{
		this.deadSceneName = deadSceneName;
		this.seperator = seperator;
		this.specialDeadScene = true;
	}

	// Token: 0x06005BF7 RID: 23543 RVA: 0x000494CF File Offset: 0x000476CF
	public void SetSpecialDeadScene(string deadSceneName, bool seperator, bool hasUniqueFace)
	{
		this.deadSceneName = deadSceneName;
		this.seperator = seperator;
		this.hasUniqueFace = hasUniqueFace;
		this.specialDeadScene = true;
	}

	// Token: 0x06005BF8 RID: 23544 RVA: 0x000494ED File Offset: 0x000476ED
	public void ResetSpecialDeadScene()
	{
		this.deadSceneName = string.Empty;
		this.seperator = true;
		this.hasUniqueFace = false;
		this.specialDeadScene = false;
	}

	// Token: 0x06005BF9 RID: 23545 RVA: 0x0004950F File Offset: 0x0004770F
	public static int CompareByID(WorkerModel x, WorkerModel y)
	{
		if (x == null || y == null)
		{
			Debug.Log("Error to compare");
			return 0;
		}
		return x.instanceId.CompareTo(y.instanceId);
	}

	// Token: 0x06005BFA RID: 23546 RVA: 0x0020921C File Offset: 0x0020741C
	public static int CompareBySefira(WorkerModel x, WorkerModel y)
	{
		if (x == null || y == null)
		{
			Debug.Log("Error to compare");
			return 0;
		}
		int index = SefiraManager.instance.GetSefira(x.currentSefira).index;
		int index2 = SefiraManager.instance.GetSefira(y.currentSefira).index;
		return index.CompareTo(index2);
	}

	// Token: 0x06005BFB RID: 23547 RVA: 0x0004953A File Offset: 0x0004773A
	public virtual bool IsCrazy()
	{
		return this.IsPanic();
	}

	// Token: 0x06005BFC RID: 23548 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void OnResurrect()
	{
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void InitialEncounteredCreature(UnitModel encountered)
	{
	}

	// Token: 0x06005BFE RID: 23550 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void InitialEncounteredCreature(RiskLevel level)
	{
	}

	// Token: 0x06005BFF RID: 23551 RVA: 0x0004954A File Offset: 0x0004774A
	public virtual void ShowUnconSpeech(string key)
	{
		if (this.unconAction != null)
		{
			this.unconAction.ShowUnconSpeech(key);
		}
	}

	// Token: 0x06005C00 RID: 23552 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual GameObject MakeCreatureEffectHead(CreatureModel creature)
	{
		return null;
	}

	// Token: 0x06005C01 RID: 23553 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual GameObject MakeCreatureEffect(long id)
	{
		return null;
	}

	// Token: 0x06005C02 RID: 23554 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual GameObject MakeCreatureEffect(CreatureModel model)
	{
		return null;
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual GameObject MakeCreatureEffectHead(CreatureModel model, bool addlist)
	{
		return null;
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ClearEffect()
	{
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x00209278 File Offset: 0x00207478
	public void SetWorkerFaceType(WorkerFaceType type)
	{
		WorkerUnit workerUnit = this.GetWorkerUnit();
		if (workerUnit != null)
		{
			workerUnit.SetWorkerFaceType(type);
		}
	}

	// Token: 0x06005C06 RID: 23558 RVA: 0x002092A0 File Offset: 0x002074A0
	protected override void PlayAttackAnimation(string animationName)
	{
		base.PlayAttackAnimation(animationName);
		int attackType = -1;
		if (this._equipment.weapon.metaInfo.specialWeaponAnim.Split(new char[]
		{
			'_'
		})[0] == "Custom")
		{
			return;
		}
		if (this._equipment.weapon != null && this._equipment.weapon.metaInfo.weaponClassType == WeaponClassType.SPECIAL)
		{
			string text = animationName.ToLower();
			if (text != null)
			{
				if (!(text == "attack1"))
				{
					if (!(text == "attack2"))
					{
						if (text == "attack3")
						{
							attackType = 2;
						}
					}
					else
					{
						attackType = 1;
					}
				}
				else
				{
					attackType = 0;
				}
			}
		}
		this.GetWorkerUnit().Attack(attackType, 0.8f + this.attackSpeed / 143f);
		try
		{
			if (this.GetWorkerUnit().animChanger.state != null)
			{
				TrackEntry trackEntry = this.GetWorkerUnit().animChanger.state.SetAnimation(0, animationName, false);
				this.GetWorkerUnit().animChanger.SetState(true);
				trackEntry.Event += this.SpecialAttackDamage;
				trackEntry.Complete += this.SpecialAttackEnd;
				bool flag = true;
				string[] noBoostList = Add_On.NoBoostList;
				for (int i = 0; i < noBoostList.Length; i++)
				{
					if (noBoostList[i] == this._equipment.weapon.metaInfo.specialWeaponAnim.Split(new char[]
					{
						'_'
					})[0])
					{
						flag = false;
					}
				}
				if (flag)
				{
					trackEntry.timeScale = 0.8f + this.attackSpeed / 143f;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06005C07 RID: 23559 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ShowSpeech(string txt)
	{
	}

	// Token: 0x06005C08 RID: 23560 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual Sefira GetCurrentSefira()
	{
		return null;
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x00209450 File Offset: 0x00207650
	public virtual GameObject ChangePuppet(string src)
	{
		return Prefab.LoadPrefab(src);
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual WorkerDeadScript ChangePuppetAnimToDie(string src)
	{
		return null;
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void ChangePuppetAnimToUncon(string src)
	{
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void PursueUnconAgent(UnitModel target)
	{
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x00049457 File Offset: 0x00047657
	public void SetAgentCommand(WorkerCommand command)
	{
		this.commandQueue.SetAgentCommand(command);
	}

	// Token: 0x06005C0E RID: 23566 RVA: 0x0000429A File Offset: 0x0000249A
	public virtual WorkerUnit GetWorkerUnit()
	{
		return null;
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x00049563 File Offset: 0x00047763
	public void SetDeadType(DeadType type)
	{
		this._deadType = type;
		this.GetWorkerUnit().SetDeadType(type);
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x00049578 File Offset: 0x00047778
	public void SetPanicAnim(bool b)
	{
		this.GetWorkerUnit().SetPanic(b);
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x00049586 File Offset: 0x00047786
	public override void OnStun(float stunVal)
	{
		base.OnStun(stunVal);
	}

	// Token: 0x06005C12 RID: 23570 RVA: 0x0004958F File Offset: 0x0004778F
	public void OnStunEffectDestroied()
	{
		this.stunEffect = null;
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x00049598 File Offset: 0x00047798
	protected override void OnSetArmor()
	{ // <Patch>
		base.OnSetArmor();
		WorkerSpriteManager.instance.GetArmorData_Mod(new LobotomyBaseMod.LcId(EquipmentTypeInfo.GetLcId(this.Equipment.armor.metaInfo).packageId, this.Equipment.armor.metaInfo.armorId), ref this.spriteData);
	}

	// Token: 0x06005C14 RID: 23572 RVA: 0x0000425D File Offset: 0x0000245D
	public virtual void CheckEGOGift()
	{
	}

	// Token: 0x06005C15 RID: 23573 RVA: 0x00209468 File Offset: 0x00207668
	public override Sprite GetWeaponSprite()
	{ // <Patch> <Mod>
		Sprite result = null;
		WeaponClassType weaponClassType = base.Equipment.weapon.metaInfo.weaponClassType;
		if (weaponClassType == WeaponClassType.FIST)
		{
			if (EquipmentTypeInfo.GetLcId(base.Equipment.weapon.metaInfo).packageId != "")
			{
				LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(base.Equipment.weapon.metaInfo).packageId, base.Equipment.weapon.metaInfo.sprite);
				Sprite[] fistSprite = WorkerSpriteManager.instance.GetFistSprite(ss);
				if (fistSprite[0] == null || fistSprite[1] == null)
				{
					return result;
				}
				result = fistSprite[1];
			}
			else
			{
				int id = (int)float.Parse(base.Equipment.weapon.metaInfo.sprite);
				Sprite[] fistSprite = WorkerSprite_WorkerSpriteManager.instance.GetFistSprite(id);
				if (fistSprite[0] == null || fistSprite[1] == null)
				{
					return result;
				}
				result = fistSprite[1];
			}
		}
		else
		{
			LobotomyBaseMod.KeyValuePairSS ss = new LobotomyBaseMod.KeyValuePairSS(EquipmentTypeInfo.GetLcId(this.Equipment.weapon.metaInfo).packageId, this.Equipment.weapon.metaInfo.sprite);
			bool flag = WeaponSetter.IsTwohanded(base.Equipment.weapon);
			Sprite weaponSprite = WorkerSpriteManager.instance.GetWeaponSprite_Mod(weaponClassType, ss);
			result = weaponSprite;
		}
		return result;
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x000495C5 File Offset: 0x000477C5
	public void SpecialAttackDamage(TrackEntry trackEntry, Spine.Event e)
	{
		if (e.data.name == "Damage")
		{
			base.OnGiveDamageByWeapon();
		}
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x000495E4 File Offset: 0x000477E4
	public void SpecialAttackEnd(TrackEntry trackEntry)
	{
		this.GetWorkerUnit().animChanger.SetState(false);
		this.GetWorkerUnit().EndAttack();
		this.GetWorkerUnit().animChanger.state.SetEmptyAnimation(0, 0.01f);
	}

	// <Mod>
	public UnitMouseEventTarget GetUnitMouseTarget()
	{
		return _unitMouseTarget;
	}

	// <Mod>
	public void SetUnitMouseTarget(UnitMouseEventTarget target)
	{
		_unitMouseTarget = target;
	}

	// <Mod>
	private UnitMouseEventTarget _unitMouseTarget;

	// Token: 0x040053C4 RID: 21444
	protected WorkerCommandQueue commandQueue;

	// Token: 0x040053C5 RID: 21445
	public WorkerClass workerClass;

	// Token: 0x040053C6 RID: 21446
	public bool isRealWorker = true;

	// Token: 0x040053C7 RID: 21447
	public string name;

	// Token: 0x040053C8 RID: 21448
	public string gender;

	// Token: 0x040053C9 RID: 21449
	private string _currentSefira;

	// Token: 0x040053CA RID: 21450
	public SefiraEnum currentSefiraEnum = SefiraEnum.DUMMY;

	// Token: 0x040053CB RID: 21451
	protected bool _revivalHp;

	// Token: 0x040053CC RID: 21452
	protected bool _revivalMental;

	// Token: 0x040053CD RID: 21453
	protected bool _revivaledHp;

	// Token: 0x040053CE RID: 21454
	protected bool _revivaledMental;

	// Token: 0x040053CF RID: 21455
	private const float revivalProb = 0.25f;

	// Token: 0x040053D0 RID: 21456
	public float movementMul = 1f;

	// Token: 0x040053D1 RID: 21457
	public int panicValue;

	// Token: 0x040053D2 RID: 21458
	public bool invincible;

	// Token: 0x040053D3 RID: 21459
	public bool blockRecover;

	// Token: 0x040053D4 RID: 21460
	public float stunTime;

	// Token: 0x040053D5 RID: 21461
	public bool haltUpdate;

	// Token: 0x040053D6 RID: 21462
	public bool returnPanic;

	// Token: 0x040053D7 RID: 21463
	public bool willDead;

	// Token: 0x040053D8 RID: 21464
	protected bool _isDead;

	// Token: 0x040053D9 RID: 21465
	public Dictionary<string, string> speechTable = new Dictionary<string, string>();

	// Token: 0x040053DA RID: 21466
	[NonSerialized]
	public CreatureModel target;

	// Token: 0x040053DB RID: 21467
	[NonSerialized]
	public WorkerModel targetWorker;

	// Token: 0x040053DC RID: 21468
	[NonSerialized]
	public StandingItemModel targetObject;

	// Token: 0x040053DD RID: 21469
	private PanicAction currentPanicAction;

	// Token: 0x040053DE RID: 21470
	public UncontrollableAction unconAction;

	// Token: 0x040053DF RID: 21471
	[NonSerialized]
	private CreatureModel _recentlyAttacked;

	// Token: 0x040053E0 RID: 21472
	public CreatureBase animationMessageRecevied;

	// Token: 0x040053E1 RID: 21473
	public bool visible = true;

	// Token: 0x040053E2 RID: 21474
	public float waitTimer;

	// Token: 0x040053E3 RID: 21475
	public bool OnWorkEndFlag;

	// Token: 0x040053E4 RID: 21476
	public bool puppetChanged;

	// Token: 0x040053E5 RID: 21477
	public MapNode lastestMoveTarget;

	// Token: 0x040053E6 RID: 21478
	private WorkerModel _attackTargetWorker;

	// Token: 0x040053E7 RID: 21479
	private bool _specialDeadScene;

	// Token: 0x040053E8 RID: 21480
	protected string deadSceneName;

	// Token: 0x040053E9 RID: 21481
	protected bool seperator = true;

	// Token: 0x040053EA RID: 21482
	protected bool hasUniqueFace;

	// Token: 0x040053EB RID: 21483
	public Sprite hairSprite;

	// Token: 0x040053EC RID: 21484
	public Sprite faceSprite;

	// Token: 0x040053ED RID: 21485
	public GameObject stunEffect;

	// Token: 0x040053EE RID: 21486
	[NonSerialized]
	public WorkerSprite.WorkerSprite spriteData;

	// Token: 0x040053EF RID: 21487
	protected PanicData _panicData;

	// Token: 0x040053F0 RID: 21488
	public bool isChangeableAnimator = true;

	// Token: 0x040053F1 RID: 21489
	private DeadType _deadType;
}
