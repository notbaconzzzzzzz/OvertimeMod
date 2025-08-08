using System;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class LcIdLong : IEquatable<LcIdLong>, IEquatable<long>, IComparable<LcIdLong>
	{
		public LcIdLong(long id)
		{
			this.id = id;
			this.packageId = "";
		}

		public LcIdLong(string packageId, long id)
		{
			this.packageId = packageId;
			this.id = id;
			bool flag = packageId == null;
			if (flag)
			{
				Debug.LogError("error");
			}
		}

		public bool IsBasic()
		{
			return !this.IsWorkshop();
		}

		public bool IsWorkshop()
		{
			return !LcIdLong.IsBasicId(this.packageId);
		}

		public bool IsNone()
		{
			return this.id < 0L;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is LcIdLong)) return false;
			LcIdLong other = obj as LcIdLong;
			bool flag = obj != null;
			return flag && this.Equals(other);
		}

		public bool Equals(LcIdLong other)
		{
			bool flag = this.id == other.id;
			return flag && this.packageId == other.packageId;
		}

		public bool Equals(long other)
		{
			bool flag = this.id == other;
			return flag && this.IsBasic();
		}

		public override int GetHashCode()
		{
			bool flag = this.packageId == null;
			if (flag)
			{
				Debug.LogError("error");
			}
			return this.id.GetHashCode() + this.packageId.GetHashCode();
		}

		public static bool operator ==(LcIdLong lhs, long rhs)
		{
			bool flag = ((object)lhs) == null;
			if (flag)
			{
				lhs = LcIdLong.None;
			}
			return lhs.Equals(rhs);
		}

		public static bool operator !=(LcIdLong lhs, long rhs)
		{
			return !(lhs == rhs);
		}

		public static bool operator ==(LcIdLong lhs, LcIdLong rhs)
		{
			bool flag = ((object)lhs) == null;
			if (flag)
			{
				lhs = LcIdLong.None;
			}
			bool flag2 = ((object)rhs) == null;
			if (flag2)
			{
				rhs = LcIdLong.None;
			}
			return lhs.Equals(rhs);
		}

		public static bool operator !=(LcIdLong lhs, LcIdLong rhs)
		{
			return !(lhs == rhs);
		}

		public int CompareTo(LcIdLong other)
		{
			int num = this.id.CompareTo(other.id);
			bool flag = num == 0;
			int result;
			if (flag)
			{
				result = this.packageId.CompareTo(other.packageId);
			}
			else
			{
				result = num;
			}
			return result;
		}

		public static bool IsModId(string packageId)
		{
			return !LcIdLong.IsBasicId(packageId);
		}

		public static bool IsBasicId(string packageId)
		{
			return string.IsNullOrEmpty(packageId);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"LcIdLong(",
				this.packageId,
				":",
				this.id.ToString(),
				")"
			});
		}

		public readonly long id;

		public readonly string packageId;

		public static readonly LcIdLong None = new LcIdLong(-1L);
	}
}
