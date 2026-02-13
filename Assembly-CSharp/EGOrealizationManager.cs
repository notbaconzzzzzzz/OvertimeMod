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
        if (armor.LcId == 200015)
        {
            if (!SpecialModeConfig.instance.GetValue<bool>("PLKeepAbilityWhenWNAbsent") && CreatureManager.instance.FindCreature_Mod(new LobotomyBaseMod.LcIdLong(100015L)) == null)
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
        if (EquipmentTypeInfo.BossIds.Contains(equipment.LcId))
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

    public static float[][] weaponDowngradeValues = new float[]
	{
		new float[] { 1.00f-1f, 1.00f-1f, 1.00f-1f, 1.00f-1f, 1.00f-1f, 0.75f-1f, },
		new float[] { 1.00f-1f, 1.00f-1f, 1.00f-1f, 1.00f-1f, 0.50f-1f, 0.38f-1f, },
		new float[] { 1.00f-1f, 1.00f-1f, 1.00f-1f, 0.70f-1f, 0.35f-1f, 0.27f-1f, },
		new float[] { 1.00f-1f, 1.00f-1f, 0.65f-1f, 0.42f-1f, 0.22f-1f, 0.18f-1f, },
		new float[] { 1.00f-1f, 0.60f-1f, 0.40f-1f, 0.20f-1f, 0.13f-1f, 0.11f-1f, },
		new float[] { 0.55f-1f, 0.33f-1f, 0.22f-1f, 0.11f-1f, 0.07f-1f, 0.06f-1f, },
	};

    public static float[][] armorDowngradeValues = new float[]
	{
		new float[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f, -0.10f, },
		new float[] { 0.00f, 0.00f, 0.00f, 0.00f, -0.15f, -0.30f, },
		new float[] { 0.00f, 0.00f, 0.00f, -0.20f, -0.40f, -0.60f, },
		new float[] { 0.00f, 0.00f, -0.25f, -0.50f, -0.75f, -1.00f, },
		new float[] { 0.00f, -0.30f, -0.60f, -0.90f, -1.20f, -1.50f, },
		new float[] { -0.35f, -0.70f, -1.05f, -1.40f, -1.75f, -2.10f, },
	};

    public static float[][] giftDowngradeValues = new float[]
	{
		new float[] { 0.00f, 0.00f, 0.00f, 0.00f, 0.00f, -0.01f, },
		new float[] { 0.00f, 0.00f, 0.00f, 0.00f, -0.01f, -0.01f, },
		new float[] { 0.00f, 0.00f, 0.00f, -0.01f, -0.01f, -0.01f, },
		new float[] { 0.00f, 0.00f, -0.01f, -0.01f, -0.01f, -0.01f, },
		new float[] { 0.00f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, },
		new float[] { -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, -0.01f, },
	};
}