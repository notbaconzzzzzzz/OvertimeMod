using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BinahBoss
{
	// Token: 0x020009D9 RID: 2521
	public class BinahOverloadUI : MonoBehaviour
	{
		// Token: 0x06004C66 RID: 19558 RVA: 0x0003F025 File Offset: 0x0003D225
		public BinahOverloadUI()
		{
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x0003F04A File Offset: 0x0003D24A
		public bool isActivated
		{
			get
			{
				return this._isActivated;
			}
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x001C1C00 File Offset: 0x001BFE00
		private void Awake()
		{
			this.originPositionY = base.transform.localPosition.y;
			this.SetActive(false);
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x001C1C30 File Offset: 0x001BFE30
		private void Update()
		{ // <Mod>
			if (this._isActivated)
			{
				if (this.currentText != this.overloadType.ToString())
				{
					this.currentText = LocalizeTextDataModel.instance.GetText(string.Format("overload_{0}", this.overloadType.ToString().ToLower()));
					this.overloadTypeText.text = this.currentText;
				}
				float a = (-Mathf.Cos(this.alarmValue) / 2f + 0.5f) * 0.5f + 0.5f;
				foreach (Image image in this.alarms)
				{
					Color color = image.color;
					color.a = a;
					image.color = color;
				}
				Color color2 = this.overloadTypeText.color;
				color2.a = a;
				this.overloadTypeText.color = color2;
                if (overloadType == OverloadType.OBLIVION &&
					GameManager.currentGameManager.state == GameState.PAUSE &&
					GameManager.currentGameManager.GetCurrentPauseCaller() == PAUSECALL.STOPGAME)
				{
					float timerSpeed = 1f;
					this.alarmValue += Time.unscaledDeltaTime * 8f;
					this.yAdder -= Time.unscaledDeltaTime * 2f;
				}
				this.alarmValue += Time.deltaTime * 4f;
				this.yAdder -= Time.deltaTime;
				if (this.yAdder < 0f)
				{
					this.yAdder = 0f;
				}
			}
			else
			{
				this.yAdder += Time.deltaTime * 2.4f;
				if (this.yAdder > 1.2f)
				{
					this.yAdder = 1.2f;
				}
			}
			Vector3 localPosition = this.timerBar.transform.parent.parent.transform.localPosition;
			localPosition.y = this.originPositionY - this.yAdder;
			this.timerBar.transform.parent.parent.transform.localPosition = localPosition;
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x001C1E24 File Offset: 0x001C0024
		public void SetTimer(float t, float max)
		{
			this.timerBar.fillAmount = t / max;
			this.timerText.text = Mathf.CeilToInt(t).ToString();
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001C1E60 File Offset: 0x001C0060
		public void SetActive(bool b)
		{
			this._isActivated = b;
			if (this.timerBar != null)
			{
				this.timerBar.gameObject.SetActive(b);
			}
			if (this.timerText != null)
			{
				this.timerText.gameObject.SetActive(b);
			}
			if (this.alarms != null)
			{
				foreach (Image image in this.alarms)
				{
					image.gameObject.SetActive(b);
				}
			}
			if (this.overloadTypeText != null)
			{
				this.overloadTypeText.gameObject.SetActive(b);
			}
			if (!b)
			{
				this.alarmValue = 0f;
			}
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001C1F4C File Offset: 0x001C014C
		public void SetColor(Color c)
		{
			foreach (Image image in this.alarms)
			{
				image.color = c;
			}
			this.overloadTypeText.color = c;
		}

		// Token: 0x040046BE RID: 18110
		private bool _isActivated;

		// Token: 0x040046BF RID: 18111
		public Image timerBar;

		// Token: 0x040046C0 RID: 18112
		public Text timerText;

		// Token: 0x040046C1 RID: 18113
		public List<Image> alarms = new List<Image>();

		// Token: 0x040046C2 RID: 18114
		public Text overloadTypeText;

		// Token: 0x040046C3 RID: 18115
		private string currentText = string.Empty;

		// Token: 0x040046C4 RID: 18116
		public OverloadType overloadType = OverloadType.BLACKFOG;

		// Token: 0x040046C5 RID: 18117
		private float alarmValue;

		// Token: 0x040046C6 RID: 18118
		public float originPositionY;

		// Token: 0x040046C7 RID: 18119
		private float yAdder;
	}
}
