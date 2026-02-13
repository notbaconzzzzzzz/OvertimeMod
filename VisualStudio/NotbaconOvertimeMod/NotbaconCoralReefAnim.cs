using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using static NotbaconOvertimeMod.NotbaconCoralReef;

namespace NotbaconOvertimeMod
{
    public class NotbaconCoralReefAnim : CreatureAnimScript
    {
        public void SetScript(NotbaconCoralReef script)
        {
            this.script = script;
            animator = base.gameObject.GetComponent<SkeletonAnimation>();
            animator.AnimationState.Complete += this.CompleteEvent;
            animator.AnimationState.Event += this.EventHandler;
            animator.AnimationState.SetAnimation(0, "Default", false);
        }

        public void Breach()
        {
            animator.AnimationState.SetAnimation(0, "Breach", false);
        }

        public void Return()
        {
            animator.AnimationState.SetAnimation(0, "Default", false);
        }

        public void Move(bool backwards)
        {
            animator.AnimationState.SetAnimation(0, backwards ? "WalkBack" : "Walk", true);
        }

        public void Idle()
        {
            animator.AnimationState.SetAnimation(0, "Idle", false);
        }

        public void InitiateAttack(CoralReefState type)
        {
            switch (type)
            {
                case CoralReefState.HIDING:
                    animator.AnimationState.SetAnimation(0, "HideStart", false);
                    break;
                case CoralReefState.CLAW_ATTACK_1:
                    animator.AnimationState.SetAnimation(0, "ClawAttack1", false);
                    break;
                case CoralReefState.CLAW_ATTACK_2:
                    animator.AnimationState.SetAnimation(0, "ClawAttack2", false);
                    break;
                case CoralReefState.SLAM_ATTACK:
                    animator.AnimationState.SetAnimation(0, "SlamAttack", false);
                    break;
                case CoralReefState.SCREAM:
                    animator.AnimationState.SetAnimation(0, "ScreamStart", false);
                    break;
                case CoralReefState.EAT_ATTACK:
                    animator.AnimationState.SetAnimation(0, "EatStart", false);
                    break;
            }
        }

        public void FinishAttack(CoralReefState type)
        {
            switch (type)
            {
                case CoralReefState.HIDING:
                    animator.AnimationState.SetAnimation(0, "HideEnd", false);
                    break;
                case CoralReefState.SCREAM:
                    animator.AnimationState.SetAnimation(0, "ScreamEnd", false);
                    break;
                case CoralReefState.EAT_ATTACK:
                    animator.AnimationState.SetAnimation(0, "EatEnd", false);
                    break;
            }
        }

        public void EventHandler(TrackEntry entry, Spine.Event eventData)
        {
            string name = eventData.Data.Name;
            if (name == null) return;
            if (name == "Damage")
            {
                script.GiveAttackDamage();
            }
        }

        public void CompleteEvent(TrackEntry entry)
        {
            string name = entry.Animation.Name;
            if (name == null) return;
            switch (name)
            {
                case "Breach":
                    script.OnBreach();
                    break;
                case "HideEnd":
                case "ClawAttack1":
                case "ClawAttack2":
                case "SlamAttack":
                case "ScreamEnd":
                case "EatEnd":
                    script.OnAttackEnd();
                    break;
                case "HideStart":
                    animator.AnimationState.SetAnimation(0, "Hiding", false);
                    script.OnHide();
                    break;
                case "ScreamStart":
                    animator.AnimationState.SetAnimation(0, "ScreamLoop", true);
                    script.OnScream();
                    break;
                case "EatStart":
                    animator.AnimationState.SetAnimation(0, "EatJump", false);
                    script.OnJump();
                    break;
                case "EatJump":
                    animator.AnimationState.SetAnimation(0, "EatEnd", false);
                    break;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /*
        public void Test()
        {
            Material mat = null;
            Slot mainBody = animator.Skeleton.FindSlot("CoralReef_MainBody");
            animator.CustomSlotMaterials[mainBody] = mat;
        }*/

        public new SkeletonAnimation animator;

        public NotbaconCoralReef script;
    }
}
