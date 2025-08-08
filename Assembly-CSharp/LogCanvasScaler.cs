using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009EE RID: 2542
public class LogCanvasScaler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	// Token: 0x06004CE2 RID: 19682 RVA: 0x0003F81D File Offset: 0x0003DA1D
	public LogCanvasScaler()
	{
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x0003C64D File Offset: 0x0003A84D
	private RectTransform _rectTr
	{
		get
		{
			return base.gameObject.GetComponent<RectTransform>();
		}
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x00004381 File Offset: 0x00002581
	public void Start()
	{
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x00004381 File Offset: 0x00002581
	private void Update()
	{
	}

	// Token: 0x06004CE6 RID: 19686 RVA: 0x00004381 File Offset: 0x00002581
	public void OnBeginDrag(PointerEventData eventData)
	{
	}

	// Token: 0x06004CE7 RID: 19687 RVA: 0x0003F83B File Offset: 0x0003DA3B
	public void OnDrag(PointerEventData eventData)
	{
		this.SetDraggedPosition(eventData);
		this.logScript.UpdateScale();
	}

	// Token: 0x06004CE8 RID: 19688 RVA: 0x00004381 File Offset: 0x00002581
	public void OnEndDrag(PointerEventData eventData)
	{
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x001C45A4 File Offset: 0x001C27A4
	private void SetDraggedPosition(PointerEventData pData)
	{
		Vector3 vector;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this._rectTr, pData.position, pData.pressEventCamera, out vector))
		{
			this._rectTr.position = new Vector3(this._rectTr.position.x, Mathf.Clamp(vector.y, this._min, this._max));
		}
	}

	// Token: 0x0400473D RID: 18237
	private bool _entered;

	// Token: 0x0400473E RID: 18238
	private bool _clicked;

	// Token: 0x0400473F RID: 18239
	public SystemLogScript logScript;

	// Token: 0x04004740 RID: 18240
	public float _min = 140f;

	// Token: 0x04004741 RID: 18241
	public float _max = 1000f;
}
