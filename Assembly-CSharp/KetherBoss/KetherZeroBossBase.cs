using System;

namespace KetherBoss
{
	// Token: 0x02000823 RID: 2083
	public class KetherZeroBossBase : KetherBossBase, IObserver
	{
		// Token: 0x060040BA RID: 16570 RVA: 0x00037D3D File Offset: 0x00035F3D
		public KetherZeroBossBase()
		{
			this.type = KetherBossType.E0;
			this.sefiraEnum = SefiraEnum.KETHER;
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x00037D54 File Offset: 0x00035F54
		public override void OnStageStart()
		{
			Notice.instance.Observe(NoticeName.OnOrdealStarted, this);
			this.max = StageTypeInfo.instnace.GetEnergyNeed(PlayerModel.instance.GetDay());
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x00037D80 File Offset: 0x00035F80
		public override void OnDestroy()
		{
			Notice.instance.Remove(NoticeName.OnOrdealStarted, this);
			base.OnDestroy();
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x00037D98 File Offset: 0x00035F98
		public override bool IsCleared()
		{
			return this._clawAppeared && this._clawSuppressed && EnergyModel.instance.GetEnergy() >= this.max;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x000140B9 File Offset: 0x000122B9
		public override bool IsReadyToClose()
		{
			return true;
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x00195ACC File Offset: 0x00193CCC
		public void OnNotice(string notice, params object[] param)
		{
			if (notice == NoticeName.OnOrdealStarted)
			{
				OrdealBase ordealBase = param[0] as OrdealBase;
				if (ordealBase is FixerOrdeal && ordealBase.level == OrdealLevel.MIDNIGHT)
				{
					this._clawAppeared = true;
				}
			}
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x00037DC7 File Offset: 0x00035FC7
		public void OnFixerClawSuppressed()
		{
			this._clawSuppressed = true;
		}

		// Token: 0x04003B77 RID: 15223
		private const int clearQliphothLevel = 10;

		// Token: 0x04003B78 RID: 15224
		private float max;

		// Token: 0x04003B79 RID: 15225
		private bool _clawAppeared;

		// Token: 0x04003B7A RID: 15226
		private bool _clawSuppressed;

		// Token: 0x04003B7B RID: 15227
		private FixerClaw fixerClaw;
	}
}
