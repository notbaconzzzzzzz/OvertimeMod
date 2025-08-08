using System;
using System.Linq;

public class EGOrealizationManager
{
	public static EGOrealizationManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EGOrealizationManager();
			}
			return _instance;
		}
	}

	public static EGOrealizationManager _instance;

    public float WeaponUpgrade(EquipmentTypeInfo weapon)
    {
        int level = EgoRiskLevel(weapon);
        if (realizationLevel < level) return 0f;
        return weaponUpgradeValues[level - 1];
    }

    public float ArmorUpgrade(EquipmentTypeInfo armor)
    {
        if (armor.id == 200015)
        {
            if (CreatureManager.instance.FindCreature(100015L) == null)
            {
                if (realizationLevel < 5) return 0f;
                return armorUpgradeValues[4];
            }
            else
            {
                if (realizationLevel < 6) return 0f;
                return 1f/30f;
            }
        }
        int level = EgoRiskLevel(armor);
        if (realizationLevel < level) return 0f;
        return armorUpgradeValues[level - 1];
    }

    public float GiftUpgrade(CreatureEquipmentMakeInfo gift)
    {
        int level = gift.GiftRiskLevel;
        if (realizationLevel < level) return 0f;
        return giftUpgradeValues[level - 1];
    }

    public static int EgoRiskLevel(EquipmentTypeInfo equipment)
    {
        if (EquipmentTypeInfo.BossIds.Contains(equipment.id))
        {
            return 6;
        }
        return (int)equipment.Grade + 1;
    }

    public int realizationLevel = 0;

    public static float[] weaponUpgradeValues = new float[]
	{
		0.6f,
		0.4f,
		0.3f,
		0.25f,
		0.225f,
		0.2f
	};

    public static float[] armorUpgradeValues = new float[]
	{
		0.2f,
		0.15f,
		0.12f,
		0.1f,
		2f/30f,
		0.05f
	};

    public static float[] giftUpgradeValues = new float[]
	{
		0.01f,
		0.01f,
		0.01f,
		0.01f,
		0.01f,
		0.01f
	};
}