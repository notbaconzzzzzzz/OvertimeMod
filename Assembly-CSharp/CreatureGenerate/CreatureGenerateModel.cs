using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureGenerate
{
	// Token: 0x020007F0 RID: 2032
	public class CreatureGenerateModel : CreatureGenerateData
	{
		// Token: 0x06003EC4 RID: 16068 RVA: 0x00036A10 File Offset: 0x00034C10
		public void Print()
		{
			Debug.Log(this.day + "day Info ");
			this.door1.Print();
			this.door2.Print();
			this.door3.Print();
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x0018776C File Offset: 0x0018596C
		public CreatureGenerateData.ActionData ParseActionNode(string nodeText)
		{
			CreatureGenerateData.ActionData result = new CreatureGenerateData.ActionData();
			if (base.ParseAction(ref nodeText, out result))
			{
				Debug.Log("Action Parsed");
			}
			return result;
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0018779C File Offset: 0x0018599C
		public override void RemoveAction(params object[] ids)
		{
			foreach (object obj in ids)
			{
				if (obj is long)
				{
					CreatureGenerateInfoManager.Instance.RemoveAction((long)obj);
				}
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x001877E0 File Offset: 0x001859E0
		public override void OnlyAction(params object[] ids)
		{
			foreach (object obj in ids)
			{
				if (obj is long)
				{
					Debug.Log((long)obj);
					this.creature.Add((long)obj);
					this.stop = true;
				}
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0018783C File Offset: 0x00185A3C
		public void SetCreature()
		{
			if (this.commonAction != null)
			{
				this.commonAction.Exectue();
			}
			if (!this.stop)
			{
				if (this.door1.commonAction != null)
				{
					this.door1.commonAction.Exectue();
				}
				if (this.door2.commonAction != null)
				{
					this.door2.commonAction.Exectue();
				}
				if (this.door3.commonAction != null)
				{
					this.door3.commonAction.Exectue();
				}
				this.door1.SetCreature();
				this.door2.SetCreature();
				this.door3.SetCreature();
				if (this.door1.Creature != -1L)
				{
					this.creature.Add(this.door1.Creature);
				}
				if (this.door2.Creature != -1L)
				{
					this.creature.Add(this.door2.Creature);
				}
				if (this.door3.Creature != -1L)
				{
					this.creature.Add(this.door3.Creature);
				}
				return;
			}
		}

		// Token: 0x0400395C RID: 14684
		public int day = -1;

		// Token: 0x0400395D RID: 14685
		public CreatureGenerateDoor door1 = new CreatureGenerateDoor();

		// Token: 0x0400395E RID: 14686
		public CreatureGenerateDoor door2 = new CreatureGenerateDoor();

		// Token: 0x0400395F RID: 14687
		public CreatureGenerateDoor door3 = new CreatureGenerateDoor();

		// Token: 0x04003960 RID: 14688
		public List<long> creature = new List<long>();

		// Token: 0x04003961 RID: 14689
		public bool stop;
	}
}
