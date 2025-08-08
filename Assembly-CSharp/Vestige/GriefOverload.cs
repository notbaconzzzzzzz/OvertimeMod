using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vestige
{
    public class GriefOverload : OvertimeOverload
    {
        public GriefOverload(CreatureModel _creature) : base(_creature)
		{
            
		}

        public override OverloadType Type
        {
            get
            {
                return OverloadType.GRIEF;
            }
        }

        public override void InitAfterWork()
		{
            base.InitAfterWork();
            creature.ProbReductionValue = 25;
            if (creature.metaInfo.creatureWorkType == CreatureWorkType.KIT) return;
            creature.Unit.room.ProgressBar.SetVisible(true);
            creature.Unit.room.ProgressBar.InitializeState();
            failCubes = 0;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (workStarted || !isInitialized) return;
			if (!creature.isOverloaded || creature.overloadType != OverloadType.GRIEF) return;
            if (creature.metaInfo.creatureWorkType == CreatureWorkType.KIT) return;
            int _failCubes = (int)(creature.overloadTimer / creature.currentOverloadMaxTime * 0.5f * creature.metaInfo.feelingStateCubeBounds.GetLastBound());
			if (_failCubes <= failCubes) return;
			failCubes++;
			creature.Unit.room.ProgressBar.AddBar(false);
        }

        public override void OnWorkStart()
		{
            base.OnWorkStart();
            creature.currentSkill.griefBoxes = failCubes;
        }

        private int failCubes;
    }
}