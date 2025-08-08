/*
private static float ComeBackTime // 
public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value) // The first time an agent attacks Fiery Bird, update comeback time to 60 seconds
public override void OnSuppressed() // An agent must attack Fiery Bird at least once or the weapon wont drop
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WorkerSpine;

// Token: 0x020003F2 RID: 1010
public class FireBird : CreatureBase, IObserver
{
	// Token: 0x060021D3 RID: 8659 RVA: 0x00100314 File Offset: 0x000FE514
	public FireBird()
	{
		float[] array = new float[4];
		array[1] = 1f;
		array[2] = 0.5f;
		this._FILTER_ALPHA = array;
		this.damaged = new List<UnitModel>();
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x060021D4 RID: 8660 RVA: 0x00022A7B File Offset: 0x00020C7B
	private static float SkillCoolTime
	{
		get
		{
			return UnityEngine.Random.Range(8f, 10f);
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x060021D5 RID: 8661 RVA: 0x00022A8C File Offset: 0x00020C8C
	private static float ComeBackTime
	{ // <Mod> changed from 45 to 90 seconds
		get
		{
			return UnityEngine.Random.Range(30f, 45f);
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060021D6 RID: 8662 RVA: 0x00022A9D File Offset: 0x00020C9D
	private static DamageInfo AttackDmg
	{
		get
		{
			return new DamageInfo(RwbpType.W, 90, 110);
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x060021D7 RID: 8663 RVA: 0x00022AA9 File Offset: 0x00020CA9
	public static DamageInfo PassageDmg
	{
		get
		{
			return new DamageInfo(RwbpType.R, 5, 5);
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00022AB3 File Offset: 0x00020CB3
	private IsolateFilter Filter
	{
		get
		{
			return base.Unit.room.SkillFilter;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x060021D9 RID: 8665 RVA: 0x00022AC5 File Offset: 0x00020CC5
	public FireBirdAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as FireBirdAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x00022AF4 File Offset: 0x00020CF4
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
		this.Filter.renderAnim.enabled = false;
		this.ParamInit();
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x0010037C File Offset: 0x000FE57C
	public override void ParamInit()
	{
		base.ParamInit();
		this.SpeedDown();
		this.healTarget = null;
		this.moveTarget = null;
		this.oldPassage = null;
		this.targetNode = null;
		this.speedUp = false;
		this.annoyed = false;
		this.hunted = false; // <Mod>
		this.damaged = new List<UnitModel>();
		this._phase = FireBird.Phase.DEFAULT;
		this.ResetQliphothCounter();
		this.RemoveBurningPassage();
		this.SetFilterAlpha();
		this.TreeOff();
		this.animScript.Init();
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x00022B20 File Offset: 0x00020D20
	public override void RoomSpriteInit()
	{
		base.RoomSpriteInit();
		base.Unit.room.SkillFilter.specialAnimKey = "Display";
		base.Unit.room.SkillFilter.hasSpecialAnimKey = true;
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x00022B58 File Offset: 0x00020D58
	public override void OnStageStart()
	{
		base.OnStageStart();
		Notice.instance.Observe(NoticeName.OnEscape, this);
		this.isSuppressed = false;
		this.ParamInit();
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x0001EDE2 File Offset: 0x0001CFE2
	public override void OnStageEnd()
	{
		base.OnStageEnd();
		this.ParamInit();
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x00022B7D File Offset: 0x00020D7D
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		Notice.instance.Remove(NoticeName.OnEscape, this);
		this.ParamInit();
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x0001E18B File Offset: 0x0001C38B
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.Escape();
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x00022B9B File Offset: 0x00020D9B
	public override void AddedQliphothCounter()
	{
		base.AddedQliphothCounter();
		this.SetFilterAlpha();
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x00022BA9 File Offset: 0x00020DA9
	public override void ReducedQliphothCounter()
	{
		base.ReducedQliphothCounter();
		this.SetFilterAlpha();
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x001003F4 File Offset: 0x000FE5F4
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		CreatureFeelingState currentFeelingState = skill.GetCurrentFeelingState();
		if (currentFeelingState != CreatureFeelingState.BAD)
		{
			if (currentFeelingState != CreatureFeelingState.NORM)
			{
				if (currentFeelingState == CreatureFeelingState.GOOD)
				{
					this.SubQliphothCounter();
				}
			}
			else if (UnityEngine.Random.value < 0.3f)
			{
				this.SubQliphothCounter();
			}
		}
		else
		{
			this.AddQliphothCounter();
		}
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x0010045C File Offset: 0x000FE65C
	public override void OnSkillGoalComplete(UseSkill skill)
	{
		base.OnSkillGoalComplete(skill);
		if (this.CheckHeal(skill))
		{
			if (this.model.currentSkill != null)
			{
				this.model.currentSkill.PauseWorking();
				this.model.Unit.room.StopWorkDesc();
			}
			this.HealStart(skill.agent);
		}
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x001004C0 File Offset: 0x000FE6C0
	public override void OnFinishWork(UseSkill skill)
	{
		base.OnFinishWork(skill);
		AgentModel agent = skill.agent;
		if (agent == null)
		{
			return;
		}
		UnitBuf unitBufByType = agent.GetUnitBufByType(UnitBufType.FIREBIRD_DEBUF);
		if (unitBufByType != null)
		{
			agent.RemoveUnitBuf(unitBufByType);
		}
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x001004F8 File Offset: 0x000FE6F8
	public override float GetDamageMultiplierInWork(UseSkill skill)
	{
		float damageMultiplierInWork = base.GetDamageMultiplierInWork(skill);
		int num = this.model.qliphothCounter;
		if (num > 3)
		{
			num = 0;
		}
		return this._WORK_DMG_RATIO[num];
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000040E7 File Offset: 0x000022E7
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x00022BB7 File Offset: 0x00020DB7
	public override bool IsAttackTargetable()
	{
		return base.IsAttackTargetable() && this._phase != FireBird.Phase.COMEBACK && this._phase != FireBird.Phase.SUPPRESSED;
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x00022BDF File Offset: 0x00020DDF
	public override bool IsSuppressable()
	{
		return base.IsSuppressable() && this._phase != FireBird.Phase.COMEBACK && this._phase != FireBird.Phase.SUPPRESSED;
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x00022C07 File Offset: 0x00020E07
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return base.CanTakeDamage(attacker, dmg) && this._phase != FireBird.Phase.COMEBACK && this._phase != FireBird.Phase.SUPPRESSED;
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x0010052C File Offset: 0x000FE72C
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{ // <Mod> the first time an agent attacks Fiery Bird, update comeback time to 60 seconds; Sound of a Star also works to prevent blindness
		base.OnTakeDamage(actor, dmg, value);
		this.annoyed = true;
		if (!(actor is AgentModel)) return;
		if (!this.hunted)
		{
			this.comeBackTimer.StartTimer(60f);
			this.hunted = true;
		}
		AgentModel agentModel = actor as AgentModel;
		if (agentModel != null && !agentModel.HasUnitBuf(UnitBufType.FIREBIRD_DEBUF) && !agentModel.HasEquipment(400035) && !agentModel.HasEquipment(400058))
		{
			agentModel.AddUnitBuf(new FireBirdDebuf());
		}
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x00100580 File Offset: 0x000FE780
	public override void Escape()
	{
		base.Escape();
		this.annoyed = false;
		this.hunted = false; // <Mod>
		this.comeBackTimer.StartTimer(FireBird.ComeBackTime);
		this.TreeOn();
		this.animScript.OnEscape();
		this.model.Escape();
		this.MakeMovement(true);
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x001005D0 File Offset: 0x000FE7D0
	public override void OnSuppressed()
	{ // <Mod> the weapon is not rewarded if Fiery Bird wasn't hit by an agent at least once
		base.OnSuppressed();
		this._phase = FireBird.Phase.SUPPRESSED;
		if (this.model.hp <= 0f)
		{
			if (!this.isSuppressed && this.hunted)
			{
				this.Present();
				this.isSuppressed = true;
			}
		}
		this.SpeedDown();
		this.RemoveBurningPassage();
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x00100624 File Offset: 0x000FE824
	private void Present()
	{
		int id = 200061;
		if (InventoryModel.Instance.CheckEquipmentCount(id))
		{
			EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(id);
		}
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x00100654 File Offset: 0x000FE854
	private void SpeedUp()
	{
		if (!this.speedUp)
		{
			this.model.movementScale = this.model.movementScale * 20f / this.model.metaInfo.speed;
			this.speedUp = true;
		}
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x001006A0 File Offset: 0x000FE8A0
	private void SpeedDown()
	{
		if (this.speedUp)
		{
			this.model.movementScale = this.model.movementScale / 20f * this.model.metaInfo.speed;
			this.speedUp = false;
		}
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x0001EEC7 File Offset: 0x0001D0C7
	public override void OnReturn()
	{
		base.OnReturn();
		this.ParamInit();
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x001006EC File Offset: 0x000FE8EC
	public override void UniqueEscape()
	{
		base.UniqueEscape();
		if (this.model.hp <= 0f)
		{
			return;
		}
		this.CheckPassage();
		this.comeBackTimer.RunTimer();
		if (this._phase != FireBird.Phase.ATTACKING && !this.comeBackTimer.started)
		{
			this._phase = FireBird.Phase.COMEBACK;
		}
		switch (this._phase)
		{
		case FireBird.Phase.DEFAULT:
			this.FixedUpdate_Default();
			return;
		case FireBird.Phase.ATTACKING:
			this.FixedUpdate_Attacking();
			return;
		case FireBird.Phase.COMEBACK:
			this.FixedUpdate_ComeBack();
			return;
		}
		this.StopMovement();
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x001007A0 File Offset: 0x000FE9A0
	private void FixedUpdate_Default()
	{
		this.skillCoolTimer.RunTimer();
		if (!this.skillCoolTimer.started && this.currentPassage != null && this.annoyed)
		{
			UnitModel nearest = this.GetNearest(15f, false);
			if (nearest != null)
			{
				this.StopMovement();
				UnitDirection targetDirection = this.GetTargetDirection(nearest);
				this.movable.SetDirection(targetDirection);
				this.AttackStart(targetDirection);
			}
		}
		else if (this.movable.InElevator())
		{
			this.animScript.OnStop();
		}
		else
		{
			this.MakeMovement(false);
		}
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x00100840 File Offset: 0x000FEA40
	private void FixedUpdate_Attacking()
	{
		List<UnitModel> list = new List<UnitModel>(this.GetTargets(0.5f, false));
		if (list.Count > 0)
		{
			foreach (UnitModel target in list)
			{
				this.AttackDamage(target);
			}
		}
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x00022C31 File Offset: 0x00020E31
	private void FixedUpdate_ComeBack()
	{
		if (this.movable.InElevator())
		{
			this.animScript.OnStop();
		}
		else
		{
			this.MakeMovement_Back();
		}
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x001008B8 File Offset: 0x000FEAB8
	private void CheckPassage()
	{
		if (this.currentPassage != this.oldPassage)
		{
			this.oldPassage = this.currentPassage;
			this.RemoveBurningPassage();
			if (this.currentPassage != null && this.currentPassage.type != PassageType.VERTICAL && !this.currentPassage.IsIsolate())
			{
				this.AddBurningPassage(this.currentPassage);
			}
		}
		if (this.burningPassage != null)
		{
			this.burningPassage.Process();
		}
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x00100938 File Offset: 0x000FEB38
	private void AttackDamage(UnitModel target)
	{
		if (this.damaged.Contains(target))
		{
			return;
		}
		AgentModel agentModel = target as AgentModel;
		OfficerModel officerModel = target as OfficerModel;
		bool flag = false;
		this.damaged.Add(target);
		DamageInfo attackDmg = FireBird.AttackDmg;
		if (agentModel != null && !agentModel.IsPanic())
		{
			flag = true;
		}
		if (officerModel != null && !officerModel.IsDead())
		{
			flag = true;
		}
		target.TakeDamage(this.model, attackDmg);
		this.animScript.MakeHitSound();
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, attackDmg.type, this.model);
		if (flag)
		{
			if (agentModel != null && agentModel.IsPanic())
			{
				agentModel.Die();
				agentModel.GetWorkerUnit().animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.FireBirdAgentDead, false);
			}
			else if (officerModel != null && officerModel.IsDead())
			{
				officerModel.GetWorkerUnit().animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.FireBirdAgentDead, false);
			}
		}
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x00100A2C File Offset: 0x000FEC2C
	private void AttackStart(UnitDirection dir)
	{
		this.damaged = new List<UnitModel>();
		if (this.currentPassage == null)
		{
			return;
		}
		List<MapNode> list = new List<MapNode>(this.currentPassage.GetNodeList());
		List<MapNode> list2 = list;
		if (FireBird.cache0 == null)
		{
			FireBird.cache0 = new Comparison<MapNode>(MapNode.CompareByX);
		}
		list2.Sort(FireBird.cache0);
		if (list.Count <= 0)
		{
			return;
		}
		if (dir == UnitDirection.RIGHT)
		{
			this.targetNode = list[list.Count - 1];
		}
		else
		{
			this.targetNode = list[0];
		}
		this.animScript.OnAttackStart();
		this._phase = FireBird.Phase.CASTING;
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x00022C59 File Offset: 0x00020E59
	public void OnCastEnd()
	{
		this.SpeedUp();
		if (this.targetNode == null)
		{
			this.AttackEnd();
			return;
		}
		this.Shoot();
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x00100AD0 File Offset: 0x000FECD0
	private void Shoot()
	{
		this._phase = FireBird.Phase.ATTACKING;
		this.model.MoveToNode(this.targetNode);
		CreatureCommand creatureCurrentCmd = this.model.GetCreatureCurrentCmd();
		if (creatureCurrentCmd != null)
		{
			creatureCurrentCmd.SetEndCommand(new CreatureCommand.OnCommandEnd(this.AttackEnd));
		}
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x00022C79 File Offset: 0x00020E79
	private void AttackEnd()
	{
		this._phase = FireBird.Phase.ATTACKEND;
		this.StopMovement();
		this.SpeedDown();
		this.animScript.OnAttackEnd();
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x00022C99 File Offset: 0x00020E99
	public void OnDelayEnd()
	{
		this.skillCoolTimer.StartTimer(FireBird.SkillCoolTime);
		if (this.comeBackTimer.started)
		{
			this._phase = FireBird.Phase.DEFAULT;
		}
		else
		{
			this._phase = FireBird.Phase.COMEBACK;
		}
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x00100B1C File Offset: 0x000FED1C
	private bool CheckHeal(UseSkill skill)
	{
		AgentModel agent = skill.agent;
		return !agent.IsDead() && !agent.IsPanic() && !agent.IsCrazy() && (this.model.qliphothCounter == 1 || agent.hp / (float)agent.maxHp <= 0.2f);
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x00022CCE File Offset: 0x00020ECE
	private void HealStart(AgentModel target)
	{
		this.healTarget = target;
		this.animScript.OnHealStart();
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x00100B84 File Offset: 0x000FED84
	public void OnHeal()
	{
		if (this.healTarget != null && !this.healTarget.IsDead() && !this.healTarget.IsPanic() && !this.healTarget.IsCrazy())
		{
			this.healTarget.RecoverHP((float)this.healTarget.maxHp);
			this.healTarget.AddUnitBuf(new FireBirdHealBuf(this));
		}
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x00022CE2 File Offset: 0x00020EE2
	public void OnHealEnd()
	{
		if (this.model.currentSkill != null)
		{
			this.model.currentSkill.ResumeWorking();
		}
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x00022D04 File Offset: 0x00020F04
	private void AddQliphothCounter()
	{
		if (this.model.qliphothCounter >= this.GetQliphothCounterMax())
		{
			return;
		}
		if (this.model.IsEscaped())
		{
			return;
		}
		this.model.AddQliphothCounter();
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x00022D39 File Offset: 0x00020F39
	private void SubQliphothCounter()
	{
		if (this.model.qliphothCounter <= 0)
		{
			return;
		}
		if (this.model.IsEscaped())
		{
			return;
		}
		this.model.SubQliphothCounter();
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x00100BF8 File Offset: 0x000FEDF8
	private void MakeMovement(bool init = false)
	{
		if (init)
		{
			Sefira sefira = this.model.sefira;
			this.moveTarget = this.GetTargetInSefira(sefira);
		}
		if (this.moveTarget == null)
		{
			this.moveTarget = this.GetTargetInAll();
		}
		else if (!this.CheckMoveCondition(this.moveTarget))
		{
			this.moveTarget = this.GetTargetInAll();
		}
		if (this.moveTarget != null)
		{
			this.model.MoveToMovable(this.moveTarget.GetMovableNode());
		}
		else
		{
			MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
			this.model.MoveToNode(creatureRoamingPoint);
		}
		this.animScript.OnMove();
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x00100CA8 File Offset: 0x000FEEA8
	private void MakeMovement_Back()
	{
		MapNode roomNode = this.model.roomNode;
		if (this.movable.GetCurrentNode() == roomNode)
		{
			this.model.Suppressed();
			this.model.suppressReturnTimer = 9f;
		}
		else
		{
			this.model.MoveToNode(roomNode);
			this.animScript.OnMove();
		}
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x00022D69 File Offset: 0x00020F69
	private void StopMovement()
	{
		this.movable.StopMoving();
		this.animScript.OnStop();
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x00100D0C File Offset: 0x000FEF0C
	private WorkerModel GetTargetInSefira(Sefira sefira)
	{
		List<WorkerModel> list = new List<WorkerModel>(this.GetWorkersInSefira(sefira));
		WorkerModel result;
		if (list.Count > 0)
		{
			result = list[UnityEngine.Random.Range(0, list.Count)];
		}
		else
		{
			result = this.GetTargetInAll();
		}
		return result;
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x00100D54 File Offset: 0x000FEF54
	private WorkerModel GetTargetInAll()
	{
		WorkerModel result = null;
		List<WorkerModel> list = new List<WorkerModel>(this.GetWorkersInAll());
		if (list.Count > 0)
		{
			result = list[UnityEngine.Random.Range(0, list.Count)];
		}
		return result;
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x00100D90 File Offset: 0x000FEF90
	private List<WorkerModel> GetWorkersInSefira(Sefira sefira)
	{
		List<WorkerModel> list = new List<WorkerModel>();
		foreach (AgentModel agentModel in sefira.agentList)
		{
			if (this.CheckMoveCondition(agentModel))
			{
				list.Add(agentModel);
			}
		}
		foreach (OfficerModel officerModel in sefira.officerList)
		{
			if (this.CheckMoveCondition(officerModel))
			{
				list.Add(officerModel);
			}
		}
		return list;
	}

	// Token: 0x06002209 RID: 8713 RVA: 0x00100E58 File Offset: 0x000FF058
	private List<WorkerModel> GetWorkersInAll()
	{
		List<WorkerModel> list = new List<WorkerModel>();
		foreach (Sefira sefira in PlayerModel.instance.GetOpenedAreaList())
		{
			List<WorkerModel> collection = new List<WorkerModel>(this.GetWorkersInSefira(sefira));
			list.AddRange(collection);
		}
		return list;
	}

	// Token: 0x0600220A RID: 8714 RVA: 0x00100EA8 File Offset: 0x000FF0A8
	private bool CheckMoveCondition(WorkerModel worker)
	{
		AgentModel agentModel = worker as AgentModel;
		PassageObjectModel currentPassage = worker.GetMovableNode().currentPassage;
		return !worker.IsDead() && (currentPassage == null || !currentPassage.IsIsolate()) && (agentModel == null || agentModel.currentSkill == null) && !this.IsInRange(worker, 0.5f);
	}

	// Token: 0x0600220B RID: 8715 RVA: 0x00100F10 File Offset: 0x000FF110
	private void AddBurningPassage(PassageObjectModel passage)
	{
		Texture2D sp = Resources.Load("Sprites/CreatureSprite/FireBird/PassageFilter") as Texture2D;
		PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(passage);
		if (passageObject != null)
		{
			GameObject filter = passageObject.SetPassageFilter(sp, "FireBirdBurningFilter", "Normal", 0);
			FireBird.BurningPassage burningPassage = new FireBird.BurningPassage(passage, filter, this);
			if (this.burningPassage != null)
			{
				this.RemoveBurningPassage();
			}
			this.burningPassage = burningPassage;
		}
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x00022D81 File Offset: 0x00020F81
	private void RemoveBurningPassage()
	{
		if (this.burningPassage != null)
		{
			this.burningPassage.Destroy();
			this.burningPassage = null;
		}
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x00100F7C File Offset: 0x000FF17C
	private void SetFilterAlpha()
	{
		int num = this.model.qliphothCounter;
		if (num > this._FILTER_ALPHA.Length - 1)
		{
			num = this._FILTER_ALPHA.Length - 1;
		}
		else if (num < 0)
		{
			num = 0;
		}
		Color renderColor = this.Filter.renderColor;
		renderColor.a = this._FILTER_ALPHA[num];
		this.Filter.renderColor = renderColor;
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x00100FE8 File Offset: 0x000FF1E8
	private void TreeOn()
	{
		if (this.tree != null)
		{
			this.tree.SetActive(true);
		}
		else
		{
			GameObject gameObject = Prefab.LoadPrefab("Unit/ETC/FireBirdTree");
			gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
			gameObject.transform.position = this.model.GetCurrentViewPosition();
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.rotation = Quaternion.identity;
			this.tree = gameObject;
		}
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x00022DA0 File Offset: 0x00020FA0
	private void TreeOff()
	{
		if (this.tree != null)
		{
			this.tree.SetActive(false);
		}
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x00101074 File Offset: 0x000FF274
	private UnitModel GetNearest(float range, bool needDir = true)
	{
		List<UnitModel> targets = this.GetTargets(range, needDir);
		UnitModel result = null;
		float num = 10000f;
		foreach (UnitModel unitModel in targets)
		{
			float distance = this.GetDistance(unitModel);
			if (distance < num)
			{
				result = unitModel;
				num = distance;
			}
		}
		return result;
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x001010F0 File Offset: 0x000FF2F0
	public List<UnitModel> GetTargets(float range, bool needDir = true)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage == null)
		{
			return list;
		}
		foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (this.IsHostile(movableObjectNode))
			{
				if (this.IsInRange(unit, range))
				{
					if (!needDir || this.IsInView(unit))
					{
						list.Add(unit);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x00101188 File Offset: 0x000FF388
	private bool IsInRange(UnitModel target, float range)
	{
		float distance = this.GetDistance(target);
		PassageObjectModel currentPassage = target.GetMovableNode().currentPassage;
		return this.currentPassage != null && currentPassage != null && this.currentPassage == currentPassage && distance <= range;
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000F7CB4 File Offset: 0x000F5EB4
	private bool IsInView(UnitModel target)
	{
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		if (this.model.GetDirection() == UnitDirection.LEFT)
		{
			if (x < x2)
			{
				return false;
			}
		}
		else if (this.model.GetDirection() == UnitDirection.RIGHT && x > x2)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000F7D20 File Offset: 0x000F5F20
	private float GetDistance(UnitModel target)
	{
		float currentScale = this.movable.currentScale;
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		float num = Math.Abs(x - x2);
		return num / currentScale;
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x001011D0 File Offset: 0x000FF3D0
	private bool IsHostile(MovableObjectNode mov)
	{
		UnitModel unit = mov.GetUnit();
		return unit.hp > 0f && unit.IsAttackTargetable() && unit != this.model;
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x00101214 File Offset: 0x000FF414
	private UnitDirection GetTargetDirection(UnitModel target)
	{
		UnitDirection unitDirection = this.model.GetDirection();
		if (!this.IsInView(target))
		{
			unitDirection = ((unitDirection != UnitDirection.LEFT) ? UnitDirection.LEFT : UnitDirection.RIGHT);
		}
		return unitDirection;
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x00101248 File Offset: 0x000FF448
	public void OnNotice(string notice, params object[] param)
	{
		if (!this.model.IsEscaped() && notice == NoticeName.OnEscape)
		{
			CreatureModel creatureModel = param[0] as CreatureModel;
			if (creatureModel != null)
			{
				if (creatureModel is ChildCreatureModel)
				{
					return;
				}
				if (creatureModel is OrdealCreatureModel)
				{
					return;
				}
				if (creatureModel is EventCreatureModel)
				{
					return;
				}
				if (creatureModel.script is SmallBird)
				{
					return;
				}
				if (creatureModel.script is BossBird)
				{
					return;
				}
				if (creatureModel.script is GeburahCoreScript)
				{
					return;
				}
				if (creatureModel.sefira != this.model.sefira)
				{
					return;
				}
				this.SubQliphothCounter();
			}
		}
	}

	// Token: 0x04002145 RID: 8517
	private Timer skillCoolTimer = new Timer();

	// Token: 0x04002146 RID: 8518
	private const float _skillCoolTimeMin = 8f;

	// Token: 0x04002147 RID: 8519
	private const float _skillCoolTimeMax = 10f;

	// Token: 0x04002148 RID: 8520
	private Timer comeBackTimer = new Timer();

	// Token: 0x04002149 RID: 8521
	private const float _comeBackTimeMin = 45f;

	// Token: 0x0400214A RID: 8522
	private const float _comeBackTimeMax = 90f;

	// Token: 0x0400214B RID: 8523
	private const int _ATTACK_DMG_MIN = 90;

	// Token: 0x0400214C RID: 8524
	private const int _ATTACK_DMG_MAX = 110;

	// Token: 0x0400214D RID: 8525
	private const RwbpType _ATTACK_DMG_TYPE = RwbpType.W;

	// Token: 0x0400214E RID: 8526
	private const int _PASSAGE_DMG_MIN = 5;

	// Token: 0x0400214F RID: 8527
	private const int _PASSAGE_DMG_MAX = 5;

	// Token: 0x04002150 RID: 8528
	private const RwbpType _PASSAGE_DMG_TYPE = RwbpType.R;

	// Token: 0x04002151 RID: 8529
	private const int _EQUIPMENT_ID = 200061;

	// Token: 0x04002152 RID: 8530
	private const int _QLIPHOTH_MAX = 3;

	// Token: 0x04002153 RID: 8531
	private const int _HEAL_CONDITION_QLIPHOTH = 1;

	// Token: 0x04002154 RID: 8532
	private const float _MOVEMENT_ATTACK = 20f;

	// Token: 0x04002155 RID: 8533
	private const float _SUB_PROB_NORM = 0.3f;

	// Token: 0x04002156 RID: 8534
	private const float _HEAL_HP_CONDITION = 0.2f;

	// Token: 0x04002157 RID: 8535
	private const float _ARRIVE_RANGE = 0.5f;

	// Token: 0x04002158 RID: 8536
	private const float _ATTACK_DMG_RANGE = 0.5f;

	// Token: 0x04002159 RID: 8537
	private const float _RECOGNIZE_RANGE = 15f;

	// Token: 0x0400215A RID: 8538
	private readonly float[] _WORK_DMG_RATIO = new float[]
	{
		2f,
		2f,
		1.5f,
		1f
	};

	// Token: 0x0400215B RID: 8539
	private readonly float[] _FILTER_ALPHA;

	// Token: 0x0400215C RID: 8540
	private const string _TREE_SRC = "Unit/ETC/FireBirdTree";

	// Token: 0x0400215D RID: 8541
	private const string _FILTER_SRC = "Sprites/CreatureSprite/FireBird/PassageFilter";

	// Token: 0x0400215E RID: 8542
	private const string _FILTER_NAME = "FireBirdBurningFilter";

	// Token: 0x0400215F RID: 8543
	private FireBird.Phase _phase;

	// Token: 0x04002160 RID: 8544
	private WorkerModel healTarget;

	// Token: 0x04002161 RID: 8545
	private WorkerModel moveTarget;

	// Token: 0x04002162 RID: 8546
	private GameObject tree;

	// Token: 0x04002163 RID: 8547
	private PassageObjectModel oldPassage;

	// Token: 0x04002164 RID: 8548
	private MapNode targetNode;

	// Token: 0x04002165 RID: 8549
	private FireBird.BurningPassage burningPassage;

	// Token: 0x04002166 RID: 8550
	private List<UnitModel> damaged;

	// Token: 0x04002167 RID: 8551
	private bool isSuppressed;

	// Token: 0x04002168 RID: 8552
	private bool speedUp;

	// Token: 0x04002169 RID: 8553
	private bool annoyed;

	// <Mod>
	private bool hunted;

	// Token: 0x0400216A RID: 8554
	private FireBirdAnim _animScript;

	// Token: 0x0400216B RID: 8555
	[CompilerGenerated]
	private static Comparison<MapNode> cache0;

	// Token: 0x020003F3 RID: 1011
	public class BurningPassage
	{
		// Token: 0x06002218 RID: 8728 RVA: 0x00022DBF File Offset: 0x00020FBF
		public BurningPassage(PassageObjectModel passage, GameObject filter, FireBird script)
		{
			this.passage = passage;
			this.filter = filter;
			this.script = script;
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x001012F8 File Offset: 0x000FF4F8
		public void Process()
		{
			if (this.script == null)
			{
				return;
			}
			if (!this.isEnable)
			{
				return;
			}
			if (this.script.currentPassage != this.passage)
			{
				this.isEnable = false;
				this.Destroy();
				return;
			}
			float deltaTime = Time.deltaTime;
			List<FireBird.BurningPassage.DamagedUnit> list = new List<FireBird.BurningPassage.DamagedUnit>();
			foreach (FireBird.BurningPassage.DamagedUnit damagedUnit in this.list)
			{
				damagedUnit.remain -= deltaTime;
				if (damagedUnit.expired)
				{
					list.Add(damagedUnit);
				}
			}
			foreach (FireBird.BurningPassage.DamagedUnit item in list)
			{
				this.list.Remove(item);
			}
			List<UnitModel> list2 = new List<UnitModel>(this.script.GetTargets(float.MaxValue, false));
			foreach (UnitModel unitModel in list2)
			{
				if (!this.CheckDamaged(unitModel))
				{
					FireBird.BurningPassage.DamagedUnit item2 = new FireBird.BurningPassage.DamagedUnit
					{
						unit = unitModel,
						remain = 1f
					};
					WorkerModel workerModel = unitModel as WorkerModel;
					this.list.Add(item2);
					unitModel.TakeDamage(FireBird.PassageDmg);
					DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unitModel, FireBird.PassageDmg.type, this.script.model);
					if (workerModel != null && workerModel.IsDead())
					{
						workerModel.GetWorkerUnit().animChanger.ChangeAnimatorWithUniqueFace(WorkerSpine.AnimatorName.FireBirdAgentDead, false);
					}
				}
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x001014F8 File Offset: 0x000FF6F8
		private bool CheckDamaged(UnitModel target)
		{
			bool result = false;
			foreach (FireBird.BurningPassage.DamagedUnit damagedUnit in this.list)
			{
				if (damagedUnit.unit == target)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00022DEE File Offset: 0x00020FEE
		public PassageObjectModel GetPassage()
		{
			return this.passage;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00022DF6 File Offset: 0x00020FF6
		public void Destroy()
		{
			if (this.filter != null)
			{
				UnityEngine.Object.Destroy(this.filter);
			}
		}

		// Token: 0x0400216C RID: 8556
		private const float defaultRemain = 1f;

		// Token: 0x0400216D RID: 8557
		private FireBird script;

		// Token: 0x0400216E RID: 8558
		private PassageObjectModel passage;

		// Token: 0x0400216F RID: 8559
		private GameObject filter;

		// Token: 0x04002170 RID: 8560
		private bool isEnable = true;

		// Token: 0x04002171 RID: 8561
		private List<FireBird.BurningPassage.DamagedUnit> list = new List<FireBird.BurningPassage.DamagedUnit>();

		// Token: 0x020003F4 RID: 1012
		private class DamagedUnit
		{
			// Token: 0x0600221D RID: 8733 RVA: 0x00022E14 File Offset: 0x00021014
			public DamagedUnit()
			{
			}

			// Token: 0x170002D7 RID: 727
			// (get) Token: 0x0600221E RID: 8734 RVA: 0x00022E27 File Offset: 0x00021027
			public bool expired
			{
				get
				{
					return this.remain <= 0f;
				}
			}

			// Token: 0x04002172 RID: 8562
			public UnitModel unit;

			// Token: 0x04002173 RID: 8563
			public float remain = 1f;
		}
	}

	// Token: 0x020003F5 RID: 1013
	public enum Phase
	{
		// Token: 0x04002175 RID: 8565
		DEFAULT,
		// Token: 0x04002176 RID: 8566
		CASTING,
		// Token: 0x04002177 RID: 8567
		ATTACKING,
		// Token: 0x04002178 RID: 8568
		ATTACKEND,
		// Token: 0x04002179 RID: 8569
		SUPPRESSED,
		// Token: 0x0400217A RID: 8570
		COMEBACK
	}
}
