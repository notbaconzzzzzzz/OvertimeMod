using System;
using UnityEngine;

public class OvertimeNetzachBossBuf : UnitBuf
{
	public OvertimeNetzachBossBuf()
	{
		type = UnitBufType.OVERTIME_NETZACHBOSS;
		duplicateType = BufDuplicateType.ONLY_ONE;
	}

	public override void FixedUpdate()
	{
		IsRegenerator = false;
        IsBullet = false;
	}

    public override void OwnerHeal(bool isMental, ref float amount)
    {
        if (IsRegenerator)
        {
            amount *= _regeneratorFactor;
        }
        else if (IsBullet)
        {
            amount *= _bulletFactor;
        }
        else
        {
            amount *= _normalFactor;
        }
        float points;
        float maxPoints;
        if (!isMental)
        {
            points = model.hp;
			maxPoints = (float)model.maxHp;
        }
        else
        {
            points = model.mental;
			maxPoints = (float)model.maxMental;
        }
        if (amount < 0f)
		{
			if (points <= maxPoints * 0.05f)
			{
				amount /= 10;
			}
			else if (points + amount <= maxPoints * 0.05f)
			{
				amount += points - maxPoints * 0.05f;
				amount /= 10;
				amount -= points - maxPoints * 0.05f;
			}
		}
    }

    public void UpdateValues(float normal, float regenerator, float bullet)
    {
        _normalFactor = normal;
        _regeneratorFactor = regenerator;
        _bulletFactor = bullet;
    }

    public override void OnUnitDie()
    {
        GlobalBullet.GlobalBulletManager.instance.UpdateMaxBullet();
    }

    private float _normalFactor = -0.1f;

    private float _regeneratorFactor = -0.2f;

    private float _bulletFactor = 1f;

	public static bool IsRegenerator = false;

    public static bool IsBullet = false;
}
