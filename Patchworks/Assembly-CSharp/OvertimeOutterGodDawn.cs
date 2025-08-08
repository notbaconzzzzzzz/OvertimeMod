using System;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeOutterGodDawn : OutterGodDawn
{
	private static float boomTime
	{
		get
		{
			return UnityEngine.Random.Range(60f, 70f);
		}
	}

	private static int attackDmg
	{
		get
		{
			return UnityEngine.Random.Range(4, 9);
		}
	}

	private static int boomDmg
	{
		get
		{
			return UnityEngine.Random.Range(35, 46);
		}
	}

	public override void OnViewInit(CreatureUnit unit)
	{
		base.OnViewInit(unit);
		boomTimer.StartTimer(boomTime);
		_paraBoomCount = 0;
	}

	public override void OnAttackDamageTimeCalled()
	{
		List<WorkerModel> targets = GetTargets(3f);
		foreach (WorkerModel workerModel in targets)
		{
			workerModel.TakeDamage(model, new DamageInfo(RwbpType.B, (float)attackDmg));
			DamageParticleEffect damageParticleEffect = DamageParticleEffect.Invoker(workerModel, RwbpType.B, model);
			workerModel.AddUnitBuf(new OvertimeOutterGodDawnAnitrecovery());
		}
	}

	public override void OnBoomEnd()
	{
		Sefira sefira = model.sefira;
		List<WorkerModel> list = new List<WorkerModel>();
		List<CreatureModel> list2 = new List<CreatureModel>(sefira.creatureList);
		foreach (AgentModel item in AgentManager.instance.GetAgentList())
		{
			list.Add(item);
		}
		foreach (OfficerModel item2 in sefira.officerList)
		{
			list.Add(item2);
		}
		foreach (WorkerModel workerModel in list)
		{
			if (!workerModel.IsDead())
			{
				workerModel.TakeDamage(this.model, new DamageInfo(RwbpType.W, (float)boomDmg));
			}
		}
		foreach (CreatureModel creatureModel in list2)
		{
			if (!creatureModel.IsEscaped())
			{
				creatureModel.SetQliphothCounter(0);
			}
		}
		foreach (Sefira sef in SefiraManager.instance.GetOpendSefiraList())
		{
			if (sef == sefira) continue;
			foreach (CreatureModel creatureModel in sef.creatureList)
			{
				if (!creatureModel.IsEscaped())
				{
					creatureModel.SubQliphothCounter();
				}
			}
		}
	}

    public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
    {
		base.OnTakeDamage(actor, dmg, value);
		if (this.model.hp <= 0f) return;
		if (model.hp / model.maxHp <= 2f/3f - _paraBoomCount/3f && _paraBoomCount < 2)
		{
			_paraBoomCount++;
			_paraBoomTimer.StartTimer(2.5f);
			OnBoom();
			boomTimer.StartTimer(boomTime);
		}
		if (_iterationGuard) return;
		_iterationGuard = true;
		DamageInfo dmg2 = dmg.Copy();
		dmg2.min *= 0.5f;
		dmg2.max *= 0.5f;
		actor.TakeDamage(model, dmg2);
		_iterationGuard = false;
		if (_reflectSoundTimer.started) return;
		_reflectSoundTimer.StartTimer(0.2f);
		SoundEffectPlayer.PlayOnce("creature/FixerWhite/Liq_White_reflect", animScript.gameObject.transform.position, 0.25f);
    }

	public override void OnFixedUpdate(CreatureModel creature)
	{
		base.OnFixedUpdate(creature);
		if (_paraBoomTimer.started && _paraBoomTimer.RunTimer())
		{
			List<WorkerModel> list = new List<WorkerModel>();
			Vector2 a = creature.GetCurrentViewPosition();
			foreach (WorkerModel worker in WorkerManager.instance.GetWorkerList())
			{
				if (worker.IsDead() || !worker.IsAttackTargetable()) continue;
				Vector2 b = worker.GetCurrentViewPosition();
				Vector2 c = (a - b);
				float magnitude = c.magnitude;
				if (magnitude < 16f)
				{
					list.Add(worker);
				}
			}
			foreach (WorkerModel worker in list)
			{
				int num = boomDmg;
				num += _paraBoomCount * 10 - 30;
				float num2 = worker.mental / (float)worker.maxMental;
				worker.TakeDamage(creature, new DamageInfo(RwbpType.W, num * num2));
				if (worker is AgentModel)
				{
					(worker as AgentModel).cannotAttackUnits.Add(creature);
					worker.StopAction();
				}
			}
		}
		if (_reflectSoundTimer.started && _reflectSoundTimer.RunTimer())
		{

		}
	}

    private bool _iterationGuard = false;

	private Timer _paraBoomTimer = new Timer();

	private int _paraBoomCount;

	private Timer _reflectSoundTimer = new Timer();
}