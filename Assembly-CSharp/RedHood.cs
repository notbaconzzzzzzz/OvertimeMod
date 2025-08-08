using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using CommandWindow;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000460 RID: 1120
public class RedHood : CreatureBase, IObserver
{
	// Token: 0x060027FA RID: 10234 RVA: 0x0002784A File Offset: 0x00025A4A
	public RedHood()
	{
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060027FB RID: 10235 RVA: 0x00027885 File Offset: 0x00025A85
	private RedHoodAnim AnimScript
	{
		get
		{
			return base.Unit.animTarget as RedHoodAnim;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060027FC RID: 10236 RVA: 0x00027897 File Offset: 0x00025A97
	// (set) Token: 0x060027FD RID: 10237 RVA: 0x0002789F File Offset: 0x00025A9F
	private UnitModel target
	{
		get
		{
			return this._target;
		}
		set
		{
			this.OnSetTarget(value);
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060027FE RID: 10238 RVA: 0x000278A8 File Offset: 0x00025AA8
	private bool IsBigBadWolfExist
	{
		get
		{
			return this._wolf != null;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060027FF RID: 10239 RVA: 0x0001F1B2 File Offset: 0x0001D3B2
	public DamageInfo MeleeDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("1");
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06002800 RID: 10240 RVA: 0x0001F1CE File Offset: 0x0001D3CE
	public DamageInfo RangeDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("2");
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06002801 RID: 10241 RVA: 0x0001F1EA File Offset: 0x0001D3EA
	public DamageInfo ThrowingDamage
	{
		get
		{
			return this.model.metaInfo.creatureSpecialDamageTable.GetSpecialDamage("3");
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06002802 RID: 10242 RVA: 0x000278B6 File Offset: 0x00025AB6
	public float CurrentSpeedFactor
	{
		get
		{
			return this._currentSpeedFactor;
		}
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x00118784 File Offset: 0x00116984
	public override void ParamInit()
	{
		this.target = null;
		this._currentDestMovable = null;
		this._currentDestNode = null;
		this._oldTargetNode = null;
		this._chase = false;
		this._isWaiting = false;
		this._isAttacking = false;
		this._isAxeThrowed = false;
		this._isApproaching = false;
		this._isWolfSuppressed = false;
		this._currentAgentPrevWorkIsWolf = false;
		this._changeTargetAsWolf = false;
		this._currentSpeedFactor = 1f;
		this._state = RedHood.RedHoodState.IDLE;
		this._throwingRespawnTimer.StopTimer();
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000278BE File Offset: 0x00025ABE
	public override void OnEnterRoom(UseSkill skill)
	{
		if (this.IsWolf(skill.agent.GetRecentWorkedCreature()))
		{
			this._currentAgentPrevWorkIsWolf = true;
		}
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000278DD File Offset: 0x00025ADD
	public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
	{
		if (this._currentAgentPrevWorkIsWolf)
		{
			this._currentAgentPrevWorkIsWolf = false;
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000278FC File Offset: 0x00025AFC
	public void OnSetTarget(UnitModel target)
	{
		this._target = target;
		if (this.AnimScript != null)
		{
			this.AnimScript.SetTarget(target);
		}
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x00118804 File Offset: 0x00116A04
	private void FindBadWolf()
	{
		CreatureModel[] creatureList = CreatureManager.instance.GetCreatureList();
		foreach (CreatureModel creatureModel in creatureList)
		{
			if (creatureModel.script is BigBadWolf)
			{
				this._wolf = (creatureModel.script as BigBadWolf);
				break;
			}
		}
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x00027922 File Offset: 0x00025B22
	public bool IsWolf(UnitModel unit)
	{
		return this.IsBigBadWolfExist && unit == this._wolf.model;
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x00027943 File Offset: 0x00025B43
	public override void OnViewInit(CreatureUnit unit)
	{
		this.AnimScript.SetScript(this);
		this._entryPassage = this.model.GetEntryNode().GetAttachedPassage();
	}

	// Token: 0x0600280A RID: 10250 RVA: 0x00027967 File Offset: 0x00025B67
	public void TryRequest()
	{
		this.AnimScript.OpenRequestUI();
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x0011885C File Offset: 0x00116A5C
	public int GetRequestCost(UnitModel unit)
	{
		if (unit is CreatureModel)
		{
			CreatureModel creatureModel = unit as CreatureModel;
			return creatureModel.GetRiskLevel() * 40;
		}
		if (unit is AgentModel)
		{
			return (unit as AgentModel).level * 40;
		}
		return 40;
	}

	// Token: 0x0600280C RID: 10252 RVA: 0x001188A4 File Offset: 0x00116AA4
	public void StartRequest(UnitModel target)
	{
		if (target == null)
		{
			return;
		}
		this.target = target;
		this.model.ShowCreatureSpeechDirect("...");
		this.AnimScript.ReadyForRequest();
		this._isWaiting = true;
		this.model.WorkParamInit();
		this.model.isOverloaded = false;
		this.model.state = CreatureState.ESCAPE;
		this.model.baseMaxHp = this.model.metaInfo.maxHp;
		this.model.hp = (float)this.model.metaInfo.maxHp;
		this.model.SetFaction(FactionTypeList.StandardFaction.EscapedCreature);
		this.model.movementScale = 1f;
		this._state = RedHood.RedHoodState.HUNTING;
	}

	// Token: 0x0600280D RID: 10253 RVA: 0x00027974 File Offset: 0x00025B74
	public override void ActivateQliphothCounter()
	{
		if (this._state == RedHood.RedHoodState.HUNTING)
		{
			return;
		}
		this.model.ShowCreatureSpeechDirect("...!");
		this.AnimScript.ReadyForRequest();
		this._isWaiting = true;
		this._state = RedHood.RedHoodState.ESCAPE;
	}

	// Token: 0x0600280E RID: 10254 RVA: 0x00003FDD File Offset: 0x000021DD
	public override void OnFixedUpdate(CreatureModel creature)
	{
	}

	// Token: 0x0600280F RID: 10255 RVA: 0x00118964 File Offset: 0x00116B64
	public void MakeMovement(MapNode targetNode, CreatureCommand.OnCommandEnd end = null)
	{
		if (this._currentDestMovable != null)
		{
			this._currentDestMovable = null;
		}
		this._currentDestNode = targetNode;
		this.model.MoveToNode(targetNode);
		if (end != null)
		{
			this.model.GetCurrentCommand().SetEndCommand(end);
		}
		this._currentMoveCommand = end;
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x001189B4 File Offset: 0x00116BB4
	public void MakeMovement(MovableObjectNode targetMovable, CreatureCommand.OnCommandEnd end = null)
	{
		if (this._currentDestNode != null)
		{
			this._currentDestNode = null;
		}
		this._currentDestMovable = targetMovable;
		this._chase = false;
		this.model.MoveToMovable(targetMovable);
		if (end != null)
		{
			this.model.GetCurrentCommand().SetEndCommand(end);
		}
		this._currentMoveCommand = end;
	}

	// Token: 0x06002811 RID: 10257 RVA: 0x00118A0C File Offset: 0x00116C0C
	public void ResetMovement()
	{
		if (this.movable.IsMoving())
		{
			return;
		}
		if (this._currentDestNode != null)
		{
			this.MakeMovement(this._currentDestNode, this._currentMoveCommand);
		}
		else if (this._currentDestMovable != null)
		{
			this.MakeMovement(this._currentDestMovable, this._currentMoveCommand);
		}
		else if (this.target != null)
		{
			this.MakeMovement(this.target.GetMovableNode(), null);
		}
		else
		{
			this.MakeMovement(MapGraph.instance.GetRoamingNodeByRandom(), new CreatureCommand.OnCommandEnd(this.EscapeMovementArrived));
		}
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x000279AC File Offset: 0x00025BAC
	public void StopMovement(bool clearDest = false)
	{
		if (clearDest)
		{
			this._currentDestNode = null;
			this._currentDestMovable = null;
			this._currentMoveCommand = null;
		}
		this.model.ClearCommand();
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x000279D4 File Offset: 0x00025BD4
	private bool CheckTargetPassage()
	{
		return this.target != null && this.currentPassage != null && this.currentPassage == this.target.GetMovableNode().currentPassage;
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x00118AAC File Offset: 0x00116CAC
	private void HuntingUpdate()
	{
		if (this.ValidateTarget())
		{
			if (!this._isAttacking)
			{
				if (this.CheckTargetPassage())
				{
					if (this.movable.IsMoving())
					{
						this.StopMovement(false);
					}
					this.StartAttack();
				}
				else
				{
					this.ResetMovement();
				}
			}
		}
		else if (!this._isAttacking && !(this.model.GetCreatureCurrentCmd() is MoveCreatureCommand))
		{
			this.StopMovement(true);
			this.MakeMovement(this.model.GetRoomNode(), new CreatureCommand.OnCommandEnd(this.OnArrivedRoom));
		}
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x00027A0A File Offset: 0x00025C0A
	private void EscapeMovementArrived()
	{
		this.StopMovement(true);
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x00118B50 File Offset: 0x00116D50
	private void EscapeUpdate()
	{
		this.model.CheckNearWorkerEncounting();
		List<UnitModel> damageTargets = this.GetDamageTargets(-1f, true);
		if (this.target == null)
		{
			if (damageTargets.Count > 0)
			{
				this.StopMovement(true);
				this.target = damageTargets[UnityEngine.Random.Range(0, damageTargets.Count)];
				this.MakeMovement(this.target.GetMovableNode(), null);
			}
			else
			{
				this.ResetMovement();
			}
		}
		else if (this.ValidateTarget())
		{
			if (!this._isAttacking)
			{
				if (this.CheckTargetPassage())
				{
					if (this.movable.IsMoving())
					{
						this.StopMovement(false);
					}
					this.StartAttack();
				}
				else
				{
					this.ResetMovement();
				}
			}
		}
		else if (!this._isAttacking)
		{
			if (this.target != null)
			{
				this.target = null;
			}
			this.StopMovement(true);
			this.ResetMovement();
		}
		if (this._state == RedHood.RedHoodState.ESCAPE)
		{
			this.CheckWolf(damageTargets);
		}
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x00118C60 File Offset: 0x00116E60
	public override void UniqueEscape()
	{
		if (this._isWaiting)
		{
			return;
		}
		if (this._freezeReturnTimer.started && this._freezeReturnTimer.RunTimer())
		{
			this.OnAttackAnimEnd();
		}
		if (this._changeTargetAsWolf && !this._isAttacking)
		{
			this._changeTargetAsWolf = false;
			this.StopMovement(true);
			this.MakeMovement(this.target.GetMovableNode(), null);
		}
		if (this._isApproaching)
		{
			this.OnApproachUpdate();
		}
		RedHood.RedHoodState state = this._state;
		if (state != RedHood.RedHoodState.HUNTING)
		{
			if (state == RedHood.RedHoodState.RAMPAGE || state == RedHood.RedHoodState.ESCAPE)
			{
				this.EscapeUpdate();
			}
		}
		else
		{
			this.HuntingUpdate();
		}
		if (this.model.GetCurrentCommand() != null && this._currentDestMovable != null)
		{
			bool flag = false;
			if (this._currentDestMovable.currentPassage != null)
			{
				if (this._currentDestMovable.currentPassage.type == PassageType.VERTICAL)
				{
					flag = true;
				}
				else if (this._currentDestMovable.GetCurrentNode() != null)
				{
					this._oldTargetNode = this._currentDestMovable.GetCurrentNode();
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (this._oldTargetNode != null)
				{
					this._chase = true;
					CreatureCommand.OnCommandEnd endCmd = this.model.GetCurrentCommand().endCmd;
					this.model.GetCurrentCommand().SetEndCommand(null);
					this.model.MoveToNode(this._oldTargetNode);
					if (endCmd != null)
					{
						this.model.GetCurrentCommand().SetEndCommand(endCmd);
					}
				}
				else
				{
					this._chase = false;
				}
			}
			else if (this._chase)
			{
				CreatureCommand.OnCommandEnd endCmd2 = this.model.GetCurrentCommand().endCmd;
				this.model.GetCurrentCommand().SetEndCommand(null);
				this.model.MoveToMovable(this._currentDestMovable);
				if (endCmd2 != null)
				{
					this.model.GetCurrentCommand().SetEndCommand(endCmd2);
				}
				this._chase = false;
			}
		}
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x00027A13 File Offset: 0x00025C13
	public void OnArrivedRoom()
	{
		this.model.Suppressed();
		this.model.suppressReturnTimer = 1f;
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x00118E60 File Offset: 0x00117060
	private bool ValidateTarget()
	{
		if (this._state == RedHood.RedHoodState.HUNTING)
		{
			if (this.target is CreatureModel)
			{
				CreatureModel creatureModel = this.target as CreatureModel;
				return creatureModel.IsEscapedOnlyEscape() && creatureModel.hp > 0f;
			}
			return this.target.IsAttackTargetable() && this.target.hp > 0f;
		}
		else
		{
			if (this._state != RedHood.RedHoodState.ESCAPE && this._state != RedHood.RedHoodState.RAMPAGE)
			{
				return false;
			}
			if (this.target == null)
			{
				return false;
			}
			if (this.target is CreatureModel)
			{
				CreatureModel creatureModel2 = this.target as CreatureModel;
				return creatureModel2.IsEscapedOnlyEscape() && creatureModel2.hp > 0f;
			}
			AgentModel agentModel = this.target as AgentModel;
			return this.target.IsAttackTargetable() && (agentModel == null || agentModel.currentSkill == null) && this.target.hp > 0f;
		}
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x00118F80 File Offset: 0x00117180
	private void StartAttack()
	{
		this.model.movementScale = 1f * this.CurrentSpeedFactor;
		this._isAttacking = true;
		this.SetTargetDirection(this.target);
		RedHood.AttackType attackType;
		if (this._isAxeThrowed)
		{
			attackType = RedHood.AttackType.RANGED_NORMAL;
		}
		else
		{
			if (UnityEngine.Random.value <= 0.66f)
			{
				if (UnityEngine.Random.value <= 0.3f)
				{
					attackType = RedHood.AttackType.MELEE_ENFORCED;
				}
				else
				{
					attackType = RedHood.AttackType.MELEE_NORMAL;
				}
			}
			else
			{
				attackType = RedHood.AttackType.RANGED_NORMAL;
			}
			if (UnityEngine.Random.value <= 0.1f)
			{
				attackType = RedHood.AttackType.THROWING;
			}
		}
		this._currentAttackType = attackType;
		if ((attackType == RedHood.AttackType.MELEE_ENFORCED || attackType == RedHood.AttackType.MELEE_NORMAL) && !this.CheckRange(this.target, 2f, true))
		{
			this._currentAttackType = RedHood.AttackType.RANGED_MOVING;
			this.model.movementScale = 0.25f * this.CurrentSpeedFactor;
			this.Approach();
			return;
		}
		this.AnimScript.OnStartAttack(attackType);
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x00119070 File Offset: 0x00117270
	public void SetTargetDirection(UnitModel target)
	{
		if (target == null)
		{
			this.movable.SetDirection(UnitDirection.RIGHT);
			return;
		}
		UnitDirection direction = (this.movable.GetCurrentViewPosition().x > target.GetCurrentViewPosition().x) ? UnitDirection.LEFT : UnitDirection.RIGHT;
		this.movable.SetDirection(direction);
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x001190CC File Offset: 0x001172CC
	public void OnApproachUpdate()
	{
		if (this.CheckRange(this.target, 0.5f, true) || !this.ValidateTarget() || this.currentPassage != this.target.GetMovableNode().currentPassage)
		{
			this.StopMovement(true);
			this._isApproaching = false;
			this.OnAttackAnimEnd();
			this.model.movementScale = 1f * this.CurrentSpeedFactor;
			this.AnimScript.animator.SetInteger("AttackType", 0);
			return;
		}
		if (!this.movable.IsMoving())
		{
			this.MakeMovement(this.target.GetMovableNode(), null);
		}
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x00027A30 File Offset: 0x00025C30
	private void Approach()
	{
		this._isApproaching = true;
		if (this._currentAttackType == RedHood.AttackType.RANGED_MOVING)
		{
			this.AnimScript.OnStartAttack(this._currentAttackType);
		}
		this.MakeMovement(this.target.GetMovableNode(), null);
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000ECBB8 File Offset: 0x000EADB8
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

	// Token: 0x0600281F RID: 10271 RVA: 0x0011917C File Offset: 0x0011737C
	public bool CanCastMovingShoot()
	{
		if (this.currentPassage == null)
		{
			return false;
		}
		if (this.target != null && this.currentPassage != this.target.GetMovableNode().currentPassage)
		{
			return false;
		}
		float width = this.currentPassage.GetWidth();
		float x = this.movable.GetCurrentViewPosition().x;
		return false;
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x00027A68 File Offset: 0x00025C68
	public static void Log(string log)
	{
		Debug.Log(string.Format("<color=#FF1200FF>[REDHOOD]</color>{0}", log));
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x001191E8 File Offset: 0x001173E8
	public void OnReadyForEscape()
	{
		RedHood.Log("Ready For Escape");
		this._isWaiting = false;
		if (this._state == RedHood.RedHoodState.HUNTING)
		{
			this.MakeMovement(this.target.GetMovableNode(), null);
		}
		else
		{
			this.model.Escape();
			this.ResetMovement();
		}
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x00027A7A File Offset: 0x00025C7A
	public void OnAttackAnimEnd()
	{
		this._isAttacking = false;
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x0011923C File Offset: 0x0011743C
	public void OnGiveDamage(UnitModel target, bool isRanged)
	{
		RedHoodBleedBuf buf = this.GetBuf(target);
		if (buf != null)
		{
			buf.OnRedHoodAttacked(isRanged, 1);
		}
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x00119260 File Offset: 0x00117460
	public void OnHowlingAttacked()
	{
		if (this._wolf == null)
		{
			return;
		}
		if (this._state == RedHood.RedHoodState.ESCAPE)
		{
		}
		if (this.target == this._wolf.model)
		{
			return;
		}
		this.target = this._wolf.model;
		if (this._isAttacking)
		{
			this._changeTargetAsWolf = true;
		}
		if (this._state == RedHood.RedHoodState.ESCAPE || this._state == RedHood.RedHoodState.HUNTING || this._state == RedHood.RedHoodState.RAMPAGE)
		{
			if (!this._changeTargetAsWolf)
			{
				this.StopMovement(true);
				this.MakeMovement(this.target.GetMovableNode(), null);
			}
			if (this._isApproaching)
			{
				this.StopMovement(true);
			}
			this._freezeReturnTimer.StartTimer(2f);
		}
		else
		{
			this.model.ShowCreatureSpeechDirect("...");
			this.AnimScript.ReadyForRequest();
			this._isWaiting = true;
			this.model.WorkParamInit();
			this.model.isOverloaded = false;
			this.model.state = CreatureState.ESCAPE;
			this.model.baseMaxHp = this.model.metaInfo.maxHp;
			this.model.hp = (float)this.model.metaInfo.maxHp;
			this.model.SetFaction(FactionTypeList.StandardFaction.EscapedCreature);
		}
		this._state = RedHood.RedHoodState.HUNTING;
		this.model.SetQliphothCounter(0);
		this._isApproaching = false;
		this.AnimScript._angryParticle.SetActive(true);
		this._currentSpeedFactor = 1.5f;
		if (this.AnimScript._rampageParticle.activeInHierarchy)
		{
			this.AnimScript._rampageParticle.SetActive(false);
		}
		this.MakeSound("Change");
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x00119428 File Offset: 0x00117628
	private RedHoodBleedBuf GetBuf(UnitModel target)
	{
		RedHoodBleedBuf redHoodBleedBuf = null;
		if (!this.bufDictionary.TryGetValue(target, out redHoodBleedBuf))
		{
			redHoodBleedBuf = new RedHoodBleedBuf(this);
			target.AddUnitBuf(redHoodBleedBuf);
			this.bufDictionary.Add(target, redHoodBleedBuf);
		}
		return redHoodBleedBuf;
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x00119468 File Offset: 0x00117668
	public override void OnReturn()
	{
		if (this._state != RedHood.RedHoodState.HUNTING || this._isWolfSuppressed)
		{
			this.model.ResetQliphothCounter();
		}
		if (this.IsWolf(this.target) && this.model.hp > 0f)
		{
			this.model.ResetQliphothCounter();
		}
		this.AnimScript.ResetAnim();
		this.ParamInit();
		this.ClearBuf();
		this.bufDictionary.Clear();
		this._state = RedHood.RedHoodState.IDLE;
		UnitBuf unitBufByType = this.model.GetUnitBufByType(UnitBufType.REDHOOD_MAD);
		if (unitBufByType != null)
		{
			this.model.RemoveUnitBuf(unitBufByType);
		}
		if (this.AnimScript.animator.GetBool("IsSuppressed"))
		{
			this.AnimScript.animator.SetBool("IsSuppressed", false);
		}
		if (this.AnimScript.animator.GetBool("Idle"))
		{
			this.AnimScript.animator.ResetTrigger("Reset");
		}
		else
		{
			this.AnimScript.animator.SetTrigger("Reset");
		}
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x00027A83 File Offset: 0x00025C83
	public override void OnStageStart()
	{
		Notice.instance.Observe(NoticeName.OnEscape, this);
		this.FindBadWolf();
		this._initialSpeed = this.model.movement;
		this.ClearBuf();
		this.bufDictionary.Clear();
	}

	// Token: 0x06002828 RID: 10280 RVA: 0x00027ABD File Offset: 0x00025CBD
	public override void OnStageRelease()
	{
		Notice.instance.Remove(NoticeName.OnEscape, this);
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x0011958C File Offset: 0x0011778C
	public void ClearBuf()
	{
		foreach (KeyValuePair<UnitModel, RedHoodBleedBuf> keyValuePair in this.bufDictionary)
		{
			keyValuePair.Key.RemoveUnitBuf(keyValuePair.Value);
		}
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x00027ACF File Offset: 0x00025CCF
	public override bool IsSuppressable()
	{
		return this._state == RedHood.RedHoodState.RAMPAGE || this._state == RedHood.RedHoodState.ESCAPE;
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x00027ACF File Offset: 0x00025CCF
	public override bool IsSensoredInPassage()
	{
		return this._state == RedHood.RedHoodState.RAMPAGE || this._state == RedHood.RedHoodState.ESCAPE;
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x00027ACF File Offset: 0x00025CCF
	public override bool IsAutoSuppressable()
	{
		return this._state == RedHood.RedHoodState.RAMPAGE || this._state == RedHood.RedHoodState.ESCAPE;
	}

	// Token: 0x0600282D RID: 10285 RVA: 0x001195F4 File Offset: 0x001177F4
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

	// Token: 0x0600282E RID: 10286 RVA: 0x001196CC File Offset: 0x001178CC
	public void OnDamageTime()
	{
		DamageInfo damageInfo = null;
		float dist = -1f;
		this.SetTargetDirection(this.target);
		RedHood.AttackType currentAttackType = this._currentAttackType;
		switch (currentAttackType)
		{
		case RedHood.AttackType.RANGED_NORMAL:
		case RedHood.AttackType.RANGED_MOVING:
			dist = 50f;
			damageInfo = this.RangeDamage;
			goto IL_D3;
		case RedHood.AttackType.DASH_ATTACK:
			break;
		default:
			switch (currentAttackType)
			{
			case RedHood.AttackType.MELEE_NORMAL:
			case RedHood.AttackType.MELEE_ENFORCED:
				break;
			case RedHood.AttackType.THROWING:
				dist = 50f;
				this.GenThrowing();
				this.MakeSound("Throw");
				goto IL_D3;
			default:
				goto IL_D3;
			}
			break;
		}
		damageInfo = this.MeleeDamage;
		dist = 2f;
		this.MakeSlashEffect();
		if (this._currentAttackType == RedHood.AttackType.MELEE_NORMAL)
		{
			string src = (UnityEngine.Random.value > 0.5f) ? "Attack2" : "Attack1";
			this.MakeSound(src);
		}
		else
		{
			this.MakeSound("PowerAttack");
		}
		IL_D3:
		if (damageInfo == null)
		{
			return;
		}
		if (this._state == RedHood.RedHoodState.HUNTING && this.IsWolf(this.target))
		{
			damageInfo *= 1.5f;
		}
		else if (this._state == RedHood.RedHoodState.RAMPAGE)
		{
			damageInfo *= 2f;
		}
		if (damageInfo != null)
		{
			this.GiveDamage(damageInfo, this.GetDamageTargets(dist, false), this._currentAttackType);
		}
	}

	// Token: 0x0600282F RID: 10287 RVA: 0x00119814 File Offset: 0x00117A14
	private void GenThrowing()
	{
		this._throwingRespawnTimer.StartTimer(10f);
		if (this.currentPassage == null)
		{
			return;
		}
		MapNode mapNode = (this.movable.GetDirection() != UnitDirection.RIGHT) ? this.currentPassage.GetLeft() : this.currentPassage.GetRight();
		if (mapNode == null)
		{
			return;
		}
		this.AnimScript.AxeScript.OnThrow(mapNode);
		this._isAxeThrowed = true;
		this.AnimScript._axeRenderer.enabled = false;
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x0011989C File Offset: 0x00117A9C
	private void GiveDamage(DamageInfo damageInfo, List<UnitModel> targets, RedHood.AttackType attackType)
	{
		bool isRanged = attackType == RedHood.AttackType.RANGED_MOVING || attackType == RedHood.AttackType.RANGED_NORMAL;
		foreach (UnitModel unitModel in targets)
		{
			unitModel.TakeDamage(this.model, damageInfo);
			this.OnGiveDamage(unitModel, isRanged);
		}
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x00119910 File Offset: 0x00117B10
	public void GiveAxeThrowingDamage(UnitModel target)
	{
		if (target is WorkerModel)
		{
			if (this._state != RedHood.RedHoodState.RAMPAGE && this._state != RedHood.RedHoodState.ESCAPE)
			{
				if (!this.IsWolf(this.target))
				{
					return;
				}
			}
			if ((target as WorkerModel).IsDead())
			{
				return;
			}
		}
		if (target == this.model)
		{
			return;
		}
		target.TakeDamage(this.model, this.ThrowingDamage);
		this.MakeThrowEffect(target, 2f);
		RedHoodBleedBuf buf = this.GetBuf(target);
		if (buf != null)
		{
			buf.OnRedHoodAttacked(false, 3);
		}
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x00027AE9 File Offset: 0x00025CE9
	public void OnAxeRespawned()
	{
		this._isAxeThrowed = false;
		this.AnimScript._axeRenderer.enabled = true;
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x001199AC File Offset: 0x00117BAC
	private void CheckWolf(List<UnitModel> near)
	{
		foreach (UnitModel unit in near)
		{
			if (this.IsWolf(unit))
			{
				this.OnHowlingAttacked();
			}
		}
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x00119A10 File Offset: 0x00117C10
	private List<UnitModel> GetDamageTargets(float dist = -1f, bool ignoreDirection = true)
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this.currentPassage == null)
		{
			return list;
		}
		List<MovableObjectNode> list2 = new List<MovableObjectNode>(this.currentPassage.GetEnteredTargets(this.movable));
		List<UnitModel> list3 = new List<UnitModel>();
		foreach (MovableObjectNode movableObjectNode in list2)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (!list3.Contains(unit))
			{
				if (unit.IsAttackTargetable())
				{
					if (dist == -1f || this.CheckRange(unit, dist, ignoreDirection))
					{
						list3.Add(unit);
					}
				}
			}
		}
		if (this._state == RedHood.RedHoodState.HUNTING)
		{
			if (this.IsWolf(this.target))
			{
				list = list3;
			}
			else
			{
				foreach (UnitModel unitModel in list3)
				{
					if (unitModel is CreatureModel)
					{
						list.Add(unitModel);
					}
					else if (unitModel is WorkerModel)
					{
						WorkerModel workerModel = unitModel as WorkerModel;
						if (workerModel.IsPanic() || workerModel.unconAction != null)
						{
							list.Add(workerModel);
						}
					}
				}
			}
		}
		else
		{
			list = list3;
		}
		return list;
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x00119BA0 File Offset: 0x00117DA0
	public override bool OnAfterSuppressed()
	{
		bool flag = this.model.hp > 0f;
		if (flag)
		{
			this.AnimScript.ResetAnim();
		}
		else
		{
			this.model.ShowCreatureSpeechDirect("...!");
		}
		return this.model.hp > 0f;
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x00027B03 File Offset: 0x00025D03
	public override bool IsWorkable()
	{
		return this._state == RedHood.RedHoodState.IDLE;
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x00027B03 File Offset: 0x00025D03
	public override bool OnOpenWorkWindow()
	{
		return this._state == RedHood.RedHoodState.IDLE;
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x00013CB1 File Offset: 0x00011EB1
	public override bool HasRoomCounter()
	{
		return true;
	}

	// Token: 0x06002839 RID: 10297 RVA: 0x00119BF8 File Offset: 0x00117DF8
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
				this.model.SubQliphothCounter();
			}
		}
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x00027B0E File Offset: 0x00025D0E
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		if (this.model.hp <= 0f && this.IsWolf(this.target))
		{
			this._wolf.OnRedHoodSuppressed();
		}
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x00119C98 File Offset: 0x00117E98
	public void OnWolfSuppressedByOther()
	{
		this.model.AddUnitBuf(new RedhoodRampageBuf(this));
		this._state = RedHood.RedHoodState.RAMPAGE;
		RedHood.Log("격노상태 돌입");
		this.MakeSound("Change");
		this._currentSpeedFactor = 1.5f;
		this.AnimScript._rampageParticle.SetActive(true);
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x00027B41 File Offset: 0x00025D41
	public void OnWolfSuppressedByRedHood()
	{
		this._isWolfSuppressed = true;
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x00119CF0 File Offset: 0x00117EF0
	public void MakeGunFlame(Transform tr)
	{
		string name = (this.movable.GetDirection() != UnitDirection.RIGHT) ? "Effect/Creature/RedHood/GunFireLeft" : "Effect/Creature/RedHood/GunFireRight";
		GameObject gameObject = Prefab.LoadPrefab(name);
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = tr.transform.position;
		gameObject.transform.localScale = Vector3.one * this.movable.currentScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.MakeSound("Gun");
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x00119D94 File Offset: 0x00117F94
	public void MakeSlashEffect()
	{
		Transform axeRef = this.AnimScript.AxeRef;
		string name = string.Format("Effect/Creature/RedHood/RedHoodSlash{0}_{1}", UnityEngine.Random.Range(1, 3), (this.model.GetDirection() != UnitDirection.RIGHT) ? "Left" : "Right");
		GameObject gameObject = Prefab.LoadPrefab(name);
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		Vector3 position = axeRef.position;
		if (this.movable.GetDirection() == UnitDirection.RIGHT)
		{
			position.x += 1f * this.movable.currentScale;
		}
		else
		{
			position.x -= 1f * this.movable.currentScale;
		}
		gameObject.transform.position = position;
		gameObject.transform.localScale = Vector3.one * this.movable.currentScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x00119E9C File Offset: 0x0011809C
	public void MakeThrowEffect(UnitModel target, float heightFctor = 2f)
	{
		Transform axeRef = this.AnimScript.AxeRef;
		string name = string.Format("Effect/Creature/RedHood/RedHoodSlash{0}_{1}", UnityEngine.Random.Range(1, 3), (this.model.GetDirection() != UnitDirection.RIGHT) ? "Left" : "Right");
		GameObject gameObject = Prefab.LoadPrefab(name);
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		Vector3 currentViewPosition = target.GetCurrentViewPosition();
		currentViewPosition.y = 1.5f * target.GetMovableNode().currentScale;
		gameObject.transform.position = currentViewPosition;
		gameObject.transform.localScale = Vector3.one * this.movable.currentScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x000EDA08 File Offset: 0x000EBC08
	public string GetSoundSrc(string key)
	{
		string empty = string.Empty;
		if (!this.model.metaInfo.soundTable.TryGetValue(key, out empty))
		{
		}
		return empty;
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x00119F68 File Offset: 0x00118168
	public override SoundEffectPlayer MakeSound(string src)
	{
		SoundEffectPlayer result = null;
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc != string.Empty)
		{
			result = SoundEffectPlayer.PlayOnce(soundSrc, this.model.GetCurrentViewPosition());
		}
		return result;
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x00104A60 File Offset: 0x00102C60
	public override void OnOpenCommandWindow(Button[] buttons)
	{
		Image component = buttons[2].transform.Find("Icon").GetComponent<Image>();
		component.sprite = CommandWindow.CommandWindow.CurrentWindow.Work_S;
		LocalizeTextLoadScript component2 = buttons[2].transform.Find("WorkName").GetComponent<LocalizeTextLoadScript>();
		component2.SetText("Swork");
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x00027B4A File Offset: 0x00025D4A
	public override bool HasUniqueCommandAction(int workType)
	{
		if (workType == 3)
		{
			this.TryRequest();
			return true;
		}
		return false;
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x00027B5C File Offset: 0x00025D5C
	public override float GetDamageFactor(UnitModel target, DamageInfo damage)
	{
		if (this._state == RedHood.RedHoodState.RAMPAGE)
		{
			return 1.5f;
		}
		return base.GetDamageFactor(target, damage);
	}

	// Token: 0x04002697 RID: 9879
	private const int RequestCostFactor = 40;

	// Token: 0x04002698 RID: 9880
	private const float MeleeAttackRange = 2f;

	// Token: 0x04002699 RID: 9881
	private const string _text_norm = "...";

	// Token: 0x0400269A RID: 9882
	private const string _text_exclam = "...!";

	// Token: 0x0400269B RID: 9883
	public const float _axeRespawn = 10f;

	// Token: 0x0400269C RID: 9884
	public const int QliphothMax = 3;

	// Token: 0x0400269D RID: 9885
	private const string GunFire_Right = "Effect/Creature/RedHood/GunFireRight";

	// Token: 0x0400269E RID: 9886
	private const string GunFire_Left = "Effect/Creature/RedHood/GunFireLeft";

	// Token: 0x0400269F RID: 9887
	private const string SlashSrc = "Effect/Creature/RedHood/RedHoodSlash{0}_{1}";

	// Token: 0x040026A0 RID: 9888
	private RedHood.RedHoodState _state;

	// Token: 0x040026A1 RID: 9889
	private RedHood.AttackType _currentAttackType = RedHood.AttackType.DASH_ATTACK;

	// Token: 0x040026A2 RID: 9890
	private UnitModel _target;

	// Token: 0x040026A3 RID: 9891
	private Dictionary<UnitModel, RedHoodBleedBuf> bufDictionary = new Dictionary<UnitModel, RedHoodBleedBuf>();

	// Token: 0x040026A4 RID: 9892
	private MapNode _currentDestNode;

	// Token: 0x040026A5 RID: 9893
	private MapNode _oldTargetNode;

	// Token: 0x040026A6 RID: 9894
	private MovableObjectNode _currentDestMovable;

	// Token: 0x040026A7 RID: 9895
	private CreatureCommand.OnCommandEnd _currentMoveCommand;

	// Token: 0x040026A8 RID: 9896
	private PassageObjectModel _entryPassage;

	// Token: 0x040026A9 RID: 9897
	private BigBadWolf _wolf;

	// Token: 0x040026AA RID: 9898
	private Timer _throwingRespawnTimer = new Timer();

	// Token: 0x040026AB RID: 9899
	private Timer _freezeReturnTimer = new Timer();

	// Token: 0x040026AC RID: 9900
	private bool _isWaiting;

	// Token: 0x040026AD RID: 9901
	private bool _isAttacking;

	// Token: 0x040026AE RID: 9902
	private bool _isAxeThrowed;

	// Token: 0x040026AF RID: 9903
	private bool _isApproaching;

	// Token: 0x040026B0 RID: 9904
	private bool _chase;

	// Token: 0x040026B1 RID: 9905
	private bool _isWolfSuppressed;

	// Token: 0x040026B2 RID: 9906
	private bool _changeTargetAsWolf;

	// Token: 0x040026B3 RID: 9907
	private bool _currentAgentPrevWorkIsWolf;

	// Token: 0x040026B4 RID: 9908
	private float _initialSpeed;

	// Token: 0x040026B5 RID: 9909
	private float _currentSpeedFactor = 1f;

	// Token: 0x02000461 RID: 1121
	public enum RedHoodState
	{
		// Token: 0x040026B7 RID: 9911
		IDLE,
		// Token: 0x040026B8 RID: 9912
		HUNTING,
		// Token: 0x040026B9 RID: 9913
		RAMPAGE,
		// Token: 0x040026BA RID: 9914
		ESCAPE
	}

	// Token: 0x02000462 RID: 1122
	public enum AttackType
	{
		// Token: 0x040026BC RID: 9916
		MELEE_NORMAL = 10,
		// Token: 0x040026BD RID: 9917
		MELEE_ENFORCED,
		// Token: 0x040026BE RID: 9918
		RANGED_NORMAL = 0,
		// Token: 0x040026BF RID: 9919
		RANGED_MOVING,
		// Token: 0x040026C0 RID: 9920
		DASH_ATTACK,
		// Token: 0x040026C1 RID: 9921
		THROWING = 12
	}
}
