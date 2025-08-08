using System;
using UnityEngine.UI;

// Token: 0x02000878 RID: 2168
public class YesodCoreScript : CreatureBase
{
	// Token: 0x060042EB RID: 17131 RVA: 0x0001E6DE File Offset: 0x0001C8DE
	public YesodCoreScript()
	{
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x060042EC RID: 17132 RVA: 0x00039380 File Offset: 0x00037580
	public YesodCoreAnim AnimScript
	{
		get
		{
			return this._animScript;
		}
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x00039388 File Offset: 0x00037588
	public override void OnViewInit(CreatureUnit unit)
	{
		this._animScript = (unit.animTarget as YesodCoreAnim);
		this.AnimScript.SetScript(this);
		this.model.GetMovableNode().SetActive(false);
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsSensoredInPassage()
	{
		return false;
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsSuppressable()
	{
		return false;
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsIndirectSuppressable()
	{
		return false;
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return false;
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool HasEscapeUI()
	{
		return false;
	}

	// Token: 0x060042F4 RID: 17140 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool SetHpSlider(Slider slider)
	{
		return false;
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsAttackTargetable()
	{
		return false;
	}

	// Token: 0x04003D92 RID: 15762
	public YesodBossBase bossBase;

	// Token: 0x04003D93 RID: 15763
	public YesodCoreAnim _animScript;
}
