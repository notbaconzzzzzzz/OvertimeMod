using System;
using LobotomyBaseModLib;
using UnityEngine;

namespace LobotomyBaseMod
{
	public class ConsoleCommand_Mod
	{
		public static void AddGift_Mod(long id, LcId equipid)
		{
			EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipmentForcely_Mod(equipid);
			AgentModel agent = AgentManager.instance.GetAgent(id);
			agent.AttachEGOgift(equipmentModel as EGOgiftModel);
		}

		public static void AddGift(long id, int equipid)
		{
			ConsoleCommand_Mod.AddGift_Mod(id, new LcId(equipid));
		}

		public static void GenerateEquipment(string modid, int id)
		{
			try
			{
				InventoryModel.Instance.CreateEquipment_Mod(new LcId(modid, id));
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		public static void AddWaitingGenCreature(string modid, long id)
		{
			try
			{
				PlayerModel.instance.AddWaitingCreature_Mod(new LcIdLong(modid, id));
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
	}
}
