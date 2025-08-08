/*
private void CheckCollision(Collider2D collision) // Does not hit Agents that are working
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
[RequireComponent(typeof(BoxCollider2D))]
public class MachineMidnightLaser : MonoBehaviour
{
	// Token: 0x06002D44 RID: 11588 RVA: 0x0002B852 File Offset: 0x00029A52
	public MachineMidnightLaser()
	{
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06002D45 RID: 11589 RVA: 0x0002B870 File Offset: 0x00029A70
	// (set) Token: 0x06002D46 RID: 11590 RVA: 0x0002B878 File Offset: 0x00029A78
	public MachineMidnight machine { get; set; }

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06002D47 RID: 11591 RVA: 0x0002B881 File Offset: 0x00029A81
	public CreatureModel Model
	{
		get
		{
			return this.machine.model;
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06002D48 RID: 11592 RVA: 0x0002B88E File Offset: 0x00029A8E
	private DamageInfo damageInfo
	{
		get
		{
			return MachineMidnight.DamageInfo;
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06002D49 RID: 11593 RVA: 0x0002B895 File Offset: 0x00029A95
	private RwbpType damageType
	{
		get
		{
			return this.damageInfo.type;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06002D4A RID: 11594 RVA: 0x0002B8A2 File Offset: 0x00029AA2
	private BoxCollider2D Collider
	{
		get
		{
			return base.gameObject.GetComponent<BoxCollider2D>();
		}
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x0002B8AF File Offset: 0x00029AAF
	private void Awake()
	{
		this.Collider.enabled = false;
		this.effect.SetActive(false);
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x001308C0 File Offset: 0x0012EAC0
	private void CheckCollision(Collider2D collision)
	{ // <Mod> Does not hit Agents that are working
		if (collision.GetComponent<UnitMouseEventTarget>() == null)
		{
			return;
		}
		MonoBehaviour target = collision.GetComponent<UnitMouseEventTarget>().target;
		if (target == null)
		{
			return;
		}
		if (target.GetComponent<OfficerUnit>() != null)
		{
			OfficerModel model = target.GetComponent<OfficerUnit>().model;
			if (model.IsDead())
			{
				return;
			}
			if (!model.IsAttackTargetable())
			{
				return;
			}
			this.GiveDamage(model);
			if (model.IsDead())
			{
				this.OnWorkerKill(model);
			}
		}
		else if (target.GetComponent<AgentUnit>() != null)
		{
			AgentModel model2 = target.GetComponent<AgentUnit>().model;
			if (model2.IsDead())
			{
				return;
			}
			if (!model2.IsAttackTargetable())
			{
				return;
			}
			if (model2.currentSkill != null)
			{
				return;
			}
			this.GiveDamage(model2);
			if (model2.IsDead())
			{
				this.OnWorkerKill(model2);
			}
		}
		else if (target.GetComponent<CreatureUnit>() != null)
		{
			CreatureModel model3 = target.GetComponent<CreatureUnit>().model;
			if (model3.hp <= 0f)
			{
				return;
			}
			if (model3.script is MachineMidnight)
			{
				return;
			}
			if (!model3.IsAttackTargetable())
			{
				return;
			}
			this.GiveDamage(model3);
		}
		else if (target.GetComponent<ChildCreatureUnit>() != null)
		{
			ChildCreatureModel model4 = target.GetComponent<ChildCreatureUnit>().Model;
			if (model4.hp <= 0f)
			{
				return;
			}
			if (!model4.IsAttackTargetable())
			{
				return;
			}
			this.GiveDamage(model4);
		}
		else if (target.GetComponent<RabbitUnit>() != null)
		{
			RabbitModel model5 = target.GetComponent<RabbitUnit>().model;
			if (model5.IsAttackTargetable())
			{
				this.GiveDamage(model5);
			}
		}
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x00130A74 File Offset: 0x0012EC74
	private void OnWorkerKill(WorkerModel worker)
	{
		Vector3 currentViewPosition = worker.GetCurrentViewPosition();
		float currentScale = worker.GetMovableNode().currentScale;
		UnitDirection unitDirection = UnitDirection.RIGHT;
		if (this.Model.GetCurrentViewPosition().x > currentViewPosition.x)
		{
			unitDirection = UnitDirection.LEFT;
		}
		Vector3 euler = new Vector3((unitDirection != UnitDirection.RIGHT) ? -45f : -135f, -90f, 90f);
		GameObject gameObject = Prefab.LoadPrefab("Effect/Ordeal/Machine/MidnightDead");
		gameObject.name = string.Format("collapse of {0}", worker.GetUnitName());
		gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
		gameObject.transform.position = currentViewPosition;
		gameObject.transform.localScale = new Vector3(currentScale, currentScale, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = this.machine.AnimScript.GetRandomDeadSprite();
		GameObject gameObject2 = Prefab.LoadPrefab("Effect/Ordeal/Machine/MachineMidnightWorkerDead");
		gameObject2.transform.position = currentViewPosition;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.Euler(euler);
		worker.GetWorkerUnit().gameObject.SetActive(false);
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x0002B8C9 File Offset: 0x00029AC9
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!this._isEnabled)
		{
			return;
		}
		this.CheckCollision(collision);
	}

	// Token: 0x06002D4F RID: 11599 RVA: 0x0002B8C9 File Offset: 0x00029AC9
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!this._isEnabled)
		{
			return;
		}
		this.CheckCollision(collision);
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x00130BC4 File Offset: 0x0012EDC4
	private void AttackDamageBuf(UnitModel target)
	{
		if (target.HasUnitBuf(UnitBufType.MIDNIGHT_MOVEMENT))
		{
			MidnightLaserDebuf midnightLaserDebuf = target.GetUnitBufByType(UnitBufType.MIDNIGHT_MOVEMENT) as MidnightLaserDebuf;
			if (midnightLaserDebuf != null)
			{
				midnightLaserDebuf.remainTime = 0.5f;
			}
		}
		else
		{
			MidnightLaserDebuf buf = new MidnightLaserDebuf();
			target.AddUnitBuf(buf);
		}
	}

	// Token: 0x06002D51 RID: 11601 RVA: 0x00130C10 File Offset: 0x0012EE10
	private void GiveDamage(UnitModel unit)
	{
		if (this.CheckDamageTarget(unit))
		{
			return;
		}
		MachineMidnightLaser.DamageUnit item = new MachineMidnightLaser.DamageUnit
		{
			unit = unit,
			remain = this._damageDelay
		};
		this.AttackDamageBuf(unit);
		this.list.Add(item);
		unit.TakeDamage(this.Model, this.damageInfo);
		this.MakeDamageEffect(unit);
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x00130C74 File Offset: 0x0012EE74
	private bool CheckDamageTarget(UnitModel unit)
	{
		bool result = false;
		foreach (MachineMidnightLaser.DamageUnit damageUnit in this.list)
		{
			if (damageUnit.unit == unit)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x00130CE0 File Offset: 0x0012EEE0
	private void MakeDamageEffect(UnitModel unit)
	{
		UnitDirection dir = UnitDirection.RIGHT;
		float x = this.Model.GetCurrentViewPosition().x;
		float x2 = unit.GetCurrentViewPosition().x;
		if (x >= x2)
		{
			dir = UnitDirection.LEFT;
		}
		DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(unit, this.damageType, unit.defense, dir);
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x0002B8DE File Offset: 0x00029ADE
	public void SetDamageActivation(bool b)
	{
		this._isEnabled = b;
		this.Collider.enabled = b;
		this.effect.SetActive(b);
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x00130D34 File Offset: 0x0012EF34
	private void FixedUpdate()
	{
		float deltaTime = Time.deltaTime;
		List<MachineMidnightLaser.DamageUnit> list = new List<MachineMidnightLaser.DamageUnit>();
		foreach (MachineMidnightLaser.DamageUnit damageUnit in this.list)
		{
			damageUnit.remain -= deltaTime;
			if (damageUnit.expired)
			{
				list.Add(damageUnit);
			}
		}
		foreach (MachineMidnightLaser.DamageUnit item in list)
		{
			this.list.Remove(item);
		}
	}

	// Token: 0x04002AE3 RID: 10979
	public float _damageDelay = 0.2f;

	// Token: 0x04002AE4 RID: 10980
	private List<MachineMidnightLaser.DamageUnit> list = new List<MachineMidnightLaser.DamageUnit>();

	// Token: 0x04002AE6 RID: 10982
	public GameObject effect;

	// Token: 0x04002AE7 RID: 10983
	private bool _isEnabled;

	// Token: 0x020004E8 RID: 1256
	private class DamageUnit
	{
		// Token: 0x06002D56 RID: 11606 RVA: 0x0002B8FF File Offset: 0x00029AFF
		public DamageUnit()
		{
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06002D57 RID: 11607 RVA: 0x0002B912 File Offset: 0x00029B12
		public bool expired
		{
			get
			{
				return this.remain <= 0f;
			}
		}

		// Token: 0x04002AE8 RID: 10984
		public UnitModel unit;

		// Token: 0x04002AE9 RID: 10985
		public float remain = 1f;
	}
}
