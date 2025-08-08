using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vestige
{
    public class PainOverload : OvertimeOverload
    {
        public PainOverload(CreatureModel _creature) : base(_creature)
		{
            
		}

        public override OverloadType Type
        {
            get
            {
                return OverloadType.PAIN;
            }
        }

        public override void InitAfterWork()
		{
            base.InitAfterWork();
            creature.ProbReductionValue = 10;
            damageTicks = 0;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
			if (damageTimer.RunTimer())
			{
				List<WorkerModel> list = new List<WorkerModel>();
				Vector2 a = creature.GetCurrentViewPosition();
				foreach (WorkerModel worker in WorkerManager.instance.GetWorkerList())
				{
					if (worker.IsDead() || !worker.IsAttackTargetable()) continue;
					if (creature.currentSkill != null && worker == creature.currentSkill.agent) continue;
					Vector2 b = worker.GetCurrentViewPosition();
					Vector2 c = (a - b);
					float magnitude = c.magnitude;
					if (magnitude < range)
					{
						list.Add(worker);
					}
				}
				foreach (WorkerModel worker in list)
				{
					worker.TakeDamage(creature, dmg);
				}
                MakePainEffect(range, dmg.type);
			}
            if (workStarted || !isInitialized) return;
			if (!creature.isOverloaded || creature.overloadType != OverloadType.PAIN) return;
            int _damageTicks = (int)(creature.overloadTimer / creature.currentOverloadMaxTime * 10f);
			if (damageTicks >= 9 || _damageTicks <= damageTicks) return;
			dmg = GetWorkDamage();
            range = 25f;
            switch (damageTicks / 3)
            {
                case 0:
                    dmg *= 1f / 3f;
                    dmg.min += 3;
                    dmg.max += 3;
                    range = 25f;
                    break;
                case 1:
                    dmg *= 0.5f;
                    dmg.min += 4;
                    dmg.max += 4;
                    range = 33f;
                    break;
                case 2:
                    dmg.min += 6;
                    dmg.max += 6;
                    range = 45f;
                    break;
            }
            creature.Unit.room.OnDamageInvoked(dmg);
			damageTimer.StartTimer(0.25f);
			damageTicks++;
        }

        public override void OnWorkTick(bool isSuccess)
        {
            base.OnWorkTick(isSuccess);
            if (!workStarted) return;
            if (isSuccess) return;
			dmg = GetWorkDamage();
            dmg *= 1f / 6f;
            dmg.min += 1;
            dmg.max += 1;
            float prog = creature.currentSkill.workCount;
			if (prog <= creature.currentSkill.maxCubeCount / 3)
			{
				range = 25f;
			}
			else if (prog <= creature.currentSkill.maxCubeCount * 2 / 3)
			{
				range = 33f;
			}
			else
			{
				range = 45f;
			}
			damageTimer.StartTimer(0.25f);
        }

        public DamageInfo GetWorkDamage()
		{
			if (creature.metaInfo.creatureWorkType == CreatureWorkType.NORMAL)
			{
				return creature.metaInfo.workDamage.Copy();
			}
			return new DamageInfo(RwbpType.P, 2, 6);
		}

        public void MakePainEffect(float _rad, RwbpType _type)
        {
            string effectType;
            float sep;
            float scale;
            switch (_type)
            {
                case RwbpType.R:
                    effectType ="DamageInfo/ViscusSnakeWeaponEffect";
                    sep = 2.2f;
                    scale = 1f;
                    break;
                case RwbpType.W:
                    effectType ="DamageInfo/DespairWeaponAttackEffect";
                    sep = 2.4f;
                    scale = 1.1f;
                    break;
                case RwbpType.B:
                    effectType ="DamageInfo/YinWeaponEffect";
                    sep = 3f;
                    scale = 0.9f;
                    break;
                case RwbpType.P:
                    effectType ="DamageInfo/ButterflyWeaponEffect";
                    sep = 1.8f;
                    scale = 1.2f;
                    break;
                default:
                    return;
            }
			Vector3 center = creature.GetCurrentViewPosition();
			int amt = (int)Mathf.Pow(_rad / sep, 2);
			float ang = UnityEngine.Random.Range(0f, 360f);
			for (int i = (int)Mathf.Pow((_rad - 10f) / sep, 2); i < amt; i++)
			{
				float dist = Mathf.Sqrt((float)i) * sep + UnityEngine.Random.Range(-1f, 1f);
				float dir = ang + UnityEngine.Random.Range(-3f, 3f);
				Vector3 a = new Vector3(dist * Mathf.Sin(dir * Mathf.Deg2Rad), dist * Mathf.Cos(dir * Mathf.Deg2Rad), 0);
				EffectInvoker effect = EffectInvoker.Invoker(effectType, (center + a), 3f, false);
                effect.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
				effect.transform.localScale *= scale;
				ang += 360f * 0.61803399f;
			}
        }

        private int damageTicks;

        private Timer damageTimer = new Timer();

        private DamageInfo dmg;

        private float range = 25f;
    }
}