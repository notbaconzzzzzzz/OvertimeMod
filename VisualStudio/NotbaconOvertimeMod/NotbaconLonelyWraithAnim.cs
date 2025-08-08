using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconLonelyWraithAnim : CreatureAnimScript
    {
        public void SetScript(NotbaconLonelyWraith script)
        {
            animator = base.gameObject.GetComponent<SkeletonAnimation>();
            animator.AnimationState.SetAnimation(0, "Default", true);
        }

        public new SkeletonAnimation animator;
    }
}
