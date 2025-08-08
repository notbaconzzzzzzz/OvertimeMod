/*
private void Update() // Made qliphoth meltdowns not go fully transparent
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E1 RID: 2529
public class IsolateOverload : MonoBehaviour
{
	// Token: 0x06004C89 RID: 19593 RVA: 0x00004094 File Offset: 0x00002294
	public IsolateOverload()
	{
	}

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06004C8A RID: 19594 RVA: 0x0003ED99 File Offset: 0x0003CF99
	public bool isActivated
	{
		get
		{
			return this._isActivated;
		}
	}

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06004C8B RID: 19595 RVA: 0x0003EDA1 File Offset: 0x0003CFA1
	public float OriginPositionY
	{
		get
		{
			return this.originPositionY;
		}
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x001C256C File Offset: 0x001C076C
	private void Awake()
	{
		this.originPositionY = base.transform.localPosition.y;
		this.SetActive(false);
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x001C259C File Offset: 0x001C079C
	private void Update()
	{ // <Mod> qliphoth meltdowns only oscillate to half transparency instead of full transparency
		if (this._isActivated)
		{
			float a = -Mathf.Cos(this.alarmValue) * 0.25f + 0.75f;
			foreach (Image image in this.alarms)
			{
				image.color = new Color(1f, 1f, 1f, a);
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
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = this.originPositionY - this.yAdder;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x001C26DC File Offset: 0x001C08DC
	public void SetTimer(float t, float max)
	{
		this.timerBar.fillAmount = t / max;
		this.timerText.text = Mathf.CeilToInt(t).ToString();
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001C2718 File Offset: 0x001C0918
	public void SetActive(bool b)
	{
		this._isActivated = b;
		this.timerBar.gameObject.SetActive(b);
		this.timerText.gameObject.SetActive(b);
		foreach (Image image in this.alarms)
		{
			image.gameObject.SetActive(b);
		}
		if (!b)
		{
			this.alarmValue = 0f;
		}
	}

	// Token: 0x040046D7 RID: 18135
	private bool _isActivated;

	// Token: 0x040046D8 RID: 18136
	public Image timerBar;

	// Token: 0x040046D9 RID: 18137
	public Text timerText;

	// Token: 0x040046DA RID: 18138
	public List<Image> alarms;

	// Token: 0x040046DB RID: 18139
	private float alarmValue;

	// Token: 0x040046DC RID: 18140
	private float originPositionY;

	// Token: 0x040046DD RID: 18141
	private float yAdder;
}
