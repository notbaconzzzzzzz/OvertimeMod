using System;
using LobotomyBaseMod;

namespace CreatureGenerate
{
	// Token: 0x020007E6 RID: 2022
	public class ActivateStateModel
	{
		// <Patch>
		public static LobotomyBaseMod.LcIdLong GetLcId(ActivateStateModel model)
		{
			return new LobotomyBaseMod.LcIdLong(model.modid, model.id);
		}

		// Token: 0x04003930 RID: 14640
		public RiskLevel riskLevel;

		// Token: 0x04003931 RID: 14641
		public long id = -1L;

		// Token: 0x04003932 RID: 14642
		public bool isUsed;

		// Token: 0x04003933 RID: 14643
		public bool isRemoved;

		// Token: 0x04003934 RID: 14644
		public bool isKit;

		// <Patch>
		public string modid;
	}
}
