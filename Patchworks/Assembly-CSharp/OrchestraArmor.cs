/*
Pretty much everything //
*/
using System;

public class OrchestraArmor : EquipmentScriptBase
{
	public OrchestraArmor()
	{
	}

	public override DefenseInfo GetDefense(UnitModel actor)
	{
		if (actor != null && _durability > 0f)
		{
			DefenseInfo defense = base.GetDefense(actor);
			defense.W = -1f;
			return defense;
		}
		return base.GetDefense(actor);
	}

    public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
    {
		UnitModel owner = model.owner;
        if (owner.mental <= (float)owner.maxMental * 0.5f && !_cooldown.started && owner.HasEquipment(400019))
		{
			_durability = 50f;
			_cooldown.StartTimer(60f);
		}
		return true;
    }

    public override bool OnTakeDamage_After(float value, RwbpType type)
    {
        if (_durability > 0f && type == RwbpType.W && value < 0f)
        {
            _durability += value;
        }
        return true;
    }

    public override void OnFixedUpdate()
    {
        if (_cooldown.started)
		{
			if (_cooldown.RunTimer())
			{

			}
		}
    }

    public override void OnStageRelease()
    {
        _durability = 0f;
		_cooldown.StopTimer();
    }

    private float _durability = 0f;

	private Timer _cooldown = new Timer();
}
