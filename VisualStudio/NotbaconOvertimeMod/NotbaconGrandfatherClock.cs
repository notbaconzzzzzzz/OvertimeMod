using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconGrandfatherClock : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconGrandfatherClockAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        private NotbaconGrandfatherClockAnim animscript;
    }
}
