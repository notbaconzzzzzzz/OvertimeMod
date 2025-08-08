using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000576 RID: 1398
[RequireComponent(typeof(EventTrigger))]
public class EventColorSetting : MonoBehaviour
{
	// Token: 0x06003043 RID: 12355 RVA: 0x0002CDB3 File Offset: 0x0002AFB3
	public EventColorSetting()
	{
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x0002CDC2 File Offset: 0x0002AFC2
	private void Start()
	{
		this.OnExitColor();
		if (this.SyncButton != null)
		{
			this.buttonState = this.SyncButton.interactable;
		}
		this.SetTrigger();
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x0002CDF2 File Offset: 0x0002AFF2
	private void Update()
	{
		if (this.SyncWithButton && this.buttonState != this.SyncButton.interactable)
		{
			this.OnButtonInteractable();
		}
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x001464EC File Offset: 0x001446EC
	public void SetDisable()
	{
		this.eventActivated = false;
		foreach (MaskableGraphic maskableGraphic in this.targetGraphics)
		{
			maskableGraphic.color = this.disabledColor;
		}
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x00146554 File Offset: 0x00144754
	private void SetTrigger()
	{
		EventTrigger eventTrigger = base.GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = base.gameObject.AddComponent<EventTrigger>();
		}
		EventTrigger.Entry entry = null;
		EventTrigger.Entry entry2 = null;
		EventTrigger.Entry entry3 = null;
		foreach (EventTrigger.Entry entry4 in eventTrigger.triggers)
		{
			if (entry4.eventID == EventTriggerType.PointerEnter)
			{
				entry = entry4;
			}
			else if (entry4.eventID == EventTriggerType.PointerExit)
			{
				entry2 = entry4;
			}
			else if (entry4.eventID == EventTriggerType.PointerDown)
			{
				entry3 = entry4;
			}
		}
		if (this.SyncWithButton)
		{
			if (entry3 == null)
			{
				entry3 = new EventTrigger.Entry();
				entry3.eventID = EventTriggerType.PointerDown;
				eventTrigger.triggers.Add(entry3);
			}
			this.AddEvent(entry3, new EventColorSetting.EventAdded(this.OnClickColor));
		}
		if (entry == null)
		{
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			eventTrigger.triggers.Add(entry);
		}
		this.AddEvent(entry, new EventColorSetting.EventAdded(this.OnEnterColor));
		if (entry2 == null)
		{
			entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			eventTrigger.triggers.Add(entry2);
		}
		this.AddEvent(entry2, new EventColorSetting.EventAdded(this.OnExitColor));
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x001466B0 File Offset: 0x001448B0
	private void AddEvent(EventTrigger.Entry target, EventColorSetting.EventAdded added)
	{
		int persistentEventCount = target.callback.GetPersistentEventCount();
		for (int i = 0; i < persistentEventCount; i++)
		{
			string persistentMethodName = target.callback.GetPersistentMethodName(i);
			if (persistentMethodName == added.Method.Name)
			{
				return;
			}
		}
		target.callback.AddListener(delegate(BaseEventData eventData)
		{
			added();
		});
	}

	// Token: 0x06003049 RID: 12361 RVA: 0x00146728 File Offset: 0x00144928
	private void OnEnterColor()
	{
		if (!this.eventActivated)
		{
			return;
		}
		foreach (MaskableGraphic maskableGraphic in this.targetGraphics)
		{
			maskableGraphic.color = this.overlayColor;
		}
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x00146798 File Offset: 0x00144998
	private void OnExitColor()
	{
		if (!this.eventActivated)
		{
			return;
		}
		foreach (MaskableGraphic maskableGraphic in this.targetGraphics)
		{
			maskableGraphic.color = this.defaultColor;
		}
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x00146808 File Offset: 0x00144A08
	private void OnClickColor()
	{
		if (!this.eventActivated)
		{
			return;
		}
		foreach (MaskableGraphic maskableGraphic in this.targetGraphics)
		{
			maskableGraphic.color = this.clickColor;
		}
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x00146878 File Offset: 0x00144A78
	private void OnButtonInteractable()
	{
		if (this.SyncButton.interactable)
		{
			foreach (MaskableGraphic maskableGraphic in this.targetGraphics)
			{
				maskableGraphic.color = this.defaultColor;
			}
		}
		else
		{
			foreach (MaskableGraphic maskableGraphic2 in this.targetGraphics)
			{
				maskableGraphic2.color = this.disabledColor;
			}
		}
		this.buttonState = this.SyncButton.interactable;
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x0002CE1B File Offset: 0x0002B01B
	private void OnDisable()
	{
		this.OnExitColor();
	}

	// Token: 0x04002E08 RID: 11784
	public Color defaultColor;

	// Token: 0x04002E09 RID: 11785
	public Color overlayColor;

	// Token: 0x04002E0A RID: 11786
	public bool eventActivated = true;

	// Token: 0x04002E0B RID: 11787
	[Space(5f)]
	public bool ClickOverrideWithDefault;

	// Token: 0x04002E0C RID: 11788
	public Color clickColor;

	// Token: 0x04002E0D RID: 11789
	[Space(5f)]
	public bool SyncWithButton;

	// Token: 0x04002E0E RID: 11790
	public Button SyncButton;

	// Token: 0x04002E0F RID: 11791
	public Color disabledColor;

	// Token: 0x04002E10 RID: 11792
	private bool buttonState;

	// Token: 0x04002E11 RID: 11793
	public List<MaskableGraphic> targetGraphics;

	// Token: 0x02000577 RID: 1399
	// (Invoke) Token: 0x0600304F RID: 12367
	private delegate void EventAdded();
}
