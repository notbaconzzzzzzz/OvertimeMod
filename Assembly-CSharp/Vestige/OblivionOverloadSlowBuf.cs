using System;

namespace Vestige
{
	public class OblivionOverloadSlowBuf : UnitBuf
	{
		public OblivionOverloadSlowBuf(CreatureModel _creature)
		{
			duplicateType = BufDuplicateType.ONLY_ONE;
			type = UnitBufType.OBLIVION_OVERLOAD_SLOW;
			creature = _creature;
		}

		public override void Init(UnitModel model)
		{
			base.Init(model);
			remainTime = float.PositiveInfinity;
		}

		public override float MovementScale()
		{
			return 0.6f;
		}

        public override void FixedUpdate()
        {
			if (!(model is AgentModel))
			{
				Destroy();
				return;
			}
			AgentModel agent = model as AgentModel;
            if (agent.GetCurrentCommand() == null ||
				!(agent.GetCurrentCommand() is ManageCreatureAgentCommand) ||
				(agent.GetCurrentCommand() as ManageCreatureAgentCommand).targetCreature != creature)
			{
				Destroy();
			}
        }

		private CreatureModel creature;
    }
}