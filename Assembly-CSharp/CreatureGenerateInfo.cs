using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007DF RID: 2015
public class CreatureGenerateInfo : MonoBehaviour
{
	// Token: 0x06003E42 RID: 15938 RVA: 0x0000460C File Offset: 0x0000280C
	public CreatureGenerateInfo()
	{
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06003E43 RID: 15939 RVA: 0x001848C8 File Offset: 0x00182AC8
	public static long[] all_except_creditCreatures
	{
		get
		{
			List<long> list = new List<long>(CreatureGenerateInfo.all);
			foreach (long item in CreatureGenerateInfo.creditCreatures)
			{
				list.Remove(item);
			}
			return list.ToArray();
		}
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0018490C File Offset: 0x00182B0C
	public static long[] GetAll(bool removeTool = false)
	{
		List<long> list;
		if (GlobalGameManager.instance.dlcCreatureOn)
		{
			list = new List<long>(CreatureGenerateInfo.all);
		}
		else
		{
			list = new List<long>(CreatureGenerateInfo.all_except_creditCreatures);
		}
		if (removeTool)
		{
			foreach (long item in CreatureGenerateInfo.kitCreature)
			{
				list.Remove(item);
			}
		}
		if (list.Contains(100038L))
		{
			list.Remove(100038L);
		}
		return list.ToArray();
	}

	// Token: 0x06003E45 RID: 15941 RVA: 0x00184984 File Offset: 0x00182B84
	public static bool IsCreditCreature(long id)
	{
		List<long> list = new List<long>(CreatureGenerateInfo.creditCreatures);
		return list.Contains(id);
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x001849A4 File Offset: 0x00182BA4
	static CreatureGenerateInfo()
	{
	}

	// Token: 0x040038FD RID: 14589
	public static readonly long[] kitCreature = new long[]
	{
		300001L,
		300002L,
		300003L,
		300004L,
		300005L,
		300006L,
		300007L,
		300101L,
		300102L,
		300103L,
		300104L,
		300105L,
		300108L
	};

	// Token: 0x040038FE RID: 14590
	public static readonly long[][] r1 = new long[][]
	{
		new long[]
		{
			100008L
		},
		new long[]
		{
			100035L
		},
		new long[]
		{
			100020L
		},
		new long[]
		{
			100064L
		}
	};

	// Token: 0x040038FF RID: 14591
	public static readonly long[][] r2 = new long[][]
	{
		new long[]
		{
			100054L
		},
		new long[]
		{
			100001L,
			100018L,
			100036L,
			100021L,
			100020L,
			100012L,
			100013L,
			100022L,
			100027L,
			100037L,
			100028L,
			100024L,
			100034L
		},
		new long[]
		{
			100001L,
			100018L,
			100036L,
			100021L,
			100020L,
			100012L,
			100013L,
			100022L,
			100027L,
			100037L,
			100003L,
			100011L,
			100002L,
			100016L,
			100041L,
			100017L,
			100102L
		},
		new long[]
		{
			100003L,
			100011L,
			100002L,
			100016L,
			100041L,
			100017L,
			100102L
		}
	};

	// Token: 0x04003900 RID: 14592
	public static readonly long[][] r3 = new long[][]
	{
		new long[]
		{
			100001L,
			100018L,
			100036L,
			100021L,
			100020L,
			100012L,
			100013L,
			100022L,
			100027L,
			100037L,
			100003L,
			100011L,
			100002L,
			100016L,
			100041L,
			100017L,
			100102L
		},
		new long[]
		{
			100012L,
			100013L,
			100022L,
			100027L,
			100037L,
			100003L,
			100011L,
			100002L,
			100016L,
			100041L,
			100017L,
			100102L,
			100028L,
			100024L,
			100034L
		},
		new long[]
		{
			100003L,
			100011L,
			100002L,
			100016L,
			100041L,
			100017L,
			100102L,
			100029L,
			100035L,
			100023L,
			100026L,
			100039L
		},
		new long[]
		{
			100029L,
			100035L,
			100023L,
			100026L,
			100039L,
			100019L,
			100005L
		}
	};

	// Token: 0x04003901 RID: 14593
	public static readonly long[][] r4 = new long[][]
	{
		new long[]
		{
			100012L,
			100022L,
			100003L,
			100011L,
			100002L,
			100016L
		},
		new long[]
		{
			100012L,
			100022L,
			100003L,
			100011L,
			100002L,
			100016L,
			100035L,
			100023L,
			100026L,
			100039L
		},
		new long[]
		{
			100003L,
			100011L,
			100002L,
			100016L,
			100035L,
			100023L,
			100026L,
			100039L
		},
		new long[]
		{
			100035L,
			100023L,
			100026L,
			100039L,
			100019L
		}
	};

	// Token: 0x04003902 RID: 14594
	public static readonly long[][] r5 = new long[][]
	{
		new long[]
		{
			100014L,
			100021L,
			100022L,
			100012L,
			100037L,
			100020L,
			100013L,
			100027L,
			100018L,
			100001L,
			100036L,
			100011L,
			100008L,
			100031L,
			100032L,
			100002L,
			100003L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L
		},
		new long[]
		{
			100028L,
			100009L,
			100025L,
			100034L,
			100014L,
			100021L,
			100022L,
			100012L,
			100037L,
			100020L,
			100013L,
			100027L,
			100018L,
			100001L,
			100036L,
			100011L,
			100008L,
			100031L,
			100032L,
			100002L,
			100003L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L,
			100035L,
			100023L,
			100004L,
			100026L,
			100029L,
			100030L,
			100033L,
			100039L,
			100040L
		},
		new long[]
		{
			100024L,
			100011L,
			100008L,
			100031L,
			100032L,
			100002L,
			100003L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L,
			100035L,
			100023L,
			100004L,
			100026L,
			100029L,
			100030L,
			100033L,
			100039L,
			100040L
		},
		new long[]
		{
			100024L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L,
			100035L,
			100023L,
			100004L,
			100026L,
			100029L,
			100030L,
			100033L,
			100039L,
			100040L,
			100005L,
			100019L
		}
	};

	// Token: 0x04003903 RID: 14595
	public static readonly long[][] r6 = new long[][]
	{
		new long[]
		{
			100014L,
			100021L,
			100022L,
			100012L,
			100037L,
			100020L,
			100013L,
			100027L,
			100018L,
			100001L,
			100036L,
			100011L,
			100008L,
			100031L,
			100032L,
			100002L,
			100003L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L
		},
		new long[]
		{
			100028L,
			100009L,
			100025L,
			100034L,
			100014L,
			100021L,
			100022L,
			100012L,
			100037L,
			100020L,
			100013L,
			100027L,
			100018L,
			100001L,
			100036L,
			100011L,
			100008L,
			100031L,
			100032L,
			100002L,
			100003L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L,
			100035L,
			100023L,
			100004L,
			100026L,
			100029L,
			100030L,
			100033L,
			100039L,
			100040L
		},
		new long[]
		{
			100024L,
			100011L,
			100008L,
			100031L,
			100032L,
			100002L,
			100003L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L,
			100035L,
			100023L,
			100004L,
			100026L,
			100029L,
			100030L,
			100033L,
			100039L,
			100040L
		},
		new long[]
		{
			100024L,
			100006L,
			100017L,
			100101L,
			100102L,
			100016L,
			100035L,
			100023L,
			100004L,
			100026L,
			100029L,
			100030L,
			100033L,
			100039L,
			100040L,
			100005L,
			100019L
		}
	};

	// Token: 0x04003904 RID: 14596
	public static readonly long[] all = new long[]
	{
		100009L,
		100028L,
		100024L,
		100034L,
		100001L,
		100018L,
		100020L,
		100021L,
		100036L,
		100012L,
		100013L,
		100022L,
		100027L,
		100037L,
		100002L,
		100003L,
		100011L,
		100016L,
		100041L,
		100017L,
		100102L,
		100023L,
		100026L,
		100029L,
		100035L,
		100039L,
		100019L,
		100005L,
		300001L,
		300002L,
		300003L,
		300004L,
		300005L,
		300006L,
		300007L,
		100007L,
		100043L,
		100103L,
		100047L,
		100048L,
		100049L,
		100044L,
		100045L,
		100042L,
		100046L,
		100053L,
		100054L,
		100051L,
		300101L,
		300102L,
		300103L,
		300104L,
		100056L,
		100050L,
		100055L,
		300105L,
		100052L,
		100057L,
		100058L,
		100031L,
		100033L,
		100032L,
		100040L,
		100006L,
		300106L,
		300107L,
		100008L,
		300108L,
		100004L,
		100059L,
		100060L,
		100061L,
		100014L,
		100104L,
		300109L,
		100064L,
		100106L,
		100105L,
		100065L,
		100063L,
		100062L,
		300110L
	};

	// Token: 0x04003905 RID: 14597
	public static readonly long[] creditCreatures = new long[]
	{
		100064L,
		100106L,
		100105L,
		100065L,
		100063L,
		100062L,
		300110L
	};

	// Token: 0x04003906 RID: 14598
	public static readonly long[] all_for_codex = new long[]
	{
		100009L,
		100028L,
		100034L,
		100001L,
		100018L,
		100020L,
		100021L,
		100036L,
		100012L,
		100013L,
		100022L,
		100027L,
		100037L,
		100002L,
		100003L,
		100011L,
		100016L,
		100041L,
		100017L,
		100102L,
		100023L,
		100026L,
		100029L,
		100035L,
		100039L,
		100019L,
		100005L,
		300001L,
		300002L,
		300003L,
		300004L,
		300005L,
		300006L,
		300007L,
		100007L,
		100043L,
		100103L,
		100047L,
		100048L,
		100049L,
		100044L,
		100045L,
		100042L,
		100046L,
		100053L,
		100054L,
		100051L,
		300101L,
		300102L,
		300103L,
		300104L,
		100056L,
		100050L,
		100055L,
		300105L,
		100052L,
		100057L,
		100058L,
		100031L,
		100033L,
		100032L,
		100040L,
		100006L,
		300106L,
		300107L,
		100008L,
		300108L,
		100004L,
		100059L,
		100060L,
		100061L,
		100104L,
		300109L,
		100015L
	};
}
