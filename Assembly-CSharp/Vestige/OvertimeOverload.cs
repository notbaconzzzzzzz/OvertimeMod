using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vestige
{
    public class OvertimeOverload : IObserver
    {
        public OvertimeOverload(CreatureModel _creature)
		{
            creature = _creature;
			Notice.instance.Observe(NoticeName.FixedUpdate, this);
			Notice.instance.Observe(NoticeName.Update, this);
			Notice.instance.Observe(NoticeName.OnIsolateOverloaded, this);
			Notice.instance.Observe(NoticeName.OnIsolateOverloadCanceled, this);
			Notice.instance.Observe(NoticeName.OnWorkStart, this);
			Notice.instance.Observe(NoticeName.OnReleaseWork, this);
			Notice.instance.Observe(NoticeName.ReportAgentSuccess, this);
			Notice.instance.Observe(NoticeName.OnProcessWorkTick, this);
			Notice.instance.Observe(NoticeName.OnEscape, this);
		}

        public virtual void DestroyOverload()
		{
			Notice.instance.Remove(NoticeName.FixedUpdate, this);
			Notice.instance.Remove(NoticeName.Update, this);
			Notice.instance.Remove(NoticeName.OnIsolateOverloaded, this);
			Notice.instance.Remove(NoticeName.OnIsolateOverloadCanceled, this);
			Notice.instance.Remove(NoticeName.OnWorkStart, this);
			Notice.instance.Remove(NoticeName.OnReleaseWork, this);
			Notice.instance.Remove(NoticeName.ReportAgentSuccess, this);
			Notice.instance.Remove(NoticeName.OnProcessWorkTick, this);
			Notice.instance.Remove(NoticeName.OnEscape, this);
        }

        public void OnNotice(string notice, params object[] param)
		{
            CreatureModel model;
            OverloadType type;
            if (notice == NoticeName.FixedUpdate)
            {
                FixedUpdate();
            }
            else if (notice == NoticeName.Update)
            {
                Update();
            }
			else if (notice == NoticeName.OnIsolateOverloaded)
            {
                model = param[0] as CreatureModel;
                type = (OverloadType)param[1];
                if (type == Type && model == creature)
                {
                    OnFailOverload();
                }
            }
            else if (notice == NoticeName.OnIsolateOverloadCanceled)
            {
                model = param[0] as CreatureModel;
                type = (OverloadType)param[1];
                if (type == Type && model == creature)
                {
                    OnClearOverload();
                }
            }
            else if (notice == NoticeName.OnWorkStart)
            {
                model = param[0] as CreatureModel;
                if (model == creature)
                {
					_prevSuccessCount = 0;
					_prevFailCount = 0;
                    OnWorkStart();
                }
            }
            else if (notice == NoticeName.OnReleaseWork)
            {
                model = param[0] as CreatureModel;
                if (model == creature)
                {
                    OnReleaseWork();
                }
            }
            else if (notice == NoticeName.ReportAgentSuccess)
            {
                model = param[1] as CreatureModel;
                if (model == creature)
                {
                    OnWorkAllocated(param[0] as AgentModel);
                }
            }
            else if (notice == NoticeName.OnProcessWorkTick)
            {
                model = param[0] as CreatureModel;
                if (model == creature)
                {
					bool isSuccess = true;
					if (_prevFailCount < creature.currentSkill.failCount)
					{
						isSuccess = false;
					}
					_prevSuccessCount = creature.currentSkill.successCount;
					_prevFailCount = creature.currentSkill.failCount;
                    OnWorkTick(isSuccess);
                }
            }
            else if (notice == NoticeName.OnEscape)
            {
                model = param[0] as CreatureModel;
                if (model == creature)
                {
                    if (!creature.isOverloaded && !failed)
                    {
                        OnClearOverload();
                    }
                }
            }
		}

		private int _prevSuccessCount;

		private int _prevFailCount;

        public virtual OverloadType Type
        {
            get
            {
                return OverloadType.PAIN;
            }
        }

        public virtual void Init()
		{
			if (creature.currentSkill == null)
			{
				InitAfterWork();
			}
        }

        public virtual void InitAfterWork()
		{
            isInitialized = true;
        }

        public virtual void FixedUpdate()
		{
			if (failed)
            {
                damageDelay -= Time.fixedDeltaTime;
                if (damageDelay <= 0f)
                {
                    DamageAll();
                }
            }
        }

        public virtual void Update()
		{
            
        }

        public virtual void OnFailOverload()
		{
            if (failed) return;
            failed = true;
            damageDelay = UnityEngine.Random.Range(0.5f, 1.5f);
        }

        public virtual void DamageAll()
        {
            int ind = (int)Type - (int)OverloadType.PAIN;
			int[] failedOverloads = OvertimeOverloadManager.instance.failedOverloads;
			int total = 0;
			for (int i = 0; i < 4; i++)
			{
				if (i == ind)
				{
					total += failedOverloads[i] * 3;
				}
				else
				{
					total += failedOverloads[i];
				}
			}
			float dmg = 10;
			dmg += (float)total / 2f;
			DamageInfo damage = new DamageInfo((RwbpType)(ind + 1), dmg);
			foreach (WorkerModel worker in WorkerManager.instance.GetWorkerList())
            {
                if (worker.IsDead() || !worker.IsAttackTargetable()) continue;
				try
				{
					worker.TakeDamage(null, damage);
				}
				catch (Exception ex)
				{
					Notice.instance.Send(NoticeName.AddSystemLog, new object[]
					{
						worker.name + ":" + ex.Message + " : " + ex.Source + " : " + ex.StackTrace
					});
				}
            }
            OvertimeOverloadManager.instance.failedOverloads[ind]++;
			DestroyOverload();
        }

        public virtual void OnClearOverload()
		{
            if (!workStarted)
            {
                DestroyOverload();
            }
        }

        public virtual void OnWorkStart()
		{
            workStarted = true;
        }

        public virtual void OnReleaseWork()
		{
            if (workStarted)
            {
                DestroyOverload();
            }
            else
            {
                InitAfterWork();
            }
        }

        public virtual void OnWorkAllocated(AgentModel agent)
		{
            
        }

        public virtual void OnWorkTick(bool isSuccess)
		{
            
        }

        public CreatureModel creature;

        public bool workStarted;

        public bool isInitialized;

        public bool failed;

        public float damageDelay;
    }
}