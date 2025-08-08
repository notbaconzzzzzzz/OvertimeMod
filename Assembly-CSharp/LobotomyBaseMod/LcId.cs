using System;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class LcId : IEquatable<LcId>, IEquatable<int>, IComparable<LcId>
	{
		public LcId(int id)
		{
			this.id = id;
			this.packageId = "";
		}

		public LcId(string packageId, int id)
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
			return !LcId.IsBasicId(this.packageId);
		}

		public bool IsNone()
		{
			return this.id < 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is LcId)) return false;
			LcId other = obj as LcId;
			bool flag = obj != null;
			return flag && this.Equals(other);
		}

		public bool Equals(LcId other)
		{
			bool flag = this.id == other.id;
			return flag && this.packageId == other.packageId;
		}

		public bool Equals(int other)
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

		public static bool operator ==(LcId lhs, int rhs)
		{
			bool flag = ((object)lhs) == null;
			if (flag)
			{
				lhs = LcId.None;
			}
			return lhs.Equals(rhs);
		}

		public static bool operator !=(LcId lhs, int rhs)
		{
			return !(lhs == rhs);
		}

		public static bool operator ==(LcId lhs, LcId rhs)
		{
			bool flag = ((object)lhs) == null;
			if (flag)
			{
				lhs = LcId.None;
			}
			bool flag2 = ((object)rhs) == null;
			if (flag2)
			{
				rhs = LcId.None;
			}
			return lhs.Equals(rhs);
		}

		public static bool operator !=(LcId lhs, LcId rhs)
		{
			return !(lhs == rhs);
		}

		public int CompareTo(LcId other)
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
			return !LcId.IsBasicId(packageId);
		}

		public static bool IsBasicId(string packageId)
		{
			return string.IsNullOrEmpty(packageId);
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"LorId(",
				this.packageId.ToString(),
				":",
				this.id.ToString(),
				")"
			});
		}

		public readonly int id;

		public readonly string packageId;

		public static readonly LcId None = new LcId(-1);
	}
}
