using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconGrandfatherClockAnim : CreatureAnimScript
    {
        public void SetScript(NotbaconGrandfatherClock script)
        {
            animator = base.gameObject.GetComponent<SkeletonAnimation>();
            animator.AnimationState.SetAnimation(0, "Default", true);
        }

        public new SkeletonAnimation animator;
    }
}
