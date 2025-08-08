using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconAnglerFish : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconAnglerFishAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        private NotbaconAnglerFishAnim animscript;
    }
}
