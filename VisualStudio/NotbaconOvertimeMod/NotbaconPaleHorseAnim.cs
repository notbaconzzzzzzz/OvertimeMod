using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconPaleHorseAnim : CreatureAnimScript
    {
        public void SetScript(NotbaconPaleHorse script)
        {
            animator = base.gameObject.GetComponent<SkeletonAnimation>();
        }

        public void IdleAnim()
        {
            animator.AnimationState.SetAnimation(0, "Idle", true);
        }

        public new SkeletonAnimation animator;
    }
}
