/*
private void SetWorkSuccess() // Success Rate displayed as percentage without decimal points
*/
using System;
using UnityEngine.UI;

namespace CreatureInfo
{
    // Token: 0x02000AD0 RID: 2768
    public class CreatureInfoWorkSlot : CreatureInfoController
    {
        // Token: 0x06005345 RID: 21317 RVA: 0x00042E8D File Offset: 0x0004108D
        public CreatureInfoWorkSlot()
        {
        }

        // Token: 0x06005346 RID: 21318 RVA: 0x0004325F File Offset: 0x0004145F
        public override void Initialize(CreatureModel creature)
        {
            base.Initialize(creature);
            this.Open.Init(this);
            this.SetWorkSuccess(creature);
        }

        // Token: 0x06005347 RID: 21319 RVA: 0x001DFF18 File Offset: 0x001DE118
        private void SetWorkSuccess(CreatureModel creautre)
        {
            for (int i = 0; i < this.levelSuccessPercentage.Length; i++)
            {
                Text text = this.levelSuccessPercentage[i];
                float num = creautre.metaInfo.workProbTable.GetWorkProb(this._type, i + 1);
                if (num <= 0f)
                {
                    num = 0f;
                }
                string percentText = UICommonTextConverter.GetPercentText(num);
                text.text = percentText;
            }
        }

        // Token: 0x06005348 RID: 21320 RVA: 0x0004327B File Offset: 0x0004147B
        public override void Initialize()
        {
            base.Initialize();
            this.Open.Init(this);
            this.SetWorkSuccess();
        }

        // Token: 0x06005349 RID: 21321 RVA: 0x001DFF80 File Offset: 0x001DE180
        private void SetWorkSuccess()
        { // <Mod> changed the function to the Info version and allowed it to display negative values
            for (int i = 0; i < this.levelSuccessPercentage.Length; i++)
            {
                Text text = this.levelSuccessPercentage[i];
                float num = base.MetaInfo.workProbTable.GetWorkProb(this._type, i + 1);
                string percentText = UICommonTextConverter.GetInfoPercentText(num);
                text.text = percentText;
            }
        }

        // Token: 0x0600534A RID: 21322 RVA: 0x00043295 File Offset: 0x00041495
        public void SetRWBPType(RwbpType type)
        {
            this._type = type;
            this.WorkName.text = SkillTypeList.instance.GetData((long)this._type).name;
        }

        // Token: 0x0600534B RID: 21323 RVA: 0x00042A1F File Offset: 0x00040C1F
        public override bool OnClick()
        {
            return CreatureInfoWindow.CurrentWindow.OnTryPurchase(this);
        }

        // Token: 0x0600534C RID: 21324 RVA: 0x00042B03 File Offset: 0x00040D03
        public override void OnPurchase()
        {
            this._isOpened = true;
        }

        // Token: 0x0600534D RID: 21325 RVA: 0x000432BF File Offset: 0x000414BF
        private void Awake()
        {
            this.Cost = this.Open.CostText;
        }

        // Token: 0x04004CCD RID: 19661
        public Text[] levelSuccessPercentage;

        // Token: 0x04004CCE RID: 19662
        public Text WorkName;

        // Token: 0x04004CCF RID: 19663
        public Image WorkIcon;

        // Token: 0x04004CD0 RID: 19664
        public CreatureInfoOpenArea Open;

        // Token: 0x04004CD1 RID: 19665
        private RwbpType _type;
    }
}
