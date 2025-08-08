using System;

namespace LobotomyBaseMod
{
	public class KeyValuePairSS
	{
		public KeyValuePairSS(string k, string v)
		{
			this.key = k;
			this.value = v;
		}

		public override bool Equals(object obj)
		{
			bool flag = obj is KeyValuePairSS;
			bool result;
			if (flag)
			{
				KeyValuePairSS keyValuePairSS = (KeyValuePairSS)obj;
				result = (keyValuePairSS.key.Equals(this.key) && keyValuePairSS.value.Equals(this.value));
			}
			else
			{
				result = base.Equals(obj);
			}
			return result;
		}

		public override int GetHashCode()
		{
			return (this.key + this.value).GetHashCode();
		}

		public string key;

		public string value;
	}
}
