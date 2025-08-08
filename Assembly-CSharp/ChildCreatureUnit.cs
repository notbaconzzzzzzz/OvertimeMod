using System;
using CommandWindow;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class ChildCreatureUnit : CreatureUnit
{
	// Token: 0x0600553E RID: 21822 RVA: 0x00044D58 File Offset: 0x00042F58
	public ChildCreatureUnit()
	{
	}

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x0600553F RID: 21823 RVA: 0x00044D60 File Offset: 0x00042F60
	public ChildCreatureModel Model
	{
		get
		{
			return this.model as ChildCreatureModel;
		}
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x000040A1 File Offset: 0x000022A1
	public override void Awake()
	{
	}

	// Token: 0x06005541 RID: 21825 RVA: 0x001E7E40 File Offset: 0x001E6040
	public void Init()
	{
		if (this.animTarget != null)
		{
			this.animTarget.gameObject.SetActive(true);
		}
		this.returnObject.SetActive(false);
		this.currentCreatureCanvas.worldCamera = Camera.main;
		this.escapeRisk.text = this.Model.parent.metaInfo.riskLevel;
	}

	// Token: 0x06005542 RID: 21826 RVA: 0x00044D6D File Offset: 0x00042F6D
	public override void Start()
	{
		this.Model.script.OnViewInit(this);
		this.castingSlider.value = 0f;
	}

	// Token: 0x06005543 RID: 21827 RVA: 0x00044D90 File Offset: 0x00042F90
	public override void FixedUpdate()
	{
		if (!this.init)
		{
			return;
		}
		this.UpdateViewPosition();
		this.UpdateDirection();
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x00044DAA File Offset: 0x00042FAA
	public override void LateUpdate()
	{
		if (!this.init)
		{
			return;
		}
		this.UpdateScale();
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x001E7EAC File Offset: 0x001E60AC
	public override void Update()
	{
		if (this.oldState != this.model.state)
		{
			this.OnChangeState();
			this.oldState = this.model.state;
		}
		if (this.Model.state == CreatureState.ESCAPE && (ResearchDataModel.instance.IsUpgradedAbility("show_agent_ui") || (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && GlobalGameManager.instance.tutorialStep > 1)) && !SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD))
		{
			if (!this.escapeUIRoot.activeInHierarchy && this.model.script.HasEscapeUI())
			{
				this.escapeUIRoot.SetActive(true);
				this.escapeRisk.text = this.model.script.GetRiskLevel();
				this.escapeRisk.color = UIColorManager.instance.GetRiskColor(CreatureTypeInfo.GetRiskLevelStringToEnum(this.model.script.GetRiskLevel()));
			}
			if (this.model.script.HasEscapeUI())
			{
				this.escapeRisk.text = this.model.script.GetRiskLevel();
				this.escapeRisk.color = UIColorManager.instance.GetRiskColor(CreatureTypeInfo.GetRiskLevelStringToEnum(this.model.script.GetRiskLevel()));
			}
			this.escapeCreatureName.text = this.Model.script.GetName();
		}
		else if (this.escapeUIRoot.activeInHierarchy)
		{
			this.escapeUIRoot.SetActive(false);
		}
		if (this.Model.script.SetHpSlider(this.hpSlider))
		{
			if (!this.hpSlider.gameObject.activeInHierarchy)
			{
				this.hpSlider.gameObject.SetActive(true);
			}
		}
		else if (this.hpSlider.gameObject.activeInHierarchy)
		{
			this.hpSlider.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x001E80B4 File Offset: 0x001E62B4
	public override void OnChangeState()
	{
		if (this.model.canBeSuppressed && (this.model.state == CreatureState.SUPPRESSED || this.model.state == CreatureState.SUPPRESSED_RETURN))
		{
			this.defaultMouseTarget.gameObject.SetActive(false);
			this._unitMouseEventTarget.gameObject.SetActive(false);
			if (!this.model.script.OnAfterSuppressed())
			{
				if (this.animTarget != null)
				{
					this.animTarget.gameObject.SetActive(false);
				}
				this.returnObject.gameObject.SetActive(true);
			}
		}
		else
		{
			if (this.animTarget != null)
			{
				this.animTarget.gameObject.SetActive(true);
			}
			this.returnObject.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x001E8198 File Offset: 0x001E6398
	public void OnClick()
	{
		if (this.model.state == CreatureState.SUPPRESSED || this.model.state == CreatureState.SUPPRESSED_RETURN)
		{
			return;
		}
		if (this.model.state == CreatureState.ESCAPE)
		{
			CommandWindow.CommandWindow.CreateWindow(CommandType.Suppress, this.Model);
		}
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x00044DBE File Offset: 0x00042FBE
	public override void OnDestroy()
	{
		this.Model.script.OnViewDestroy();
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x001E81E8 File Offset: 0x001E63E8
	protected override void UpdateViewPosition()
	{
		MapEdge currentEdge = this.model.GetCurrentEdge();
		base.transform.localScale = new Vector3(this.model.GetMovableNode().currentScale, this.model.GetMovableNode().currentScale, base.transform.localScale.z);
		if (currentEdge != null && currentEdge.type == "door")
		{
			if (this.visible)
			{
				this.visible = false;
				Vector3 currentViewPosition = this.model.GetCurrentViewPosition();
				currentViewPosition.z = 100000f;
				this.viewPosition = currentViewPosition;
			}
		}
		else
		{
			if (!this.visible)
			{
				this.visible = true;
			}
			this.viewPosition = this.model.GetCurrentViewPosition();
		}
		base.transform.localPosition = this.viewPosition;
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x001E82CC File Offset: 0x001E64CC
	protected override void UpdateDirection()
	{
		MovableObjectNode movableNode = this.Model.GetMovableNode();
		UnitDirection direction = movableNode.GetDirection();
		Vector3 directionScaleFactor = this.directionScaleFactor;
		if (direction == UnitDirection.RIGHT)
		{
			if (directionScaleFactor.x < 0f)
			{
				directionScaleFactor.x = -directionScaleFactor.x;
			}
		}
		else if (directionScaleFactor.x > 0f)
		{
			directionScaleFactor.x = -directionScaleFactor.x;
		}
		this.directionScaleFactor = directionScaleFactor;
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x001E8348 File Offset: 0x001E6548
	protected override void UpdateScale()
	{
		if (this.scaleSetting)
		{
			return;
		}
		if (this.animTarget != null)
		{
			Vector3 localScale = this.animTarget.transform.localScale;
			if (localScale.x < 0f && this.directionScaleFactor.x > 0f)
			{
				localScale.x = -localScale.x;
			}
			if (localScale.x > 0f && this.directionScaleFactor.x < 0f)
			{
				localScale.x = -localScale.x;
			}
			this.animTarget.transform.localScale = localScale;
		}
	}

	// Token: 0x04004EA2 RID: 20130
	public bool init;
}
