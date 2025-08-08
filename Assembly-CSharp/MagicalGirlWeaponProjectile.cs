/*
private void CheckUnit(UnitModel unit) // 
+private int healed // 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054F RID: 1359
public class MagicalGirlWeaponProjectile : MonoBehaviour
{
	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06002FBD RID: 12221 RVA: 0x0002D085 File Offset: 0x0002B285
	// (set) Token: 0x06002FBE RID: 12222 RVA: 0x0002D08D File Offset: 0x0002B28D
	public MagicalGirlWeapon script { get; set; }

	// Token: 0x06002FBF RID: 12223 RVA: 0x00146004 File Offset: 0x00144204
	private void FixedUpdate()
	{
		this._effect.Translate(5f * this.dir * Time.fixedDeltaTime, 0f, 0f);
		if (this.dir > 0f)
		{
			if (this._effect.position.x > this.target.x)
			{
				this.Destroy();
			}
		}
		else if (this._effect.position.x < this.target.x)
		{
			this.Destroy();
		}
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x0002D096 File Offset: 0x0002B296
	private void Awake()
	{
		this.done = new List<UnitModel>();
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x001460A0 File Offset: 0x001442A0
	public void CollisionCheck(Collider2D collision)
	{
		if (!this.isEnabled)
		{
			return;
		}
		UnitMouseEventTarget component = collision.GetComponent<UnitMouseEventTarget>();
		if (component == null)
		{
			return;
		}
		MonoBehaviour monoBehaviour = component.target;
		if (monoBehaviour == null)
		{
			return;
		}
		if (monoBehaviour.GetComponent<OfficerUnit>() != null)
		{
			OfficerModel model = monoBehaviour.GetComponent<OfficerUnit>().model;
			if (model.IsDead())
			{
				return;
			}
			this.CheckUnit(model);
		}
		else if (monoBehaviour.GetComponent<AgentUnit>() != null)
		{
			AgentModel model2 = monoBehaviour.GetComponent<AgentUnit>().model;
			if (model2.IsDead())
			{
				return;
			}
			this.CheckUnit(model2);
		}
		else if (monoBehaviour.GetComponent<CreatureUnit>() != null)
		{
			CreatureModel model3 = monoBehaviour.GetComponent<CreatureUnit>().model;
			if (model3.hp <= 0f)
			{
				return;
			}
			this.CheckUnit(model3);
		}
		else if (monoBehaviour.GetComponent<ChildCreatureUnit>() != null)
		{
			ChildCreatureModel model4 = monoBehaviour.GetComponent<ChildCreatureUnit>().Model;
			if (model4.hp <= 0f)
			{
				return;
			}
			this.CheckUnit(model4);
		}
		else if (monoBehaviour.GetComponent<RabbitUnit>() != null)
		{
			RabbitModel model5 = monoBehaviour.GetComponent<RabbitUnit>().model;
			if (model5.IsAttackTargetable())
			{
				this.CheckUnit(model5);
			}
		}
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x001461F8 File Offset: 0x001443F8
	private void CheckUnit(UnitModel unit)
	{ // <Mod>
		if (this.done.Contains(unit))
		{
			return;
		}
		this.done.Add(unit);
		if (this.script.CheckHit(unit, this.type, ref healed))
		{
			GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/MagicalGirl/MagicalGirlDamageEffect_Hero");
			if (gameObject == null)
			{
				return;
			}
			gameObject.transform.position = unit.GetCurrentViewPosition();
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			this.Destroy();
		}
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x0002D0A3 File Offset: 0x0002B2A3
	public void Destroy()
	{
		this.isEnabled = false;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002D22 RID: 11554
	private const float _effectSpeed = 5f;

	// Token: 0x04002D23 RID: 11555
	private const string _dmgEffectSrc = "Effect/Creature/MagicalGirl/MagicalGirlDamageEffect_Hero";

	// Token: 0x04002D25 RID: 11557
	private List<UnitModel> done = new List<UnitModel>();

	// Token: 0x04002D26 RID: 11558
	public Transform _effect;

	// Token: 0x04002D27 RID: 11559
	public RwbpType type;

	// Token: 0x04002D28 RID: 11560
	public Vector3 target;

	// Token: 0x04002D29 RID: 11561
	public float dir;

	// Token: 0x04002D2A RID: 11562
	public bool isEnabled;

    // <Mod>
    private int healed = 0;
}
