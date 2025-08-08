using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconQuietBirdAnim : CreatureAnimScript
    {
        public void OnStageStart()
        {
            try
            {
                GameObject birdCage = Add_On.LoadObject("overtimeabnoscommon", "BirdCage");
                birdCage.transform.SetParent(base.transform);
                birdCage.transform.localPosition = new Vector3(0f, 0f, 0f);
                birdCage.transform.localRotation = Quaternion.identity;
                birdCage.transform.SetParent(base.transform.parent);
                birdCage.transform.localScale = new Vector3(1f, 1f, 1f);
                //this.perchPosInit = true;
                //this.fixedPerchPos = birdCage.transform.position;
                //this.perchParentInitialScale = birdCage.transform.parent.localScale;
                birdCage.transform.SetParent(EffectLayer.currentLayer.transform);
                birdCage.name = "quietBirdCage";
            }
            catch (Exception ex)
            {
                message = ex.Message + " : " + ex.StackTrace;
            }
        }

        public void OnFixedUpdate()
        {
            if (message != "")
            {
                Notice.instance.Send("AddSystemLog", new object[]
                {
                    message
                });
                message = "";
            }
        }

        private string message = "";
    }
}