using System;
using System.Reflection;

// Token: 0x0200069B RID: 1691
public class EGOgiftModel : EquipmentModel
{
	// Token: 0x0600373C RID: 14140 RVA: 0x00031F0B File Offset: 0x0003010B
	public EGOgiftModel()
	{
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x00168654 File Offset: 0x00166854
	public static EGOgiftModel MakeGift(EquipmentTypeInfo info)
	{
		EGOgiftModel egogiftModel = new EGOgiftModel();
		egogiftModel.metaInfo = info;
		Type type = Type.GetType(info.script);
		object obj = null;
		if (type != null)
		{
			foreach (Assembly assembly in Add_On.instance.AssemList)
			{
				foreach (Type type2 in assembly.GetTypes())
				{
					if (type2.Name == info.script)
					{
						obj = Activator.CreateInstance(type2);
					}
				}
			}
			if (obj == null)
			{
				obj = Activator.CreateInstance(type);
			}
		}
		if (obj is EquipmentScriptBase)
		{
			egogiftModel.script = (EquipmentScriptBase)obj;
		}
		egogiftModel.script.SetModel(egogiftModel);
		return egogiftModel;
	}

	// Token: 0x0600373E RID: 14142 RVA: 0x00031F35 File Offset: 0x00030135
	public static EGOgiftModel GetDummyGift()
	{
		return EGOgiftModel.MakeGift(EquipmentTypeInfo.GetDummyGiftInfo());
	}
}
