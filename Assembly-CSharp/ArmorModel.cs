using System;

// Token: 0x0200069A RID: 1690
public class ArmorModel : EquipmentModel
{
	// Token: 0x06003737 RID: 14135 RVA: 0x00031F0B File Offset: 0x0003010B
	public ArmorModel()
	{
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x00031F13 File Offset: 0x00030113
	public DefenseInfo GetDefense(UnitModel actor)
	{
		return this.script.GetDefense(actor);
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x001685F4 File Offset: 0x001667F4
	public static ArmorModel MakeArmor(EquipmentTypeInfo info)
	{
		ArmorModel armorModel = new ArmorModel();
		armorModel.metaInfo = info;
		Type type = Type.GetType(info.script);
		object obj = null;
		if (type != null)
		{
			obj = Activator.CreateInstance(type);
		}
		if (obj is EquipmentScriptBase)
		{
			armorModel.script = (EquipmentScriptBase)obj;
		}
		armorModel.script.SetModel(armorModel);
		return armorModel;
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x00031F21 File Offset: 0x00030121
	public static ArmorModel GetDummyArmor()
	{
		return ArmorModel.MakeArmor(EquipmentTypeInfo.GetDummyArmorInfo());
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x00031F2D File Offset: 0x0003012D
	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
	}
}
