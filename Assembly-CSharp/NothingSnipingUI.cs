using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020004F6 RID: 1270
public class NothingSnipingUI : MonoBehaviour
{
	// Token: 0x06002DCE RID: 11726 RVA: 0x000046B8 File Offset: 0x000028B8
	public NothingSnipingUI()
	{
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x00135EA4 File Offset: 0x001340A4
	public void StartSnipe(Nothing nothingScript)
	{
		this.nothingScript = nothingScript;
		this.canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
		GameManager.currentGameManager.Pause(PAUSECALL.INGAMEEFFECT);
		CursorManager.instance.cursorMode = CursorMode.ForceSoftware;
		CursorManager.instance.CursorSet(MouseCursorType.SCOPE);
		CursorManager.instance.LockCursor();
		this.countDown = 10f;
		this.countDown_integer = Mathf.CeilToInt(this.countDown);
		this.countDownText.text = this.countDown_integer.ToString();
		nothingScript.model.Unit.PlaySoundMono("reload");
	}

    // <Mod>
	public void StartSnipe(Nothing nothingScript, float shootTime)
	{
		this.nothingScript = nothingScript;
		this.canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
		GameManager.currentGameManager.Pause(PAUSECALL.INGAMEEFFECT);
		CursorManager.instance.cursorMode = CursorMode.ForceSoftware;
		CursorManager.instance.CursorSet(MouseCursorType.SCOPE);
		CursorManager.instance.LockCursor();
		this.countDown = shootTime;
		this.countDown_integer = Mathf.CeilToInt(this.countDown);
		this.countDownText.text = this.countDown_integer.ToString();
		nothingScript.model.Unit.PlaySoundMono("reload");
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x00135F4C File Offset: 0x0013414C
	private void Update()
	{
		this.countDown -= Time.unscaledDeltaTime;
		this.snipingFilter.transform.localPosition = new Vector3((Mathf.Clamp01(Input.mousePosition.x / (float)Screen.width) - 0.5f) * 1600f, (Mathf.Clamp01(Input.mousePosition.y / (float)Screen.height) - 0.5f) * 900f, 0f);
		if (Input.GetMouseButtonDown(0))
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			List<UnitModel> list2 = new List<UnitModel>();
			foreach (RaycastResult raycastResult in list)
			{
				SnipingTarget component = raycastResult.gameObject.GetComponent<SnipingTarget>();
				if (component != null)
				{
					Debug.Log("Try Snipe " + component);
					UnitModel target = component.GetTarget();
					if (!(target is CreatureModel))
					{
						list2.Add(target);
						Debug.Log("Snipe " + target.GetUnitName());
					}
				}
			}
			Vector3 position = Camera.main.ScreenToWorldPoint(pointerEventData.position);
			position.z = 0f;
			GameObject gameObject = Prefab.LoadPrefab("Effect/GunShotEffect");
			gameObject.transform.position = position;
			gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
			gameObject.transform.localRotation = Quaternion.identity;
			this.Shot(list2);
			return;
		}
		int num = Mathf.CeilToInt(this.countDown);
		if (num != this.countDown_integer)
		{
			this.nothingScript.model.Unit.PlaySoundMono("count");
			this.countDown_integer = num;
			this.countDownText.text = this.countDown_integer.ToString();
		}
		if (this.countDown <= 0f)
		{
			this.FinishSniping();
		}
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x00004459 File Offset: 0x00002659
	private void LateUpdate()
	{
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x0013619C File Offset: 0x0013439C
	public void Shot(List<UnitModel> _list)
	{
		this.nothingScript.model.Unit.PlaySoundMono("shot");
		foreach (UnitModel unitModel in _list)
		{
			unitModel.TakeDamage(new DamageInfo(RwbpType.R, 10000f));
		}
		this.FinishSniping();
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x0002C1B2 File Offset: 0x0002A3B2
	public void FinishSniping()
	{
		GameManager.currentGameManager.Resume(PAUSECALL.INGAMEEFFECT);
		CursorManager.instance.UnlockCursor();
		CursorManager.instance.cursorMode = CursorMode.Auto;
		CursorManager.instance.CursorSet(MouseCursorType.NORMAL);
		this.nothingScript.FinishSniping();
	}

	// Token: 0x04002B2C RID: 11052
	public Canvas canvas;

	// Token: 0x04002B2D RID: 11053
	public Image snipingFilter;

	// Token: 0x04002B2E RID: 11054
	public Text countDownText;

	// Token: 0x04002B2F RID: 11055
	private Nothing nothingScript;

	// Token: 0x04002B30 RID: 11056
	private float countDown;

	// Token: 0x04002B31 RID: 11057
	private int countDown_integer;
}
