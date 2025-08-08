using System;
using UnityEngine;
using UnityEngine.UI;
using WhiteNightSpace;

namespace CreatureSelect
{
	// Token: 0x02000975 RID: 2421
	public class CreatureSelectUnit : MonoBehaviour, IAnimatorEventCalled
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600491F RID: 18719 RVA: 0x0003D326 File Offset: 0x0003B526
		public long CreatureID
		{
			get
			{
				return this._creatureId;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06004920 RID: 18720 RVA: 0x0003CC60 File Offset: 0x0003AE60
		private RectTransform RectTransform
		{
			get
			{
				return base.gameObject.GetComponent<RectTransform>();
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06004921 RID: 18721 RVA: 0x001B7CB8 File Offset: 0x001B5EB8
		private float DullFreq
		{
			get
			{
				float num = 1f;
				if (this.pointer)
				{
					num = 0.5f;
				}
				return UnityEngine.Random.Range(4f, 8f) * num;
			}
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x0003D32E File Offset: 0x0003B52E
		private void LateInit()
		{
			if (this.savedId != -1L)
			{
				this.Init(this.savedId);
				this.savedId = -1L;
			}
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x0003D351 File Offset: 0x0003B551
		public void OnChange()
		{
			this.isChanging = true;
			this.TransAnim.SetTrigger("Change");
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x0003D36A File Offset: 0x0003B56A
		public void OnChangeComplete()
		{
			this.isChanging = false;
			this.LateInit();
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x001B7CF0 File Offset: 0x001B5EF0
		public void Init(long creatureId)
		{
			if (this.isChanging)
			{
				this.savedId = creatureId;
				return;
			}
			if (creatureId == 100014L && PlagueDoctor.CheckAdvent())
			{
				creatureId = 100015L;
			}
			this._creatureId = creatureId;
			if (this._creatureId == -1L)
			{
				this.SetDisabled();
				return;
			}
			base.gameObject.SetActive(true);
			this.TransSelected = false;
			this.metaInfo = CreatureTypeList.instance.GetData(creatureId);
			this.SetEnabled();
			this.IdText.text = this.GetName();
			this.DullTimer.StartTimer(this.DullFreq);
			this.DullAnimCTRL.SetFloat("Speed", 0.2f);
			foreach (Image image in this.Frame)
			{
				image.enabled = false;
			}
			if (CreatureGenerateInfo.IsCreditCreature(this._creatureId))
			{
				this.NormalCreatureFrame.SetActive(false);
				this.CreditCreatureFrame.SetActive(true);
			}
			else
			{
				this.NormalCreatureFrame.SetActive(true);
				this.CreditCreatureFrame.SetActive(false);
			}
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x000043CD File Offset: 0x000025CD
		public void SetEnabled()
		{
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x0002B38B File Offset: 0x0002958B
		public void SetDisabled()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x0003D379 File Offset: 0x0003B579
		public void Start()
		{
			this.OnExit();
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x000043CD File Offset: 0x000025CD
		public void Update()
		{
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x001B7E18 File Offset: 0x001B6018
		public void FixedUpdate()
		{
			if (this.DullTimer.RunTimer())
			{
				this.DullTimer.StartTimer(this.DullFreq);
				this.DullAnimCTRL.SetInteger("DullType", UnityEngine.Random.Range(0, 3));
				this.DullAnimCTRL.SetTrigger("Dull");
			}
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x001B7E70 File Offset: 0x001B6070
		public void OnPointerEnter()
		{
			if (!CreatureSelectUI.instance.IsInteractable())
			{
				return;
			}
			if (this.isChanging)
			{
				return;
			}
			this.DullAnimCTRL.SetFloat("Speed", 1f);
			this.OnEnter();
			foreach (Image image in this.Frame)
			{
				image.enabled = true;
			}
			this.pointer = true;
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x0003D381 File Offset: 0x0003B581
		public void OnPointerExit()
		{
			if (!CreatureSelectUI.instance.IsInteractable())
			{
				return;
			}
			if (this.isChanging)
			{
				return;
			}
			this.DullAnimCTRL.SetFloat("Speed", 0.2f);
			this.OnExit();
			this.pointer = false;
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x0003D3C1 File Offset: 0x0003B5C1
		public void OnPointerClick()
		{
			if (this.isChanging)
			{
				return;
			}
			if (!CreatureSelectUI.instance.IsInteractable())
			{
				return;
			}
			this.TransSelected = true;
			CreatureSelectUI.instance.OnClickUnit(this);
			this.TransAnim.SetTrigger("OnClick");
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x000043CD File Offset: 0x000025CD
		private void ResetPos()
		{
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x001B7EE4 File Offset: 0x001B60E4
		private string GetName()
		{
			if (this.metaInfo == null)
			{
				return string.Empty;
			}
			string result = string.Empty;
			CreatureObserveInfoModel observeInfo = CreatureManager.instance.GetObserveInfo(this.metaInfo.id);
			if (observeInfo != null)
			{
				result = CreatureModel.GetUnitName(this.metaInfo, observeInfo);
			}
			else
			{
				result = this.metaInfo.codeId;
			}
			return result;
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x0003D401 File Offset: 0x0003B601
		private void OnEnter()
		{
			this.TransAnim.SetTrigger("Enter");
			CreatureSelectUI.instance.OnEnterUnit(this);
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x001B7F44 File Offset: 0x001B6144
		private void OnExit()
		{
			if (!this.TransSelected)
			{
				this.TransAnim.SetTrigger("Exit");
				foreach (Image image in this.Frame)
				{
					image.enabled = false;
				}
			}
			CreatureSelectUI.instance.OnExitUnit(this);
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x000043CD File Offset: 0x000025CD
		public void OnCalled()
		{
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void OnCalled(int i)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void AgentReset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void SimpleReset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void AnimatorEventInit()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void CreatureAnimCall(int i, CreatureBase script)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void AttackCalled(int i)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void AttackDamageTimeCalled()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00013DE4 File Offset: 0x00011FE4
		public void SoundMake(string src)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x0003D41E File Offset: 0x0003B61E
		public string GetText()
		{
			if (this.metaInfo == null)
			{
				return string.Empty;
			}
			return this.metaInfo.openText;
		}

		// Token: 0x0400437A RID: 17274
		public const long EmptyId = -1L;

		// Token: 0x0400437B RID: 17275
		public GameObject RootObject;

		// Token: 0x0400437C RID: 17276
		public float TransitionTime = 1f;

		// Token: 0x0400437D RID: 17277
		private long _creatureId;

		// Token: 0x0400437E RID: 17278
		private CreatureTypeInfo metaInfo;

		// Token: 0x0400437F RID: 17279
		private Timer TransitionTimer = new Timer();

		// Token: 0x04004380 RID: 17280
		private Timer DullTimer = new Timer();

		// Token: 0x04004381 RID: 17281
		public Text IdText;

		// Token: 0x04004382 RID: 17282
		public Animator TransAnim;

		// Token: 0x04004383 RID: 17283
		public Animator DoorAnim;

		// Token: 0x04004384 RID: 17284
		public RectTransform PositionPivot;

		// Token: 0x04004385 RID: 17285
		public Animator DullAnimCTRL;

		// Token: 0x04004386 RID: 17286
		public Image[] Frame;

		// Token: 0x04004387 RID: 17287
		public GameObject NormalCreatureFrame;

		// Token: 0x04004388 RID: 17288
		public GameObject CreditCreatureFrame;

		// Token: 0x04004389 RID: 17289
		public bool TransSelected;

		// Token: 0x0400438A RID: 17290
		private bool pointer;

		// Token: 0x0400438B RID: 17291
		private long savedId = -1L;

		// Token: 0x0400438C RID: 17292
		private bool isChanging;
	}
}
