using System;
using System.Collections.Generic;
using UnityEngine; // 

// Token: 0x02000657 RID: 1623
public class ButterflyWeapon : EquipmentScriptBase, IObserver
{
	// Token: 0x06003647 RID: 13895 RVA: 0x00166E14 File Offset: 0x00165014
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		list.Add(base.model.metaInfo.damageInfos[0].Copy());
		list.Add(base.model.metaInfo.damageInfos[1].Copy());
		animationName = base.model.metaInfo.animationNames[0];
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

    //> <Mod>
    public override void OnStageStart()
    {
        clerkPoints = 0;
        agentPoints = 0;
        critBoostTime = 0f;
		Notice.instance.Observe(NoticeName.OnOfficerDie, this);
		Notice.instance.Observe(NoticeName.OnAgentDead, this);
        base.OnStageStart();
    }

    public override void OnStageRelease()
    {
		Notice.instance.Remove(NoticeName.OnOfficerDie, this);
		Notice.instance.Remove(NoticeName.OnAgentDead, this);
        base.OnStageRelease();
    }

    public override void OnRelease()
    {
		Notice.instance.Remove(NoticeName.OnOfficerDie, this);
		Notice.instance.Remove(NoticeName.OnAgentDead, this);
        base.OnRelease();
    }

    public override bool OnGiveDamage(UnitModel actor, UnitModel target, ref DamageInfo dmg)
    {
        isCrit = false;
        float critChance = 1f;
        if (clerkPoints >= 30) critChance += 10f;
        else if (clerkPoints >= 10) critChance += 5f + 0.25f * ((float)clerkPoints - 10f);
        else critChance += 0.5f * (float)clerkPoints;
        if (agentPoints >= 24) critChance += 24f;
        else critChance += 1f * (float)agentPoints;
        if (UnityEngine.Random.value * 100f < critChance)
        {
            isCrit = true;
            float critBoost = 3f;
            if (critBoostTime > 0f) critBoost = 5f;
            dmg.min *= critBoost;
            dmg.max *= critBoost;
        }
        return base.OnGiveDamage(actor, target, ref dmg);
    }

    public override void OnGiveDamageAfter(UnitModel actor, UnitModel target, DamageInfo dmg)
    {
        if (!isCrit) return;
        isCrit = false;
        bool boostedCrit = critBoostTime > 0f;
		AgentUnit unit = null;
		if (model.owner is AgentModel)
		{
			unit = (model.owner as AgentModel).GetUnit();
		}
		else
		{
			base.OnGiveDamageAfter(actor, target, dmg);
			return;
		}
        GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/Butterfly/ButterflyAttackEffect");
        gameObject.transform.position = target.GetCurrentViewPosition() + new Vector3(0f, 1.5f * unit.gameObject.transform.localScale.y, 0f);
        gameObject.transform.localScale = new Vector3(unit.gameObject.transform.localScale.y * (boostedCrit ? 1f : 0.8f), unit.gameObject.transform.localScale.y * (boostedCrit ? 1f : 0.8f), unit.gameObject.transform.localScale.z);
        SoundEffectPlayer.PlayOnce("creature/Butterfly/Butterfly_Attack", target.GetCurrentViewPosition(), boostedCrit ? 0.6f : 0.5f);
        base.OnGiveDamageAfter(actor, target, dmg);
    }

	public void OnNotice(string notice, params object[] param)
	{
		if (notice == NoticeName.OnOfficerDie)
		{
			OfficerModel officerModel = param[0] as OfficerModel;
			if (officerModel == null) return;
			PassageObjectModel passage = model.owner.GetMovableNode().currentPassage;
			if (passage == null || officerModel.DeadType == DeadType.EXECUTION || officerModel.GetMovableNode().currentPassage != passage) return;
            if (!HostileExists(passage)) return;
			clerkPoints += 1;
		}
		else if (notice == NoticeName.OnAgentDead)
		{
			AgentModel agentModel = param[0] as AgentModel;
			if (agentModel == null) return;
			PassageObjectModel passage = model.owner.GetMovableNode().currentPassage;
			if (passage == null || agentModel.DeadType == DeadType.EXECUTION || agentModel.GetMovableNode().currentPassage != passage) return;
            if (!HostileExists(passage)) return;
			agentPoints += agentModel.level;
            critBoostTime += (float)agentModel.level;
            if (critBoostTime < 5f + 5f * (float)agentModel.level)
            {
                critBoostTime = 5f + 5f * (float)agentModel.level;
            }
		}
	}

	private bool HostileExists(PassageObjectModel passage)
	{
        foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
        {
            UnitModel unit = movableObjectNode.GetUnit();
            if (unit.IsAttackTargetable() && model.owner.IsHostile(unit))
            {
                return true;
            }
        }
        return false;
	}

    private bool isCrit;

    private int clerkPoints;

    private int agentPoints;

    private float critBoostTime;
}