using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconDreamCatcher : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconDreamCatcherAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        private NotbaconDreamCatcherAnim animscript;
    }
}
