using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vestige
{
    public class RuinOverload : OvertimeOverload
    {
        public RuinOverload(CreatureModel _creature) : base(_creature)
		{
            
		}

        public override OverloadType Type
        {
            get
            {
                return OverloadType.RUIN;
            }
        }

        public override void InitAfterWork()
		{
            base.InitAfterWork();
            creature.ProbReductionValue = 15;
			if (creature.overloadReduction >= 55)
			{
				nextReduction = float.PositiveInfinity;
			}
			else if (creature.overloadReduction >= 30)
			{
				nextReduction = 2f;
			}
			else
			{
				nextReduction = 1f;
			}
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (workStarted || !isInitialized) return;
			if (!creature.isOverloaded || creature.overloadType != OverloadType.RUIN) return;
			if (creature.overloadTimer < nextReduction) return;
			creature.overloadReduction += 1;
			if (creature.overloadReduction >= 55)
			{
				nextReduction = float.PositiveInfinity;
			}
			else if (creature.overloadReduction >= 30)
			{
				nextReduction += 2f;
			}
			else
			{
				nextReduction += 1f;
			}
        }

        public override void OnFailOverload()
        {
            base.OnFailOverload();
			if (creature.currentOverloadMaxTime < nextReduction) return;
			creature.overloadReduction += 1;
        }

        private float nextReduction;
    }
}