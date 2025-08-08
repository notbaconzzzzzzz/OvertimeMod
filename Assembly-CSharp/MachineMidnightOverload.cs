using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineMidnightOverload : IObserver
{
    public MachineMidnightOverload(MachineMidnight _midnight)
    {
        midnight = _midnight;
        Notice.instance.Observe(NoticeName.OnIsolateOverloaded, this);
        Notice.instance.Observe(NoticeName.OnIsolateOverloadCanceled, this);
        Notice.instance.Observe(NoticeName.Update, this);
    }

    public void OnDestroy()
    {
        Notice.instance.Remove(NoticeName.OnIsolateOverloaded, this);
        Notice.instance.Remove(NoticeName.OnIsolateOverloadCanceled, this);
        Notice.instance.Remove(NoticeName.Update, this);
    }

    public void CastOverload()
    {
        int overloadTargetCount = 3;
        overloadedCreatures.AddRange(CreatureOverloadManager.instance.ActivateOverload(overloadTargetCount, OVERLOAD_TYPE, DURATION, false, true, true));
        if (overloadedCreatures.Count <= 0)
        {
            OnSuccess();
        }
    }

    public int GetCreatureCount()
    {
        return overloadedCreatures.Count;
    }

    public void OnSuccess()
    {
        midnight.OverloadSuccess();
    }

    public void OnFail()
    {
        midnight.OverloadFail();
    }

    public void OnReducedCreature(CreatureModel creature)
    {
        if (overloadedCreatures.Contains(creature))
        {
            overloadedCreatures.Remove(creature);
            if (overloadedCreatures.Count == 0)
            {
                OnSuccess();
            }
        }
    }

    public void OnNotice(string notice, params object[] param)
    {
        if (notice == NoticeName.OnIsolateOverloaded)
        {
            CreatureModel item = param[0] as CreatureModel;
            OverloadType overloadType = (OverloadType)param[1];
            if (overloadType == OverloadType.HELIX && overloadedCreatures.Contains(item))
            {
                OnFail();
                return;
            }
        }
        else if (notice == NoticeName.OnIsolateOverloadCanceled)
        {
            CreatureModel creatureModel = param[0] as CreatureModel;
            OverloadType overloadType2 = (OverloadType)param[1];
            if (overloadType2 == OverloadType.HELIX && overloadedCreatures.Contains(creatureModel))
            {
                OnReducedCreature(creatureModel);
            }
        }
        else if (notice == NoticeName.Update)
        {
            Update();
        }
    }

    public void Update()
    {
        List<CreatureModel> removeList = new List<CreatureModel>();
        foreach (CreatureModel creatureModel in overloadedCreatures)
        {
            if (!creatureModel.isOverloaded)
            {
                removeList.Add(creatureModel);
            }
        }
        foreach (CreatureModel creatureModel in removeList)
        {
            overloadedCreatures.Remove(creatureModel);
            overloadedCreatures.AddRange(CreatureOverloadManager.instance.ActivateOverload(1, OVERLOAD_TYPE, DURATION, false, true, true));
        }
    }

    public List<CreatureModel> overloadedCreatures = new List<CreatureModel>();

    public MachineMidnight midnight;

	public const OverloadType OVERLOAD_TYPE = OverloadType.HELIX;

	public const float DURATION = 300f;
}
