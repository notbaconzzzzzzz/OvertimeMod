using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotbaconOvertimeMod
{
    public class NotbaconPaleHorseGift : EquipmentScriptBase
    {
        public override float GetDamageFactor()
        {
            return 1.07f;
        }

        public override bool OnTakeDamage(UnitModel actor, ref DamageInfo dmg)
        {
            dmg.min *= 1.07f;
            dmg.max *= 1.07f;
            return base.OnTakeDamage(actor, ref dmg);
        }
    }
}
