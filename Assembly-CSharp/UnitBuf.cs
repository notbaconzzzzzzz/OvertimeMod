using System;
using UnityEngine;

// Token: 0x02000B89 RID: 2953
[Serializable]
public class UnitBuf
{
	// Token: 0x06005965 RID: 22885 RVA: 0x00048007 File Offset: 0x00046207
	public UnitBuf()
	{
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x0004801A File Offset: 0x0004621A
	public virtual void Init(UnitModel model)
	{
		this.model = model;
	}

	// Token: 0x06005967 RID: 22887 RVA: 0x00048023 File Offset: 0x00046223
	public virtual void FixedUpdate()
	{
		this.remainTime -= Time.deltaTime;
		if (this.remainTime <= 0f)
		{
			this.Destroy();
		}
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x0004804A File Offset: 0x0004624A
	public virtual void Destroy()
	{
		if (this.model != null)
		{
			this.model.RemoveUnitBuf(this);
		}
	}

	// Token: 0x06005969 RID: 22889 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnDestroy()
	{
	}

	// Token: 0x0600596A RID: 22890 RVA: 0x0002215C File Offset: 0x0002035C
	public virtual float MovementScale()
	{
		return 1f;
	}

	// Token: 0x0600596B RID: 22891 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnUnitDie()
	{
	}

	// Token: 0x0600596C RID: 22892 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnUnitPanic()
	{
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x00046755 File Offset: 0x00044955
	public virtual void OnStageRelease()
	{
		this.Destroy();
	}

	// Token: 0x0600596E RID: 22894 RVA: 0x0002215C File Offset: 0x0002035C
	public virtual float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		return 1f;
	}

	// Token: 0x0600596F RID: 22895 RVA: 0x0002215C File Offset: 0x0002035C
	public virtual float GetDamageFactor()
	{
		return 1f;
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x0002215C File Offset: 0x0002035C
	public virtual float GetDamageFactor(UnitModel target, DamageInfo info)
	{
		return 1f;
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x00014081 File Offset: 0x00012281
	public virtual bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
	{
		return true;
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x00022270 File Offset: 0x00020470
	public virtual float GetWorkProbSpecialBonus(UnitModel actor, SkillTypeInfo skill)
	{
		return 0f;
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x000043AD File Offset: 0x000025AD
	public virtual void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
	{
	}

	// Token: 0x04005282 RID: 21122
	public UnitBufType type;

	// Token: 0x04005283 RID: 21123
	public float remainTime;

	// Token: 0x04005284 RID: 21124
	[NonSerialized]
	public UnitModel model;

	// Token: 0x04005285 RID: 21125
	public string effectSrc = string.Empty;

	// Token: 0x04005286 RID: 21126
	public BufDuplicateType duplicateType;
}
