using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vestige
{
    public class OvertimeOverloadManager
    {
        public static OvertimeOverloadManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OvertimeOverloadManager();
					_instance.overloadPool.Add(OverloadType.PAIN);
					_instance.overloadPool.Add(OverloadType.GRIEF);
					_instance.overloadPool.Add(OverloadType.RUIN);
					_instance.overloadPool.Add(OverloadType.OBLIVION);
                }
                return _instance;
            }
        }

        public static OvertimeOverloadManager _instance;

        public void OnStageStart()
        {
			overloadPool.Clear();
            overloadPool.Add(OverloadType.PAIN);
            overloadPool.Add(OverloadType.GRIEF);
            overloadPool.Add(OverloadType.RUIN);
            overloadPool.Add(OverloadType.OBLIVION);
            activateIndex = 0;
            failedOverloads = new int[4];
            storedOverloads.Clear();
        }

        public List<CreatureModel> ActivateOverload()
        {
            return ActivateOverload(GetNextOverloadNum(true), true);
        }

        public List<CreatureModel> ActivateOverload(int overloadCount, bool isNatural = false)
        {
            List<CreatureModel> list;
            List<CreatureModel> list4;
            List<CreatureModel> list2 = new List<CreatureModel>();
            List<long> list3 = new List<long>();
            if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
            {
                return new List<CreatureModel>();
            }
            GetValidOverloadTargets(out list, out list4);
            for (int j = 0; j < overloadCount; j++)
            {
                if (list.Count == 0)
                {
                    break;
                }
                int index = UnityEngine.Random.Range(0, list.Count);
                list2.Add(list[index]);
                list.RemoveAt(index);
            }
			if (list2.Count < overloadCount)
			{
				for (int j = list2.Count; j < overloadCount; j++)
				{
					if (list4.Count == 0)
					{
						break;
					}
					int index = UnityEngine.Random.Range(0, list4.Count);
					list2.Add(list4[index]);
					list4.RemoveAt(index);
				}
			}
            foreach (CreatureModel creatureModel2 in list2)
            {
				MakeOverload(GetRandomOverload(), creatureModel2, 60f, isNatural);
            }
			if (list2.Count < overloadCount)
			{
                overloadCount -= list2.Count;
                for (int i = 0; i < overloadCount; i++)
                {
                    storedOverloads.Add(new StoredOverload(GetRandomOverload()));
                }
			}
            return list2;
        }

        private void GetValidOverloadTargets(out List<CreatureModel> list, out List<CreatureModel> list4)
        {
            list = new List<CreatureModel>();
            list4 = new List<CreatureModel>();
            List<long> list3 = new List<long>();
            if (SefiraBossManager.Instance.IsKetherBoss(KetherBossType.E4))
            {
                return;
            }
            foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
            {
                if (!creatureModel.IsEscaped())
                {
                    if (creatureModel.GetMaxWorkCount() != 0)
                    {
                        if (creatureModel.metaInfo.creatureKitType != CreatureKitType.EQUIP || creatureModel.kitEquipOwner == null)
                        {
                            if (creatureModel.metadataId != 300101L && creatureModel.metadataId != 100024L && creatureModel.metadataId != 300110L)
                            {
                                if (!list3.Contains(creatureModel.metadataId))
                                {
                                    if (creatureModel.metaInfo.creatureWorkType == CreatureWorkType.KIT)
									{
                                        //list4.Add(creatureModel);
                                        continue;
									}
                                    if (creatureModel.isOverloaded)
                                    {
                                        if (creatureModel.overloadType != OverloadType.DEFAULT)
                                        {
                                            continue;
                                        }
                                        list4.Add(creatureModel);
                                        continue;
                                    }
                                    list.Add(creatureModel);
                                }
                            }
                        }
                    }
                }
            }
        }

		public OvertimeOverload MakeOverload(OverloadType type, CreatureModel target, float timer = 60f, bool isNatural = false)
		{
			OvertimeOverload overload;
			switch (type)
			{
				case OverloadType.PAIN:
					overload = new PainOverload(target);
					break;
				case OverloadType.GRIEF:
					overload = new GriefOverload(target);
					break;
				case OverloadType.RUIN:
					overload = new RuinOverload(target);
					break;
				case OverloadType.OBLIVION:
					overload = new OblivionOverload(target);
					break;
				default:
					return null;
			}
            if (target.Unit.room.binahOverloadUI != null && target.Unit.room.binahOverloadUI.isActivated)
            {
                target.Unit.room.binahOverloadUI.SetActive(false);
            }
			target.ActivateOverload(CreatureOverloadManager.instance.GetQliphothOverloadLevel(), timer, type, isNatural);
			overload.Init();
			return overload;
		}

        public OverloadType GetRandomOverload()
        {
			OverloadType overload;
			int num = UnityEngine.Random.Range(0, overloadPool.Count);
			overload = overloadPool[num];
			overloadPool.RemoveAt(num);
            if (overloadPool.Count <= 0)
			{
				overloadPool.Add(OverloadType.PAIN);
				overloadPool.Add(OverloadType.GRIEF);
				overloadPool.Add(OverloadType.RUIN);
				overloadPool.Add(OverloadType.OBLIVION);
			}
			return overload;
        }

        public int GetNextOverloadNum(bool increment)
        {
            int overloadCount = 1;
            if (activateIndex < activateAmount.Length)
            {
                overloadCount = activateAmount[activateIndex];
            }
            else
            {
                overloadCount = 16 + (activateIndex - activateAmount.Length) * 4;
            }
            if (increment)
            {
                activateIndex++;
            }
            return overloadCount;
        }

        public void OnFixedUpdate()
        {
            try
            {
                if (storedOverloads.Count <= 0) return;
                List<CreatureModel> list;
                List<CreatureModel> list4;
                GetValidOverloadTargets(out list, out list4);
                if (list.Count > 0 || list4.Count > 0)
                {
                    while (list.Count > 0 && storedOverloads.Count > 0)
                    {
                        MakeOverload(storedOverloads[0].type, list[0], storedOverloads[0].overloadTimer, storedOverloads[0].isNatural);
                        list.RemoveAt(0);
                        storedOverloads.RemoveAt(0);
                    }
                    while (list4.Count > 0 && storedOverloads.Count > 0)
                    {
                        MakeOverload(storedOverloads[0].type, list4[0], storedOverloads[0].overloadTimer, storedOverloads[0].isNatural);
                        list4.RemoveAt(0);
                        storedOverloads.RemoveAt(0);
                    }
                }
                for (int i = 0; i < storedOverloads.Count; i++)
                {
                    if (storedOverloads[i].CheckExplode(Time.fixedDeltaTime / (float)(i + 1)))
                    {
                        storedOverloads.RemoveAt(i);
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                Notice.instance.Send("AddSystemLog", new object[]
                {
                    ex.Message + " : " + ex.StackTrace
                });
            }
        }

        public void OnUpdate()
        {
            if (storedOverloads.Count <= 0) return;
            if (GameManager.currentGameManager.state != GameState.PAUSE || GameManager.currentGameManager.GetCurrentPauseCaller() != PAUSECALL.STOPGAME) return;
            for (int i = 0; i < storedOverloads.Count; i++)
            {
                if (storedOverloads[i].type == OverloadType.OBLIVION && storedOverloads[i].CheckExplode(Time.unscaledDeltaTime / (float)(i + 1)))
                {
                    storedOverloads.RemoveAt(i);
                    i--;
                }
            }
        }

        public List<OverloadType> overloadPool = new List<OverloadType>();

        public int activateIndex;

        public int[] failedOverloads = new int[4];

        public List<StoredOverload> storedOverloads = new List<StoredOverload>();

        public static int[] activateAmount = new int[]
        {
            1,
            1,
            2,
            3,
            5,
            8,
            12
        };

        public class StoredOverload
        {
            public StoredOverload(OverloadType _type)
            {
                type = _type;
                isNatural = true;
                overloadMaxTime = 60f;
                overloadTimer = overloadMaxTime;
            }

            public StoredOverload(OverloadType _type, bool nat)
            {
                type = _type;
                isNatural = nat;
                overloadMaxTime = 60f;
                overloadTimer = overloadMaxTime;
            }

            public bool CheckExplode(float delta)
            {
                overloadTimer -= delta;
                if (overloadTimer <= 0f)
                {
                    int ind = (int)type - (int)OverloadType.PAIN;
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
                    return true;
                }
                return false;
            }

            public OverloadType type;

            public float overloadTimer;

            public float overloadMaxTime;

            public bool isNatural;
        }
    }
}