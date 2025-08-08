using System;
using UnityEngine.UI;

// Token: 0x02000876 RID: 2166
public class TipherethCoreScript : CreatureBase
{
	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06004375 RID: 17269 RVA: 0x000398AE File Offset: 0x00037AAE
	public TipherethCoreAnim AnimScript
	{
		get
		{
			return this._animScript;
		}
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x000398B6 File Offset: 0x00037AB6
	public override void OnViewInit(CreatureUnit unit)
	{
		this._animScript = (unit.animTarget as TipherethCoreAnim);
		this.AnimScript.SetScript(this);
		this.model.GetMovableNode().SetActive(false);
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsSensoredInPassage()
	{
		return false;
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsSuppressable()
	{
		return false;
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsAutoSuppressable()
	{
		return false;
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsIndirectSuppressable()
	{
		return false;
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool CanTakeDamage(UnitModel attacker, DamageInfo dmg)
	{
		return false;
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool HasEscapeUI()
	{
		return false;
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool SetHpSlider(Slider slider)
	{
		return false;
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x000044EF File Offset: 0x000026EF
	public override bool IsAttackTargetable()
	{
		return false;
	}

	// Token: 0x04003DED RID: 15853
	private TipherethCoreAnim _animScript;

	// Token: 0x04003DEE RID: 15854
	public TipherethBossBase bossBase;
}
