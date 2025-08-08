using System;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconShatteredMirror : CreatureBase
    {
        public override void OnViewInit(CreatureUnit unit)
        {
            base.OnViewInit(unit);
            animscript = (NotbaconShatteredMirrorAnim)unit.animTarget;
            animscript.SetScript(this);
        }

        private NotbaconShatteredMirrorAnim animscript;
    }
}
