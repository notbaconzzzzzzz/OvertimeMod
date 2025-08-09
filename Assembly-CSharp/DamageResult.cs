using System;
using System.Collections.Generic;

public class DamageResult
{
    public void ResetValues(DamageInfo damageInfo)
    {
        originDamage = 0f;
        byResist = 0f;
        resultDamage = 0f;
        resultNumber = 0;
        scaling = 1f;
        hpDamage = 0f;
        spDamage = 0f;
        switch (damageInfo.type)
        {
            case RwbpType.R:
                type = RwbptiohgclmyednType.R;
                break;
            case RwbpType.W:
                type = RwbptiohgclmyednType.W;
                break;
            case RwbpType.B:
                type = RwbptiohgclmyednType.B;
                break;
            case RwbpType.P:
                type = RwbptiohgclmyednType.P;
                break;
            default:
                type = RwbptiohgclmyednType.NONE;
                break;
        }
    }

    public string Tostring()
    {
        return originDamage.ToString() + "," + byResist.ToString() + "," + beforeShield.ToString() + "," + resultDamage.ToString() + "," + resultNumber.ToString() + "," + scaling.ToString() + "," + hpDamage.ToString() + "," + spDamage.ToString() + "," + type.ToString() + "," + activated.ToString();
    }

    public float originDamage = 0f;

    public float byResist = 0f;

    public float beforeShield = 0f;

    public float resultDamage = 0f;

    public int resultNumber = 0;

    public float scaling = 1f;

    public float hpDamage = 0f;

    public float spDamage = 0f;

    public RwbptiohgclmyednType type = RwbptiohgclmyednType.NONE;

    public bool activated = false;
}
