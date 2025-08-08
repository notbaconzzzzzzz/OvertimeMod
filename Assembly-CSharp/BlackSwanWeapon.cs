using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class BlackSwanWeapon : EquipmentScriptBase
{
	// Token: 0x060035D1 RID: 13777 RVA: 0x00030D14 File Offset: 0x0002EF14
	public BlackSwanWeapon()
	{
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x0015FF9C File Offset: 0x0015E19C
	public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
	{
		List<DamageInfo> list = new List<DamageInfo>();
		string animationName = string.Empty;
		list.Add(base.model.metaInfo.damageInfos[0].Copy());
		list.Add(base.model.metaInfo.damageInfos[1].Copy());
		animationName = base.model.metaInfo.animationNames[0];
		return new EquipmentScriptBase.WeaponDamageInfo(animationName, list.ToArray());
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x00160010 File Offset: 0x0015E210
	public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
	{
		WorkerModel workerModel = base.model.owner as WorkerModel;
		UnitDirection direction = workerModel.GetMovableNode().GetDirection();
		bool flag = true;
		if (actor == null)
		{
			return base.OnTakeDamage(actor, ref dmg);
		}
		if ((workerModel as AgentModel).currentSkill != null)
		{
			Debug.Log("working");
			return base.OnTakeDamage(actor, ref dmg);
		}
		if (direction == UnitDirection.LEFT)
		{
			if (actor.GetCurrentViewPosition().x > workerModel.GetCurrentViewPosition().x)
			{
				flag = false;
			}
		}
		else if (actor.GetCurrentViewPosition().x < workerModel.GetCurrentViewPosition().x)
		{
			flag = false;
		}
		if (flag && UnityEngine.Random.value < 0.1f)
		{
			actor.TakeDamage(base.model.owner, dmg);
			this.MakeReflectEffect(direction);
			dmg.min = 0f;
			dmg.max = 0f;
		}
		return base.OnTakeDamage(actor, ref dmg);
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x00160114 File Offset: 0x0015E314
	private void MakeReflectEffect(UnitDirection dir)
	{
		GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/BlackSwan/BlackSwanReflectEffect");
		WorkerModel workerModel = base.model.owner as WorkerModel;
		PassageObjectModel currentPassage = workerModel.GetMovableNode().currentPassage;
		Vector3 a = this.fixedPosition;
		SoundEffectPlayer.PlayOnce("creature/BlackSwan/Sis_Reflect", workerModel.GetCurrentViewPosition());
		float num = 1f;
		float num2 = 1f;
		if (dir == UnitDirection.LEFT)
		{
			num2 = -1f;
		}
		if (currentPassage != null)
		{
			num = currentPassage.scaleFactor;
		}
		a.x *= num2;
		gameObject.transform.position = workerModel.GetCurrentViewPosition() + a * num;
		gameObject.transform.localScale = new Vector3(num * num2, num, num) * 1.5f;
	}

	// Token: 0x040031D2 RID: 12754
	private const float _reflectProb = 0.1f;

	// Token: 0x040031D3 RID: 12755
	private const float _effectSize = 1.5f;

	// Token: 0x040031D4 RID: 12756
	private readonly Vector3 fixedPosition = new Vector3(1f, 1f, 0f);
}
