using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003B1 RID: 945
public class BlackSwanSister : CreatureBase
{
	// Token: 0x06001E13 RID: 7699 RVA: 0x000F5BF4 File Offset: 0x000F3DF4
	public BlackSwanSister()
	{
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06001E14 RID: 7700 RVA: 0x00020560 File Offset: 0x0001E760
	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(5, 9);
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0002056A File Offset: 0x0001E76A
	private static int skillDmg
	{
		get
		{
			return UnityEngine.Random.Range(30, 46);
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0001E847 File Offset: 0x0001CA47
	private ChildCreatureModel Model
	{
		get
		{
			return this.model as ChildCreatureModel;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001E17 RID: 7703 RVA: 0x00020575 File Offset: 0x0001E775
	public BlackSwanSisterAnim animScript
	{
		get
		{
			if (this._animScript == null)
			{
				this._animScript = (base.Unit.animTarget as BlackSwanSisterAnim);
			}
			return this._animScript;
		}
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000F5C60 File Offset: 0x000F3E60
	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		this.initTimer.StartTimer(4f);
		this.reflectCoolTimer.StartTimer(10f);
		this.init = false;
		this.oldPassage = null;
		this.neighborDoors = new List<MapNode>();
		this.animScript.SetScript(this);
		this.neighbors = new List<BlackSwanSister.AnnoyedNeighbor>();
		this.MakeEffect("BlackSwanSummonEffect", 0f, 0.5f, this.model);
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000205A4 File Offset: 0x0001E7A4
	public override void SetModel(CreatureModel model)
	{
		this.model = model;
		this.SetBrother(((ChildCreatureModel)model).parent.script as BlackSwan);
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000205C8 File Offset: 0x0001E7C8
	private void SetBrother(BlackSwan brother)
	{
		this.brother = brother;
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000F5CE0 File Offset: 0x000F3EE0
	public override void OnFixedUpdate(CreatureModel creature)
	{
		if (this.model.hp <= 0f)
		{
			return;
		}
		base.OnFixedUpdate(creature);
		this.model.CheckNearWorkerEncounting();
		foreach (BlackSwanSister.AnnoyedNeighbor annoyedNeighbor in this.neighbors)
		{
			annoyedNeighbor.Process();
		}
		for (int i = this.neighbors.Count - 1; i >= 0; i--)
		{
			if (!this.neighbors[i].IsEnable())
			{
				this.neighbors.RemoveAt(i);
			}
		}
		if (this.currentPassage != null && this.currentPassage != this.oldPassage)
		{
			this.oldPassage = this.currentPassage;
			this.neighborDoors = new List<MapNode>(this.oldPassage.GetNodeList());
			for (int j = this.neighborDoors.Count - 1; j >= 0; j--)
			{
				if (this.neighborDoors[j].connectedCreature == null)
				{
					this.neighborDoors.RemoveAt(j);
				}
			}
		}
		if (this.neighborDoors.Count > 0)
		{
			foreach (MapNode mapNode in this.neighborDoors)
			{
				if (Math.Abs(this.movable.GetCurrentViewPosition().x - mapNode.GetPosition().x) <= 0.2f)
				{
					CreatureModel neighbor = mapNode.connectedCreature;
					if (neighbor != null && !this.neighbors.Exists((BlackSwanSister.AnnoyedNeighbor x) => x.GetModel() == neighbor) && !neighbor.IsEscaped())
					{
						BlackSwanSister.AnnoyedNeighbor item = new BlackSwanSister.AnnoyedNeighbor(neighbor, 30f);
						this.neighbors.Add(item);
						neighbor.SubQliphothCounter();
						GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/BlackSwan/BlackSwanAnnoyEffect");
						float num = 1f;
						if (this.currentPassage != null)
						{
							num = this.currentPassage.scaleFactor;
						}
						gameObject.transform.position = mapNode.GetPosition() + new Vector3(1.5f * num, 0.5f * num, -0.1f);
						gameObject.transform.localScale = new Vector3(num, num, num);
					}
				}
			}
		}
		if (this.animScript.animator)
		{
			this.animScript.animator.SetBool("Move", this.movable.IsMoving());
		}
		if (!this.init)
		{
			this.movable.StopMoving();
			if (this.initTimer.RunTimer())
			{
				this.MakeMovement();
				this.init = true;
			}
			return;
		}
		this.attackCoolTimer.RunTimer();
		this.skillCoolTimer.RunTimer();
		this.reflectCoolTimer.RunTimer();
		if (this.animScript.IsReflecting())
		{
			if (!this.movable.IsMoving())
			{
				this.MakeMovement();
			}
			if (this.reflectRemainTimer.RunTimer())
			{
				this.animScript.SetRefelect(false);
				this.reflectCoolTimer.StartTimer(15f);
			}
			return;
		}
		if (this.attackDelayTimer.started)
		{
			this.movable.StopMoving();
			if (this.attackDelayTimer.RunTimer())
			{
				this.MakeMovement();
			}
			return;
		}
		if (this.animScript.IsAttacking() || this.animScript.IsCastingSkill())
		{
			this.movable.StopMoving();
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
			for (int k = list.Count - 1; k >= 0; k--)
			{
				UnitModel unit = list[k].GetUnit();
				if (unit.hp <= 0f)
				{
					list.RemoveAt(k);
				}
				if (unit == (this.model as ChildCreatureModel).parent)
				{
					list.RemoveAt(k);
				}
				if (unit is Butterfly.ButterflyEffect)
				{
					list.RemoveAt(k);
				}
				else if (unit is SnowWhite.VineArea)
				{
					list.RemoveAt(k);
				}
				else if (unit is YoungPrinceFriend.Spore)
				{
					list.RemoveAt(k);
				}
			}
			if (list.Count <= 0)
			{
				return;
			}
		}
		if (this.animScript.IsTransformed() && !this.skillCoolTimer.started)
		{
			this.SkillStart();
			return;
		}
		if (!this.reflectCoolTimer.started)
		{
			foreach (MovableObjectNode movableObjectNode in list)
			{
				if (this.movable.GetDirection() == UnitDirection.RIGHT != movableObjectNode.GetCurrentViewPosition().x < this.movable.GetCurrentViewPosition().x)
				{
					if (!this.animScript.IsReflecting())
					{
						this.animScript.SetRefelect(true);
						this.reflectRemainTimer.StartTimer(10f);
						return;
					}
				}
			}
		}
		if (!this.attackCoolTimer.started)
		{
			foreach (MovableObjectNode movableObjectNode2 in list)
			{
				UnitModel unit2 = movableObjectNode2.GetUnit();
				if (unit2.IsAttackTargetable())
				{
					if (this.IsInRange(unit2, 4f))
					{
						float x3 = unit2.GetCurrentViewPosition().x;
						float x2 = this.model.GetCurrentViewPosition().x;
						if (x3 < x2)
						{
							this.movable.SetDirection(UnitDirection.LEFT);
						}
						else if (x3 > x2)
						{
							this.movable.SetDirection(UnitDirection.RIGHT);
						}
						this.AttackStart();
						break;
					}
				}
			}
		}
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x000205D1 File Offset: 0x0001E7D1
	private void MakeMovement()
	{
		this.movable.MoveToNode(MapGraph.instance.GetCreatureRoamingPoint(), false);
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000F63E0 File Offset: 0x000F45E0
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		if (attacker == null || dmg == null)
		{
			return base.CanTakeDamage(attacker, dmg);
		}
		if (!this.animScript.animator.GetBool("Reflect"))
		{
			return true;
		}
		float x = attacker.GetCurrentViewPosition().x;
		float x2 = this.movable.GetCurrentViewPosition().x;
		if (this.movable.GetDirection() == UnitDirection.LEFT && x2 < x)
		{
			return true;
		}
		if (this.movable.GetDirection() == UnitDirection.RIGHT && x2 > x)
		{
			return true;
		}
		attacker.TakeDamage(this.model, dmg);
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(attacker, dmg.type, this.model);
		this.MakeSound("reflect");
		this.MakeEffect("BlackSwanReflectEffect", 5f, 2f, this.model);
		return false;
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000F64C0 File Offset: 0x000F46C0
	public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
	{
		base.OnTakeDamage(actor, dmg, value);
		if (this.model.hp <= (float)this.model.maxHp * 0.4f)
		{
			if (this.animScript.IsTransformed())
			{
				return;
			}
			this.init = false;
			this.initTimer.StartTimer(1.4f);
			this.skillCoolTimer.StartTimer(10f);
			this.animScript.Transform();
			this.MakeSound("trans");
		}
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000205E9 File Offset: 0x0001E7E9
	private void AttackStart()
	{
		this.movable.StopMoving();
		this.animScript.AttackStart();
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x00020601 File Offset: 0x0001E801
	public void AttackEnd()
	{
		this.attackDelayTimer.StartTimer(1f);
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x000F6548 File Offset: 0x000F4748
	private void SkillStart()
	{
		this.movable.StopMoving();
		this.animScript.SkillStart();
		this.cryingSound = this.MakeSoundLoop("skill");
		this.MakeEffect("BlackSwanSkillEffect", 0.8f, 4.6f, this.model);
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x000F6598 File Offset: 0x000F4798
	public void SkillDamage()
	{
		if (this.currentPassage != null)
		{
			List<PassageObjectModel> list = new List<PassageObjectModel>(this.model.sefira.passageList);
			foreach (PassageObjectModel passageObjectModel in list)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.type != PassageType.VERTICAL)
					{
						foreach (MovableObjectNode movableObjectNode in passageObjectModel.GetEnteredTargets(this.movable))
						{
							UnitModel unit = movableObjectNode.GetUnit();
							if (unit.hp > 0f)
							{
								if (!(unit is SnowWhite.VineArea))
								{
									if (!(unit is YoungPrinceFriend.Spore))
									{
										if (!(unit is Butterfly.ButterflyEffect))
										{
											if (unit != (this.model as ChildCreatureModel).parent)
											{
												unit.TakeDamage(this.model, new DamageInfo(RwbpType.W, (float)BlackSwanSister.skillDmg));
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

	// Token: 0x06001E23 RID: 7715 RVA: 0x000F66E0 File Offset: 0x000F48E0
	public void SkillEnd()
	{
		if (this.cryingSound != null)
		{
			this.cryingSound.Stop();
			this.cryingSound = null;
		}
		this.skillCoolTimer.StartTimer(25f);
		this.attackDelayTimer.StartTimer(1f);
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000F6730 File Offset: 0x000F4930
	private bool IsInRange(UnitModel unit, float range)
	{
		float num = 1f;
		if (this.currentPassage != null)
		{
			num = this.currentPassage.scaleFactor;
		}
		return Math.Abs(unit.GetCurrentViewPosition().x - this.movable.GetCurrentViewPosition().x) <= range * num;
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000F678C File Offset: 0x000F498C
	public override bool OnAfterSuppressed()
	{
		if (this.cryingSound != null)
		{
			this.cryingSound.Stop();
			this.cryingSound = null;
		}
		this.animScript.animator.SetBool("Dead", true);
		this.MakeSound("dead");
		return true;
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000F67E0 File Offset: 0x000F49E0
	public void GiveAttackDamage()
	{
		if (this.currentPassage != null)
		{
			bool flag = false;
			foreach (MovableObjectNode movableObjectNode in this.currentPassage.GetEnteredTargets(this.movable))
			{
				UnitModel unit = movableObjectNode.GetUnit();
				if (unit.hp > 0f)
				{
					if (unit != (this.model as ChildCreatureModel).parent)
					{
						if (!(unit is SnowWhite.VineArea))
						{
							if (!(unit is YoungPrinceFriend.Spore))
							{
								if (!(unit is Butterfly.ButterflyEffect))
								{
									if (this.IsInRange(unit, 4.6f))
									{
										float x = unit.GetCurrentViewPosition().x;
										float x2 = this.model.GetCurrentViewPosition().x;
										if (this.model.GetDirection() == UnitDirection.RIGHT && x < x2)
										{
											return;
										}
										if (this.model.GetDirection() == UnitDirection.LEFT && x > x2)
										{
											return;
										}
										flag = true;
										unit.TakeDamage(this.model, new DamageInfo(RwbpType.R, (float)BlackSwanSister.attackDmg));
										DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unit, RwbpType.R, this.model);
										this.MakeEffect("BlackSwanAttackEffect", 0f, 0.5f, unit);
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				this.MakeSound("atk_dmg");
			}
		}
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000F4778 File Offset: 0x000F2978
	public string GetSoundSrc(string key)
	{
		string empty = string.Empty;
		if (this.model.metaInfo.soundTable.TryGetValue(key, out empty))
		{
		}
		return empty;
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x000F695C File Offset: 0x000F4B5C
	public override SoundEffectPlayer MakeSound(string src)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		return SoundEffectPlayer.PlayOnce(soundSrc, this.model.Unit.transform.position);
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x000F69A4 File Offset: 0x000F4BA4
	public override SoundEffectPlayer MakeSoundLoop(string src)
	{
		string soundSrc = this.GetSoundSrc(src);
		if (soundSrc == string.Empty)
		{
			return null;
		}
		return SoundEffectPlayer.Play(soundSrc, this.model.Unit.transform);
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x000140D5 File Offset: 0x000122D5
	public override bool HasEscapeUI()
	{
		return true;
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x000F69E4 File Offset: 0x000F4BE4
	private GameObject MakeEffect(string src, float fixedX, float fixedY, UnitModel target)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/BlackSwan/" + src);
		float num = 1f;
		float num2 = 1f;
		float d = 1f;
		if (this.model.GetDirection() == UnitDirection.LEFT)
		{
			num = -1f;
		}
		if (this.currentPassage != null)
		{
			num2 = this.currentPassage.scaleFactor;
		}
		if (src == "BlackSwanReflectEffect")
		{
			d = 2.5f;
		}
		gameObject.transform.position = target.GetCurrentViewPosition() + new Vector3(fixedX * num * num2, fixedY * num2, 0f);
		gameObject.transform.localScale = new Vector3(num2 * num, num2, num2) * d;
		if (src != "BlackSwanSkillEffect" && src != "BlackSwanReflectEffect")
		{
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = component.main;
			ParticleSystem.MinMaxCurve gravityModifier = main.gravityModifier;
			ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
			ParticleSystem.MinMaxCurve startSize = main.startSize;
			float num3 = gravityModifier.constant;
			float num4 = startSpeed.constantMin;
			float num5 = startSpeed.constantMax;
			float num6 = startSize.constantMin;
			float num7 = startSize.constantMax;
			num3 *= num2;
			num4 *= num2;
			num5 *= num2;
			num6 *= num2;
			num7 *= num2;
			gravityModifier.constant = num3;
			startSpeed.constantMin = num4;
			startSpeed.constantMax = num5;
			startSize.constantMin = num6;
			startSize.constantMax = num7;
			main.gravityModifier = gravityModifier;
			main.startSpeed = startSpeed;
			main.startSize = startSize;
			gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
		}
		return gameObject;
	}

	// Token: 0x04001E4A RID: 7754
	private Timer initTimer = new Timer();

	// Token: 0x04001E4B RID: 7755
	private const float initTime = 4f;

	// Token: 0x04001E4C RID: 7756
	private const float transformInitTime = 1.4f;

	// Token: 0x04001E4D RID: 7757
	private Timer attackDelayTimer = new Timer();

	// Token: 0x04001E4E RID: 7758
	private const float attackDelayTime = 1f;

	// Token: 0x04001E4F RID: 7759
	private Timer attackCoolTimer = new Timer();

	// Token: 0x04001E50 RID: 7760
	private const float attackCoolTime = 1f;

	// Token: 0x04001E51 RID: 7761
	private Timer skillCoolTimer = new Timer();

	// Token: 0x04001E52 RID: 7762
	private const float skillCoolTime = 25f;

	// Token: 0x04001E53 RID: 7763
	private Timer reflectCoolTimer = new Timer();

	// Token: 0x04001E54 RID: 7764
	private const float reflectCoolTime = 15f;

	// Token: 0x04001E55 RID: 7765
	private Timer reflectRemainTimer = new Timer();

	// Token: 0x04001E56 RID: 7766
	private const float reflectRemainTime = 10f;

	// Token: 0x04001E57 RID: 7767
	private const float attackRange = 4f;

	// Token: 0x04001E58 RID: 7768
	private const float damageRange = 4.6f;

	// Token: 0x04001E59 RID: 7769
	private const int attackDmgMin = 5;

	// Token: 0x04001E5A RID: 7770
	private const int attackDmgMax = 8;

	// Token: 0x04001E5B RID: 7771
	private const int skillDmgMin = 30;

	// Token: 0x04001E5C RID: 7772
	private const int skillDmgMax = 45;

	// Token: 0x04001E5D RID: 7773
	private const float transformHpCondition = 0.4f;

	// Token: 0x04001E5E RID: 7774
	private const float SubQliphothCounterCoolTime = 30f;

	// Token: 0x04001E5F RID: 7775
	public const string sound_atk_dmg = "atk_dmg";

	// Token: 0x04001E60 RID: 7776
	public const string sound_pre_atk = "pre_atk";

	// Token: 0x04001E61 RID: 7777
	public const string sound_dead = "dead";

	// Token: 0x04001E62 RID: 7778
	public const string sound_reflect = "reflect";

	// Token: 0x04001E63 RID: 7779
	public const string sound_skill = "skill";

	// Token: 0x04001E64 RID: 7780
	public const string sound_trans = "trans";

	// Token: 0x04001E65 RID: 7781
	private const string effectSrc = "Effect/Creature/BlackSwan/";

	// Token: 0x04001E66 RID: 7782
	private const string effect_skill = "BlackSwanSkillEffect";

	// Token: 0x04001E67 RID: 7783
	private const string effect_attack = "BlackSwanAttackEffect";

	// Token: 0x04001E68 RID: 7784
	private const string effect_summon = "BlackSwanSummonEffect";

	// Token: 0x04001E69 RID: 7785
	private const string effect_annoy = "BlackSwanAnnoyEffect";

	// Token: 0x04001E6A RID: 7786
	private const string effect_reflect = "BlackSwanReflectEffect";

	// Token: 0x04001E6B RID: 7787
	private const float skillEffectFixedX = 0.8f;

	// Token: 0x04001E6C RID: 7788
	private const float skillEffectFixedY = 4.6f;

	// Token: 0x04001E6D RID: 7789
	private const float attackEffectFixedX = 0f;

	// Token: 0x04001E6E RID: 7790
	private const float attackEffectFixedY = 0.5f;

	// Token: 0x04001E6F RID: 7791
	private const float summonEffectFixedX = 0f;

	// Token: 0x04001E70 RID: 7792
	private const float summonEffectFixedY = 0.5f;

	// Token: 0x04001E71 RID: 7793
	private const float annoyEffectFixedX = 1.5f;

	// Token: 0x04001E72 RID: 7794
	private const float annoyEffectFixedY = 0.5f;

	// Token: 0x04001E73 RID: 7795
	private const float reflectEffectFixedX = 5f;

	// Token: 0x04001E74 RID: 7796
	private const float reflectEffectFixedY = 2f;

	// Token: 0x04001E75 RID: 7797
	private bool init;

	// Token: 0x04001E76 RID: 7798
	private List<BlackSwanSister.AnnoyedNeighbor> neighbors = new List<BlackSwanSister.AnnoyedNeighbor>();

	// Token: 0x04001E77 RID: 7799
	private BlackSwan brother;

	// Token: 0x04001E78 RID: 7800
	private SoundEffectPlayer cryingSound;

	// Token: 0x04001E79 RID: 7801
	private ChildCreatureUnit _unit;

	// Token: 0x04001E7A RID: 7802
	private PassageObjectModel oldPassage;

	// Token: 0x04001E7B RID: 7803
	private List<MapNode> neighborDoors = new List<MapNode>();

	// Token: 0x04001E7C RID: 7804
	private BlackSwanSisterAnim _animScript;

	// Token: 0x020003B2 RID: 946
	public class AnnoyedNeighbor
	{
		// Token: 0x06001E2C RID: 7724 RVA: 0x00020613 File Offset: 0x0001E813
		public AnnoyedNeighbor(CreatureModel creature, float coolTime)
		{
			this.enable = true;
			this.creature = creature;
			this.coolTime = coolTime;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00020637 File Offset: 0x0001E837
		public void Process()
		{
			this.coolTime -= Time.deltaTime;
			if (this.coolTime <= 0f)
			{
				this.enable = false;
			}
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00020662 File Offset: 0x0001E862
		public CreatureModel GetModel()
		{
			return this.creature;
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0002066A File Offset: 0x0001E86A
		public bool IsEnable()
		{
			return this.enable;
		}

		// Token: 0x04001E7D RID: 7805
		private bool enable = true;

		// Token: 0x04001E7E RID: 7806
		private CreatureModel creature;

		// Token: 0x04001E7F RID: 7807
		private float coolTime;
	}
}
