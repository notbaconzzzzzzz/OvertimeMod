using System;
using UnityEngine;

public class StimBulletBuf : UnitBuf
{
	public StimBulletBuf(float time)
	{
		this.type = UnitBufType.STIM_BULLET;
		this.duplicateType = BufDuplicateType.ONLY_ONE;
		this.remainTime = time;
	}

	public override void Init(UnitModel model)
	{
		base.Init(model);
		//stimEffect = EffectInvoker.Invoker("SlowEffect", model.GetMovableNode(), this.remainTime, false);
		//stimEffect.Attach();
		//stimEffect.transform.localRotation *= Quaternion.AngleAxis(180, Vector3.forward);
	}

	public override float MovementScale()
	{
		return 1.5f;
	}

    public override float GetDamageFactor()
	{
		return 1.25f;
	}

    public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
	{
		return 1.5f;
	}

    public override void OnDestroy()
	{
		base.OnDestroy();
	}

	private EffectInvoker stimEffect;
}
