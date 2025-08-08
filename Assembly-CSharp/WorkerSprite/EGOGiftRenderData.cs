using System;
using UnityEngine;

namespace WorkerSprite
{
	// Token: 0x020008CA RID: 2250
	[Serializable]
	public class EGOGiftRenderData
	{
		// Token: 0x04003FC7 RID: 16327
		public Sprite Sprite;

		// Token: 0x04003FC8 RID: 16328
		public EGOgiftAttachRegion region;

		// Token: 0x04003FC9 RID: 16329
		public EGOgiftAttachType attachType;

		// Token: 0x04003FCA RID: 16330
		public string slot = string.Empty;

		// Token: 0x04003FCB RID: 16331
		public string attachmentName = string.Empty;

		// Token: 0x04003FCC RID: 16332
		public string DataName = string.Empty;

		// Token: 0x04003FCD RID: 16333
		public SpriteRenderer renderer;

		// Token: 0x04003FCE RID: 16334
		public long metaId;
	}
}
