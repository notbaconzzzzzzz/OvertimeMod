using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000582 RID: 1410
public class DamageInfo
{
    // Token: 0x0600306D RID: 12397 RVA: 0x00146B78 File Offset: 0x00144D78
    public DamageInfo(RwbpType type, float damage)
    {
        this.type = type;
        this.max = damage;
        this.min = damage;
    }

    // Token: 0x0600306E RID: 12398 RVA: 0x0002CF26 File Offset: 0x0002B126
    public DamageInfo(RwbpType type, int min, int max)
    {
        this.type = type;
        this.min = (float)min;
        this.max = (float)max;
    }

    // Token: 0x1700047D RID: 1149
    // (get) Token: 0x0600306F RID: 12399 RVA: 0x0002CF66 File Offset: 0x0002B166
    public static DamageInfo zero
    {
        get
        {
            return new DamageInfo(RwbpType.N, 0f);
        }
    }

    // Token: 0x06003070 RID: 12400 RVA: 0x00146BC4 File Offset: 0x00144DC4
    public DamageInfo Copy()
    {
        return new DamageInfo(this.type, (int)this.min, (int)this.max)
        {
            soundInfo = this.soundInfo,
            effectInfo = this.effectInfo,
            effectInfos = this.effectInfos,
            param = this.param
        };
    }

    // Token: 0x06003071 RID: 12401 RVA: 0x00146C20 File Offset: 0x00144E20
    public static DamageInfo operator *(DamageInfo d, float f)
    {
        DamageInfo damageInfo = d.Copy();
        damageInfo.min *= f;
        damageInfo.max *= f;
        return damageInfo;
    }

    // Token: 0x06003072 RID: 12402 RVA: 0x0002CF73 File Offset: 0x0002B173
    public static DamageInfo operator *(float f, DamageInfo d)
    {
        return d * f;
    }

    // Token: 0x06003073 RID: 12403 RVA: 0x0002CF7C File Offset: 0x0002B17C
    public float GetDamage()
    {
        return UnityEngine.Random.Range(this.min, this.max);
    }

    // Token: 0x06003074 RID: 12404 RVA: 0x0002CF8F File Offset: 0x0002B18F
    public float GetDamageWithDefenseInfo(DefenseInfo defense)
    {
        return this.GetDamage() * defense.GetMultiplier(this.type);
    }

    // Token: 0x04002E39 RID: 11833
    public RwbpType type;

    // Token: 0x04002E3A RID: 11834
    public SoundInfo soundInfo;

    // Token: 0x04002E3B RID: 11835
    public EffectInfo effectInfo;

    // Token: 0x04002E3C RID: 11836
    public List<EffectInfo> effectInfos = new List<EffectInfo>();

    // Token: 0x04002E3D RID: 11837
    public string specialDeadSceneName = string.Empty;

    // Token: 0x04002E3E RID: 11838
    public bool specialDeadSceneEnable;

    // Token: 0x04002E3F RID: 11839
    public string param = string.Empty;

    // Token: 0x04002E40 RID: 11840
    public float min;

    // Token: 0x04002E41 RID: 11841
    public float max;
}
