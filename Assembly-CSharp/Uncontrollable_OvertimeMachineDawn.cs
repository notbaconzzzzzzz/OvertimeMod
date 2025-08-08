using System;
using System.Collections.Generic;

public class Uncontrollable_OvertimeMachineDawn : UncontrollableAction
{
	public Uncontrollable_OvertimeMachineDawn(WorkerModel _model)
	{
		model = _model;
		_panicked = model.IsPanic();
	}

	public override void Init()
	{
        model.LoseControl();
		model.StopAction();
		model.GetMovableNode().StopMoving();
		OvertimeMachineDawnUnconDebuf overtimeMachineDawnUnconDebuf = new OvertimeMachineDawnUnconDebuf();
		overtimeMachineDawnUnconDebuf._panicked = _panicked;
		model.AddUnitBuf(overtimeMachineDawnUnconDebuf);
		regainControlTimer.StartTimer(5f);
		if (model is AgentModel)
		{
			(model as AgentModel).FeignDeathScene(WorkerSpine.AnimatorName.MachineDawnAgentDead);
		}
		else if (model is OfficerModel)
		{
			(model as OfficerModel).FeignDeathScene(WorkerSpine.AnimatorName.MachineDawnAgentDead);
		}
	}

	public override void Execute()
	{
		if (regainControlTimer.RunTimer())
		{
			model.GetControl();
			return;
		}
	}

    public override void OnDestroy()
    {
		if (model is AgentModel)
		{
			(model as AgentModel).GetUnit().animChanger.ChangeAnimator();
		}
		else if (model is OfficerModel)
		{
			(model as OfficerModel).GetUnit().animChanger.ChangeAnimator();
		}
		if (_panicked || model.mental <= 0f)
		{
			model.Panic();
		}
    }

    private WorkerModel model;

	private Timer regainControlTimer = new Timer();

	private bool _panicked;

	public class OvertimeMachineDawnUnconDebuf : UnitBuf
	{
		public OvertimeMachineDawnUnconDebuf()
		{
			this.type = UnitBufType.OVERTIME_MACHINE_DAWN_UNCON;
			this.duplicateType = BufDuplicateType.ONLY_ONE;
		}

		public override void Init(UnitModel model)
		{
			base.Init(model);
			this.remainTime = 5f;
		}

		public override float OnTakeDamage(UnitModel attacker, DamageInfo damageInfo)
		{
			if (attacker == null) return 1f;
			MovableObjectNode self = model.GetMovableNode();
			MovableObjectNode other = attacker.GetMovableNode();
			if (self == null || other == null) return 1f;
			PassageObjectModel passage = self.GetPassage();
			if (passage != other.GetPassage() || passage == null) return 1f;
            float x = self.GetCurrentViewPosition().x;
            float x2 = other.GetCurrentViewPosition().x;
			float num = Math.Abs(x - x2) / self.currentScale;
			if (num <= 6.5f)
			{
				return 2f;
			}
			else if (num <= 15.5f)
			{
				return 1.5f;
			}
			return 1f;
		}

        public override void OwnerHeal(bool isMental, ref float amount)
        {
			if (isMental && _panicked)
			{
				amount = 0f;
			}
        }

		public bool _panicked;
    }
}
