using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class BigBadWolf : CreatureBase, IObserver
{
	// Token: 0x06001CEF RID: 7407 RVA: 0x000EC334 File Offset: 0x000EA534
	public BigBadWolf()
	{
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0001F1B2 File Offset: 0x0001D3B2
	public DamageInfo BiteDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("1");
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x0001F1CE File Offset: 0x0001D3CE
	public DamageInfo ScratchDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("2");
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0001F1EA File Offset: 0x0001D3EA
	public DamageInfo ChargeDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("3");
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x0001F206 File Offset: 0x0001D406
	public BigBadWolfAnim AnimScript
	{
		get
		{
			return base.Unit.animTarget as BigBadWolfAnim;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x0001F218 File Offset: 0x0001D418
	// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x0001F22A File Offset: 0x0001D42A
	private float SenseRadius
	{
		get
		{
			return this.AnimScript.sense.GetRadius();
		}
		set
		{
			if (value < 1f)
			{
				this.AnimScript.sense.SetRadius(1f);
				return;
			}
			this.AnimScript.sense.SetRadius(value);
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0001F25E File Offset: 0x0001D45E
	private float SenseRadiusDouble
	{
		get
		{
			return this.GetDoubleValue(this.SenseRadius);
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x0001F26C File Offset: 0x0001D46C
	public DamageInfo _defaultHowlingDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("4");
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0001F288 File Offset: 0x0001D488
	private Vector3 CurrentPos
	{
		get
		{
			return this.movable.GetCurrentViewPosition();
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0001F295 File Offset: 0x0001D495
	private bool IsRedHoodExist
	{
		get
		{
			return this._redHood != null;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0001F2A3 File Offset: 0x0001D4A3
	private bool IsRedMoon
	{
		get
		{
			return this.model.hp <= (float)this.model.maxHp * 0.5f;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001CFB RID: 7419 RVA: 0x0001F2C7 File Offset: 0x0001D4C7
	public BigBadWolf.WolfAttackType CurrentAttackType
	{
		get
		{
			return this._currentAttackType;
		}
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x0001F2CF File Offset: 0x0001D4CF
	public static void Log(string log)
	{
		Debug.Log(string.Format("<color={0}>[BigBadWolf]</color>{1}", "#3211DFFF", log));
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x000EC39C File Offset: 0x000EA59C
	public void AddGift(AgentModel agent)
	{
		EGOgiftModel gift = EGOgiftModel.MakeGift(EquipmentTypeList.instance.GetData(1033));
		agent.AttachEGOgift(gift);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x0001F2E6 File Offset: 0x0001D4E6
	public override void OnInit()
	{
		this.damageModule.stdDamage = 300f;
		this.damageModule.onFilled = new BigBadWolf.DamageCumlative.OnFilled(this.OnDamageFilled);
		this.ParamInit();
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x0001F315 File Offset: 0x0001D515
	public override void OnStageStart()
	{
		this.ParamInit();
		this.FindRedHood();
		this.eatenWorker.Clear();
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x000EC3C8 File Offset: 0x000EA5C8
	private void FindRedHood()
	{
		CreatureModel[] creatureList = CreatureManager.instance.GetCreatureList();
		foreach (CreatureModel creatureModel in creatureList)
		{
			if (creatureModel.script is RedHood)
			{
				this._redHood = (creatureModel.script as RedHood);
				break;
			}
		}
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x0001A435 File Offset: 0x00018635
	public override void OnStageEnd()
	{
		this.ParamInit();
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x0001A435 File Offset: 0x00018635
	public override void OnStageRelease()
	{
		this.ParamInit();
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x0001F32E File Offset: 0x0001D52E
	public override float GetRadius()
	{
		return 2f;
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x000EC420 File Offset: 0x000EA620
	public override void RoomSpriteInit()
	{
		base.RoomSpriteInit();
		Sprite renderSprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/_Special/" + this.model.metadataId + "/eat1");
		Sprite renderSprite2 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/_Special/" + this.model.metadataId + "/eat2");
		Sprite renderSprite3 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/_Special/" + this.model.metadataId + "/spit1");
		Sprite renderSprite4 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/_Special/" + this.model.metadataId + "/spit2");
		this.eatFilter_1 = base.Unit.room.AddFilter(base.Unit.room.StateFilter);
		this.eatFilter_2 = base.Unit.room.AddFilter(this.eatFilter_1);
		this.vomitFilter_1 = base.Unit.room.AddFilter(base.Unit.room.StateFilter);
		this.vomitFilter_2 = base.Unit.room.AddFilter(this.vomitFilter_1);
		this.eatFilter_1.renderSprite = renderSprite;
		this.eatFilter_1.hasSpecialAnimKey = true;
		this.eatFilter_1.specialAnimKey = "Display";
		this.eatFilter_1.transform.localPosition = this.eatFilter_1.transform.localPosition + new Vector3(0f, 0f, -1f);
		this.eatFilter_2.renderSprite = renderSprite2;
		this.eatFilter_2.hasSpecialAnimKey = true;
		this.eatFilter_2.specialAnimKey = "Display";
		this.eatFilter_2.transform.localPosition = this.eatFilter_2.transform.localPosition + new Vector3(0f, 0f, -2f);
		this.vomitFilter_1.renderSprite = renderSprite3;
		this.vomitFilter_1.hasSpecialAnimKey = true;
		this.vomitFilter_1.specialAnimKey = "Display";
		this.vomitFilter_1.transform.localPosition = this.vomitFilter_1.transform.localPosition + new Vector3(0f, 0f, -1f);
		this.vomitFilter_2.renderSprite = renderSprite4;
		this.vomitFilter_2.hasSpecialAnimKey = true;
		this.vomitFilter_2.specialAnimKey = "Display";
		this.vomitFilter_2.transform.localPosition = this.vomitFilter_2.transform.localPosition + new Vector3(0f, 0f, -2f);
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x0001F335 File Offset: 0x0001D535
	public override void OnEnterRoom(UseSkill skill)
	{
		if (this.IsRedHood(skill.agent.GetRecentWorkedCreature()))
		{
			this._activateEvent = true;
			return;
		}
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x0001F355 File Offset: 0x0001D555
	public override void OnFinishWork(UseSkill skill)
	{
		if (skill.skillTypeInfo.rwbpType == RwbpType.R)
		{
			if (this.eatenWorker.Count > 0)
			{
				this.VomitStart();
			}
			return;
		}
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x000EC6D0 File Offset: 0x000EA8D0
	public override void OnWorkClosed(UseSkill skill, int successCount)
	{
		if (skill.agent.IsDead() || skill.agent.IsCrazy())
		{
			this._activateEvent = false;
			return;
		}
		if (this.model.metaInfo.feelingStateCubeBounds.CalculateFeelingState(successCount) == CreatureFeelingState.BAD && skill.skillTypeInfo.rwbpType != RwbpType.R)
		{
			this._activateEvent = true;
		}
		if (this._activateEvent)
		{
			this.ActivateEatEvent(skill.agent);
		}
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x0001F380 File Offset: 0x0001D580
	public void OnDamageFilled()
	{
		this.Getaway();
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x000EC750 File Offset: 0x000EA950
	private void ActivateEatEvent(AgentModel target)
	{
		target.LoseControl();
		Uncontrollable_WolfEaten uncontrollable_WolfEaten = new Uncontrollable_WolfEaten(target, this);
		target.SetUncontrollableAction(uncontrollable_WolfEaten);
		if (target.HasUnitBuf(UnitBufType.OTHER_WORLD_PORTRAIT_VICTIM))
		{
			CreatureModel creatureModel = CreatureManager.instance.FindCreature(300104L);
			if (creatureModel != null)
			{
				(creatureModel.script as OtherWorldPortrait).ReleaseVictim(target);
			}
		}
		if (target.HasUnitBuf(UnitBufType.OTHER_WORLD_PORTRAIT))
		{
		}
		this._filterTimer.StartTimer(20f);
		this._filterTimer.SetEndCmd(new Timer.OnTimerRunningEnd(this.OnEatAnimEnd));
		this.eatFilter_1.Activated = true;
		this._filterElapSave = 0f;
		this.eatenWorker.Add(uncontrollable_WolfEaten);
		this.AnimScript.SetEatenSprite(this.eatenWorker.Count);
		this._filterType = BigBadWolf.FilterType.EAT;
		this._activateEvent = false;
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x0001F388 File Offset: 0x0001D588
	private void OnEatAnimEnd()
	{
		BigBadWolf.Log("Eat Filter Closed");
		this.model.SubQliphothCounter();
		this.AnimScript.SetWolfAnimState(3);
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000EC824 File Offset: 0x000EAA24
	private void VomitStart()
	{
		this._filterTimer.StartTimer(20f);
		this._filterTimer.SetEndCmd(new Timer.OnTimerRunningEnd(this.VomitEatenWorker));
		this.vomitFilter_1.Activated = true;
		this._filterElapSave = 0f;
		this._filterType = BigBadWolf.FilterType.VOMIT;
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000EC878 File Offset: 0x000EAA78
	private void VomitEatenWorker()
	{
		foreach (Uncontrollable_WolfEaten uncontrollable_WolfEaten in this.eatenWorker)
		{
			uncontrollable_WolfEaten.OnRelease(true);
			uncontrollable_WolfEaten.agent.GetControl();
		}
		this.eatenWorker.Clear();
		this.AnimScript.SetEatenSprite(this.eatenWorker.Count);
		this.AnimScript.SetWolfAnimState(1);
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x0001F3AB File Offset: 0x0001D5AB
	public override void OnViewInit(CreatureUnit unit)
	{
		this.AnimScript.SetScript(this);
		this._entryPassage = this.model.GetEntryNode().GetAttachedPassage();
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000EC90C File Offset: 0x000EAB0C
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this._filterTimer.started)
		{
			float elapsed = this._filterTimer.elapsed;
			bool flag = false;
			if (elapsed - this._filterElapSave > 1f)
			{
				this._filterElapSave = elapsed;
				flag = true;
			}
			if (this._filterType == BigBadWolf.FilterType.EAT)
			{
				if (flag)
				{
					this.eatFilter_2.Activated = !this.eatFilter_2.Activated;
					this.eatFilter_1.Activated = !this.eatFilter_2.Activated;
				}
			}
			else if (flag)
			{
				this.vomitFilter_2.Activated = !this.vomitFilter_2.Activated;
				this.vomitFilter_1.Activated = !this.vomitFilter_2.Activated;
			}
			if (this._filterTimer.RunTimer())
			{
				IsolateFilter isolateFilter = this.eatFilter_2;
				bool activated = false;
				this.eatFilter_1.Activated = activated;
				isolateFilter.Activated = activated;
				IsolateFilter isolateFilter2 = this.vomitFilter_2;
				activated = false;
				this.vomitFilter_1.Activated = activated;
				isolateFilter2.Activated = activated;
			}
		}
		if (!this.model.IsEscaped())
		{
			this.CheckPassage();
			if (this._qlipothSubCount > 0 && !this.model.IsWorkingState())
			{
				for (int i = 0; i < this._qlipothSubCount; i++)
				{
					this.model.SubQliphothCounter();
				}
				this._qlipothSubCount = 0;
			}
		}
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x000ECA74 File Offset: 0x000EAC74
	private void CheckPassage()
	{
		foreach (MovableObjectNode movableObjectNode in this._entryPassage.GetEnteredTargets())
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (!this.CheckedUnit.Contains(unit))
			{
				if (unit.IsAttackTargetable())
				{
					if (this.IsRedHood(unit))
					{
						this._qlipothSubCount = 2;
						break;
					}
					if (unit.HasUnitBuf(UnitBufType.REDHOOD_BLEED))
					{
						this.CheckedUnit.Add(unit);
						this._qlipothSubCount++;
					}
				}
			}
		}
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000ECB38 File Offset: 0x000EAD38
	public override void ParamInit()
	{
		this.damageModule.curDamage = 0f;
		this._filterElapSave = 0f;
		this._qlipothSubCount = 0;
		this._isAttacking = false;
		this._isMovingToSefira = false;
		this._isApproaching = false;
		this._activateEvent = false;
		this._isRedHoodSuppressed = false;
		this._currentMoveTargetNode = null;
		this._currentMoveTargetMovable = null;
		this._nextGetawaySefira = null;
		this._howlingDelayTimer.StopTimer();
		this.model.ResetQliphothCounter();
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x0001F3CF File Offset: 0x0001D5CF
	public override void ActivateQliphothCounter()
	{
		BigBadWolf.Log("Escape");
		this.ReadyForEscape();
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x0001F3E1 File Offset: 0x0001D5E1
	private void ReadyForEscape()
	{
		this.AnimScript.OnEscape();
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x0001F3EE File Offset: 0x0001D5EE
	public void OnReadyForEscape()
	{
		this.model.Escape();
		this.model.hp = (float)this.model.maxHp * 0.8f;
		this.InitialEscapeAction();
		this._isMovingToSefira = true;
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x0001F425 File Offset: 0x0001D625
	public void InitialEscapeAction()
	{
		this.MakeMovement(this.model.sefiraOrigin.sefiraPassage.centerNode, new CreatureCommand.OnCommandEnd(this.InitialEscapeArrived));
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x0001F44E File Offset: 0x0001D64E
	public void InitialEscapeArrived()
	{
		this._currentAttackType = BigBadWolf.WolfAttackType.HOWLING;
		this.Howling();
		this._isMovingToSefira = false;
		this._isAttacking = true;
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x0001F46B File Offset: 0x0001D66B
	private bool IsRedHood(UnitModel target, out RedHood redHood)
	{
		redHood = null;
		if (target == null)
		{
			return false;
		}
		if (this.IsRedHoodExist && target == this._redHood.model)
		{
			redHood = this._redHood;
			return true;
		}
		return false;
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0001F49F File Offset: 0x0001D69F
	private bool IsRedHood(UnitModel target)
	{
		return this.IsRedHoodExist && target == this._redHood.model;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x0001F4C0 File Offset: 0x0001D6C0
	private void Approach(UnitModel target)
	{
		this._isApproaching = true;
		this._currentApproachingTarget = target;
		this.MakeMovement(target.GetMovableNode(), null);
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000ECBB8 File Offset: 0x000EADB8
	private bool CheckRange(UnitModel target, float range, bool ignoreDirection = true)
	{
		float num = this.movable.GetDistanceDouble(target.GetMovableNode());
		float radius = target.radius;
		if (radius > 0f)
		{
			num -= radius * radius;
		}
		bool flag = num <= range * range;
		if (ignoreDirection)
		{
			return flag;
		}
		UnitDirection direction = this.movable.GetDirection();
		float x = this.movable.GetCurrentViewPosition().x;
		float x2 = target.GetCurrentViewPosition().x;
		if (direction == UnitDirection.RIGHT)
		{
			return flag && x <= x2;
		}
		return flag && x >= x2;
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000ECC60 File Offset: 0x000EAE60
	public override void UniqueEscape()
	{
		if (this.IsRedMoon)
		{
			if (!this.AnimScript.IsRedMoon())
			{
				this.AnimScript.SetMoonState(true);
			}
		}
		else if (this.AnimScript.IsRedMoon())
		{
			this.AnimScript.SetMoonState(false);
		}
		if (!this._howlingDelayTimer.started || this._howlingDelayTimer.RunTimer())
		{
		}
		if (this._getawayGroggyTimer.started && this._getawayGroggyTimer.RunTimer())
		{
			this._isMovingToSefira = false;
			this.AnimScript.SetGroggy(false);
		}
		if (this._castingGroggyTimer.started && this._castingGroggyTimer.RunTimer())
		{
			this.AnimScript.OnCastingAttackEnd();
			this._isAttacking = false;
		}
		if (this._isMovingToSefira)
		{
			return;
		}
		if (this._isApproaching)
		{
			if (this._currentApproachingTarget == null)
			{
				this._isApproaching = false;
			}
			else
			{
				PassageObjectModel passage = this._currentApproachingTarget.GetMovableNode().GetPassage();
				if (passage != null && this.currentPassage == passage && this.CheckRange(this._currentApproachingTarget, 4f, true))
				{
					this._isApproaching = false;
					this.movable.SetDirection(this.GetDirection(this._currentApproachingTarget));
					this.AnimScript.OnAttack(this.CurrentAttackType);
				}
			}
			if (this._currentApproachingTarget != null && !this.movable.IsMoving() && !this.movable.IsMoving())
			{
				this.MakeMovement(this._currentApproachingTarget.GetMovableNode(), null);
			}
			if (this._isApproaching)
			{
				return;
			}
		}
		List<UnitModel> nearTarget = this.GetNearTarget();
		if (nearTarget.Count > 0)
		{
			if (!this._isAttacking)
			{
				if (this.movable.IsMoving())
				{
					this.StopMovement(false);
				}
				this.movable.SetDirection(this.GetDirection(nearTarget[0]));
				this.StartAttack(nearTarget[UnityEngine.Random.Range(0, nearTarget.Count)]);
			}
		}
		else if (!this._isAttacking && !this.movable.IsMoving())
		{
			this.MakeMovement(MapGraph.instance.GetRoamingNodeByRandom(), null);
		}
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000ECEB0 File Offset: 0x000EB0B0
	private List<UnitModel> GetNearTarget()
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage == null)
		{
			return list;
		}
		foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (unit != null)
			{
				if (!(unit is WorkerModel) || !(unit as WorkerModel).IsDead())
				{
					if (unit.IsAttackTargetable())
					{
						list.Add(unit);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000ECF48 File Offset: 0x000EB148
	private void StartAttack(UnitModel mainTarget)
	{
		this._isAttacking = true;
		if (this._isRedHoodSuppressed && UnityEngine.Random.value <= 0.4f)
		{
			this._currentAttackType = BigBadWolf.WolfAttackType.CASTING;
			this.OnCastingAttack();
			return;
		}
		if (UnityEngine.Random.value <= 0.5f)
		{
			this._currentAttackType = BigBadWolf.WolfAttackType.BITE;
		}
		else
		{
			this._currentAttackType = BigBadWolf.WolfAttackType.SCRATCH;
		}
		if (!this._howlingDelayTimer.started && UnityEngine.Random.value <= 0.2f)
		{
			this._currentAttackType = BigBadWolf.WolfAttackType.HOWLING;
		}
		if (this.CurrentAttackType == BigBadWolf.WolfAttackType.HOWLING)
		{
			this.Howling();
			return;
		}
		if (!this.CheckRange(mainTarget, 4f, true))
		{
			this.Approach(mainTarget);
			return;
		}
		this.movable.SetDirection(this.GetDirection(mainTarget));
		this.AnimScript.OnAttack(this.CurrentAttackType);
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x000ED01C File Offset: 0x000EB21C
	private UnitDirection GetDirection(UnitModel unit)
	{
		return (this.movable.GetCurrentViewPosition().x > unit.GetCurrentViewPosition().x) ? UnitDirection.LEFT : UnitDirection.RIGHT;
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000ED058 File Offset: 0x000EB258
	public void OnCastingAttack()
	{
		if (this.currentPassage == null)
		{
			this._isAttacking = false;
			return;
		}
		MapNode castingNode = null;
		MapNode[] nodeList = this.currentPassage.GetNodeList();
		Vector3 currentViewPosition = this.movable.GetCurrentViewPosition();
		float num = -1f;
		foreach (MapNode mapNode in nodeList)
		{
			float num2 = Mathf.Abs(mapNode.GetPosition().x - currentViewPosition.x);
			if (num2 >= num)
			{
				castingNode = mapNode;
				num = num2;
			}
		}
		this._castingNode = castingNode;
		this.AnimScript.OnCastingTrigger();
		this.MakeSound("Casting");
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x0001F4DD File Offset: 0x0001D6DD
	public void OnCastingReady()
	{
		this.model.movementScale = 0.1f;
		this.MakeMovement(this._castingNode, new CreatureCommand.OnCommandEnd(this.OnCastingArrived));
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x0001F507 File Offset: 0x0001D707
	public void OnCastingArrived()
	{
		this.model.movementScale = 1f;
		this._castingGroggyTimer.StartTimer(3f);
		this.AnimScript.OnCastingEnd();
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x0001F534 File Offset: 0x0001D734
	public void OnAttackEnd()
	{
		if (this.CurrentAttackType == BigBadWolf.WolfAttackType.HOWLING && this._nextGetawaySefira == null)
		{
			this.SetGetawaySefira();
		}
		this._isAttacking = false;
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x000ED108 File Offset: 0x000EB308
	public void MakeScratchEffect()
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/BigBadWolf/Slash");
		Vector3 a = new Vector3(3f, 3f, 6f);
		Vector3 position = this.AnimScript._effectHeight.transform.position;
		if (this.movable.GetDirection() == UnitDirection.LEFT)
		{
			a.x = -a.x;
		}
		position.z = -1f;
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.localScale = a * this.movable.currentScale;
		gameObject.transform.localPosition = position;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000ED1CC File Offset: 0x000EB3CC
	public void MakeBiteEffect()
	{
		string name = string.Format("Effect/Creature/BigBadWolf/Charge1-{0}", (this.movable.GetDirection() != UnitDirection.RIGHT) ? "Left" : "Right");
		GameObject gameObject = Prefab.LoadPrefab(name);
		Vector3 a = Vector3.one * 2f;
		Vector3 position = this.AnimScript._effectHeight.transform.position;
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.localScale = a * this.movable.currentScale;
		gameObject.transform.localPosition = position;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000ED284 File Offset: 0x000EB484
	public void OnDamage()
	{
		DamageInfo dmg = new DamageInfo(RwbpType.R, 5f);
		if (this.CurrentAttackType == BigBadWolf.WolfAttackType.BITE)
		{
			dmg = this.BiteDamage;
			this.MakeBiteEffect();
			this.MakeSound("Bite");
		}
		else if (this.CurrentAttackType == BigBadWolf.WolfAttackType.SCRATCH)
		{
			dmg = this.ScratchDamage;
			this.MakeScratchEffect();
			this.MakeSound("Scratch");
		}
		else if (this.CurrentAttackType == BigBadWolf.WolfAttackType.CASTING)
		{
			dmg = this.ChargeDamage;
		}
		List<UnitModel> damagedTarget = this.GetDamagedTarget(this.GetNearTarget(), 4f, this.CurrentAttackType == BigBadWolf.WolfAttackType.CASTING);
		foreach (UnitModel unitModel in damagedTarget)
		{
			if (unitModel.IsAttackTargetable())
			{
				if (unitModel != this.model)
				{
					unitModel.TakeDamage(this.model, dmg);
					if (this.CurrentAttackType == BigBadWolf.WolfAttackType.CASTING)
					{
						this.MakeCastingAttackEffect(unitModel);
					}
				}
			}
		}
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000ED3A4 File Offset: 0x000EB5A4
	private void MakeCastingAttackEffect(UnitModel unit)
	{
		string name = string.Format("Effect/Creature/BigBadWolf/Scratch{0}", UnityEngine.Random.Range(1, 3));
		GameObject gameObject = Prefab.LoadPrefab(name);
		Vector3 currentViewPosition = unit.GetCurrentViewPosition();
		currentViewPosition.y = 2f * unit.GetMovableNode().currentScale;
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.localPosition = currentViewPosition;
		gameObject.transform.localScale = Vector3.one * 2f * unit.GetMovableNode().currentScale;
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x000ED43C File Offset: 0x000EB63C
	private List<UnitModel> GetDamagedTarget(List<UnitModel> near, float range, bool ignoreDir = false)
	{
		List<UnitModel> list = new List<UnitModel>();
		foreach (UnitModel unitModel in near)
		{
			if (this.CheckRange(unitModel, range, ignoreDir))
			{
				list.Add(unitModel);
			}
		}
		return list;
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x0001F55A File Offset: 0x0001D75A
	public override bool IsWorkable()
	{
		return !this._filterTimer.started;
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x0001F56A File Offset: 0x0001D76A
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return !this._isMovingToSefira;
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x000ED4A8 File Offset: 0x000EB6A8
	private void Howling()
	{
		RedHood redHood = null;
		foreach (UnitModel unitModel in this.GetTargetsInRange())
		{
			if (unitModel != null)
			{
				if (unitModel != this.model)
				{
					if (unitModel is CreatureModel)
					{
						if (!this.IsRedHood(unitModel, out redHood))
						{
							CreatureModel creatureModel = unitModel as CreatureModel;
							if (!creatureModel.IsEscaped())
							{
								if (this.IsRedMoon && creatureModel.script.HasRoomCounter())
								{
									creatureModel.SubQliphothCounter();
								}
							}
							continue;
						}
						redHood.OnHowlingAttacked();
					}
					if (unitModel.IsAttackTargetable())
					{
						if (unitModel is AgentModel)
						{
							this.HowlingDamageToAgent(unitModel as AgentModel);
						}
						else
						{
							unitModel.TakeDamage(this.model, this._defaultHowlingDamage);
						}
					}
				}
			}
		}
		this.AnimScript.Howling();
		this.MakeSound("Howl");
		this._howlingDelayTimer.StartTimer(20f);
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000ED5E4 File Offset: 0x000EB7E4
	private void HowlingDamageToAgent(AgentModel target)
	{
		float num = 1f - Mathf.Sqrt(this.GetDistDouble(target)) / this.SenseRadius;
		float num2 = 1.75f - 0.25f * (float)target.fortitudeLevel;
		float num3 = this._defaultHowlingDamage.GetDamage() * num * num2;
		DamageInfo dmg = new DamageInfo(RwbpType.W, num3);
		target.TakeDamage(this.model, dmg);
		this.Recover(num3);
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x0001F575 File Offset: 0x0001D775
	private void Recover(float recoverValue)
	{
		BigBadWolf.Log("Recover : " + recoverValue);
		this.model.hp += recoverValue;
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x000ED64C File Offset: 0x000EB84C
	public override bool UniqueMoveControl()
	{
		if (this.movable.IsMoving())
		{
			if (this.movable.currentPassage != null)
			{
				if (this.movable.currentPassage.type == PassageType.VERTICAL)
				{
					this.AnimScript.animator.SetBool("Move", false);
				}
				else if (this.movable.IsNextElevator)
				{
					this.AnimScript.animator.SetBool("Move", false);
				}
				else
				{
					this.AnimScript.animator.SetBool("Move", true);
				}
			}
			else
			{
				this.AnimScript.animator.SetBool("Move", true);
			}
		}
		else
		{
			this.AnimScript.animator.SetBool("Move", false);
		}
		return true;
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x000ED724 File Offset: 0x000EB924
	private void SetGetawaySefira()
	{
		List<Sefira> list = new List<Sefira>(SefiraManager.instance.GetOpendSefiraList());
		if (list.Count > 1)
		{
			if (this._nextGetawaySefira != null)
			{
				list.Remove(this._nextGetawaySefira);
			}
			else
			{
				list.Remove(this.model.sefiraOrigin);
			}
			this._nextGetawaySefira = list[UnityEngine.Random.Range(0, list.Count)];
		}
		else
		{
			this._nextGetawaySefira = this.model.sefiraOrigin;
		}
		this.AnimScript.SetMoon(this._nextGetawaySefira);
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x000ED7BC File Offset: 0x000EB9BC
	private void Getaway()
	{
		BigBadWolf.Log("Getaway");
		if (this._nextGetawaySefira == null)
		{
			this.SetGetawaySefira();
		}
		Sefira nextGetawaySefira = this._nextGetawaySefira;
		if (nextGetawaySefira == null)
		{
			return;
		}
		MapNode[] nodeList = nextGetawaySefira.sefiraPassage.GetNodeList();
		MapNode centerNode = nextGetawaySefira.sefiraPassage.centerNode;
		this.MakeMovement(centerNode, new CreatureCommand.OnCommandEnd(this.OnArriveGetaway));
		this._isMovingToSefira = true;
		this.AnimScript.SetGetaway(true);
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x000ED830 File Offset: 0x000EBA30
	public void OnArriveGetaway()
	{
		this.AnimScript.SetGetaway(false);
		if (this.movable.currentNode == this._currentMoveTargetNode)
		{
			this._getawayGroggyTimer.StartTimer(7.5f);
			this.AnimScript.SetGroggy(true);
		}
		this.SetGetawaySefira();
		this._isAttacking = false;
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x0001F59F File Offset: 0x0001D79F
	public void MakeMovement(MapNode targetNode, CreatureCommand.OnCommandEnd commandEnd = null)
	{
		this.model.MoveToNode(targetNode);
		if (this._currentMoveTargetMovable != null)
		{
			this._currentMoveTargetMovable = null;
		}
		this._currentMoveTargetNode = targetNode;
		if (commandEnd != null)
		{
			this.model.GetCurrentCommand().SetEndCommand(commandEnd);
		}
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0001F5DD File Offset: 0x0001D7DD
	public void MakeMovement(MovableObjectNode movable, CreatureCommand.OnCommandEnd commandEnd = null)
	{
		this.model.MoveToMovable(movable);
		if (this._currentMoveTargetNode != null)
		{
			this._currentMoveTargetNode = null;
		}
		this._currentMoveTargetMovable = movable;
		if (commandEnd != null)
		{
			this.model.GetCurrentCommand().SetEndCommand(commandEnd);
		}
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0001F61B File Offset: 0x0001D81B
	public void StopMovement(bool clearTarget = false)
	{
		if (clearTarget)
		{
			this._currentMoveTargetMovable = null;
			this._currentMoveTargetNode = null;
		}
		this.model.ClearCommand();
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x0001F63C File Offset: 0x0001D83C
	private Sefira GetCurrentStandingSefira()
	{
		if (this.currentPassage == null)
		{
			return null;
		}
		return SefiraManager.instance.GetSefira(this.currentPassage.GetSefiraName());
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x0001F660 File Offset: 0x0001D860
	private float GetDistDouble(UnitModel target)
	{
		return this.movable.GetDistanceDouble(target.GetMovableNode());
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x0001F673 File Offset: 0x0001D873
	private float GetDoubleValue(float val)
	{
		return val * val;
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x0001F678 File Offset: 0x0001D878
	private List<UnitModel> GetTargetsInRange()
	{
		return this.AnimScript.sense.GetEnteredUnit(new BigBadWolfSense.UnitType[0]);
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x000ED890 File Offset: 0x000EBA90
	public void AddDefenseBuf(UnitModel target, float factor, float lifeTime = -1f)
	{
		if (target.HasUnitBuf(UnitBufType.WOLF_DEFENSE))
		{
			(target.GetUnitBufByType(UnitBufType.WOLF_DEFENSE) as WolfDefenseBuf).SetFactor(factor);
		}
		else if (lifeTime == -1f)
		{
			target.AddUnitBuf(new WolfDefenseBuf(factor));
		}
		else
		{
			target.AddUnitBuf(new WolfDefenseBuf(factor, lifeTime));
		}
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x000ED8F0 File Offset: 0x000EBAF0
	public void OnRedHoodSuppressed()
	{
		if (!this.model.IsEscapedOnlyEscape())
		{
			this.model.hp = (float)this.model.maxHp;
			this._isRedHoodSuppressed = true;
			foreach (Uncontrollable_WolfEaten uncontrollable_WolfEaten in this.eatenWorker)
			{
				uncontrollable_WolfEaten.agent.Die();
			}
			this.eatenWorker.Clear();
		}
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x000ED98C File Offset: 0x000EBB8C
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		if (this.model.hp <= 0f)
		{
			RedHood redHood = null;
			if (this.IsRedHood(actor, out redHood))
			{
				this.OnSuppressedByRedHood();
				this._redHood.OnWolfSuppressedByRedHood();
				this.MakeSound("Howl");
			}
			else if (this._redHood != null)
			{
				this._redHood.OnWolfSuppressedByOther();
			}
		}
		else
		{
			this.damageModule.OnTakeDamage(value);
		}
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x0001F690 File Offset: 0x0001D890
	public override void OnReturn()
	{
		this.ParamInit();
		this.AnimScript.ResetAnim();
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x00003FDD File Offset: 0x000021DD
	public void OnNotice(string notice, params object[] param)
	{
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x000EDA08 File Offset: 0x000EBC08
	public string GetSoundSrc(string key)
	{
		string empty = string.Empty;
		if (!this.model.metaInfo.soundTable.TryGetValue(key, out empty))
		{
		}
		return empty;
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x0001F6A3 File Offset: 0x0001D8A3
	public override void ResetQliphothCounter()
	{
		base.ResetQliphothCounter();
		this.CheckedUnit.Clear();
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x000EDA3C File Offset: 0x000EBC3C
	public override SoundEffectPlayer MakeSound(string src)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		return SoundEffectPlayer.PlayOnce(soundSrc, this.model.Unit.transform.position);
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x000EDA84 File Offset: 0x000EBC84
	public void OnSuppressedByRedHood()
	{
		if (this.eatenWorker.Count > 0)
		{
			foreach (Uncontrollable_WolfEaten uncontrollable_WolfEaten in this.eatenWorker)
			{
				uncontrollable_WolfEaten.OnRelease(false);
				uncontrollable_WolfEaten.agent.GetControl();
			}
			this.eatenWorker.Clear();
		}
		foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
		{
			if (!agentModel.IsDead())
			{
				if (!agentModel.IsPanic())
				{
					agentModel.AddUnitBuf(new WolfDefenseBuf(0.9f));
				}
			}
		}
	}

	// Token: 0x04001D92 RID: 7570
	private const int _qliphothMax = 2;

	// Token: 0x04001D93 RID: 7571
	private const int _howlingDamageMin = 15;

	// Token: 0x04001D94 RID: 7572
	private const int _howlingDamageMax = 20;

	// Token: 0x04001D95 RID: 7573
	private const int _normalFortitudeLevel = 3;

	// Token: 0x04001D96 RID: 7574
	private const float _filterDisplayTime = 20f;

	// Token: 0x04001D97 RID: 7575
	private const float _radius = 2f;

	// Token: 0x04001D98 RID: 7576
	private const float _groggyTime = 7.5f;

	// Token: 0x04001D99 RID: 7577
	private const float _getaWayStdDamage = 300f;

	// Token: 0x04001D9A RID: 7578
	private const float _howlingDelay = 20f;

	// Token: 0x04001D9B RID: 7579
	private const float _defaultAttackRange = 4f;

	// Token: 0x04001D9C RID: 7580
	private const float _groggyCasting = 3f;

	// Token: 0x04001D9D RID: 7581
	private const float _initialHealthRate = 0.8f;

	// Token: 0x04001D9E RID: 7582
	private const float _redMoonValue = 0.5f;

	// Token: 0x04001D9F RID: 7583
	public const int GiftId = 1033;

	// Token: 0x04001DA0 RID: 7584
	public const string SlashSrc = "Effect/Creature/BigBadWolf/Slash";

	// Token: 0x04001DA1 RID: 7585
	public const string BiteSrc = "Effect/Creature/BigBadWolf/Charge1-{0}";

	// Token: 0x04001DA2 RID: 7586
	public const string ChargeSrc = "Effect/Creature/BigBadWolf/Scratch{0}";

	// Token: 0x04001DA3 RID: 7587
	private MapNode _currentMoveTargetNode;

	// Token: 0x04001DA4 RID: 7588
	private MovableObjectNode _currentMoveTargetMovable;

	// Token: 0x04001DA5 RID: 7589
	private RedHood _redHood;

	// Token: 0x04001DA6 RID: 7590
	private AgentModel _redHoodWorker;

	// Token: 0x04001DA7 RID: 7591
	private List<Uncontrollable_WolfEaten> eatenWorker = new List<Uncontrollable_WolfEaten>();

	// Token: 0x04001DA8 RID: 7592
	private List<UnitModel> CheckedUnit = new List<UnitModel>();

	// Token: 0x04001DA9 RID: 7593
	private IsolateFilter eatFilter_1;

	// Token: 0x04001DAA RID: 7594
	private IsolateFilter eatFilter_2;

	// Token: 0x04001DAB RID: 7595
	private IsolateFilter vomitFilter_1;

	// Token: 0x04001DAC RID: 7596
	private IsolateFilter vomitFilter_2;

	// Token: 0x04001DAD RID: 7597
	private Timer _filterTimer = new Timer();

	// Token: 0x04001DAE RID: 7598
	private Timer _getawayGroggyTimer = new Timer();

	// Token: 0x04001DAF RID: 7599
	private Timer _howlingDelayTimer = new Timer();

	// Token: 0x04001DB0 RID: 7600
	private Timer _castingGroggyTimer = new Timer();

	// Token: 0x04001DB1 RID: 7601
	private MapNode _castingNode;

	// Token: 0x04001DB2 RID: 7602
	private Sefira _nextGetawaySefira;

	// Token: 0x04001DB3 RID: 7603
	private BigBadWolf.DamageCumlative damageModule = new BigBadWolf.DamageCumlative();

	// Token: 0x04001DB4 RID: 7604
	private PassageObjectModel _entryPassage;

	// Token: 0x04001DB5 RID: 7605
	private UnitModel _currentApproachingTarget;

	// Token: 0x04001DB6 RID: 7606
	private BigBadWolf.FilterType _filterType;

	// Token: 0x04001DB7 RID: 7607
	private float _filterElapSave;

	// Token: 0x04001DB8 RID: 7608
	private bool _isAttacking;

	// Token: 0x04001DB9 RID: 7609
	private bool _isMovingToSefira;

	// Token: 0x04001DBA RID: 7610
	private bool _activateEvent;

	// Token: 0x04001DBB RID: 7611
	private bool _isApproaching;

	// Token: 0x04001DBC RID: 7612
	private bool _isRedHoodSuppressed;

	// Token: 0x04001DBD RID: 7613
	private int _qlipothSubCount;

	// Token: 0x04001DBE RID: 7614
	private BigBadWolf.WolfAttackType _currentAttackType = BigBadWolf.WolfAttackType.BITE;

	// Token: 0x020003A1 RID: 929
	public enum WolfActionState
	{
		// Token: 0x04001DC0 RID: 7616
		PEACEFULL,
		// Token: 0x04001DC1 RID: 7617
		ESCAPED,
		// Token: 0x04001DC2 RID: 7618
		RAMPAGE
	}

	// Token: 0x020003A2 RID: 930
	public enum FilterType
	{
		// Token: 0x04001DC4 RID: 7620
		EAT,
		// Token: 0x04001DC5 RID: 7621
		VOMIT
	}

	// Token: 0x020003A3 RID: 931
	public enum WolfAttackType
	{
		// Token: 0x04001DC7 RID: 7623
		SCRATCH,
		// Token: 0x04001DC8 RID: 7624
		BITE,
		// Token: 0x04001DC9 RID: 7625
		HOWLING,
		// Token: 0x04001DCA RID: 7626
		CASTING
	}

	// Token: 0x020003A4 RID: 932
	private class DamageCumlative
	{
		// Token: 0x06001D40 RID: 7488 RVA: 0x00003FB0 File Offset: 0x000021B0
		public DamageCumlative()
		{
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000EDB80 File Offset: 0x000EBD80
		public void OnTakeDamage(float damage)
		{
			this.curDamage += damage;
			if (this.curDamage >= this.stdDamage)
			{
				this.curDamage = 0f;
				if (this.onFilled != null)
				{
					this.onFilled();
				}
			}
		}

		// Token: 0x04001DCB RID: 7627
		public float curDamage;

		// Token: 0x04001DCC RID: 7628
		public float stdDamage;

		// Token: 0x04001DCD RID: 7629
		public BigBadWolf.DamageCumlative.OnFilled onFilled;

		// Token: 0x020003A5 RID: 933
		// (Invoke) Token: 0x06001D43 RID: 7491
		public delegate void OnFilled();
	}
}
