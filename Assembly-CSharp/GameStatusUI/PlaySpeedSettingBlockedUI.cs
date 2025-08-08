using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameStatusUI
{
	// Token: 0x02000AF7 RID: 2807
	public class PlaySpeedSettingBlockedUI : MonoBehaviour
	{
		// Token: 0x060054A4 RID: 21668 RVA: 0x00044B29 File Offset: 0x00042D29
		public PlaySpeedSettingBlockedUI()
		{
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060054A5 RID: 21669 RVA: 0x00044B59 File Offset: 0x00042D59
		public bool IsDisplaying
		{
			get
			{
				return this._isDisplaying;
			}
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x00044B61 File Offset: 0x00042D61
		public void SetCloseAction(PlaySpeedSettingBlockedUI.voidAction closeAction)
		{
			this.closeAction = closeAction;
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x00044B6A File Offset: 0x00042D6A
		protected void CloseActionExecute()
		{
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x00044B82 File Offset: 0x00042D82
		public virtual void OnClose()
		{
			this.CloseActionExecute();
			base.gameObject.SetActive(false);
			this._isDisplaying = false;
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x00044B9D File Offset: 0x00042D9D
		public virtual void OnShow()
		{
			base.gameObject.SetActive(true);
			this.Timer.StartTimer(this.Duration);
			this._isDisplaying = true;
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x00044BC3 File Offset: 0x00042DC3
		public virtual void Update()
		{
			if (this.Timer.started && this.Timer.RunTimer())
			{
				this.OnClose();
			}
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x000140A1 File Offset: 0x000122A1
		public virtual bool IsFunctionEnabled(PlaySpeedSettingBlockFunction function)
		{
			return true;
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001E8600 File Offset: 0x001E6800
		public virtual void AddAction(PlaySpeedSettingBlockFunction function, PlaySpeedSettingBlockedUI.voidAction action)
		{
			PlaySpeedSettingBlockedUI.voidAction voidAction = null;
			if (this.actionLibrary.TryGetValue(function, out voidAction))
			{
				this.actionLibrary.Remove(function);
			}
			this.actionLibrary.Add(function, action);
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x00044BEB File Offset: 0x00042DEB
		public virtual void SetTimeStopAction(PlaySpeedSettingBlockedUI.voidAction action)
		{
			this.AddAction(PlaySpeedSettingBlockFunction.TIMESTOP, action);
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x00044BF5 File Offset: 0x00042DF5
		public virtual void SetTimeMultiplyingAction(PlaySpeedSettingBlockedUI.voidAction action)
		{
			this.AddAction(PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER, action);
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001E863C File Offset: 0x001E683C
		public virtual void OnTryFunction(PlaySpeedSettingBlockFunction function, bool isOnCheck = false)
		{
			PlaySpeedSettingBlockedUI.voidAction voidAction = null;
			if (this.actionLibrary.TryGetValue(function, out voidAction))
			{
				voidAction();
			}
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x00044BFF File Offset: 0x00042DFF
		public virtual bool OnTryDisplay(PlaySpeedSettingBlockFunction function)
		{
			return this.blockFunction == function;
		}

		// Token: 0x04004E0F RID: 19983
		public PlaySpeedSettingBlockFunction blockFunction = PlaySpeedSettingBlockFunction.SPEEDMULTIPLIER;

		// Token: 0x04004E10 RID: 19984
		public PlaySpeedSettingBlockedUI.voidAction closeAction;

		// Token: 0x04004E11 RID: 19985
		public PlaySpeedSettingBlockedUI.voidAction timeStopTryAction;

		// Token: 0x04004E12 RID: 19986
		public PlaySpeedSettingBlockedUI.voidAction timeMultiplyingAction;

		// Token: 0x04004E13 RID: 19987
		public Dictionary<PlaySpeedSettingBlockFunction, PlaySpeedSettingBlockedUI.voidAction> actionLibrary = new Dictionary<PlaySpeedSettingBlockFunction, PlaySpeedSettingBlockedUI.voidAction>();

		// Token: 0x04004E14 RID: 19988
		[NonSerialized]
		public UnscaledTimer Timer = new UnscaledTimer();

		// Token: 0x04004E15 RID: 19989
		public float Duration = 1f;

		// Token: 0x04004E16 RID: 19990
		private bool _isDisplaying;

		// Token: 0x02000AF8 RID: 2808
		// (Invoke) Token: 0x060054B2 RID: 21682
		public delegate void voidAction();
	}
}
