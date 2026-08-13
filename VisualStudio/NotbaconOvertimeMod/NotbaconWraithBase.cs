using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NotbaconOvertimeMod
{
    public class NotbaconWraithBase : CreatureBase
    {
        public bool WraithMode
        {
            get
            {
                return false;
            }
        }

        public virtual float HealAmount
        {
            get
            {
                return 6f;
            }
        }

        public virtual float HealInterval
        {
            get
            {
                return 4f;
            }
        }
    }
}
