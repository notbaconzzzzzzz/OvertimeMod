/*
private void Init() // 
private void MakeDamage(List<UnitModel> target) // 
public override float GetDamageFactor() // 
+private bool _setOption // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class BossBirdArmor : EquipmentScriptBase
{
	// Token: 0x06003547 RID: 13639 RVA: 0x00030BBC File Offset: 0x0002EDBC
	public BossBirdArmor()
	{
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x00160130 File Offset: 0x0015E330
	private void LoadEffect()
	{
		this._footEffect = Prefab.LoadPrefab("Effect/Creature/BossBird/BossBirdArmorEffect");
		this._footEffect.transform.SetParent(this._worker.GetWorkerUnit().transform);
		this._footEffect.transform.localPosition = new Vector3(0f, -0.15f, -0.8f);
		this._footEffect.transform.localScale = Vector3.one;
		this._footEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x001601C0 File Offset: 0x0015E3C0
	private void ClearEffect()
	{
		try
		{
			if (this._footEffect.gameObject != null)
			{
				UnityEngine.Object.Destroy(this._footEffect.gameObject);
				this._footEffect = null;
			}
		}
		catch (Exception ex)
		{
			this._footEffect = null;
		}
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x00030BE0 File Offset: 0x0002EDE0
	public override void OnStageStart()
	{
		base.OnStageStart();
		this.Init();
		this.LoadEffect();
		this.SetActiveEffect(false);
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x00030BFB File Offset: 0x0002EDFB
	public override void OnStageRelease()
	{
		base.OnStageRelease();
		this.ClearEffect();
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x00030C09 File Offset: 0x0002EE09
	private void SetActiveEffect(bool state)
	{
		this._footEffect.SetActive(state);
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x0016021C File Offset: 0x0015E41C
	private void Init()
	{ // <Mod>
		this._worker = (base.model.owner as WorkerModel);
		this._setOption = false;
		if (this._worker.HasEquipment(200038) && this._worker.HasEquipment(400038))
		{
			this._setOption = true;
		}
		_setOption2 = false;
		if (_worker.HasEquipment(400038))
		{
			_setOption2 = true;
		}
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x00160278 File Offset: 0x0015E478
	public override void OnFixedUpdate()
	{
		List<UnitModel> nearHostileUnit = this.GetNearHostileUnit();
		bool flag = false;
		if (nearHostileUnit.Count != 0)
		{
			flag = true;
		}
		if (flag)
		{
			if (!this._footEffect.activeInHierarchy)
			{
				this.SetActiveEffect(true);
			}
		}
		else if (this._footEffect.activeInHierarchy)
		{
			this.SetActiveEffect(false);
		}
		if (this._nearDamageTimer.started)
		{
			if (this._nearDamageTimer.RunTimer() && flag)
			{
				this.MakeDamage(nearHostileUnit);
			}
		}
		else if (flag)
		{
			this.MakeDamage(nearHostileUnit);
		}
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x00160314 File Offset: 0x0015E514
	private void MakeDamageEffect(UnitModel target)
	{
		GameObject gameObject;
		if (this._setOption)
		{
			gameObject = Prefab.LoadPrefab("Effect/Creature/BossBird/BossBirdArmorHit_Set");
		}
		else
		{
			gameObject = Prefab.LoadPrefab("Effect/Creature/BossBird/BossBirdArmorHit");
		}
		Vector3 currentViewPosition = target.GetCurrentViewPosition();
		float currentScale = target.GetMovableNode().currentScale;
		currentViewPosition.y += 2f * currentScale;
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.localPosition = currentViewPosition;
		gameObject.transform.localScale = Vector3.one * currentScale;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x001603C0 File Offset: 0x0015E5C0
	private void MakeDamage(List<UnitModel> target)
	{ // <Mod>
		foreach (UnitModel unitModel in target)
		{
			if (this._setOption)
			{
				foreach (DamageInfo dmg in BossBirdArmor._setDamage)
				{
					unitModel.TakeDamage(dmg * GetDamageFactor());
				}
			}
			else
			{
				unitModel.TakeDamage(this._nearDamage  * GetDamageFactor());
			}
			this.MakeDamageEffect(unitModel);
		}
		this._nearDamageTimer.StartTimer(5f);
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x00160470 File Offset: 0x0015E670
	private List<UnitModel> GetNearHostileUnit()
	{
		List<UnitModel> list = new List<UnitModel>();
		if (this._worker.GetMovableNode().currentPassage == null)
		{
			return list;
		}
		MovableObjectNode[] enteredTargets = this._worker.GetMovableNode().currentPassage.GetEnteredTargets(this._worker.GetMovableNode());
		foreach (MovableObjectNode movableObjectNode in enteredTargets)
		{
			UnitModel unit = movableObjectNode.GetUnit();
			if (!(unit is WorkerModel))
			{
				if (unit.IsAttackTargetable())
				{
					if (this._worker.IsHostile(unit))
					{
						if (!list.Contains(unit))
						{
							list.Add(unit);
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x00160538 File Offset: 0x0015E738
	public override float GetDamageFactor()
	{ // <Mod>
		float result = 1f;
		if (_setOption2)
		{
            float factor = _worker.hp / _worker.maxHp * 3f;
            if (factor <= 2f)
            {
                if (factor < 0f)
                {
                    factor = 0f;
                }
                result = 6f / (factor + 3f);
            }
		}
		return result;
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x0016059C File Offset: 0x0015E79C
	// Note: this type is marked as 'beforefieldinit'.
	static BossBirdArmor()
	{
	}

	// Token: 0x04003190 RID: 12688
	private const string _footEffectSrc = "Effect/Creature/BossBird/BossBirdArmorEffect";

	// Token: 0x04003191 RID: 12689
	private const string _damageEffect = "Effect/Creature/BossBird/BossBirdArmorHit";

	// Token: 0x04003192 RID: 12690
	private const string _damageEffect_set = "Effect/Creature/BossBird/BossBirdArmorHit_Set";

	// Token: 0x04003193 RID: 12691
	private const float _nearDamageFreq = 5f;

	// Token: 0x04003194 RID: 12692
	private GameObject _footEffect;

	// Token: 0x04003195 RID: 12693
	private WorkerModel _worker;

	// Token: 0x04003196 RID: 12694
	private DamageInfo _nearDamage = new DamageInfo(RwbpType.B, 5f);

	// Token: 0x04003197 RID: 12695
	private static DamageInfo[] _setDamage = new DamageInfo[]
	{
		new DamageInfo(RwbpType.R, 5f),
		new DamageInfo(RwbpType.W, 5f),
		new DamageInfo(RwbpType.B, 5f),
		new DamageInfo(RwbpType.P, 5f)
	};

	// Token: 0x04003198 RID: 12696
	private Timer _nearDamageTimer = new Timer();

	// Token: 0x04003199 RID: 12697
	private bool _setOption;

    // <Mod>
	private bool _setOption2;
}
