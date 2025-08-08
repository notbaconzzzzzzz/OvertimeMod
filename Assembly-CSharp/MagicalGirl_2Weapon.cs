using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200066A RID: 1642
public class MagicalGirl_2Weapon : EquipmentScriptBase
{
	// Token: 0x06003644 RID: 13892 RVA: 0x001615D4 File Offset: 0x0015F7D4
	public MagicalGirl_2Weapon()
	{
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x0016162C File Offset: 0x0015F82C
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		if (num < this._PROB_ANIM_PATTERN_NORMAL)
		{
			this.PrintLog("Pattern 1");
			for (int i = 0; i < 3; i++)
			{
				list.Add(base.model.metaInfo.damageInfos[0].Copy());
			}
			animationName = base.model.metaInfo.animationNames[0];
			this._attackPattern = MagicalGirl_2Weapon.ATTACK_PATTERN.NORMAL;
		}
		else
		{
			this.PrintLog("Pattern 2");
			list.Add(base.model.metaInfo.damageInfos[1].Copy());
			animationName = base.model.metaInfo.animationNames[1];
			this._attackPattern = MagicalGirl_2Weapon.ATTACK_PATTERN.SPECIAL;
		}
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x00030E38 File Offset: 0x0002F038
	public override void OnAttackEnd(UnitModel actor, UnitModel target)
	{
		this.PrintLog("AttackEnd");
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x00161708 File Offset: 0x0015F908
	public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		AgentModel agentModel = actor as AgentModel;
		this.PrintLog("GiveDamage");
		if (this._attackPattern == MagicalGirl_2Weapon.ATTACK_PATTERN.NORMAL)
		{
			if (actor.HasUnitBuf(UnitBufType.MAGICALGIRL_2_WEAPON))
			{
				dmg.min += this._AMOUNT_INCREASE_DMG;
				dmg.max += this._AMOUNT_INCREASE_DMG;
				this.PrintLog(string.Concat(new object[]
				{
					"Buf damage Dmg(min, max) : (",
					dmg.min,
					", ",
					dmg.max,
					")"
				}));
			}
			else
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				if (num < this._PROB_INCREASE_DMG)
				{
					actor.AddUnitBuf(new UnitStatBuf(this._DURATION_ATTACK_BUF, UnitBufType.MAGICALGIRL_2_WEAPON)
					{
						duplicateType = BufDuplicateType.ONLY_ONE
					});
					this.PrintLog("Add buf");
					actor.AddUnitBuf(new UnitStatBuf(this._DURATION_WORK_DEBUFF, UnitBufType.MAGICALGIRL_2_WEAPON_DEBUFF)
					{
						workProb = (int)((float)agentModel.workProb * this._AMOUNT_RATIO_DEBUFF),
						cubeSpeed = (int)((float)agentModel.workSpeed * this._AMOUNT_RATIO_DEBUFF),
						duplicateType = BufDuplicateType.ONLY_ONE
					});
				}
			}
		}
		return true;
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x00030E45 File Offset: 0x0002F045
	private void PrintLog(object s)
	{
		if (this._LOG_STATE)
		{
			Debug.LogError(s);
		}
	}

	// Token: 0x0400322E RID: 12846
	private readonly bool _LOG_STATE;

	// Token: 0x0400322F RID: 12847
	private readonly float _PROB_ANIM_PATTERN_NORMAL = 0.9f;

	// Token: 0x04003230 RID: 12848
	private readonly float _PROB_INCREASE_DMG = 0.1f;

	// Token: 0x04003231 RID: 12849
	private readonly float _AMOUNT_INCREASE_DMG = 5f;

	// Token: 0x04003232 RID: 12850
	private readonly float _AMOUNT_RATIO_DEBUFF = -0.5f;

	// Token: 0x04003233 RID: 12851
	private readonly float _DURATION_ATTACK_BUF = 12f;

	// Token: 0x04003234 RID: 12852
	private readonly float _DURATION_WORK_DEBUFF = 120f;

	// Token: 0x04003235 RID: 12853
	private MagicalGirl_2Weapon.ATTACK_PATTERN _attackPattern;

	// Token: 0x0200066B RID: 1643
	private enum ATTACK_PATTERN
	{
		// Token: 0x04003237 RID: 12855
		NORMAL,
		// Token: 0x04003238 RID: 12856
		SPECIAL
	}
}
