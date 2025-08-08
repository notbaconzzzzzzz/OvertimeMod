using System;
using GameStatusUI;
using UnityEngine;

// Token: 0x02000610 RID: 1552
public class EnergyModel : IObserver
{
	// Token: 0x06003512 RID: 13586 RVA: 0x000307FB File Offset: 0x0002E9FB
	public EnergyModel()
	{
		Notice.instance.Observe(NoticeName.EnergyTimer, this);
		Notice.instance.Observe(NoticeName.FixedUpdate, this);
		this.OnStageStart();
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06003513 RID: 13587 RVA: 0x00030829 File Offset: 0x0002EA29
	public static EnergyModel instance
	{
		get
		{
			if (EnergyModel._instance == null)
			{
				EnergyModel._instance = new EnergyModel();
			}
			return EnergyModel._instance;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06003514 RID: 13588 RVA: 0x00030844 File Offset: 0x0002EA44
	// (set) Token: 0x06003515 RID: 13589 RVA: 0x0003084C File Offset: 0x0002EA4C
	public bool fillBlock
	{
		get
		{
			return this._fillBlock;
		}
		set
		{
			this._fillBlock = value;
		}
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x00030855 File Offset: 0x0002EA55
	public void OnStageStart()
	{
		this.energy = 0f;
		this.finishCounter = 0;
		this.finishTimer = 0f;
		this.fillBlock = false;
		this.halfSay = false;
		this.fullSay = false;
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x0015EB84 File Offset: 0x0015CD84
	public void AddEnergy(float added)
	{
		int day = PlayerModel.instance.GetDay();
		float energyNeed = StageTypeInfo.instnace.GetEnergyNeed(day);
		this.energy += added;
		if (this.energy >= energyNeed)
		{
			this.energy = energyNeed;
		}
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x00030889 File Offset: 0x0002EA89
	public void SubEnergy(float sub)
	{
		this.energy -= sub;
		if (this.energy < 0f)
		{
			this.energy = 0f;
		}
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x000308B4 File Offset: 0x0002EAB4
	public float GetEnergy()
	{
		return this.energy;
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x000308BC File Offset: 0x0002EABC
	public int GetFinishCounter()
	{
		return this.finishCounter;
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0015EBCC File Offset: 0x0015CDCC
	private void OnFixedUpdate()
	{
		if (this.fillBlock)
		{
			return;
		}
		int day = PlayerModel.instance.GetDay();
		float energyNeed = StageTypeInfo.instnace.GetEnergyNeed(day);
		float num = EnergyModel.instance.GetEnergy();
		if (num >= energyNeed / 2f && !this.halfSay)
		{
			this.halfSay = true;
			AngelaConversation.instance.MakeMessage(AngelaMessageState.ENERGY_HALF, new object[0]);
		}
		if (num >= energyNeed)
		{
			if (!this.fullSay)
			{
				this.fullSay = true;
				AngelaConversation.instance.MakeMessage(AngelaMessageState.ENERGY_FULL, new object[0]);
			}
			if (SnowQueen.IsFreezeEnergy)
			{
				return;
			}
			if (!SefiraBossManager.Instance.IsAnyBossSessionActivated() && !GameStatusUI.GameStatusUI.Window.sceneController.IsGameCleared && !GameManager.currentGameManager.StageEnded)
			{
				GameManager.currentGameManager.SuccessStage();
			}
		}
		else if (GameManager.currentGameManager.StageEnded)
		{
			GameManager.currentGameManager.RevertSuccess();
		}
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x0015ECC8 File Offset: 0x0015CEC8
	private void UpdateEnergy()
	{
		int day = PlayerModel.instance.GetDay();
		float energyNeed = StageTypeInfo.instnace.GetEnergyNeed(day);
		CreatureModel[] creatureList = CreatureManager.instance.GetCreatureList();
		float num = 0f;
		foreach (CreatureModel creatureModel in creatureList)
		{
			float num2 = 0f;
			GameObject gameObject = null;
			if (num2 > 0f)
			{
				if (num2 <= 5f)
				{
					gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyUp_1");
				}
				else if (num2 > 5f && num2 <= 10f)
				{
					gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyUp_2");
				}
				else if (num2 > 10f)
				{
					gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyUp_3");
				}
			}
			else if (num2 < 0f)
			{
				if (num2 >= -3f)
				{
					gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyDown_1");
				}
				else if (num2 < -3f && num2 >= -6f)
				{
					gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyDown_2");
				}
				else if (num2 < -6f)
				{
					gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyDown_3");
				}
			}
			num += num2;
			if (gameObject != null)
			{
				IsolateRoom room = CreatureLayer.currentLayer.GetCreature(creatureModel.instanceId).room;
				gameObject.transform.SetParent(room.transform);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
		if (num > 0f)
		{
			this.AddEnergy(num);
		}
		else if (num < 0f)
		{
			this.SubEnergy(-num);
		}
		if (this.energy > energyNeed)
		{
			this.energy = energyNeed;
		}
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0015EEA0 File Offset: 0x0015D0A0
	public void ManualAdd(CreatureModel creature, float value)
	{
		GameObject gameObject = null;
		if (value > 0f)
		{
			if (value <= 5f)
			{
				gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyUp_1");
			}
			else if (value > 5f && value <= 10f)
			{
				gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyUp_2");
			}
			else if (value > 10f)
			{
				gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyUp_3");
			}
		}
		else if (value < 0f)
		{
			if (value >= -3f)
			{
				gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyDown_1");
			}
			else if (value < -3f && value >= -6f)
			{
				gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyDown_2");
			}
			else if (value < -6f)
			{
				gameObject = Prefab.LoadPrefab("Effect/Isolate/EnergyDown_3");
			}
		}
		IsolateRoom room = CreatureLayer.currentLayer.GetCreature(creature.instanceId).room;
		gameObject.transform.SetParent(room.transform);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		if (value > 0f)
		{
			this.AddEnergy(value);
		}
		else if (value < 0f)
		{
			this.SubEnergy(-value);
		}
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x000308C4 File Offset: 0x0002EAC4
	public void OnNotice(string notice, params object[] param)
	{
		if (!(notice == NoticeName.EnergyTimer))
		{
			if (notice == NoticeName.FixedUpdate)
			{
				this.OnFixedUpdate();
			}
		}
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0000431D File Offset: 0x0000251D
	// Note: this type is marked as 'beforefieldinit'.
	static EnergyModel()
	{
	}

	// Token: 0x0400316B RID: 12651
	private static EnergyModel _instance;

	// Token: 0x0400316C RID: 12652
	private float energy;

	// Token: 0x0400316D RID: 12653
	private int finishCounter;

	// Token: 0x0400316E RID: 12654
	private float finishTimer;

	// Token: 0x0400316F RID: 12655
	private bool halfSay;

	// Token: 0x04003170 RID: 12656
	private bool fullSay;

	// Token: 0x04003171 RID: 12657
	private bool _fillBlock;
}
