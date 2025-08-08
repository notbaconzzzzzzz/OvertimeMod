using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AA9 RID: 2729
public class SystemLogScript : MonoBehaviour, IObserver
{
	// Token: 0x060051C5 RID: 20933 RVA: 0x00042C0A File Offset: 0x00040E0A
	public SystemLogScript()
	{
	}

	// Token: 0x060051C6 RID: 20934 RVA: 0x00042C28 File Offset: 0x00040E28
	private void Start()
	{
		this.texture.alpha = 0f;
	}

	// Token: 0x060051C7 RID: 20935 RVA: 0x00042C3A File Offset: 0x00040E3A
	private void OnEnable()
	{
		Notice.instance.Observe(NoticeName.AddSystemLog, this);
		Notice.instance.Observe(NoticeName.CreatureObserveLevelAdded, this);
		Notice.instance.Observe(NoticeName.SetSystemLogSize, this);
	}

	// Token: 0x060051C8 RID: 20936 RVA: 0x00042C5C File Offset: 0x00040E5C
	private void OnDisable()
	{
		Notice.instance.Remove(NoticeName.AddSystemLog, this);
		Notice.instance.Remove(NoticeName.CreatureObserveLevelAdded, this);
		Notice.instance.Remove(NoticeName.SetSystemLogSize, this);
	}

	// Token: 0x060051C9 RID: 20937 RVA: 0x001DB8E4 File Offset: 0x001D9AE4
	private void CreatureLog(CreatureModel cm, string desc)
	{
		SystemLogScript.CreatureSystemLog creatureSystemLog = this.CheckContains(cm.instanceId);
		if (creatureSystemLog == null)
		{
			creatureSystemLog = new SystemLogScript.CreatureSystemLog(cm.instanceId);
			this.creatureList.Add(creatureSystemLog);
		}
		LogItemScript t = this.script.MakeText(desc);
		creatureSystemLog.AddLog(t);
	}

	// Token: 0x060051CA RID: 20938 RVA: 0x001DB930 File Offset: 0x001D9B30
	public SystemLogScript.CreatureSystemLog CheckContains(long target)
	{
		SystemLogScript.CreatureSystemLog result = null;
		foreach (SystemLogScript.CreatureSystemLog creatureSystemLog in this.creatureList)
		{
			if (creatureSystemLog.targetId == target)
			{
				result = creatureSystemLog;
				break;
			}
		}
		return result;
	}

	// Token: 0x060051CB RID: 20939 RVA: 0x001DB99C File Offset: 0x001D9B9C
	public void OnNotice(string notice, params object[] param)
	{
		string text = string.Empty;
		if (NoticeName.AddSystemLog == notice)
		{
			if (param[0] is CreatureModel)
			{
				text = "▶  " + (string)param[1];
				this.CreatureLog((CreatureModel)param[0], text);
			}
			else
			{
				text = (string)param[0];
				this.script.MakeText("▶  " + text);
			}
			this.script.Sort();
			Debug.Log("AddSystemLog::" + text);
		}
		else if (NoticeName.CreatureObserveLevelAdded == notice)
		{
			CreatureModel creatureModel = param[0] as CreatureModel;
			if (creatureModel.observeInfo.GetObserveState(CreatureModel.regionName[0]))
			{
				SystemLogScript.CreatureSystemLog creatureSystemLog = this.CheckContains(creatureModel.instanceId);
				if (creatureSystemLog != null)
				{
					creatureSystemLog.OnObserveLevelUpdated(creatureModel);
				}
			}
		}
		else if (NoticeName.SetSystemLogSize == notice)
		{
			ScalePivot.gameObject.GetComponent<LogCanvasScaler>().ForcelySetScale((float)param[0]);
		}
	}

	// Token: 0x060051CC RID: 20940 RVA: 0x00042C7E File Offset: 0x00040E7E
	public void OnPointerEnter()
	{
		this.currentStart = this.texture.alpha;
		this.currentDest = 1f;
		this.effectTimer.StartTimer(0.2f);
		this.isOpen = true;
	}

	// Token: 0x060051CD RID: 20941 RVA: 0x00042CB3 File Offset: 0x00040EB3
	public void OnPointerExit()
	{
		this.currentStart = this.texture.alpha;
		this.currentDest = 0f;
		this.effectTimer.StartTimer(0.2f);
		this.isOpen = false;
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001DBA80 File Offset: 0x001D9C80
	private void Update()
	{
		if (this.effectTimer.started)
		{
			float alpha = Mathf.Lerp(this.currentStart, this.currentDest, this.effectTimer.Rate);
			this.texture.alpha = alpha;
			if (this.effectTimer.RunTimer())
			{
				if (this.isOpen)
				{
					this.texture.alpha = 1f;
				}
				else
				{
					this.texture.alpha = 0f;
				}
			}
		}
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x001DBB08 File Offset: 0x001D9D08
	public void UpdateScale()
	{
		Vector2 anchoredPosition = this.ScalePivot.anchoredPosition;
		Vector2 sizeDelta = this.Frame.sizeDelta;
		sizeDelta.y = (anchoredPosition.y - 37f) * 1.1111f;
		this.Frame.sizeDelta = sizeDelta;
	}

	// Token: 0x04004BAF RID: 19375
	private const string SystemLogStartFormat = "▶  ";

	// Token: 0x04004BB0 RID: 19376
	private const float effectTime = 0.2f;

	// Token: 0x04004BB1 RID: 19377
	private UnscaledTimer effectTimer = new UnscaledTimer();

	// Token: 0x04004BB2 RID: 19378
	public LoggingScript script;

	// Token: 0x04004BB3 RID: 19379
	public CanvasGroup texture;

	// Token: 0x04004BB4 RID: 19380
	public RectTransform Frame;

	// Token: 0x04004BB5 RID: 19381
	public RectTransform ScalePivot;

	// Token: 0x04004BB6 RID: 19382
	private List<SystemLogScript.CreatureSystemLog> creatureList = new List<SystemLogScript.CreatureSystemLog>();

	// Token: 0x04004BB7 RID: 19383
	private bool isOpen;

	// Token: 0x04004BB8 RID: 19384
	private float currentDest;

	// Token: 0x04004BB9 RID: 19385
	private float currentStart;

	// Token: 0x02000AAA RID: 2730
	public class CreatureSystemLog
	{
		// Token: 0x060051D0 RID: 20944 RVA: 0x00042CE8 File Offset: 0x00040EE8
		public CreatureSystemLog(long id)
		{
			this.targetId = id;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x001DBB54 File Offset: 0x001D9D54
		public void OnObserveLevelUpdated()
		{ // <Mod>
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true)) return;
			CreatureModel creature = CreatureManager.instance.GetCreature(this.targetId);
			if (creature != null && creature.metaInfo.collectionName == creature.metaInfo.name)
			{
				this.ChangeText(creature.metaInfo.codeId, creature.metaInfo.collectionName);
			}
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00042D02 File Offset: 0x00040F02
		public void OnObserveLevelUpdated(CreatureModel model)
		{ // <Mod>
			if (SefiraBossManager.Instance.CheckBossActivation(SefiraEnum.YESOD, true)) return;
			if (model.metaInfo.collectionName == model.metaInfo.name)
			{
				this.ChangeText(model.metaInfo.codeId, model.metaInfo.collectionName);
			}
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00042D40 File Offset: 0x00040F40
		public void AddLog(LogItemScript t)
		{
			this.textList.Add(t);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001DBBB4 File Offset: 0x001D9DB4
		public void ChangeText(string origin, string dest)
		{
			foreach (LogItemScript logItemScript in this.textList)
			{
				string text = logItemScript.textTarget.text;
				text = text.Replace(origin, dest);
				logItemScript.SetText(text);
			}
		}

		// Token: 0x04004BBA RID: 19386
		public long targetId;

		// Token: 0x04004BBB RID: 19387
		public List<LogItemScript> textList = new List<LogItemScript>();
	}
}
