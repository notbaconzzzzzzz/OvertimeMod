using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconLonelyWraith : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconLonelyWraithAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        private NotbaconLonelyWraithAnim animscript;
    }
}
