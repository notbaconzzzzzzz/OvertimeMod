using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconAnglerFishAnim : CreatureAnimScript
    {
        public void SetScript(NotbaconAnglerFish script)
        {
            animator = base.gameObject.GetComponent<SkeletonAnimation>();
            animator.AnimationState.SetAnimation(0, "Default", true);
        }

        public new SkeletonAnimation animator;
    }
}
