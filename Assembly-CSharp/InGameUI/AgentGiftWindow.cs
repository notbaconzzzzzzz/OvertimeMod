/*
public void SetAgent(AgentModel agent) // EGO gifts can now be locked/hidden at anytime and reguardless of employee level
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGameUI
{
	// Token: 0x020009E6 RID: 2534
	public class AgentGiftWindow : MonoBehaviour
	{
		// Token: 0x06004CB7 RID: 19639 RVA: 0x0003F106 File Offset: 0x0003D306
		public AgentGiftWindow()
		{
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x001C2F50 File Offset: 0x001C1150
		private void Awake()
		{
			this.dic.Clear();
			foreach (AgentGiftSlot agentGiftSlot in this.slots)
			{
				this.dic.Add(agentGiftSlot.region, agentGiftSlot);
			}
			this.DisableAllSlots();
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x001C2FC8 File Offset: 0x001C11C8
		public AgentGiftSlot GetGiftSlot(EGOgiftAttachRegion region, EGOgiftAttachType type)
		{
			AgentGiftSlot result = null;
			int key = (int)type * 100 + (int)region;
			if (this.dic.TryGetValue(key, out result))
			{
			}
			return result;
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x001C2FF4 File Offset: 0x001C11F4
		private void DisableAllSlots()
		{
			foreach (KeyValuePair<int, AgentGiftSlot> keyValuePair in this.dic)
			{
				keyValuePair.Value.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x001C305C File Offset: 0x001C125C
		public void SetAgent(AgentModel agent)
		{ // <Mod> EGO gifts can now be locked/hidden at anytime and reguardless of employee level
			this.DisableAllSlots();
			if (agent == null)
			{
				return;
			}
			bool flag = agent.level >= 3;
			bool flag2 = agent.level >= 4;
			flag = (flag && GameManager.currentGameManager.state == GameState.STOP);
			flag2 = (flag2 && GameManager.currentGameManager.state == GameState.STOP);
            flag = true;
            flag2 = true;
			List<EGOgiftModel> addedGifts = agent.Equipment.gifts.addedGifts;
			foreach (EGOgiftModel gift in addedGifts)
			{
				this.SetGiftSlot(agent, gift, flag, flag2);
			}
			List<EGOgiftModel> replacedGifts = agent.Equipment.gifts.replacedGifts;
			foreach (EGOgiftModel gift2 in replacedGifts)
			{
				this.SetGiftSlot(agent, gift2, flag, flag2);
			}
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x001C317C File Offset: 0x001C137C
		private void SetGiftSlot(AgentModel agent, EGOgiftModel gift, bool dispState, bool lockState)
		{
			AgentGiftSlot giftSlot = this.GetGiftSlot(gift.metaInfo.AttachRegion, gift.metaInfo.attachType);
			if (giftSlot == null)
			{
				return;
			}
			giftSlot.gameObject.SetActive(true);
			giftSlot.SetButtonInteractable(dispState, lockState);
			giftSlot.SetGift(agent, gift);
		}

		// Token: 0x04004718 RID: 18200
		private const int dispActiveLevel = 3;

		// Token: 0x04004719 RID: 18201
		private const int lockActiveLevel = 4;

		// Token: 0x0400471A RID: 18202
		public List<AgentGiftSlot> slots;

		// Token: 0x0400471B RID: 18203
		public Dictionary<int, AgentGiftSlot> dic = new Dictionary<int, AgentGiftSlot>();
	}
}
