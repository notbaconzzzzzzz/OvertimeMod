using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InGameUI
{
	// Token: 0x020009C3 RID: 2499
	public class AgentUI : MonoBehaviour
	{
		// Token: 0x06004BBB RID: 19387 RVA: 0x0003EF41 File Offset: 0x0003D141
		public AgentUI()
		{
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06004BBC RID: 19388 RVA: 0x0003EF66 File Offset: 0x0003D166
		public UnitModel Model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06004BBD RID: 19389 RVA: 0x0003EF6E File Offset: 0x0003D16E
		// (set) Token: 0x06004BBE RID: 19390 RVA: 0x0003EF76 File Offset: 0x0003D176
		public AgentUI.UIAgentState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					this.OnSetState(value);
				}
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06004BBF RID: 19391 RVA: 0x0003EF8B File Offset: 0x0003D18B
		private Color Black
		{
			get
			{
				return Color.black;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06004BC0 RID: 19392 RVA: 0x0003EF92 File Offset: 0x0003D192
		private bool _isActivated
		{
			get
			{
				return this.ActiveControl.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x001BEFD8 File Offset: 0x001BD1D8
		public void Init(UnitModel model)
		{
			this._model = model;
			this.SetActive(this.ValidateUIActiate());
			this.OnSetState(AgentUI.UIAgentState.NORMAL);
			this.EncounterActiveControl.SetActive(false);
			this.currentHorrorLevel = -1;
			this.UIAnim.SetBool("Damage", false);
			if (model is AgentModel)
			{
				this.Grade.sprite = DeployUI.GetAgentGradeSprite(model as AgentModel);
			}
			else if (model is RabbitModel)
			{
				this.Grade.sprite = DeployUI.GetGradeSprite(4);
			}
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, false))
			{
				base.gameObject.layer = LayerMask.NameToLayer("Front1");
				IEnumerator enumerator = base.gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						transform.gameObject.layer = base.gameObject.layer;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			try
			{
				if (model is AgentModel)
				{
					this._continueUI = (model as AgentModel).GetUnit().continueUI;
				}
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x001BF130 File Offset: 0x001BD330
		private bool ValidateUIActiate()
		{ // <Mod> Overtime Yesod Suppression
			if (this.Model == null)
			{
				return false;
			}
			bool flag = false;
			if ((this.Model is AgentModel && (this.Model as AgentModel).IsDead()) || (this.Model is RabbitModel && (this.Model as RabbitModel).IsDead()))
			{
				flag = true;
			}
			if (flag)
			{
				if (this.ActiveControl.activeInHierarchy)
				{
					this.ActiveControl.gameObject.SetActive(false);
				}
				return false;
			}
			if (Model is AgentModel && (Model as AgentModel).ForceHideUI)
			{
				return false;
			}
			return ResearchDataModel.instance.IsUpgradedAbility("show_agent_ui") || (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && GlobalGameManager.instance.tutorialStep > 1);
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0003EFA4 File Offset: 0x0003D1A4
		public void SetActive(bool state)
		{
			this.BlackHide.gameObject.SetActive(!state);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x000043A5 File Offset: 0x000025A5
		public void SetAgentState(AgentUI.UIAgentState state)
		{
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x001BF1F8 File Offset: 0x001BD3F8
		public void SetOverlayState(bool b)
		{
			if (b)
			{
				this.ActiveControl.transform.localScale = new Vector3(1.15f, 1.15f, 1f);
			}
			else
			{
				this.ActiveControl.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x001BF258 File Offset: 0x001BD458
		public void SetSelectState(bool b)
		{
			if (b)
			{
				this.Texture.color = new Color(1f, 0.5803922f, 0.25490198f);
				this.BlackHide.color = new Color(1f, 0.5803922f, 0.25490198f);
				Graphic name = this.Name;
				Color color = Color.black;
				this.Grade.color = color;
				name.color = color;
			}
			else
			{
				Graphic name2 = this.Name;
				Color color = DeployUI.instance.UIDefaultFill;
				this.Grade.color = color;
				name2.color = color;
				this.Texture.color = this.stateColor[(int)this._state];
				this.BlackHide.color = this.stateColor[(int)this._state];
			}
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x001BF334 File Offset: 0x001BD534
		private void Update()
		{
			if (this._model == null)
			{
				return;
			}
			if (this._encounterTimer.started && this._encounterTimer.RunTimer())
			{
				this.currentHorrorLevel = -1;
				this.EncounterActiveControl.SetActive(false);
				this.UIAnim.SetBool("Damage", false);
			}
			if (!this.ValidateUIActiate())
			{
				this.SetActive(false);
			}
			else
			{
				this.SetActive(true);
			}
			if (!this._isActivated)
			{
				return;
			}
			this.UpdateState();
			this.UpdateHp();
			this.UpdateMental();
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001BF3D0 File Offset: 0x001BD5D0
		private void UpdateHp()
		{
			float num = (float)this.Model.maxHp;
			float hp = this.Model.hp;
			this.HpFill.fillAmount = hp / num;
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x001BF404 File Offset: 0x001BD604
		private void UpdateMental()
		{
			float num = (float)this.Model.maxMental;
			float mental = this.Model.mental;
			this.MentalFill.fillAmount = mental / num;
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x001BF438 File Offset: 0x001BD638
		private void UpdateState()
		{
			AgentModel agentModel = this.Model as AgentModel;
			RabbitModel rabbitModel = this.Model as RabbitModel;
			bool flag = false;
			if ((this.Model is AgentModel && (this.Model as AgentModel).IsDead()) || (this.Model is RabbitModel && (this.Model as RabbitModel).IsDead()))
			{
				flag = true;
			}
			if (flag)
			{
				this.State = AgentUI.UIAgentState.DEAD;
				return;
			}
			if (agentModel != null)
			{
				AgentAIState state = agentModel.GetState();
				if (state == AgentAIState.CANNOT_CONTROLL)
				{
					this.State = AgentUI.UIAgentState.UNCON;
					return;
				}
				if (agentModel.IsPanic())
				{
					this.State = AgentUI.UIAgentState.PANIC;
					return;
				}
				this.State = AgentUI.UIAgentState.NORMAL;
			}
			else if (rabbitModel != null)
			{
				this.State = AgentUI.UIAgentState.NORMAL;
			}
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x001BF504 File Offset: 0x001BD704
		private void OnSetState(AgentUI.UIAgentState state)
		{
			this._state = state;
			this.Texture.color = this.stateColor[(int)state];
			if (state == AgentUI.UIAgentState.UNCON || state == AgentUI.UIAgentState.DEAD || state == AgentUI.UIAgentState.PANIC)
			{
				if (!this.ValidateUIActiate())
				{
					this.BlackHide.color = this.Texture.color;
				}
			}
			else
			{
				this.BlackHide.color = this.Black;
			}
			switch (state)
			{
			case AgentUI.UIAgentState.NORMAL:
				if (this._continueUI != null)
				{
					this._continueUI.SetSefiraColor();
				}
				break;
			case AgentUI.UIAgentState.PANIC:
				if (this._continueUI != null)
				{
					this._continueUI.SetColor(this.stateColor[(int)state]);
				}
				break;
			case AgentUI.UIAgentState.DEAD:
				this.SetActive(false);
				break;
			case AgentUI.UIAgentState.UNCON:
				if (this._continueUI != null)
				{
					this._continueUI.SetColor(this.stateColor[(int)state]);
				}
				break;
			}
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x001BF62C File Offset: 0x001BD82C
		public void EncounterActivate(int level)
		{
			if (this.EncounterActiveControl.activeInHierarchy && this.currentHorrorLevel > level)
			{
				return;
			}
			this.currentHorrorLevel = level;
			string text = "Encounter_" + level;
			text = LocalizeTextDataModel.instance.GetText(text);
			Sprite sprite = this.level_Exclams[level];
			Color color = this.level_Color[level];
			this.EncounterIcon.sprite = sprite;
			Graphic encounterText = this.EncounterText;
			Color color2 = color;
			this.EncounterIcon.color = color2;
			encounterText.color = color2;
			this.EncounterText.text = text;
			if (this.UIAnim != null)
			{
				this.UIAnim.SetBool("Damage", true);
			}
			this._encounterTimer.StartTimer(this.EncounterEffectTime);
			this.EncounterActiveControl.SetActive(true);
		}

		// Token: 0x0400462B RID: 17963
		[NonSerialized]
		private UnitModel _model;

		// Token: 0x0400462C RID: 17964
		private int currentHorrorLevel = -1;

		// Token: 0x0400462D RID: 17965
		private AgentUI.UIAgentState _state;

		// Token: 0x0400462E RID: 17966
		public GameObject ActiveControl;

		// Token: 0x0400462F RID: 17967
		public Image Texture;

		// Token: 0x04004630 RID: 17968
		public Image HpFill;

		// Token: 0x04004631 RID: 17969
		public Image MentalFill;

		// Token: 0x04004632 RID: 17970
		public Text Name;

		// Token: 0x04004633 RID: 17971
		public Image Grade;

		// Token: 0x04004634 RID: 17972
		public Image BlackHide;

		// Token: 0x04004635 RID: 17973
		[Header("Color")]
		public Color[] stateColor;

		// Token: 0x04004636 RID: 17974
		[Header("Encounter")]
		public GameObject EncounterActiveControl;

		// Token: 0x04004637 RID: 17975
		public float EncounterEffectTime = 2f;

		// Token: 0x04004638 RID: 17976
		public Image EncounterIcon;

		// Token: 0x04004639 RID: 17977
		public Text EncounterText;

		// Token: 0x0400463A RID: 17978
		[Header("Encounter_common")]
		public Sprite[] level_Exclams;

		// Token: 0x0400463B RID: 17979
		public Color[] level_Color;

		// Token: 0x0400463C RID: 17980
		public Animator UIAnim;

		// Token: 0x0400463D RID: 17981
		private AgentContinueUI _continueUI;

		// Token: 0x0400463E RID: 17982
		private Timer _encounterTimer = new Timer();

		// Token: 0x020009C4 RID: 2500
		public enum UIAgentState
		{
			// Token: 0x04004640 RID: 17984
			NORMAL,
			// Token: 0x04004641 RID: 17985
			PANIC,
			// Token: 0x04004642 RID: 17986
			DEAD,
			// Token: 0x04004643 RID: 17987
			UNCON
		}
	}
}
