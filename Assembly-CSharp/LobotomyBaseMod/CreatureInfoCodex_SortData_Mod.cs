using System;

namespace LobotomyBaseMod
{
	public class CreatureInfoCodex_SortData_Mod
	{
		public static int Compare(CreatureInfoCodex_SortData_Mod a, CreatureInfoCodex_SortData_Mod b)
		{
			bool flag = a.index == b.index;
			int result;
			if (flag)
			{
				result = a.id.CompareTo(b.id);
			}
			else
			{
				result = a.index.CompareTo(b.index);
			}
			return result;
		}

		public int index = -1;

		public LcIdLong id;
	}
}
