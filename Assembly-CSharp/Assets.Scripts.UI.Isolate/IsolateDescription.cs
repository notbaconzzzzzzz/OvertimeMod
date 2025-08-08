/*
public void Display(string desc, int fontSize, int textId) // 
*/
using System;
using Assets.Scripts.UI.Effect.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Isolate
{
	// Token: 0x020009E8 RID: 2536
	public class IsolateDescription : MonoBehaviour
	{
		// Token: 0x06004CCD RID: 19661 RVA: 0x0003F928 File Offset: 0x0003DB28
		public IsolateDescription()
		{
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06004CCE RID: 19662 RVA: 0x0003F942 File Offset: 0x0003DB42
		public AutoSizeSetter SizeFitter
		{
			get
			{
				return base.GetComponent<AutoSizeSetter>();
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06004CCF RID: 19663 RVA: 0x0003F94A File Offset: 0x0003DB4A
		public Text Text
		{
			get
			{
				return this.SizeFitter.fitter;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x0003E094 File Offset: 0x0003C294
		public Image Texture
		{
			get
			{
				return base.transform.GetChild(0).GetComponent<Image>();
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06004CD1 RID: 19665 RVA: 0x0003F957 File Offset: 0x0003DB57
		public UIController Controller
		{
			get
			{
				return base.GetComponent<UIController>();
			}
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x0003F95F File Offset: 0x0003DB5F
		public void Init(IsolateDescController ctrl)
		{
			this.controller = ctrl;
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x001C4A18 File Offset: 0x001C2C18
		public void Display(string desc, int fontSize, int textId)
		{ // <Mod> Overtime Yesod Suppression
			this.textId = textId;
			base.gameObject.SetActive(true);
			this.Texture.sprite = this.controller.texture[UnityEngine.Random.Range(0, 4)];
			this.Text.text = desc;
			this.Text.fontSize = fontSize;
			this.SizeFitter.SetSize();
            if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true))
			{
				timer.StartTimer(12f * controller.GetRoom().GetCurrentWorkSpeed());
			}
			else
			{
				this.timer.StartTimer(UnityEngine.Random.Range(3f, 5f) * this.controller.GetRoom().GetCurrentWorkSpeed());
			}
			base.GetComponent<RectTransform>().anchoredPosition = this.initial;
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0003F968 File Offset: 0x0003DB68
		private void Start()
		{
			this.initial = base.GetComponent<RectTransform>().anchoredPosition;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0003F987 File Offset: 0x0003DB87
		public void Terminate()
		{
			this.Controller.animator.SetTrigger("Forcely");
			this.Controller.Hide();
			this.timer.StopTimer();
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0003F9B4 File Offset: 0x0003DBB4
		public void FixedUpdate()
		{
			if (this.timer.RunTimer())
			{
				this.Controller.Hide();
			}
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0003F9D1 File Offset: 0x0003DBD1
		private void OnDisable()
		{
			if (this.controller != null)
			{
				this.controller.OnDisableUnit(this);
			}
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0003F9F0 File Offset: 0x0003DBF0
		public void SetEndEvent(IsolateDescription.OnDisplayEnd end)
		{
			this.endEvent = end;
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0003F9F9 File Offset: 0x0003DBF9
		public void OnEnd()
		{
			if (this.endEvent != null)
			{
				this.endEvent(this);
			}
		}

		// Token: 0x04004723 RID: 18211
		private const float min = 3f;

		// Token: 0x04004724 RID: 18212
		private const float max = 5f;

		// Token: 0x04004725 RID: 18213
		private Timer timer = new Timer();

		// Token: 0x04004726 RID: 18214
		private Vector2 initial;

		// Token: 0x04004727 RID: 18215
		[NonSerialized]
		public IsolateDescController controller;

		// Token: 0x04004728 RID: 18216
		[NonSerialized]
		public int textId = -1;

		// Token: 0x04004729 RID: 18217
		public DescPos pos;

		// Token: 0x0400472A RID: 18218
		public DescController DescController;

		// Token: 0x0400472B RID: 18219
		private IsolateDescription.OnDisplayEnd endEvent;

		// Token: 0x020009E9 RID: 2537
		// (Invoke) Token: 0x06004CDB RID: 19675
		public delegate void OnDisplayEnd(IsolateDescription i);
	}
}
