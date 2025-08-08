using System;

namespace Customizing
{
	// Token: 0x02000516 RID: 1302
	public class AgentData
	{
		// Token: 0x06002E66 RID: 11878 RVA: 0x0013801C File Offset: 0x0013621C
		public AgentData()
		{
			this.agentName = null;
			this.isCustomName = false;
			this.appearance = new Appearance();
			this.isCustomAppearance = false;
			this.statBonus = new StatBonus();
			this.isStatBonusAdded = false;
			this.CustomName = string.Empty;
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06002E67 RID: 11879 RVA: 0x0002C7F9 File Offset: 0x0002A9F9
		public int RLevel
		{
			get
			{
				if (this.originalModel != null)
				{
					return AgentModel.CalculateStatLevelForCustomizing(this.stat.hp + this.originalModel.titleBonus.hp);
				}
				return AgentModel.CalculateStatLevelForCustomizing(this.stat.hp);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06002E68 RID: 11880 RVA: 0x0002C838 File Offset: 0x0002AA38
		public int WLevel
		{
			get
			{
				if (this.originalModel != null)
				{
					return AgentModel.CalculateStatLevelForCustomizing(this.stat.mental + this.originalModel.titleBonus.mental);
				}
				return AgentModel.CalculateStatLevelForCustomizing(this.stat.mental);
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x0002C877 File Offset: 0x0002AA77
		public int BLevel
		{
			get
			{
				if (this.originalModel != null)
				{
					return AgentModel.CalculateStatLevelForCustomizing(this.stat.work + this.originalModel.titleBonus.work);
				}
				return AgentModel.CalculateStatLevelForCustomizing(this.stat.work);
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06002E6A RID: 11882 RVA: 0x0002C8B6 File Offset: 0x0002AAB6
		public int PLevel
		{
			get
			{
				if (this.originalModel != null)
				{
					return AgentModel.CalculateStatLevelForCustomizing(this.stat.battle + this.originalModel.titleBonus.battle);
				}
				return AgentModel.CalculateStatLevelForCustomizing(this.stat.battle);
			}
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x00138074 File Offset: 0x00136274
		public void AppearCopy(AgentData copied)
		{
			this.agentName = copied.agentName;
			this.CustomName = copied.CustomName;
			this.isCustomName = copied.isCustomName;
			this.isUniqueCredit = copied.isUniqueCredit;
			this.uniqueScriptIndex = copied.uniqueScriptIndex;
			this.appearance.spriteSet = copied.appearance.spriteSet;
			this.isCustomAppearance = copied.isCustomAppearance;
			this.appearance.FrontHair = copied.appearance.FrontHair;
			this.appearance.RearHair = copied.appearance.RearHair;
			this.appearance.Eyebrow_Def = copied.appearance.Eyebrow_Def;
			this.appearance.Eyebrow_Panic = copied.appearance.Eyebrow_Panic;
			this.appearance.Eyebrow_Battle = copied.appearance.Eyebrow_Battle;
			this.appearance.Eye_Def = copied.appearance.Eye_Def;
			this.appearance.Eye_Panic = copied.appearance.Eye_Panic;
			this.appearance.Eye_Dead = copied.appearance.Eye_Dead;
			this.appearance.Mouth_Def = copied.appearance.Mouth_Def;
			this.appearance.Mouth_Battle = copied.appearance.Mouth_Battle;
			this.appearance.HairColor = copied.appearance.HairColor;
			this.appearance.EyeColor = copied.appearance.EyeColor;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x001381E8 File Offset: 0x001363E8
		public int GetVanliaLevel()
		{
			int num = AgentModel.CalculateStatLevel(this.stat.hp) + AgentModel.CalculateStatLevel(this.stat.mental) + AgentModel.CalculateStatLevel(this.stat.work) + AgentModel.CalculateStatLevel(this.stat.battle);
			if (num < 6)
			{
				return 1;
			}
			if (num < 9)
			{
				return 2;
			}
			if (num < 12)
			{
				return 3;
			}
			if (num < 16)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x04002BF0 RID: 11248
		public AgentName agentName;

		// Token: 0x04002BF1 RID: 11249
		public string CustomName;

		// Token: 0x04002BF2 RID: 11250
		public bool isCustomName;

		// Token: 0x04002BF3 RID: 11251
		public Appearance appearance;

		// Token: 0x04002BF4 RID: 11252
		public bool isCustomAppearance;

		// Token: 0x04002BF5 RID: 11253
		public StatBonus statBonus;

		// Token: 0x04002BF6 RID: 11254
		public bool isStatBonusAdded;

		// Token: 0x04002BF7 RID: 11255
		public WorkerPrimaryStat stat;

		// Token: 0x04002BF8 RID: 11256
		public AgentModel originalModel;

		// Token: 0x04002BF9 RID: 11257
		public bool isUniqueCredit;

		// Token: 0x04002BFA RID: 11258
		public int uniqueScriptIndex = -1;
	}
}
