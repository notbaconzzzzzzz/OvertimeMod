using System;
using System.Collections.Generic;

// Token: 0x02000718 RID: 1816
public class FactionTypeInfo
{
	// Token: 0x060039F5 RID: 14837 RVA: 0x00033CCE File Offset: 0x00031ECE
	public FactionTypeInfo()
	{
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x00174358 File Offset: 0x00172558
	public FactionActionType Check(UnitModel unit)
	{
		if (unit is CreatureModel)
		{
			FactionActionType result = FactionActionType.HOSTILE;
			if (this.except.CheckContains((unit as CreatureModel).metadataId.ToString(), out result))
			{
				return result;
			}
		}
		FactionActionType result2 = FactionActionType.HOSTILE;
		try
		{
			if (!this.lib.TryGetValue(unit.GetFaction().code, out result2))
			{
			}
		}
		catch (Exception ex)
		{
			return FactionActionType.HOSTILE;
		}
		return result2;
	}

	// Token: 0x0400351F RID: 13599
	public string code = string.Empty;

	// Token: 0x04003520 RID: 13600
	public string name = string.Empty;

	// Token: 0x04003521 RID: 13601
	public string type = string.Empty;

	// Token: 0x04003522 RID: 13602
	public Dictionary<string, FactionActionType> lib = new Dictionary<string, FactionActionType>();

	// Token: 0x04003523 RID: 13603
	public FactionTypeInfo.ExceptType except = new FactionTypeInfo.ExceptType();

	// Token: 0x02000719 RID: 1817
	public class ExceptType
	{
		// Token: 0x060039F7 RID: 14839 RVA: 0x00033D0D File Offset: 0x00031F0D
		public ExceptType()
		{
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x001743DC File Offset: 0x001725DC
		public bool CheckContains(string key, out FactionActionType type)
		{
			FactionActionType factionActionType = FactionActionType.HOSTILE;
			if (this.lib.TryGetValue(key, out factionActionType))
			{
				type = factionActionType;
				return true;
			}
			type = FactionActionType.HOSTILE;
			return false;
		}

		// Token: 0x04003524 RID: 13604
		public Dictionary<string, FactionActionType> lib = new Dictionary<string, FactionActionType>();
	}
}
