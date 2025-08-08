using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using static ChopLegAnim;
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
            //  animator.AnimationState.Start += this.StartEvent;
            // animator.AnimationState.ClearTracks();
            animator.AnimationState.SetAnimation(0, "Default", false);
        }

        public void Breach()
        {
            // animator.AnimationState.ClearTracks();
            animator.AnimationState.SetAnimation(0, "Breach", false);
        }

        public void Return()
        {
            // animator.AnimationState.ClearTracks();
            animator.AnimationState.SetAnimation(0, "Default", false);
        }

        public void Move(bool backwards)
        {
            // animator.AnimationState.ClearTracks();
            animator.AnimationState.SetAnimation(0, backwards ? "WalkBack" : "Walk", true);
        }

        public void Idle()
        {
            // animator.AnimationState.ClearTracks();
            animator.AnimationState.SetAnimation(0, "Idle", false);
        }

        public void InitiateAttack(CoralReefState type)
        {
            switch (type)
            {
                case CoralReefState.HIDING:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "HideStart", false);
                    break;
                case CoralReefState.CLAW_ATTACK_1:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "ClawAttack1", false);
                    break;
                case CoralReefState.CLAW_ATTACK_2:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "ClawAttack2", false);
                    break;
                case CoralReefState.SLAM_ATTACK:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "SlamAttack", false);
                    break;
                case CoralReefState.SCREAM:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "ScreamStart", false);
                    break;
                case CoralReefState.EAT_ATTACK:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "EatStart", false);
                    break;
            }
        }

        public void FinishAttack(CoralReefState type)
        {
            switch (type)
            {
                case CoralReefState.HIDING:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "HideEnd", false);
                    break;
                case CoralReefState.SCREAM:
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "ScreamEnd", false);
                    break;
                case CoralReefState.EAT_ATTACK:
                    // animator.AnimationState.ClearTracks();
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
            /*
            float currentScale = this.script.movable.currentScale;
            Vector3 position = this.center.transform.position;
            Vector3 b = new Vector3(2.6f, 2.5f, 0f) * currentScale;
            Vector3 localPosition = new Vector3(-0.2f, 9.6f, 0f) * currentScale;
            if (this.script.movable.GetDirection() == UnitDirection.LEFT)
            {
                b.x *= -1f;
            }
            Vector3 position2 = position + b;
            Debug.Log(name);
            if (name != null)
            {
                if (!(name == "Create"))
                {
                    if (!(name == "Damage"))
                    {
                        if (!(name == "Digin"))
                        {
                            if (name == "Digout")
                            {
                                GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/BugDusk/GroundExplode_Middle");
                                gameObject.transform.position = position2;
                                gameObject.transform.localScale = Vector3.one;
                                gameObject.transform.localRotation = Quaternion.identity;
                            }
                        }
                        else
                        {
                            GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/BugDusk/GroundExplode_Middle");
                            gameObject.transform.position = position2;
                            gameObject.transform.localScale = Vector3.one;
                            gameObject.transform.localRotation = Quaternion.identity;
                        }
                    }
                    else
                    {
                        this.script.GiveAttackDamage();
                        string str = string.Empty;
                        if (this.script.movable.GetDirection() == UnitDirection.RIGHT)
                        {
                            str = "right";
                        }
                        else
                        {
                            str = "left";
                        }
                        GameObject gameObject2 = Prefab.LoadPrefab("Effect/Creature/BugDusk/bloodEffect_" + str);
                        gameObject2.transform.position = position2;
                        gameObject2.transform.localScale = Vector3.one;
                        gameObject2.transform.localRotation = Quaternion.identity;
                    }
                }
                else
                {
                    GameObject gameObject3 = Prefab.LoadPrefab("Effect/Creature/BugDusk/SpawnEffect");
                    gameObject3.transform.SetParent(this.center.transform);
                    gameObject3.transform.localPosition = localPosition;
                    gameObject3.transform.localScale = Vector3.one;
                    gameObject3.transform.localRotation = Quaternion.identity;
                    this.script.CreateDaughter(this._spawnCounter++);
                }
            }*/
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
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "Hiding", false);
                    script.OnHide();
                    break;
                case "ScreamStart":
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "ScreamLoop", true);
                    script.OnScream();
                    break;
                case "EatStart":
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "EatJump", false);
                    script.OnJump();
                    break;
                case "EatJump":
                    // animator.AnimationState.ClearTracks();
                    animator.AnimationState.SetAnimation(0, "EatEnd", false);
                    break;
            }
            /*
            if (name != null)
            {
                if (!(name == "DigOut"))
                {
                    if (!(name == "Eat"))
                    {
                        if (!(name == "Spawn"))
                        {
                            if (name == "DigIn")
                            {
                                this.script.OnDisappear();
                            }
                        }
                        else
                        {
                            this.SetAnimation(BugDusk.AnimationState.MOVE);
                            this.script.OnEndSpawn();
                        }
                    }
                    else
                    {
                        this.SetAnimation(BugDusk.AnimationState.MOVE);
                        this.script.OnEndAttack();
                    }
                }
                else
                {
                    this.SetAnimation(BugDusk.AnimationState.MOVE);
                    this.script.OnEndDigOut();
                    this.center.transform.localPosition = this._defaultPosition;
                }
            }*/
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
