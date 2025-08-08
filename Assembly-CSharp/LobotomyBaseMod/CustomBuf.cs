using System;

namespace LobotomyBaseMod
{
	public class CustomBuf : UnitBuf
	{
		public CustomBuf()
		{
			this.type = UnitBufType.ADD_SUPERARMOR;
		}

		public CustomBuf(float buftime)
		{
			this.type = UnitBufType.ADD_SUPERARMOR;
			this.remainTime = buftime;
		}
	}
}