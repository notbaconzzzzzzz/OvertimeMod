using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004FC RID: 1276
public class RedHoodRequestUI : MonoBehaviour
{
	// Token: 0x06002DC2 RID: 11714 RVA: 0x000045AC File Offset: 0x000027AC
	public RedHoodRequestUI()
	{
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x0002BF68 File Offset: 0x0002A168
	// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x0002BF70 File Offset: 0x0002A170
	public UnitModel CurrentOverlayCreature
	{
		get
		{
			return this._currentOverlayCreature;
		}
		set
		{
			this._currentOverlayCreature = value;
			this.OnSetOverlay(value);
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x0002BF80 File Offset: 0x0002A180
	private RedHood _redHood
	{
		get
		{
			return this._redHoodAnim.RedHood;
		}
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x00132F30 File Offset: 0x00131130
	public void OpenRequest(RedHoodAnim redHoodAnim)
	{
		this._redHoodAnim = redHoodAnim;
		this.canvas.worldCamera = Camera.main;
		CursorManager.instance.cursorMode = CursorMode.ForceSoftware;
		Cursor.visible = false;
		CursorManager.instance.LockCursor();
		this.cursorSprite.SetActive(true);
		this.CurrentOverlayCreature = null;
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x0002BF8D File Offset: 0x0002A18D
	public void OnCloseRequest()
	{
		CursorManager.instance.UnlockCursor();
		CursorManager.instance.cursorMode = CursorMode.Auto;
		CursorManager.instance.CursorSet(MouseCursorType.NORMAL);
		Cursor.visible = true;
		this.cursorSprite.SetActive(false);
		this._redHoodAnim.OnCloseRequestUI();
	}

	// Token: 0x06002DC8 RID: 11720 RVA: 0x00132F84 File Offset: 0x00131184
	public void OnTryRequest()
	{
		if (this.CurrentOverlayCreature != null && this.CanPurchase())
		{
			EnergyModel.instance.SubEnergy((float)this._redHood.GetRequestCost(this.CurrentOverlayCreature));
			if (this.CurrentOverlayCreature is CreatureModel)
			{
				CreatureModel creatureModel = this.CurrentOverlayCreature as CreatureModel;
				if (creatureModel.script is RedShoes)
				{
					RedShoes redShoes = creatureModel.script as RedShoes;
					this._currentOverlayCreature = (redShoes.skill as RedShoesSkill).attractTargetAgent;
				}
			}
			this._redHood.StartRequest(this.CurrentOverlayCreature);
		}
		this.OnCloseRequest();
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x00133028 File Offset: 0x00131228
	private void Update()
	{
		if (this.CurrentOverlayCreature != null)
		{
			this.cursorAttachedRiskLevel.enabled = true;
			this.cursorAttachedCost.enabled = true;
			if (!this.CanPurchase())
			{
				this.cursorAttachedCost.color = this._cursorColorFail;
			}
			else
			{
				this.cursorAttachedCost.color = this._cursorColorNormal;
			}
			if (this.CurrentOverlayCreature is CreatureModel)
			{
				CreatureModel creatureModel = this.CurrentOverlayCreature as CreatureModel;
				this.cursorAttachedRiskLevel.text = string.Format("<color=#{0}FF>{1}</color>", ColorUtility.ToHtmlStringRGB(UIColorManager.instance.GetRiskColor(creatureModel.metaInfo.GetRiskLevel())), creatureModel.metaInfo.GetRiskLevel().ToString().ToUpper());
			}
		}
		else
		{
			this.cursorAttachedCost.enabled = false;
			this.cursorAttachedRiskLevel.enabled = false;
		}
		if (Input.GetMouseButton(0))
		{
			this.OnTryRequest();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnCloseRequest();
		}
		this.CheckRaycasts();
		this.CursorUpdate();
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x00133140 File Offset: 0x00131340
	private bool CanPurchase()
	{
		if (this.CurrentOverlayCreature == null)
		{
			return false;
		}
		int requestCost = this._redHood.GetRequestCost(this.CurrentOverlayCreature);
		return EnergyModel.instance.GetEnergy() >= (float)requestCost;
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x00133180 File Offset: 0x00131380
	private void CursorUpdate()
	{
		Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		this.cursorSprite.transform.position = new Vector3(vector.x, vector.y, 0f);
		this.cursorSprite.transform.localScale = new Vector3(1f, 1f, 1f) * Camera.main.orthographicSize / 15f;
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x00133204 File Offset: 0x00131404
	private void CheckRaycasts()
	{
		List<UnitMouseEventTarget> currentPointerTargets = UnitMouseEventManager.instance.GetCurrentPointerTargets();
		foreach (UnitMouseEventTarget unitMouseEventTarget in currentPointerTargets)
		{
			CreatureUnit component = unitMouseEventTarget.target.GetComponent<CreatureUnit>();
			if (component != null)
			{
				if (this.CurrentOverlayCreature == component.model)
				{
					return;
				}
				if (component.model.IsEscaped())
				{
					this.CurrentOverlayCreature = component.model;
					return;
				}
			}
			else
			{
				ChildCreatureUnit component2 = unitMouseEventTarget.target.GetComponent<ChildCreatureUnit>();
				if (component2 != null)
				{
					if (this.CurrentOverlayCreature == component2.Model)
					{
						return;
					}
					if (component2.Model.IsEscaped())
					{
						this.CurrentOverlayCreature = component2.Model;
						return;
					}
				}
			}
		}
		if (this.CurrentOverlayCreature != null)
		{
			this.CurrentOverlayCreature = null;
		}
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x00133320 File Offset: 0x00131520
	public void OnSetOverlay(UnitModel creature)
	{
		if (creature == null)
		{
			this.cursorAttachedCost.text = string.Empty;
		}
		else
		{
			this.cursorAttachedCost.text = this._redHood.GetRequestCost(creature).ToString();
		}
	}

	// Token: 0x04002B4B RID: 11083
	public Canvas canvas;

	// Token: 0x04002B4C RID: 11084
	public GameObject cursorSprite;

	// Token: 0x04002B4D RID: 11085
	public SpriteRenderer spriteRenderer;

	// Token: 0x04002B4E RID: 11086
	public Text cursorAttachedCost;

	// Token: 0x04002B4F RID: 11087
	public Text cursorAttachedRiskLevel;

	// Token: 0x04002B50 RID: 11088
	public Text cursorTitle;

	// Token: 0x04002B51 RID: 11089
	public Color _cursorColorNormal;

	// Token: 0x04002B52 RID: 11090
	public Color _cursorColorFail;

	// Token: 0x04002B53 RID: 11091
	private UnitModel _currentOverlayCreature;

	// Token: 0x04002B54 RID: 11092
	private RedHoodAnim _redHoodAnim;
}
