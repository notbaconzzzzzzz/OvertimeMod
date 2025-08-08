using System;
using UnityEngine.UI;

// Token: 0x02000875 RID: 2165
public class MalkutCoreScript : CreatureBase
{
	// Token: 0x060042BA RID: 17082 RVA: 0x0001E63C File Offset: 0x0001C83C
	public MalkutCoreScript()
	{
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x060042BB RID: 17083 RVA: 0x000390E0 File Offset: 0x000372E0
	public MalkutCoreAnim AnimScript
	{
		get
		{
			return this._animScript;
		}
	}

	// Token: 0x060042BC RID: 17084 RVA: 0x000390E8 File Offset: 0x000372E8
	public override void OnViewInit(CreatureUnit unit)
	{
		this._animScript = (unit.animTarget as MalkutCoreAnim);
		this.AnimScript.SetScript(this);
		this.bossBase.StartEffect();
		this.model.GetMovableNode().SetActive(false);
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsSensoredInPassage()
	{
		return false;
	}

	// Token: 0x060042BE RID: 17086 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsSuppressable()
	{
		return false;
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsIndirectSuppressable()
	{
		return false;
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return false;
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool HasEscapeUI()
	{
		return false;
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool SetHpSlider(Slider slider)
	{
		return false;
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x000044AF File Offset: 0x000026AF
	public override bool IsAttackTargetable()
	{
		return false;
	}

	// Token: 0x04003D86 RID: 15750
	private MalkutCoreAnim _animScript;

	// Token: 0x04003D87 RID: 15751
	public MalkutBossBase bossBase;
}
