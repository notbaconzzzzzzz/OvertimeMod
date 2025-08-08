using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OvertimeBugDawnOrdeal : BugOrdeal
{
	public OvertimeBugDawnOrdeal()
	{
		this._ordealName = "bug_dawn";
		this.level = OrdealLevel.OVERTIME_DAWN;
		base.SetRiskLevel(RiskLevel.TETH);
		this.managers = new List<BugDawnManager>();
	}

	public override void OnOrdealStart()
	{
		base.OnOrdealStart();
		this.MakeBugs();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		List<BugDawnManager> list = new List<BugDawnManager>();
		foreach (BugDawnManager bugDawnManager in this.managers)
		{
			if (bugDawnManager.IsAvailable())
			{
				bugDawnManager.Run();
			}
			else
			{
				list.Add(bugDawnManager);
			}
		}
		foreach (BugDawnManager item in list)
		{
			this.managers.Remove(item);
		}
	}

	private PassageObjectModel GetPassage(BugDawnManager group = null, float minScore = 0f, float randScore = 100f)
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		List<PassageObjectModel> list = new List<PassageObjectModel>();
		for (int i = 0; i < openedAreaList.Length; i++)
		{
			foreach (PassageObjectModel passageObjectModel in openedAreaList[i].passageList)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.GetPassageType() == PassageType.HORIZONTAL)
					{
						if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
						{
							if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
							{
								if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBLONG)
								{
									if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.NONE)
									{
										if (passageObjectModel != this._curOrdealCreatureList[0].GetMovableNode().currentPassage)
										{
											list.Add(passageObjectModel);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		if (group == null)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}/*
		float[] medianDps = new float[]
		{
			1.2f * 0.8f,
			3f * 0.8f,
			4.5f,
			6f,
			8f * 1.2f,
			12f * 1.5f,
			16f * 1.5f
		};
		float[] amberRes = new float[]
		{
			1f,
			1.5f,
			0.8f,
			1f,
			2f,
			1.15f
		};*/
		float maxScore = minScore;
        float baseScore = 0f;
		PassageObjectModel bestPassage = null;
        foreach (PassageObjectModel passage in list)
        {
            /*
            float dps = 0f;
            float vuln = 0f;
            float juicy = 0f;
            foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
                if (!(unit is WorkerModel)) continue;
				float res = unit.defense.R;
				float num = 1000f;
				if (res > 1f)
				{
					num *= 2f;
				}
                if (unit is OfficerModel)
                {
                    num2 *= 0.1f;
                }
				res *= UnitModel.GetDmgMultiplierByEgoLevel(2, unit.GetDefenseLevel());
				num *= res / unit.hp;
				vuln += num * num;
				juicy += unit.hp / res;
				WeaponModel weapon = unit.Equipment.weapon;
				if (weapon != null)
				{
					float num2 = 0f;
					if (weapon.metaInfo.id == 2)
					{
						num2 = medianDps[0] * amberRes[1];
					}
					else
					{
						num2 = medianDps[weapon.GetUpgradeRisk] * amberRes[(int)weapon.GetDamage(unit).type];
					}
					if (unit is AgentModel)
					{
						num2 *= 0.8f + (unit as AgentModel).attackSpeed / 143f;
					}
					else
					{
						
					}
					dps += num2;
				}
			}
			vuln = Mathf.Sqrt(vuln);
			if (juicy > 700f)
			{
				juicy = 700f;
			}
			float score = vuln*2f - dps + juicy*0.04f + UnityEngine.Random.Range(-10f, 10f);
            */
            float score = 0f;
            float dillution = 0f;
            foreach (MovableObjectNode movableObjectNode in passage.GetEnteredTargets())
			{
				UnitModel unit = movableObjectNode.GetUnit();
                if (!(unit is AgentModel)) continue;
                float num = unit.maxHp - unit.hp;
				float res = unit.defense.R;
				if (res > 1f)
				{
					num *= 2f;
				}
                dillution += 1f;
				res *= UnitModel.GetDmgMultiplierByEgoLevel(2, unit.GetDefenseLevel());
                num *= res;
				score += num;
			}
            if (dillution > 0f)
            {
                score /= Mathf.Sqrt(dillution);
            }
            score += UnityEngine.Random.Range(0f, randScore);
			if (score > maxScore)
			{
				bestPassage = passage;
				maxScore = score;
			}
        }/*
		if (bestPassage != null)
		{
			Notice.instance.Send(NoticeName.AddSystemLog, new object[]
			{
				minScore.ToString() + ", " + randScore.ToString() + ", Score : "  + maxScore.ToString()
			});
		}*/
		return bestPassage;
	}

	private void MakeBugs()
	{
		Sefira[] openedAreaList = PlayerModel.instance.GetOpenedAreaList();
		Sefira[] array = openedAreaList;
		int i = 0;
		while (i < array.Length)
		{
			Sefira sefira = array[i];
			BugDawnManager bugDawnManager = new BugDawnManager(this);
			List<PassageObjectModel> list = new List<PassageObjectModel>();
			List<PassageObjectModel> list2 = new List<PassageObjectModel>(sefira.passageList);
            List<MapNode> list4 = new List<MapNode>();
			foreach (PassageObjectModel passageObjectModel in list2)
			{
				if (passageObjectModel.isActivate)
				{
					if (passageObjectModel.GetPassageType() == PassageType.HORIZONTAL)
					{
						if (!(SefiraMapLayer.instance.GetPassageObject(passageObjectModel) == null))
						{
							if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBSHORT)
							{
								if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.HUBLONG)
								{
									if (SefiraMapLayer.instance.GetPassageObject(passageObjectModel).type != SpaceObjectType.NONE)
									{
										list.Add(passageObjectModel);
									}
								}
							}
						}
					}
				}
			}
            if (list.Count <= 0)
            {
                continue;
            }
			do
			{
				PassageObjectModel passageObjectModel2 = list[UnityEngine.Random.Range(0, list.Count)];
				List<MapNode> list3 = passageObjectModel2.GetNodeList().ToList<MapNode>();
				list4.Clear();
				float num = 0f;
				float num2 = 0f;
				float scaleFactor = passageObjectModel2.scaleFactor;
				passageObjectModel2.GetVerticalRange(ref num, ref num2);
				foreach (MapNode mapNode in list3)
				{
					if (mapNode.GetPosition().x - num >= 3f * scaleFactor)
					{
						if (num2 - mapNode.GetPosition().x >= 3f * scaleFactor)
						{
							list4.Add(mapNode);
						}
					}
				}
				if (list4.Count > 0)
				{
					goto IL_1F8;
				}
				list.Remove(passageObjectModel2);
			}
			while (list.Count > 0);
			IL_256:
			this.managers.Add(bugDawnManager);
			i++;
			continue;
			IL_1F8:
			for (int j = 0; j < _numOfBugs; j++)
			{
				MapNode node = list4[UnityEngine.Random.Range(0, list4.Count)];
				BugOrdealCreature bugOrdealCreature = this.MakeOrdealCreature(OrdealLevel.OVERTIME_DAWN, node, null, new UnitDirection[0]);
				bugDawnManager.AddCreature(bugOrdealCreature as BugDawn);
			}
			goto IL_256;
		}
	}

	private const int _numOfBugs = 8;

	private const float _sideNodeRemoveRange = 3f;

	private List<BugDawnManager> managers = new List<BugDawnManager>();

	public class BugDawnManager
	{
		public BugDawnManager(OvertimeBugDawnOrdeal script)
		{
			this.script = script;
			this.isAvailable = true;
			this.teleportTimer.StartTimer(BugDawnManager.teleportTime);
            earlyTeleportTimer.StartTimer(UnityEngine.Random.Range(5f, 10f));
		}

		private static float teleportTime
		{
			get
			{
				return UnityEngine.Random.Range(40f, 60f);
			}
		}

		public void AddCreature(BugDawn bug)
		{
			this.bugs.Add(bug);
		}

		public void Run()
		{
			if (!this.CheckAvailable())
			{
				this.teleportTimer.StopTimer();
				earlyTeleportTimer.StopTimer();
				this.isAvailable = false;
				return;
			}
			if (this.teleportTimer.RunTimer())
			{
				this.ReadyToTeleport();
			}
			if (this.earlyTeleportTimer.RunTimer())
			{
                earlyTeleportTimer.StartTimer(UnityEngine.Random.Range(5f, 10f));
                PassageObjectModel passage = this.script.GetPassage(this, 425f, 400f);
                if (passage != null)
                {
                    foreach (BugDawn bugDawn in this.bugs)
                    {
                        bugDawn.ReadyToTeleport(passage);
                    }
                    teleportTimer.StartTimer(BugDawnManager.teleportTime);
                }
			}
		}

		private bool CheckAvailable()
		{
			List<BugDawn> list = new List<BugDawn>();
			if (this.bugs.Count <= 0)
			{
				return true;
			}
			foreach (BugDawn bugDawn in this.bugs)
			{
				if (bugDawn.model.hp <= 0f)
				{
					list.Add(bugDawn);
				}
			}
			foreach (BugDawn item in list)
			{
				this.bugs.Remove(item);
			}
			return this.bugs.Count > 0;
		}

		private void ReadyToTeleport()
		{
			PassageObjectModel passage = this.script.GetPassage(this);
			if (passage == null)
			{
				Debug.Log("nowhere to go");
				return;
			}
			foreach (BugDawn bugDawn in this.bugs)
			{
				bugDawn.ReadyToTeleport(passage);
			}
			this.teleportTimer.StartTimer(BugDawnManager.teleportTime);
		}

		public bool IsAvailable()
		{
			return this.isAvailable;
		}

		private OvertimeBugDawnOrdeal script;

		private bool isAvailable = true;

		private Timer teleportTimer = new Timer();

		private Timer earlyTeleportTimer = new Timer();

		private const float _teleportTimeMin = 40f;

		private const float _teleportTimeMax = 60f;

		private List<BugDawn> bugs = new List<BugDawn>();
	}
}
