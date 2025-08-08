using Spine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NotbaconOvertimeMod
{
    public class NotbaconAnglerFishWeapon : EquipmentScriptBase
    {
        public override EquipmentScriptBase.WeaponDamageInfo OnAttackStart(UnitModel actor, UnitModel target)
        {
            List<DamageInfo> list = new List<DamageInfo>();
            for (int i = 0; i < this._COUNT_ATTACK_PER_ANIM; i++)
            {
                list.Add(base.model.metaInfo.damageInfo);
            }
            return new EquipmentScriptBase.WeaponDamageInfo(base.model.metaInfo.animationNames[0], list.ToArray());
        }

        private int _COUNT_ATTACK_PER_ANIM = 2;
    }
}
