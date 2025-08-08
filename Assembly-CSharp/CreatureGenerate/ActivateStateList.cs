using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureGenerate
{
	// Token: 0x020007E7 RID: 2023
	public class ActivateStateList
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06003E7B RID: 15995 RVA: 0x000367E7 File Offset: 0x000349E7
		public int Max
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06003E7C RID: 15996 RVA: 0x000367F4 File Offset: 0x000349F4
		public int UsableCount
		{
			get
			{
				return this.Usable.Count;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06003E7D RID: 15997 RVA: 0x00036801 File Offset: 0x00034A01
		public bool LevelEnabled
		{
			get
			{
				this.CheckUsableState();
				return this.UsableCount > 0;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x00036812 File Offset: 0x00034A12
		private int CurrentDay
		{
			get
			{
				return CreatureGenerateInfoManager.Instance.GenDay;
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x0003681E File Offset: 0x00034A1E
		public void Add(ActivateStateModel model)
		{
			this.list.Add(model);
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x0003682C File Offset: 0x00034A2C
		public void CheckUsableState()
		{
			this.Usable = this.GetUsableCreatures();
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x001863F0 File Offset: 0x001845F0
		public void DayUpdate()
		{
			List<long> list = new List<long>();
			List<long> list2 = PlayerModel.instance.CopyWaitingCreatures();
			foreach (CreatureModel creatureModel in CreatureManager.instance.GetCreatureList())
			{
				list.Add(creatureModel.metadataId);
			}
			foreach (ActivateStateModel activateStateModel in this.list)
			{
				activateStateModel.isRemoved = false;
				if (!activateStateModel.isUsed)
				{
					if (list.Contains(activateStateModel.id))
					{
						activateStateModel.isUsed = true;
					}
					else if (list2.Contains(activateStateModel.id))
					{
						activateStateModel.isUsed = true;
					}
				}
			}
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x001864E0 File Offset: 0x001846E0
		public List<ActivateStateModel> GetUsableCreatures()
		{
			List<ActivateStateModel> list = new List<ActivateStateModel>();
			int currentDay = this.CurrentDay;
			bool genKit = CreatureGenerateInfoManager.Instance.GenKit;
			List<long> list2 = PlayerModel.instance.CopyWaitingCreatures();
			foreach (ActivateStateModel activateStateModel in this.list)
			{
				if (list2.Contains(activateStateModel.id))
				{
					activateStateModel.isUsed = true;
				}
				if (!activateStateModel.isUsed && !activateStateModel.isRemoved)
				{
					if (genKit && activateStateModel.isKit)
					{
						if (activateStateModel.id != 300109L || CreatureSelectUI.CheckCreatureExisting(100104L))
						{
							list.Add(activateStateModel);
						}
					}
					else if (!genKit && !activateStateModel.isKit)
					{
						if (activateStateModel.id != 100014L || !CreatureSelectUI.CheckCreatureExisting(100015L))
						{
							if (activateStateModel.id != 100015L)
							{
								list.Add(activateStateModel);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x0003683A File Offset: 0x00034A3A
		public ActivateStateModel GetRandomCreature()
		{
			if (!this.LevelEnabled)
			{
				return null;
			}
			return this.Usable[UnityEngine.Random.Range(0, this.UsableCount)];
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x0018662C File Offset: 0x0018482C
		public void RemoveAction(long id)
		{
			foreach (ActivateStateModel activateStateModel in this.list)
			{
				if (activateStateModel.id == id)
				{
					activateStateModel.isRemoved = true;
					break;
				}
			}
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x0018669C File Offset: 0x0018489C
		public void OnUsed(long id)
		{
			foreach (ActivateStateModel activateStateModel in this.list)
			{
				if (activateStateModel.id == id)
				{
					activateStateModel.isUsed = true;
					break;
				}
			}
		}

		// Token: 0x04003935 RID: 14645
		public List<ActivateStateModel> list = new List<ActivateStateModel>();

		// Token: 0x04003936 RID: 14646
		public RiskLevel riskLevel;

		// Token: 0x04003937 RID: 14647
		public List<ActivateStateModel> Usable;
	}
}
