using System;
using UnityEngine.UI;

// Token: 0x02000878 RID: 2168
public class NetzachCoreScript : CreatureBase
{
	// Token: 0x06004314 RID: 17172 RVA: 0x0001E77B File Offset: 0x0001C97B
	public NetzachCoreScript()
	{
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06004315 RID: 17173 RVA: 0x000394E5 File Offset: 0x000376E5
	public NetzachCoreAnim AnimScript
	{
		get
		{
			return this._animScript;
		}
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x000394ED File Offset: 0x000376ED
	public override void OnViewInit(CreatureUnit unit)
	{ // <Mod>
		this._animScript = (unit.animTarget as NetzachCoreAnim);
		this.AnimScript.SetScript(this);
        if (bossBase != null)
        {
		    this.bossBase.StartEffect();
        }
		this.model.GetMovableNode().SetActive(false);
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool IsSensoredInPassage()
	{
		return false;
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool IsSuppressable()
	{
		return false;
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x0600431A RID: 17178 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool IsIndirectSuppressable()
	{
		return false;
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return false;
	}

	// Token: 0x0600431C RID: 17180 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool HasEscapeUI()
	{
		return false;
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool SetHpSlider(Slider slider)
	{
		return false;
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x000044D7 File Offset: 0x000026D7
	public override bool IsAttackTargetable()
	{
		return false;
	}

	// Token: 0x04003DBF RID: 15807
	private NetzachCoreAnim _animScript;

	// Token: 0x04003DC0 RID: 15808
	public NetzachBossBase bossBase;
}
