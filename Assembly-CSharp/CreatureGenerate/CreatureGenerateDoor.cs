/*
public void SetCreature_Mod() // 
*/
using System;
using LobotomyBaseMod; // 
using UnityEngine;

namespace CreatureGenerate
{
	// Token: 0x020007E9 RID: 2025
	public class CreatureGenerateDoor : CreatureGenerateData
	{
		// Token: 0x06003E78 RID: 15992 RVA: 0x00184FD0 File Offset: 0x001831D0
		public CreatureGenerateDoor()
		{
			for (int i = 0; i < 5; i++)
			{
				this.prob[i] = CreatureGenerateDoor.zeroAry[i];
				this.probState[i] = CreatureGenerateDoor.initialState[i];
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06003E79 RID: 15993 RVA: 0x00185034 File Offset: 0x00183234
		public float TotalProb
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < 5; i++)
				{
					if (this.probState[i])
					{
						num += this.prob[i];
					}
				}
				return num;
			}
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00185074 File Offset: 0x00183274
		public static CreatureGenerateDoor Parse(string parsed)
		{
			CreatureGenerateDoor creatureGenerateDoor = new CreatureGenerateDoor();
			CreatureGenerateData.ActionData actionData = null;
			try
			{
				if (!creatureGenerateDoor.ParseAction(ref parsed, out actionData))
				{
				}
				string[] array = parsed.Split(CreatureGenerateData.split);
				for (int i = 0; i < array.Length; i++)
				{
					if (i > 5)
					{
						break;
					}
					float num = 0f;
					if (float.TryParse(array[i], out num))
					{
						creatureGenerateDoor.prob[i] = num;
					}
				}
				for (int j = 0; j < 5; j++)
				{
					creatureGenerateDoor.probState[j] = (creatureGenerateDoor.prob[j] != 0f);
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return creatureGenerateDoor;
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x0018513C File Offset: 0x0018333C
		public void Print()
		{
			string text = string.Empty;
			for (int i = 0; i < 5; i++)
			{
				text = text + this.prob[i] + " ";
			}
			Debug.Log(text);
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x000043A5 File Offset: 0x000025A5
		public override void RemoveAction(params object[] ids)
		{
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x000043A5 File Offset: 0x000025A5
		public override void OnlyAction(params object[] ids)
		{
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00185180 File Offset: 0x00183380
		public void CheckProb()
		{
			for (int i = 0; i < 5; i++)
			{
				ActivateStateList list = this.GetList(i);
				if (list != null)
				{
					this.probState[i] = list.LevelEnabled;
				}
				else
				{
					Debug.LogError("List is Null " + i);
				}
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x001851D8 File Offset: 0x001833D8
		public void SetCreature()
		{ // <Patch>
			SetCreature_Mod();
			/*
			this.Creature = -1L;
			this.CheckProb();
			if (this.TotalProb == 0f)
			{
                for (int k = 0; k < 5; k++)
                {
                    prob[k] = 0.2f;
                }
			}
			if (this.TotalProb == 0f)
			{
				return;
			}
			float num = UnityEngine.Random.Range(0f, this.TotalProb);
			float num2 = 0f;
			int i = 0;
			for (int j = 0; j < 5; j++)
			{
				if (this.probState[j])
				{
					num2 += this.prob[j];
					if (num <= num2)
					{
						i = j;
						break;
					}
				}
			}
			ActivateStateList list = this.GetList(i);
			ActivateStateModel randomCreature = list.GetRandomCreature();
			this.Creature = randomCreature.id;
			list.RemoveAction(randomCreature.id);*/
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00185284 File Offset: 0x00183484
		public ActivateStateList GetList(int i)
		{
			ActivateStateList result = null;
			if (!CreatureGenerateInfoManager.Instance.GetCreatureState((RiskLevel)i, out result))
			{
				Debug.LogError("Failed to list");
			}
			return result;
		}

		// <Patch>
		public void SetCreature_Mod()
		{ // <Mod> Made way to handle 0% total prob
			this.CreatureMod = new LobotomyBaseMod.LcIdLong(-1L);
			this.CheckProb();
			if (this.TotalProb == 0f)
			{
                for (int k = 0; k < 5; k++)
                {
                    prob[k] = 0.2f;
                }
			}
			if (this.TotalProb == 0f)
			{
				return;
			}
			float num = UnityEngine.Random.Range(0f, this.TotalProb);
			float num2 = 0f;
			int i = 0;
			for (int j = 0; j < 5; j++)
			{
				bool flag2 = this.probState[j];
				if (flag2)
				{
					num2 += this.prob[j];
					bool flag3 = num <= num2;
					if (flag3)
					{
						i = j;
						break;
					}
				}
			}
			ActivateStateList list = this.GetList(i);
			ActivateStateModel randomCreature = list.GetRandomCreature();
			this.CreatureMod = new LobotomyBaseMod.LcIdLong(randomCreature.modid, randomCreature.id);
			list.RemoveAction_Mod(this.CreatureMod);
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x000367E7 File Offset: 0x000349E7
		// Note: this type is marked as 'beforefieldinit'.
		static CreatureGenerateDoor()
		{
		}

		// Token: 0x04003922 RID: 14626
		public static readonly float[] zeroAry = new float[5];

		// Token: 0x04003923 RID: 14627
		public static bool[] initialState = new bool[]
		{
			true,
			true,
			true,
			true,
			true
		};

		// Token: 0x04003924 RID: 14628
		public const int MAX = 5;

		// Token: 0x04003925 RID: 14629
		public float[] prob = new float[5];

		// Token: 0x04003926 RID: 14630
		public bool[] probState = new bool[5];

		// Token: 0x04003927 RID: 14631
		public long Creature = -1L;

		// <Patch>
		public LobotomyBaseMod.LcIdLong CreatureMod;
	}
}
