using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconQuietBird : CreatureBase
    {
        public NotbaconQuietBirdAnim animScript
        {
            get
            {
                if (_animScript == null)
                {
                    _animScript = (Unit.animTarget as NotbaconQuietBirdAnim);
                }
                return _animScript;
            }
        }

        public override void OnStageStart()
        {
            animScript.OnStageStart();
        }

        public override void OnFixedUpdate(CreatureModel creature)
        {
            animScript.OnFixedUpdate();
        }

        private NotbaconQuietBirdAnim _animScript;
    }
}