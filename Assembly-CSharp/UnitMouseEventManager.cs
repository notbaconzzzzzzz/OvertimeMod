using System;
using System.Collections.Generic;
using GlobalBullet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BAD RID: 2989
public class UnitMouseEventManager : MonoBehaviour
{
    // Token: 0x06005A61 RID: 23137 RVA: 0x00047C4A File Offset: 0x00045E4A
    public UnitMouseEventManager()
    {
    }

    // Token: 0x17000824 RID: 2084
    // (get) Token: 0x06005A62 RID: 23138 RVA: 0x00047C68 File Offset: 0x00045E68
    public static UnitMouseEventManager instance
    {
        get
        {
            return UnitMouseEventManager._instance;
        }
    }

    // Token: 0x17000825 RID: 2085
    // (get) Token: 0x06005A63 RID: 23139 RVA: 0x00047C6F File Offset: 0x00045E6F
    public bool isPointerEntered
    {
        get
        {
            return this._pointerEnter;
        }
    }

    // Token: 0x17000826 RID: 2086
    // (get) Token: 0x06005A64 RID: 23140 RVA: 0x00047C77 File Offset: 0x00045E77
    public bool draggingRight
    {
        get
        {
            return this._draggingRight;
        }
    }

    // Token: 0x17000827 RID: 2087
    // (get) Token: 0x06005A65 RID: 23141 RVA: 0x00047C7F File Offset: 0x00045E7F
    // (set) Token: 0x06005A66 RID: 23142 RVA: 0x00047C87 File Offset: 0x00045E87
    public bool DefaultClickBlocked
    {
        get
        {
            return this._defualtClickBlocked;
        }
        set
        {
            this._defualtClickBlocked = value;
        }
    }

    // Token: 0x06005A67 RID: 23143 RVA: 0x00047C90 File Offset: 0x00045E90
    private void Awake()
    {
        UnitMouseEventManager._instance = this;
        this._currentInDragTargets = new List<UnitMouseEventTarget>();
        this._pointerEnteredTarget = null;
        this._pointerEnteredRightTarget = null;
        this._dragging = false;
        this.dragImage.gameObject.SetActive(false);
    }

    // Token: 0x06005A68 RID: 23144 RVA: 0x00047CC9 File Offset: 0x00045EC9
    public void OnStageStart()
    {
        this._stageStarted = true;
    }

    // Token: 0x06005A69 RID: 23145 RVA: 0x002026D8 File Offset: 0x002008D8
    public void Unselect(IMouseCommandTargetModel targetModel)
    {
        UnitMouseEventTarget unitMouseEventTarget = null;
        foreach (UnitMouseEventTarget unitMouseEventTarget2 in this._selectedTargets)
        {
            if (targetModel == unitMouseEventTarget2.GetCommandTargetModel())
            {
                unitMouseEventTarget = unitMouseEventTarget2;
            }
        }
        if (unitMouseEventTarget != null)
        {
            this._selectedTargets.Remove(unitMouseEventTarget);
        }
        if (this._selectedTargets.Count == 0 && AgentInfoWindow.currentWindow.IsEnabled)
        {
            AgentInfoWindow.currentWindow.UnPinCurrentAgent();
        }
    }

    // Token: 0x06005A6A RID: 23146 RVA: 0x0020276C File Offset: 0x0020096C
    public void UnselectAll()
    {
        if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && TutorialManager.instance != null)
        {
            IsolateTutorial isolateTutorial = TutorialManager.instance.CurrentTutorial as IsolateTutorial;
            if (isolateTutorial != null && isolateTutorial.targetClick == PointerEventData.InputButton.Right)
            {
                return;
            }
        }
        foreach (UnitMouseEventTarget unitMouseEventTarget in this._selectedTargets)
        {
            unitMouseEventTarget.OnUnselect();
        }
        this._selectedTargets.Clear();
        if (AgentInfoWindow.currentWindow.IsEnabled)
        {
            AgentInfoWindow.currentWindow.UnPinCurrentAgent();
        }
    }

    // Token: 0x06005A6B RID: 23147 RVA: 0x0020281C File Offset: 0x00200A1C
    private void SelectTarget(UnitMouseEventTarget target)
    {
        this.UnselectAll();
        target.OnSelect();
        this._selectedTargets.Add(target);
        AgentModel agentModel = this._selectedTargets[0].GetCommandTargetModel() as AgentModel;
        if (agentModel != null)
        {
            AgentInfoWindow.CreateWindow(agentModel, false);
        }
    }

    // Token: 0x06005A6C RID: 23148 RVA: 0x00202864 File Offset: 0x00200A64
    private void SelectTargets(List<UnitMouseEventTarget> targets)
    {
        if (targets.Count == 1)
        {
            this.SelectTarget(targets[0]);
        }
        this.UnselectAll();
        foreach (UnitMouseEventTarget unitMouseEventTarget in targets)
        {
            unitMouseEventTarget.OnSelect();
            this._selectedTargets.Add(unitMouseEventTarget);
        }
    }

    // Token: 0x06005A6D RID: 23149 RVA: 0x002028DC File Offset: 0x00200ADC
    public List<AgentModel> GetSelectedAgents()
    {
        List<AgentModel> list = new List<AgentModel>();
        foreach (UnitMouseEventTarget unitMouseEventTarget in this._selectedTargets)
        {
            IMouseCommandTargetModel commandTargetModel = unitMouseEventTarget.GetCommandTargetModel();
            if (commandTargetModel is AgentModel)
            {
                list.Add(commandTargetModel as AgentModel);
            }
        }
        return list;
    }

    // Token: 0x06005A6E RID: 23150 RVA: 0x00202948 File Offset: 0x00200B48
    private bool IsRightClickable(UnitMouseEventTarget target)
    {
        IMouseCommandTargetModel commandTargetModel = target.GetCommandTargetModel();
        if (commandTargetModel is PassageObjectModel)
        {
            return true;
        }
        if (commandTargetModel is CreatureModel && (commandTargetModel as CreatureModel).IsAttackTargetable())
        {
            return true;
        }
        if (commandTargetModel is WorkerModel)
        {
            WorkerModel workerModel = commandTargetModel as WorkerModel;
            if ((workerModel is AgentModel && workerModel.IsPanic()) || workerModel.unconAction is Uncontrollable_RedShoes || workerModel.unconAction is Uncontrollable_SingingMachine_attacker)
            {
                return true;
            }
        }
        return false;
    }

    // Token: 0x06005A6F RID: 23151 RVA: 0x002029BC File Offset: 0x00200BBC
    private void UpdateDrag()
    {
        Vector2 pointA = Camera.main.ScreenToWorldPoint(this._dragBeginPosition);
        Vector2 pointB = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] array = Physics2D.OverlapAreaAll(pointA, pointB);
        List<UnitMouseEventTarget> list = new List<UnitMouseEventTarget>();
        Collider2D[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            UnitMouseEventTarget component = array2[i].GetComponent<UnitMouseEventTarget>();
            if (component != null && component.IsDragSelectable())
            {
                if (this._dragEnteredTargets.ContainsKey(component))
                {
                    list.Add(component);
                    this._dragEnteredTargets.Remove(component);
                }
                else
                {
                    component.OnEnterDragArea();
                    list.Add(component);
                }
            }
        }
        foreach (KeyValuePair<UnitMouseEventTarget, bool> keyValuePair in this._dragEnteredTargets)
        {
            keyValuePair.Key.OnExitDragArea();
        }
        this._dragEnteredTargets.Clear();
        foreach (UnitMouseEventTarget key in list)
        {
            this._dragEnteredTargets.Add(key, true);
        }
    }

    // Token: 0x06005A70 RID: 23152 RVA: 0x00202B04 File Offset: 0x00200D04
    private void SetPointerEnteredTarget(UnitMouseEventTarget target)
    {
        if (this._pointerEnteredTarget == target)
        {
            return;
        }
        if (this._pointerEnteredTarget != null)
        {
            this._pointerEnteredTarget.OnPointExit();
            this._pointerEnteredTarget = null;
        }
        if (target != null)
        {
            this._pointerEnteredTarget = target;
            this._pointerEnteredTarget.OnPointEnter();
        }
    }

    // Token: 0x06005A71 RID: 23153 RVA: 0x00047CD2 File Offset: 0x00045ED2
    public UnitMouseEventTarget GetPointerEnteredTarget()
    {
        return this._pointerEnteredTarget;
    }

    // Token: 0x06005A72 RID: 23154 RVA: 0x00202B5C File Offset: 0x00200D5C
    private void SetPointerEnteredRightTarget(UnitMouseEventTarget rightTarget)
    {
        if (this._pointerEnteredRightTarget == rightTarget)
        {
            return;
        }
        if (this._pointerEnteredRightTarget != null)
        {
            this._pointerEnteredRightTarget.OnPointExit();
            this._pointerEnteredRightTarget = null;
            UnitMouseEventManager.instance.suppressCursor = false;
            if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
            {
                CursorManager.instance.CursorSet(MouseCursorType.NORMAL);
            }
        }
        if (rightTarget != null)
        {
            this._pointerEnteredRightTarget = rightTarget;
            this._pointerEnteredRightTarget.OnPointEnter();
            if (!(this._pointerEnteredRightTarget.GetCommandTargetModel() is PassageObjectModel))
            {
                UnitMouseEventManager.instance.suppressCursor = true;
                if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                {
                    CursorManager.instance.CursorSet(MouseCursorType.SUPPRESS);
                }
            }
        }
    }

    // Token: 0x06005A73 RID: 23155 RVA: 0x00202C14 File Offset: 0x00200E14
    private void UpdatePointEntered()
    {
        if (!this._pointerEnter)
        {
            return;
        }
        bool flag = false;
        Collider2D[] array = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        List<UnitMouseEventTarget> list = new List<UnitMouseEventTarget>();
        List<UnitMouseEventTarget> list2 = new List<UnitMouseEventTarget>();
        List<UnitMouseEventTarget> list3 = new List<UnitMouseEventTarget>();
        List<UnitMouseEventTarget> list4 = new List<UnitMouseEventTarget>();
        Collider2D[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            UnitMouseEventTarget component = array2[i].GetComponent<UnitMouseEventTarget>();
            if (!(component == null))
            {
                if (component == this._pointerEnteredTarget)
                {
                    flag = true;
                }
                else if (component != null && component.HasPointListener())
                {
                    list4.Add(component);
                }
                if (this._selectedTargets.Count > 0)
                {
                    IMouseCommandTargetModel commandTargetModel = component.GetCommandTargetModel();
                    if (commandTargetModel is CreatureModel && (commandTargetModel as CreatureModel).IsAttackTargetable())
                    {
                        list.Add(component);
                    }
                    else if (commandTargetModel is PassageObjectModel)
                    {
                        list3.Add(component);
                    }
                    else if (commandTargetModel is WorkerModel && this.IsRightClickable(component))
                    {
                        list2.Add(component);
                    }
                }
            }
        }
        UnitMouseEventTarget unitMouseEventTarget = null;
        if (list.Count > 0)
        {
            unitMouseEventTarget = list[0];
        }
        else if (list2.Count > 0)
        {
            unitMouseEventTarget = list2[0];
        }
        else if (list3.Count > 0)
        {
            unitMouseEventTarget = list3[0];
        }
        if (!flag)
        {
            if (list4.Count > 0)
            {
                this.SetPointerEnteredTarget(list4[0]);
            }
            else
            {
                this.SetPointerEnteredTarget(null);
            }
        }
        if (unitMouseEventTarget == this._pointerEnteredRightTarget)
        {
            return;
        }
        this.SetPointerEnteredRightTarget(unitMouseEventTarget);
    }

    // Token: 0x06005A74 RID: 23156 RVA: 0x00047CDA File Offset: 0x00045EDA
    private void Update()
    {
        if (!this._stageStarted)
        {
            return;
        }
        if (this._dragging)
        {
            this.UpdateDrag();
            return;
        }
        this.UpdatePointEntered();
    }

    // Token: 0x06005A75 RID: 23157 RVA: 0x00202DA0 File Offset: 0x00200FA0
    private void OnPointerClick_bullet(BaseEventData eventData)
    {
        if ((eventData as PointerEventData).button == PointerEventData.InputButton.Right)
        {
            if (this._draggingRight)
            {
                return;
            }
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && TutorialManager.instance.CurrentTutorial is ClickPanickedAgentTutorial)
            {
                return;
            }
            if (GlobalBulletWindow.CurrentWindow != null)
            {
                GlobalBulletWindow.CurrentWindow.OnSlotSelected(GlobalBulletType.NONE);
            }
        }
    }

    // Token: 0x06005A76 RID: 23158 RVA: 0x00202DFC File Offset: 0x00200FFC
    public List<UnitMouseEventTarget> GetCurrentPointerTargets()
    {
        Collider2D[] array = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        List<UnitMouseEventTarget> list = new List<UnitMouseEventTarget>();
        Collider2D[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            UnitMouseEventTarget component = array2[i].GetComponent<UnitMouseEventTarget>();
            if (component != null && component.IsSelectable())
            {
                list.Add(component);
            }
        }
        return list;
    }

    // Token: 0x06005A77 RID: 23159 RVA: 0x00202E5C File Offset: 0x0020105C
    private void OnPointerClick_default(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && TutorialManager.instance.CurrentTutorial is AgentMoveCommandTutorial)
            {
                return;
            }
            Collider2D[] array = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(pointerEventData.position));
            List<UnitMouseEventTarget> list = new List<UnitMouseEventTarget>();
            Collider2D[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                UnitMouseEventTarget component = array2[i].GetComponent<UnitMouseEventTarget>();
                if (component != null && component.IsSelectable())
                {
                    if (component.GetCommandTargetModel() is CreatureModel && (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL || !(TutorialManager.instance.CurrentTutorial is SuppressClickCommandTutorial)))
                    {
                        component.OnSelect();
                        return;
                    }
                    if (GlobalGameManager.instance.gameMode != GameMode.TUTORIAL || !(TutorialManager.instance.CurrentTutorial is ClickEscapedCreatureTutorial))
                    {
                        list.Add(component);
                    }
                }
            }
            if (!this._dragging)
            {
                if (list.Count <= 0)
                {
                    this.UnselectAll();
                    return;
                }
                if (this._pointerEnteredTarget != null && list.Contains(this._pointerEnteredTarget))
                {
                    this.SelectTarget(this._pointerEnteredTarget);
                    return;
                }
                this.SelectTarget(list[0]);
                return;
            }
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            if (this._draggingRight)
            {
                return;
            }
            if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && TutorialManager.instance.CurrentTutorial is ClickPanickedAgentTutorial)
            {
                return;
            }
            Collider2D[] array3 = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(pointerEventData.position));
            List<UnitMouseEventTarget> list2 = new List<UnitMouseEventTarget>();
            Collider2D[] array2 = array3;
            for (int i = 0; i < array2.Length; i++)
            {
                UnitMouseEventTarget component2 = array2[i].GetComponent<UnitMouseEventTarget>();
                if (component2 != null && this.IsRightClickable(component2))
                {
                    list2.Add(component2);
                }
            }
            if (list2.Count > 0)
            {
                List<CreatureModel> list3 = new List<CreatureModel>();
                List<WorkerModel> list4 = new List<WorkerModel>();
                List<PassageObjectModel> list5 = new List<PassageObjectModel>();
                foreach (UnitMouseEventTarget unitMouseEventTarget in list2)
                {
                    IMouseCommandTargetModel commandTargetModel = unitMouseEventTarget.GetCommandTargetModel();
                    if (commandTargetModel is CreatureModel && (commandTargetModel as CreatureModel).IsAttackTargetable())
                    {
                        list3.Add(commandTargetModel as CreatureModel);
                    }
                    else if (commandTargetModel is PassageObjectModel)
                    {
                        list5.Add(commandTargetModel as PassageObjectModel);
                    }
                    else if (commandTargetModel is WorkerModel)
                    {
                        list4.Add(commandTargetModel as WorkerModel);
                    }
                }
                if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && TutorialManager.instance.CurrentTutorial is AgentMoveCommandTutorial)
                {
                    if (list5.Count > 0)
                    {
                        PassageObjectModel passageObjectModel = list5[0];
                        PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(passageObjectModel);
                        Sefira sefira = SefiraManager.instance.GetSefira(SefiraEnum.MALKUT);
                        if (passageObjectModel != sefira.sefiraPassage)
                        {
                            return;
                        }
                        if (passageObject != null)
                        {
                            passageObject.OnPointerClick();
                        }
                        foreach (UnitMouseEventTarget unitMouseEventTarget2 in this._selectedTargets)
                        {
                            IMouseCommandTargetModel commandTargetModel2 = unitMouseEventTarget2.GetCommandTargetModel();
                            if (commandTargetModel2 is AgentModel)
                            {
                                AgentModel agentModel = (AgentModel)commandTargetModel2;
                                if (!agentModel.IsDead() && !agentModel.IsCrazy() && agentModel.currentSkill == null && !passageObject.IsRabbitExecuting() && (SefiraBossManager.Instance.IsWorkCancelable || agentModel.GetState() != AgentAIState.MANAGE))
                                {
                                    agentModel.SetWaitingPassage(passageObjectModel);
                                    agentModel.StopAction();
                                    agentModel.counterAttackEnabled = false;
                                }
                            }
                        }
                        Notice.instance.Send(NoticeName.OnAgentMoveCommand, new object[]
                        {
                            passageObjectModel
                        });
                    }
                }
                else
                {
                    if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && TutorialManager.instance.CurrentTutorial is SuppressClickCommandTutorial)
                    {
                        if (list3.Count <= 0)
                        {
                            return;
                        }
                        CreatureModel target = list3[0];
                        if (RabbitManager.instance.CheckUnitRabbitExecution(target))
                        {
                            goto IL_67C;
                        }
                        using (List<UnitMouseEventTarget>.Enumerator enumerator = this._selectedTargets.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                UnitMouseEventTarget unitMouseEventTarget3 = enumerator.Current;
                                IMouseCommandTargetModel commandTargetModel3 = unitMouseEventTarget3.GetCommandTargetModel();
                                if (commandTargetModel3 is AgentModel)
                                {
                                    AgentModel agentModel2 = (AgentModel)commandTargetModel3;
                                    if (!agentModel2.IsDead() && !agentModel2.IsCrazy() && agentModel2.currentSkill == null && agentModel2.CheckSuppressCommand())
                                    {
                                        agentModel2.Suppress(target, false);
                                    }
                                }
                            }
                            goto IL_67C;
                        }
                    }
                    if (list3.Count > 0)
                    {
                        CreatureModel target2 = list3[0];
                        if (RabbitManager.instance.CheckUnitRabbitExecution(target2))
                        {
                            goto IL_67C;
                        }
                        using (List<UnitMouseEventTarget>.Enumerator enumerator = this._selectedTargets.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                UnitMouseEventTarget unitMouseEventTarget4 = enumerator.Current;
                                IMouseCommandTargetModel commandTargetModel4 = unitMouseEventTarget4.GetCommandTargetModel();
                                if (commandTargetModel4 is AgentModel)
                                {
                                    AgentModel agentModel3 = (AgentModel)commandTargetModel4;
                                    if (!agentModel3.IsDead() && !agentModel3.IsCrazy() && agentModel3.currentSkill == null && agentModel3.CheckSuppressCommand())
                                    {
                                        agentModel3.Suppress(target2, false);
                                    }
                                }
                            }
                            goto IL_67C;
                        }
                    }
                    if (list4.Count > 0)
                    {
                        WorkerModel target3 = list4[0];
                        if (RabbitManager.instance.CheckUnitRabbitExecution(target3))
                        {
                            goto IL_67C;
                        }
                        using (List<UnitMouseEventTarget>.Enumerator enumerator = this._selectedTargets.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                UnitMouseEventTarget unitMouseEventTarget5 = enumerator.Current;
                                IMouseCommandTargetModel commandTargetModel5 = unitMouseEventTarget5.GetCommandTargetModel();
                                if (commandTargetModel5 is AgentModel)
                                {
                                    AgentModel agentModel4 = (AgentModel)commandTargetModel5;
                                    if (!agentModel4.IsDead() && !agentModel4.IsCrazy() && agentModel4.currentSkill == null && agentModel4.CheckSuppressCommand())
                                    {
                                        agentModel4.Suppress(target3, false);
                                    }
                                }
                            }
                            goto IL_67C;
                        }
                    }
                    if (list5.Count > 0)
                    {
                        PassageObjectModel passageObjectModel2 = list5[0];
                        PassageObject passageObject2 = SefiraMapLayer.instance.GetPassageObject(passageObjectModel2);
                        if (passageObject2 != null)
                        {
                            passageObject2.OnPointerClick();
                        }
                        foreach (UnitMouseEventTarget unitMouseEventTarget6 in this._selectedTargets)
                        {
                            IMouseCommandTargetModel commandTargetModel6 = unitMouseEventTarget6.GetCommandTargetModel();
                            if (commandTargetModel6 is AgentModel)
                            {
                                AgentModel agentModel5 = (AgentModel)commandTargetModel6;
                                if (!agentModel5.IsDead() && !agentModel5.IsCrazy() && agentModel5.currentSkill == null && !passageObject2.IsRabbitExecuting() && (SefiraBossManager.Instance.IsWorkCancelable || agentModel5.GetState() != AgentAIState.MANAGE))
                                {
                                    agentModel5.SetWaitingPassage(passageObjectModel2);
                                    agentModel5.StopAction();
                                    agentModel5.counterAttackEnabled = false;
                                }
                            }
                        }
                    }
                }
            IL_67C:
                LocalAudioManager.instance.PlayClip(19);
                this.UnselectAll();
            }
        }
    }

    // Token: 0x06005A78 RID: 23160 RVA: 0x00047CFA File Offset: 0x00045EFA
    public void OnPointerClick(BaseEventData eventData)
    {
        UIActivateManager.instance.OnClickBackGround(eventData);
        if (GlobalBulletWindow.CurrentWindow != null && GlobalBulletWindow.CurrentWindow.CurrentSelectedBullet != GlobalBulletType.NONE)
        {
            this.OnPointerClick_bullet(eventData);
            return;
        }
        if (!this.DefaultClickBlocked)
        {
            this.OnPointerClick_default(eventData);
        }
    }

    // Token: 0x06005A79 RID: 23161 RVA: 0x00203544 File Offset: 0x00201744
    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            this._dragBeginPosition = pointerEventData.position;
        }
    }

    // Token: 0x06005A7A RID: 23162 RVA: 0x00003E21 File Offset: 0x00002021
    public void OnPointerUp(BaseEventData eventData)
    {
    }

    // Token: 0x06005A7B RID: 23163 RVA: 0x00047D37 File Offset: 0x00045F37
    public void OnPointerEnter(BaseEventData eventData)
    {
        this._pointerEnter = true;
    }

    // Token: 0x06005A7C RID: 23164 RVA: 0x00047D40 File Offset: 0x00045F40
    public void OnPointerExit(BaseEventData eventData)
    {
        this._pointerEnter = false;
        this.SetPointerEnteredTarget(null);
        this.SetPointerEnteredRightTarget(null);
    }

    // Token: 0x06005A7D RID: 23165 RVA: 0x0020356C File Offset: 0x0020176C
    public void OnBeginDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData.button == PointerEventData.InputButton.Left && GlobalBulletWindow.CurrentWindow != null && GlobalBulletWindow.CurrentWindow.CurrentSelectedBullet != GlobalBulletType.NONE)
        {
            return;
        }
        if (GlobalGameManager.instance.gameMode == GameMode.TUTORIAL && (TutorialManager.instance.CurrentTutorial is ClickEscapedCreatureTutorial || TutorialManager.instance.CurrentTutorial is AgentMoveCommandTutorial))
        {
            return;
        }
        if (pointerEventData.button != PointerEventData.InputButton.Left)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                this._draggingRight = true;
            }
            return;
        }
        this.dragImage.gameObject.SetActive(true);
        Camera.main.ScreenToWorldPoint(pointerEventData.position);
        this._dragging = true;
        if (this._pointerEnteredTarget != null)
        {
            this._pointerEnteredTarget.OnPointExit();
            this._pointerEnteredTarget = null;
        }
    }

    // Token: 0x06005A7E RID: 23166 RVA: 0x00203640 File Offset: 0x00201840
    public void OnDrag(BaseEventData eventData)
    {
        if (!this._dragging)
        {
            return;
        }
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        this.dragImage.rectTransform.anchoredPosition = (this._dragBeginPosition + pointerEventData.position) / 2f;
        this.dragImage.rectTransform.sizeDelta = new Vector2(Mathf.Abs(this._dragBeginPosition.x - pointerEventData.position.x), Mathf.Abs(this._dragBeginPosition.y - pointerEventData.position.y));
    }

    // Token: 0x06005A7F RID: 23167 RVA: 0x002036E0 File Offset: 0x002018E0
    public void OnEndDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData.button != PointerEventData.InputButton.Left)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                this._draggingRight = false;
            }
            return;
        }
        if (!this._dragging)
        {
            return;
        }
        Vector2 pointA = Camera.main.ScreenToWorldPoint(this._dragBeginPosition);
        Vector2 pointB = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        this.UnselectAll();
        Collider2D[] array = Physics2D.OverlapAreaAll(pointA, pointB);
        List<UnitMouseEventTarget> list = new List<UnitMouseEventTarget>();
        Collider2D[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            UnitMouseEventTarget component = array2[i].GetComponent<UnitMouseEventTarget>();
            if (component != null && component.IsDragSelectable())
            {
                list.Add(component);
            }
        }
        this.SelectTargets(list);
        this.CancelDrag();
    }

    // Token: 0x06005A80 RID: 23168 RVA: 0x00047D57 File Offset: 0x00045F57
    public void CancelDrag()
    {
        this.dragImage.gameObject.SetActive(false);
        this._dragging = false;
    }

    // Token: 0x17000828 RID: 2088
    // (get) Token: 0x06005A81 RID: 23169 RVA: 0x00047D71 File Offset: 0x00045F71
    public List<UnitMouseEventTarget> seletedtargets
    {
        get
        {
            return this._selectedTargets;
        }
    }

    // Token: 0x040052D4 RID: 21204
    private static UnitMouseEventManager _instance;

    // Token: 0x040052D5 RID: 21205
    private List<UnitMouseEventTarget> _currentInDragTargets;

    // Token: 0x040052D6 RID: 21206
    private List<UnitMouseEventTarget> _selectedTargets = new List<UnitMouseEventTarget>();

    // Token: 0x040052D7 RID: 21207
    private UnitMouseEventTarget _pointerEnteredTarget;

    // Token: 0x040052D8 RID: 21208
    private UnitMouseEventTarget _pointerEnteredRightTarget;

    // Token: 0x040052D9 RID: 21209
    private Dictionary<UnitMouseEventTarget, bool> _dragEnteredTargets = new Dictionary<UnitMouseEventTarget, bool>();

    // Token: 0x040052DA RID: 21210
    private PointerEventData _dragBeginPointer;

    // Token: 0x040052DB RID: 21211
    private Vector2 _dragBeginPosition;

    // Token: 0x040052DC RID: 21212
    public AnimationCurve ClickTransitionCurve;

    // Token: 0x040052DD RID: 21213
    [NonSerialized]
    public bool suppressCursor;

    // Token: 0x040052DE RID: 21214
    public Image dragImage;

    // Token: 0x040052DF RID: 21215
    private bool _stageStarted;

    // Token: 0x040052E0 RID: 21216
    private bool _pointerEnter;

    // Token: 0x040052E1 RID: 21217
    private bool _dragging;

    // Token: 0x040052E2 RID: 21218
    private bool _draggingRight;

    // Token: 0x040052E3 RID: 21219
    private bool _defualtClickBlocked;
}
