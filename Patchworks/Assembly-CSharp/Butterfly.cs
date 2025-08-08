using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using WorkerSpine;

// Token: 0x020003CE RID: 974
public class Butterfly : CreatureBase, IObserver // <Mod>
{
	// Token: 0x06001F67 RID: 8039 RVA: 0x000F9D4C File Offset: 0x000F7F4C
	public Butterfly()
	{
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06001F68 RID: 8040 RVA: 0x00020A5F File Offset: 0x0001EC5F
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(10, 16);
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06001F69 RID: 8041 RVA: 0x00020A80 File Offset: 0x0001EC80
	private static int skillDmg
	{
		get
		{
			return UnityEngine.Random.Range(3, 5);
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06001F6A RID: 8042 RVA: 0x00021605 File Offset: 0x0001F805
	public ButterflyAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as ButterflyAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x00021634 File Offset: 0x0001F834
	public override void ParamInit()
	{
		base.ParamInit();
		this.skillCoolTimer.StopTimer();
		this.skillRemainTimer.StopTimer();
		this.skillEffectTimer.StopTimer();
		this.attackTarget = null;
		this.attackDelays = new List<Butterfly.AttackDelay>();
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x0002166F File Offset: 0x0001F86F
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.animScript.SetScript(this);
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x00021684 File Offset: 0x0001F884
	public override void OnStageStart()
	{ // <Mod>
		base.OnStageStart();
		this.ParamInit();
		this.effects = new List<Butterfly.ButterflyEffect>();
		_deadCount = 0;
		_extinctCount = 0;
		_clerkTotal = 0;
		_departmentTotal = 0;
		foreach (Sefira sefira in SefiraManager.instance.GetActivatedSefiras())
		{
			int num = sefira.GetAliveOfficerCnt();
			_clerkTotal += num;
			if (num > 0)
			{
				_departmentTotal++;
			}
		}
        Notice.instance.Observe(NoticeName.OnOfficerDie, this);
		deadList.Clear();
		extinctList.Clear();
        model.movementScale = 1f;
        model.SetDefenseId("1");
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x0001F321 File Offset: 0x0001D521
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ParamInit();
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x0001E6E0 File Offset: 0x0001C8E0
	public override void ActivateQliphothCounter()
	{
		base.ActivateQliphothCounter();
		this.Escape();
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000F9DB0 File Offset: 0x000F7FB0
	public override void OnReleaseWork(UseSkill skill)
	{
		base.OnReleaseWork(skill);
		if (skill.agent.justiceLevel < 3)
		{
			this.model.SubQliphothCounter();
		}
		if (skill.agent.fortitudeLevel > 3)
		{
			this.model.SubQliphothCounter();
		}
		if (this.model.feelingState == CreatureFeelingState.BAD && this.Prob(80))
		{
			this.model.SubQliphothCounter();
		}
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x0002169D File Offset: 0x0001F89D
	public override void Escape()
	{ // <Mod>
		base.Escape();
		this.model.Escape();
        if (ReworkedVersion)
        {
            model.baseMaxHp += 5 * _deadCount;
            model.hp += 5f * (float)_deadCount;
        }
		this.skillCoolTimer.StartTimer(10f);
		this.MakeMovement();
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x000216C6 File Offset: 0x0001F8C6
	public override void OnSuppressed()
	{
		base.OnSuppressed();
		this.EndShooting();
		this.movable.StopMoving();
		this.MakeSound("Dead", this.model);
		this.ParamInit();
	}

	// Token: 0x06001F73 RID: 8051 RVA: 0x000F9E28 File Offset: 0x000F8028
	public override void OnReturn()
	{
		base.OnReturn();
		this.animScript.animator.SetBool("Attack", false);
		this.animScript.animator.SetBool("Skill", false);
		this.animScript.animator.SetBool("Move", false);
		this.animScript.animator.SetBool("Dead", false);
		this.model.ResetQliphothCounter();
	}

	// Token: 0x06001F74 RID: 8052 RVA: 0x000F9EA0 File Offset: 0x000F80A0
	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		foreach (Butterfly.ButterflyEffect butterflyEffect in this.effects)
		{
			butterflyEffect.Process();
		}
		for (int i = this.effects.Count - 1; i >= 0; i--)
		{
			if (!this.effects[i].IsEnable())
			{
				Notice.instance.Send(NoticeName.RemoveEtcUnit, new object[]
				{
					this.effects[i]
				});
				this.effects.RemoveAt(i);
			}
		}
		for (int j = this.attackDelays.Count - 1; j >= 0; j--)
		{
			this.attackDelays[j].Process();
			if (!this.attackDelays[j].IsEnable())
			{
				this.attackDelays.RemoveAt(j);
			}
		}
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x000F9FB8 File Offset: 0x000F81B8
	public override void UniqueEscape()
	{
		base.UniqueEscape();
		if (this.model.hp <= 0f || this.model.state != CreatureState.ESCAPE)
		{
			this.movable.StopMoving();
			return;
		}
		this.model.CheckNearWorkerEncounting();
		this.attackCoolTimer.RunTimer();
		this.skillCoolTimer.RunTimer();
		if (this.skillRemainTimer.started)
		{
			if (this.skillEffectTimer.RunTimer())
			{
				this.ShootButterfly();
				this.skillEffectTimer.StartTimer(0.3f);
			}
			if (this.skillRemainTimer.RunTimer())
			{
				this.EndShooting();
			}
			return;
		}
		if (this.animScript.animator.GetBool("Attack") || this.animScript.animator.GetBool("Skill"))
		{
			this.animScript.animator.SetBool("Move", false);
			this.movable.StopMoving();
			return;
		}
		if (this.attackAfterDelayTimer.started)
		{
			this.animScript.animator.SetBool("Move", false);
			this.movable.StopMoving();
			if (this.attackAfterDelayTimer.RunTimer())
			{
				this.MakeMovement();
			}
			return;
		}
		if (!this.movable.IsMoving() && !this.movable.InElevator())
		{
			this.MakeMovement();
		}
		List<MovableObjectNode> list = new List<MovableObjectNode>();
		if (this.currentPassage != null)
		{
			list = this.currentPassage.GetEnteredTargets(this.movable).ToList<MovableObjectNode>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				UnitModel unit = list[i].GetUnit();
				if (unit.hp <= 0f)
				{
					list.RemoveAt(i);
				}
				else if (unit is Butterfly.ButterflyEffect)
				{
					list.RemoveAt(i);
				}
				else if (unit is SnowWhite.VineArea)
				{
					list.RemoveAt(i);
				}
				else if (unit is YoungPrinceFriend.Spore)
				{
					list.RemoveAt(i);
				}
			}
			if (list.Count <= 0)
			{
				return;
			}
		}
		if (!this.skillCoolTimer.started && this.currentPassage != null && SefiraMapLayer.instance.GetPassageObject(this.currentPassage) != null && SefiraMapLayer.instance.GetPassageObject(this.currentPassage).type != SpaceObjectType.HUBSHORT && !this.movable.InElevator())
		{
			foreach (MovableObjectNode movableObjectNode in list)
			{
				if (this.movable.GetDirection() == UnitDirection.RIGHT != movableObjectNode.GetCurrentViewPosition().x < this.movable.GetCurrentViewPosition().x)
				{
					if (!this.animScript.animator.GetBool("Skill"))
					{
						this.StartSkill();
						return;
					}
				}
			}
		}
		if (!this.animScript.animator.GetBool("Skill") && !this.animScript.animator.GetBool("Attack") && !this.attackCoolTimer.started && this.attackTarget == null)
		{
			UnitModel unitModel = null;
			float num = 0f;
			foreach (MovableObjectNode movableObjectNode2 in list)
			{
				UnitModel unit2 = movableObjectNode2.GetUnit();
				if (unit2.hp > 0f)
				{
					if (unit2.IsAttackTargetable())
					{
						float x = movableObjectNode2.GetCurrentViewPosition().x;
						float x2 = this.movable.GetCurrentViewPosition().x;
						float num2 = Mathf.Abs(x2 - x);
						if (num2 <= 15f && num2 > num)
						{
							num = num2;
							unitModel = unit2;
						}
					}
				}
			}
			if (unitModel != null)
			{
				this.attackTarget = unitModel;
				if (unitModel.GetCurrentViewPosition().x <= this.movable.GetCurrentViewPosition().x)
				{
					this.movable.SetDirection(UnitDirection.LEFT);
				}
				else
				{
					this.movable.SetDirection(UnitDirection.RIGHT);
				}
				this.animScript.animator.SetBool("Move", false);
				this.movable.StopMoving();
				this.animScript.StartAttack();
			}
		}
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x000FA48C File Offset: 0x000F868C
	private void MakeMovement()
	{
		MapNode creatureRoamingPoint = MapGraph.instance.GetCreatureRoamingPoint();
		this.movable.MoveToNode(creatureRoamingPoint, false);
		this.animScript.animator.SetBool("Move", true);
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x000FA4C8 File Offset: 0x000F86C8
	public void AttackEnd()
	{ // <Mod>
		this.attackTarget = null;
		this.attackAfterDelayTimer.StartTimer(1f);
		float num = 4f;
		if (ReworkedVersion && _clerkTotal > 0)
		{
			num *= 1f - 0.5f * (float)_deadCount / (float)_clerkTotal;
		}
		this.attackCoolTimer.StartTimer(num);
		this.animScript.animator.SetBool("Skill", false);
		this.animScript.animator.SetBool("Attack", false);
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x000FA528 File Offset: 0x000F8728
	private void StartSkill()
	{
		this.animScript.animator.SetBool("Move", false);
		this.movable.StopMoving();
		this.animScript.StartSkill();
		this.MakeSound("Open", this.model, 0.6f);
		this.animScript.animator.SetBool("SkillEnd", false);
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000FA590 File Offset: 0x000F8790
	public void StartShooting()
	{
		this.skillRemainTimer.StartTimer(10f);
		this.skillEffectTimer.StartTimer(0.3f);
		this.skillCoolTimer.StartTimer(20f);
		this.MakeSound("Skill", this.model);
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x000FA5E0 File Offset: 0x000F87E0
	private void EndShooting()
	{
		this.skillRemainTimer.StopTimer();
		this.skillEffectTimer.StopTimer();
		this.animScript.animator.SetBool("SkillEnd", true);
		this.MakeSound("Close", this.model, 0.6f);
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000FA630 File Offset: 0x000F8830
	public void AttackDamage()
	{ // <Mod>
		if (this.attackTarget != null)
		{
			if (this.attackTarget.hp <= 0f)
			{
				return;
			}
			if (this.attackTarget.GetMovableNode().currentPassage != this.currentPassage)
			{
				return;
			}
			float x = this.attackTarget.GetCurrentViewPosition().x;
			float x2 = this.model.GetCurrentViewPosition().x;
			if (this.model.GetDirection() == UnitDirection.RIGHT && x < x2)
			{
				return;
			}
			if (this.model.GetDirection() == UnitDirection.LEFT && x > x2)
			{
				return;
			}
			if (this.attackTarget is OfficerModel)
			{
				OfficerModel officerModel = this.attackTarget as OfficerModel;
				officerModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.ButterflyAgentDead, false, true);
			}
			GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/Butterfly/ButterflyAttackEffect");
			gameObject.transform.position = this.attackTarget.GetCurrentViewPosition() + new Vector3(0f, 1.5f * base.Unit.gameObject.transform.localScale.y, 0f);
			gameObject.transform.localScale = new Vector3(base.Unit.gameObject.transform.localScale.y, base.Unit.gameObject.transform.localScale.y, base.Unit.gameObject.transform.localScale.z);
			this.MakeSound("Attack", this.attackTarget);
			float dmg = (float)Butterfly.attackDmg;
			RwbpType type = RwbpType.W;
			if (ReworkedVersion)
			{
				dmg += (float)_extinctCount * 1f;
				if (IsExtinctArea)
				{
					type = RwbpType.P;
					if (attackTarget is AgentModel)
					{
						AgentModel agentModel = attackTarget as AgentModel;
						agentModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.ButterflyAgentDead, false, true);
					}
				}
			}
			this.attackTarget.TakeDamage(this.model, new DamageInfo(type, dmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(this.attackTarget, type, this.model);
			if (this.attackTarget is OfficerModel)
			{
				OfficerModel officerModel2 = this.attackTarget as OfficerModel;
				if (!officerModel2.IsDead())
				{
					officerModel2.ResetSpecialDeadScene();
				}
			}
			else if (this.attackTarget is AgentModel)
			{
				AgentModel agentModel = this.attackTarget as AgentModel;
				if (agentModel.IsPanic())
				{
					agentModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.ButterflyAgentDead, false, true);
					agentModel.Die();
				}
				else if (type != RwbpType.W)
				{
					agentModel.ResetSpecialDeadScene();
				}
			}
		}
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000FA87C File Offset: 0x000F8A7C
	public void ShootButterfly()
	{
		Butterfly.ButterflyEffect butterflyEffect = new Butterfly.ButterflyEffect(this, this.model.GetDirection());
		this.effects.Add(butterflyEffect);
		Notice.instance.Send(NoticeName.AddEtcUnit, new object[]
		{
			butterflyEffect
		});
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x000FA8C0 File Offset: 0x000F8AC0
	public void TrySkillAttack(UnitModel target)
	{ // <Mod>
		if (target == this.model)
		{
			return;
		}
		if (this.attackDelays.Exists((Butterfly.AttackDelay x) => x.GetModel() == target && x.IsEnable()))
		{
			return;
		}
		if (target.hp <= 0f)
		{
			return;
		}
		if (target is OfficerModel)
		{
			OfficerModel officerModel = target as OfficerModel;
			officerModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.ButterflyAgentDead, false, true);
		}
		float dmg = (float)Butterfly.skillDmg;
		RwbpType type = RwbpType.W;
		if (ReworkedVersion)
		{
			dmg += (float)_extinctCount * 0.5f;
			if (IsExtinctArea)
			{
				type = RwbpType.P;
				if (target is AgentModel)
				{
					AgentModel agentModel = target as AgentModel;
					agentModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.ButterflyAgentDead, false, true);
				}
			}
		}
		target.TakeDamage(this.model, new DamageInfo(type, dmg));
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(target, type, this.model);
		if (target is OfficerModel)
		{
			OfficerModel officerModel2 = target as OfficerModel;
			if (!officerModel2.IsDead())
			{
				officerModel2.ResetSpecialDeadScene();
			}
		}
		else if (target is AgentModel)
		{
			AgentModel agentModel = target as AgentModel;
			if (agentModel.IsPanic())
			{
				agentModel.SetSpecialDeadScene(WorkerSpine.AnimatorName.ButterflyAgentDead, false, true);
				agentModel.Die();
			}
			else if (type != RwbpType.W)
			{
				agentModel.ResetSpecialDeadScene();
			}
		}
		if (target.hp <= 0f)
		{
			return;
		}
		Butterfly.AttackDelay item = new Butterfly.AttackDelay(target, 1f);
		this.attackDelays.Add(item);
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x000FAA20 File Offset: 0x000F8C20
	private SoundEffectPlayer MakeSound(string src, UnitModel target, float volume)
	{
		SoundEffectPlayer result = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(src, out filename))
		{
			result = SoundEffectPlayer.PlayOnce(filename, target.GetCurrentViewPosition(), volume);
		}
		return result;
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000FAA60 File Offset: 0x000F8C60
	private SoundEffectPlayer MakeSound(string src, UnitModel target)
	{
		SoundEffectPlayer result = null;
		string filename;
		if (this.model.metaInfo.soundTable.TryGetValue(src, out filename))
		{
			result = SoundEffectPlayer.PlayOnce(filename, target.GetCurrentViewPosition());
		}
		return result;
	}

	//> <Mod> Funeral Rework
	public bool ReworkedVersion
	{
		get
		{
			return SpecialModeConfig.instance.GetValue<bool>("FuneralRework");
		}
	}

    public bool IsExtinctArea
    {
        get
        {
			if (currentPassage == null) return false;
            return extinctList.Contains(currentPassage.GetSefira());
        }
    }
	
	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.OnOfficerDie)
		{
			OfficerModel officerModel = param[0] as OfficerModel;
			if (officerModel == null)
			{
				return;
			}
			if (deadList.Contains(officerModel))
			{
				return;
			}
			deadList.Add(officerModel);
            _deadCount++;
            if (ReworkedVersion)
            {
                if (model.IsEscaped())
                {
                    model.baseMaxHp += 5;
                    model.hp += 5f;
                }
				model.movementScale = 1f + _deadCount * 0.02f / 3f;
                if (_deadCount >= _clerkTotal)
                {
                    model.SetDefenseId("2");
                }
            }
			Sefira sefira = officerModel.GetCurrentSefira();
			if (sefira.GetAliveOfficerCnt() <= 0)
			{
				if (extinctList.Contains(sefira))
				{
					return;
				}
				extinctList.Add(sefira);
				_extinctCount++;
				if (ReworkedVersion)
				{
					model.SubQliphothCounter();
				}
			}
		}
	}

    public override void OnStageEnd()
    {
        Notice.instance.Remove(NoticeName.OnOfficerDie, this);
		deadList.Clear();
		extinctList.Clear();
        model.movementScale = 1f;
        model.SetDefenseId("1");
    }

	private int _deadCount;

	private int _extinctCount;

	private int _clerkTotal;

	private int _departmentTotal;

	private List<WorkerModel> deadList = new List<WorkerModel>();

	private List<Sefira> extinctList = new List<Sefira>();
    //<

	// Token: 0x04001FD2 RID: 8146
	private const int justiceCondition = 3;

	// Token: 0x04001FD3 RID: 8147
	private const int fortitudeCondition = 3;

	// Token: 0x04001FD4 RID: 8148
	private const int badResultProb = 80;

	// Token: 0x04001FD5 RID: 8149
	private const float attackRange = 15f;

	// Token: 0x04001FD6 RID: 8150
	private const int attackDmgMin = 10;

	// Token: 0x04001FD7 RID: 8151
	private const int attackDmgMax = 15;

	// Token: 0x04001FD8 RID: 8152
	private const float dmgDelay = 1f;

	// Token: 0x04001FD9 RID: 8153
	private const int skillDmgMin = 3;

	// Token: 0x04001FDA RID: 8154
	private const int skillDmgMax = 4;

	// Token: 0x04001FDB RID: 8155
	private Timer attackCoolTimer = new Timer();

	// Token: 0x04001FDC RID: 8156
	private const float attackCoolTime = 4f;

	// Token: 0x04001FDD RID: 8157
	private Timer attackAfterDelayTimer = new Timer();

	// Token: 0x04001FDE RID: 8158
	private const float attackAfterDelay = 1f;

	// Token: 0x04001FDF RID: 8159
	private Timer skillCoolTimer = new Timer();

	// Token: 0x04001FE0 RID: 8160
	private const float skillCoolTime = 20f;

	// Token: 0x04001FE1 RID: 8161
	private Timer skillRemainTimer = new Timer();

	// Token: 0x04001FE2 RID: 8162
	private const float skillRemainTime = 10f;

	// Token: 0x04001FE3 RID: 8163
	private Timer skillEffectTimer = new Timer();

	// Token: 0x04001FE4 RID: 8164
	private const float skillEffectTime = 0.3f;

	// Token: 0x04001FE5 RID: 8165
	private const float skillEffectGenDistacne = 2f;

	// Token: 0x04001FE6 RID: 8166
	private const float skillEffectMovement = 12f;

	// Token: 0x04001FE7 RID: 8167
	private const float skillEffectDamageDistance = 0.2f;

	// Token: 0x04001FE8 RID: 8168
	private UnitModel attackTarget;

	// Token: 0x04001FE9 RID: 8169
	public List<Butterfly.ButterflyEffect> effects = new List<Butterfly.ButterflyEffect>();

	// Token: 0x04001FEA RID: 8170
	public long nextEffectId = 10000L;

	// Token: 0x04001FEB RID: 8171
	private List<Butterfly.AttackDelay> attackDelays;

	// Token: 0x04001FEC RID: 8172
	private ButterflyAnim _animScript;

	// Token: 0x020003CF RID: 975
	public class AttackDelay
	{
		// Token: 0x06001F80 RID: 8064 RVA: 0x000216F7 File Offset: 0x0001F8F7
		public AttackDelay(UnitModel target, float remainDelay)
		{
			this.enable = true;
			this.target = target;
			this.remainDelay = remainDelay;
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x00021714 File Offset: 0x0001F914
		public void Process()
		{
			this.remainDelay -= Time.deltaTime;
			if (this.remainDelay <= 0f)
			{
				this.enable = false;
			}
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0002173F File Offset: 0x0001F93F
		public UnitModel GetModel()
		{
			return this.target;
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00021747 File Offset: 0x0001F947
		public bool IsEnable()
		{
			return this.enable;
		}

		// Token: 0x04001FED RID: 8173
		private bool enable;

		// Token: 0x04001FEE RID: 8174
		private UnitModel target;

		// Token: 0x04001FEF RID: 8175
		private float remainDelay;
	}

	// Token: 0x020003D0 RID: 976
	public class ButterflyEffect : UnitModel
	{
		// Token: 0x06001F84 RID: 8068 RVA: 0x000FAAA0 File Offset: 0x000F8CA0
		public ButterflyEffect(Butterfly script, UnitDirection dir)
		{
			this.script = script;
			long nextEffectId;
			script.nextEffectId = (nextEffectId = script.nextEffectId) + 1L;
			this.instanceId = nextEffectId;
			this.movableNode = new MovableObjectNode(false);
			this.movableNode.Assign(script.movable.GetSideMovableNode(script.model.GetDirection(), 2f * script.Unit.gameObject.transform.localScale.y));
			this.movableNode.SetDirection(dir);
			this.passage = this.movableNode.currentPassage;
			List<MapNode> list = new List<MapNode>(this.passage.GetNodeList());
			List<MapNode> list2 = list;
			if (Butterfly.ButterflyEffect.cache0 == null)
			{
				Butterfly.ButterflyEffect.cache0 = new Comparison<MapNode>(MapNode.CompareByX);
			}
			list2.Sort(Butterfly.ButterflyEffect.cache0);
			if (dir == UnitDirection.RIGHT)
			{
				this.goalNode = list[list.Count - 1];
			}
			else
			{
				this.goalNode = list[0];
			}
			base.AddUnitBuf(new UnitStatBuf(float.MaxValue)
			{
				duplicateType = BufDuplicateType.ONLY_ONE,
				movementSpeed = 12f
			});
			this.enable = true;
			this.removeTimer.StartTimer(15f);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0002174F File Offset: 0x0001F94F
		public bool IsEnable()
		{
			return this.enable;
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000FABF0 File Offset: 0x000F8DF0
		public void Process()
		{
			if (!this.enable)
			{
				return;
			}
			if (this.passage != null)
			{
				foreach (MovableObjectNode movableObjectNode in this.passage.GetEnteredTargets(this.movableNode))
				{
					UnitModel unit = movableObjectNode.GetUnit();
					if (!(unit is Butterfly.ButterflyEffect))
					{
						if (unit != this.script.model)
						{
							if (Math.Abs(movableObjectNode.GetCurrentViewPosition().x - this.movableNode.GetCurrentViewPosition().x) <= 0.2f)
							{
								this.script.TrySkillAttack(unit);
							}
						}
					}
				}
			}
			this.movableNode.MoveToNode(this.goalNode, false);
			this.movableNode.ProcessMoveNode(this.movement);
			if (this.movableNode.GetDirection() == UnitDirection.RIGHT)
			{
				if (this.movableNode.GetCurrentViewPosition().x >= this.goalNode.GetPosition().x)
				{
					this.OnArrive();
				}
			}
			else if (this.movableNode.GetCurrentViewPosition().x <= this.goalNode.GetPosition().x)
			{
				this.OnArrive();
			}
			if (Math.Abs(this.movableNode.GetCurrentViewPosition().x - this.goalNode.GetPosition().x) <= 0.15f)
			{
				this.OnArrive();
			}
			if (this.removeTimer.RunTimer())
			{
				this.OnArrive();
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x00021757 File Offset: 0x0001F957
		public void OnArrive()
		{
			this.enable = false;
		}

		// Token: 0x04001FF0 RID: 8176
		private Butterfly script;

		// Token: 0x04001FF1 RID: 8177
		private MapNode goalNode;

		// Token: 0x04001FF2 RID: 8178
		private PassageObjectModel passage;

		// Token: 0x04001FF3 RID: 8179
		private bool enable = true;

		// Token: 0x04001FF4 RID: 8180
		private Timer removeTimer = new Timer();

		// Token: 0x04001FF5 RID: 8181
		private const float removeTime = 15f;

		// Token: 0x04001FF6 RID: 8182
		[CompilerGenerated]
		private static Comparison<MapNode> cache0;
	}
}
