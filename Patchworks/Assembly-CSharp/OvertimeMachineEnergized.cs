using System;
using UnityEngine;

public class OvertimeMachineEnergized : UnitBuf
{
	public OvertimeMachineEnergized(int _ordealLevel)
	{
		type = UnitBufType.OVERTIME_MACHINE_ENERGIZED;
		duplicateType = BufDuplicateType.ONLY_ONE;
        ordealLevel = _ordealLevel;
	}

    public override void FixedUpdate()
    {
        int max = 0;
        for (int i = ordealLevel; i < 4; i++)
		{
            if (remainTimes[i] <= 0f) continue;
			remainTimes[i] -= Time.deltaTime;
            if (remainTimes[i] <= 0f)
            {
                remainTimes[i] = 0f;
                if (Level - 1 == i)
                {
                    Level = max;
                }
            }
            else
            {
                max = i + 1;
            }
		}
	}

    public void SetEnergized(int lvl, float time)
    {
        if (lvl <= ordealLevel || lvl > 4) return;
        if (remainTimes[lvl - 1] > time) return;
        remainTimes[lvl - 1] = time;
        if (Level < lvl)
        {
            Level = lvl;
        }
    }

	public int Level
	{
		get
		{
			return _level;
		}
        set
        {
            _level = value;
        }
	}

    public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
    {
		if (ordealLevel == 0 && Level >= 1)
		{
			return 0.8f - Level * 0.1f;
		}
        else if (ordealLevel == 2 && Level >= 3)
        {
            return 1.2f - Level * 0.15f;
        }
        return 1f;
    }

    public override float MovementScale()
    {
		if (ordealLevel <= 1 && Level >= 3)
		{
			return 0.5f + Level * 0.5f;
		}
        return 1f;
    }

    private int _level = 0;

    private int ordealLevel;

	private float[] remainTimes = new float[4];
}
