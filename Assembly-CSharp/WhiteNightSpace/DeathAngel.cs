/*
public override void OnGamemanagerInit() // 
public void ChangeIsolateRoom() // 
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO; // 
using System.Runtime.Serialization.Formatters.Binary; // 
using CreatureCameraUtil;
using UnityEngine;

namespace WhiteNightSpace
{
	// Token: 0x020003E9 RID: 1001
	public class DeathAngel : CreatureBase
	{
		// Token: 0x06002144 RID: 8516 RVA: 0x000FCF58 File Offset: 0x000FB158
		public DeathAngel()
		{
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x00022400 File Offset: 0x00020600
		private static float _escapeSkillFreq
		{
			get
			{
				return DeathAngel._escapeFreqRange.GetRandomFloat();
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x0002240C File Offset: 0x0002060C
		public Sefira CurrentSefira
		{
			get
			{
				return this._currentSefira;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06002147 RID: 8519 RVA: 0x00022414 File Offset: 0x00020614
		public bool IsPrevEscaped
		{
			get
			{
				return this._isPrevEscaped;
			}
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0002241C File Offset: 0x0002061C
		public static bool Prob(float value)
		{
			return UnityEngine.Random.value <= value;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x00022429 File Offset: 0x00020629
		public static void Log(string log, bool isError = false)
		{
			if (!isError)
			{
				Debug.Log("<color=#DDDDDDFF>[WhiteNight]</color> " + log);
			}
			else
			{
				Debug.LogError("<color=#DDDDDDFF>[WhiteNight]</color> " + log);
			}
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000FCFB0 File Offset: 0x000FB1B0
		public static List<ApostleGenData> GetAdeventTargets(List<ApostleData> apostles)
		{
			List<ApostleGenData> list = new List<ApostleGenData>();
			AgentModel agentModel = null;
			List<AgentModel> list2 = new List<AgentModel>(AgentManager.instance.GetAgentList());
			List<OfficerModel> list3 = new List<OfficerModel>(OfficerManager.instance.GetOfficerList());
			List<WorkerModel> list4 = new List<WorkerModel>();
			List<WorkerModel> list5 = new List<WorkerModel>();
			ApostleGenData apostleGenData = null;
			foreach (AgentModel agentModel2 in list2)
			{
				if (agentModel2.IsDead())
				{
					list5.Add(agentModel2);
				}
				else
				{
					list4.Add(agentModel2);
				}
			}
			foreach (OfficerModel officerModel in list3)
			{
				if (officerModel.IsDead())
				{
					list5.Add(officerModel);
				}
				else
				{
					list4.Add(officerModel);
				}
			}
			ApostleData data = apostles[11];
			agentModel = DeathAngel.FindAgent(data, list2);
			if (agentModel == null)
			{
				if (list2.Count == 0)
				{
					DeathAngel.Log("Failed To Make Betrayer", false);
				}
				else
				{
					List<AgentModel> list6 = new List<AgentModel>();
					foreach (AgentModel agentModel3 in list2)
					{
						if (!agentModel3.IsDead())
						{
							list6.Add(agentModel3);
						}
					}
					if (list6.Count == 0)
					{
						DeathAngel.Log("Failed To Make Betrayer", false);
					}
					else
					{
						agentModel = list6[UnityEngine.Random.Range(0, list6.Count)];
					}
				}
			}
			if (agentModel != null)
			{
				list2.Remove(agentModel);
				list4.Remove(agentModel);
				list5.Remove(agentModel);
				apostleGenData = new ApostleGenData(agentModel, apostles[11], 11);
				agentModel.GetControl();
			}
			int i = 0;
			List<int> list7 = new List<int>();
			while (i < 11)
			{
				ApostleData data2 = apostles[i];
				AgentModel agentModel4 = DeathAngel.FindAgent(data2, list2);
				if (agentModel4 == null)
				{
					if (list2.Count == 0)
					{
						Debug.LogError("Error by small list ");
					}
					else
					{
						list7.Add(i);
					}
				}
				else
				{
					list2.Remove(agentModel4);
					list4.Remove(agentModel4);
					list5.Remove(agentModel4);
					ApostleGenData item = new ApostleGenData(agentModel4, data2, i);
					list.Add(item);
				}
				i++;
			}
			foreach (WorkerModel workerModel in list4)
			{
				if (workerModel is AgentModel)
				{
					AgentModel agentModel5 = workerModel as AgentModel;
					if (agentModel5.GetState() == AgentAIState.MANAGE && agentModel5.target != null && agentModel5.target.script is Shelter)
					{
						list4.Remove(agentModel5);
						break;
					}
				}
			}
			List<WorkerModel> list8 = new List<WorkerModel>();
			for (i = 0; i < list7.Count; i++)
			{
				List<WorkerModel> list9;
				if (list4.Count > 0)
				{
					list9 = list4;
				}
				else
				{
					list9 = list5;
				}
				WorkerModel item2 = list9[UnityEngine.Random.Range(0, list9.Count)];
				list9.Remove(item2);
				list8.Add(item2);
			}
			List<WorkerModel> list10 = new List<WorkerModel>();
			while (list8.Count > 0)
			{
				WorkerModel item3 = list8[UnityEngine.Random.Range(0, list8.Count)];
				list10.Add(item3);
				list8.Remove(item3);
			}
			for (i = 0; i < list7.Count; i++)
			{
				int index = list7[i];
				ApostleData data3 = apostles[index];
				WorkerModel worker = list10[i];
				ApostleGenData item4 = new ApostleGenData(worker, data3, index);
				list.Add(item4);
			}
			List<ApostleGenData> list11 = list;
			if (DeathAngel.cache0 == null)
			{
				DeathAngel.cache0 = new Comparison<ApostleGenData>(ApostleGenData.Compare);
			}
			list11.Sort(DeathAngel.cache0);
			if (apostleGenData != null)
			{
				list.Add(apostleGenData);
			}
			return list;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000FD418 File Offset: 0x000FB618
		public void SecondAdvent()
		{
			AgentModel agentModel = null;
			List<AgentModel> list = new List<AgentModel>(AgentManager.instance.GetAgentList());
			ApostleData data = this.apostleData[11];
			agentModel = DeathAngel.FindAgent(data, list);
			if (agentModel == null)
			{
				if (list.Count == 0)
				{
					DeathAngel.Log("Failed To Make Betrayer", false);
				}
				else
				{
					List<AgentModel> list2 = new List<AgentModel>();
					foreach (AgentModel agentModel2 in list)
					{
						if (!agentModel2.IsDead())
						{
							list2.Add(agentModel2);
						}
					}
					if (list2.Count == 0)
					{
						DeathAngel.Log("Failed To Make Betrayer", false);
					}
					else
					{
						agentModel = list2[UnityEngine.Random.Range(0, list2.Count)];
					}
				}
			}
			if (agentModel != null)
			{
				list.Remove(agentModel);
				agentModel.GetControl();
				DeathAngelBetrayerBuf buf = new DeathAngelBetrayerBuf(this, data);
				agentModel.AddUnitBuf(buf);
				this.betrayer = buf;
			}
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000FD52C File Offset: 0x000FB72C
		public static AgentModel FindAgent(ApostleData data, List<AgentModel> searchPool)
		{
			foreach (AgentModel agentModel in searchPool)
			{
				if (data.instId == agentModel.instanceId && data.NameId == agentModel._agentName.id)
				{
					agentModel.LoseControl();
					return agentModel;
				}
			}
			return null;
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x00022456 File Offset: 0x00020656
		public DeathAngelAnim AnimScript
		{
			get
			{
				return this._animScript;
			}
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0002245E File Offset: 0x0002065E
		public override void OnViewInit(CreatureUnit unit)
		{
			this._animScript = (unit.animTarget as DeathAngelAnim);
			this.AnimScript.SetScript(this);
			this.AnimScript.SetTransform(DeathAngel._roomPos, DeathAngel._roomScale);
			this.ClearEscapeLoop();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00022498 File Offset: 0x00020698
		public void SetCameraUtil(CreatureCameraUtil_Inspector insp)
		{
			this._escapeSense = new CreatureCameraUtil.CreatureCameraUtil(this.model, insp);
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000FD5B4 File Offset: 0x000FB7B4
		public override void OnStageStart()
		{
			this._isPrevEscaped = false;
			this.betrayer = null;
			this.oneBadManyGood = null;
			this.apostles.Clear();
			this._currentSefira = this.model.GetCurrentNode().GetAttachedPassage().GetSefira();
			DeathAngel.Log(this._currentSefira.name, false);
			this._qliphothSubTimer.StartTimer(90f);
			this.CheckStateSprite();
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000FD624 File Offset: 0x000FB824
		public override void OnReturn()
		{
			this.CheckStateSprite();
			this.RoomStateSpriteOn();
			this.model.ResetQliphothCounter();
			this.betrayer = null;
			this.oneBadManyGood = null;
			this.AnimScript.SetTransform(DeathAngel._roomPos, DeathAngel._roomScale);
			this.AnimScript.InitParam();
			this._qliphothSubTimer.StartTimer(90f);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000224AC File Offset: 0x000206AC
		public override void OnStageRelease()
		{
			if (this.betrayer != null)
			{
				this.betrayer.OnDeathAngelSuppressed();
			}
			this.betrayer = null;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000224CB File Offset: 0x000206CB
		public override void OnEnterRoom(UseSkill skill)
		{
			this._qliphothSubTimer.StopTimer();
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000224D8 File Offset: 0x000206D8
		public override void OnWorkCoolTimeEnd(CreatureFeelingState oldState)
		{
			this.ActivateWorkSkill(oldState);
			this._qliphothSubTimer.StartTimer(90f);
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00003FDD File Offset: 0x000021DD
		public void Update()
		{
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000FD688 File Offset: 0x000FB888
		public override void UniqueEscape()
		{
			bool flag = this._escapeSense.CheckIgnoreOrtho();
			if (this._confessDead.started && this._confessDead.RunTimer())
			{
				this._confessDead.StartTimer(0.3f);
				this.model.TakeDamage(DeathAngel._confessDeadDamage);
				if (this.model.state == CreatureState.SUPPRESSED)
				{
					this._confessDead.StopTimer();
				}
			}
			if (this._escapeSkillTimer.started && this._escapeSkillTimer.RunTimer())
			{
				this.EscapeSkill();
				this._escapeSkillTimer.StartTimer(DeathAngel._escapeSkillFreq);
			}
			if (flag)
			{
				if (!this.bgmChanged)
				{
					BgmManager.instance.FadeOut();
					this.bgmChanged = true;
					this._bgmFadeTime.StartTimer(1f);
				}
			}
			else if (this.bgmChanged)
			{
				BgmManager.instance.FadeIn();
				this.bgmChanged = false;
				this._bgmFadeTime.StartTimer(1f);
			}
			if (this._bgmFadeTime.started)
			{
				float num = Mathf.Lerp(0f, 1f, this._bgmFadeTime.Rate);
				if (!this.bgmChanged)
				{
					num = 1f - num;
				}
				if (this._bgmFadeTime.RunTimer())
				{
					if (!this.bgmChanged)
					{
						num = 0f;
					}
					else
					{
						num = 1f;
					}
				}
				if (this._escapeLoop != null)
				{
					this._escapeLoop.src.volume = num;
				}
			}
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000FD824 File Offset: 0x000FBA24
		private void ClearEscapeLoop()
		{
			if (this._escapeLoop != null && this._escapeLoop.gameObject != null)
			{
				UnityEngine.Object.Destroy(this._escapeLoop.gameObject);
			}
			this.bgmChanged = false;
			BgmManager.instance.FadeIn();
			this._escapeLoop = null;
			this._bgmFadeTime.StopTimer();
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00003FDD File Offset: 0x000021DD
		public override void OnSkillGoalComplete(UseSkill skill)
		{
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000224F1 File Offset: 0x000206F1
		public void SetApostleData(List<ApostleData> data)
		{
			this.apostleData = data;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00013CB1 File Offset: 0x00011EB1
		public override bool HasRoomCounter()
		{
			return true;
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000FD88C File Offset: 0x000FBA8C
		public override void ActivateQliphothCounter()
		{ // <Patch>
			PlaySpeedSettingUI.instance.SetNormalSpeedForcely();
			this.AnimScript.AdventClockUI.SetAdventEffectEndEvent(new AdventClockUI.EndEvent(this.Escape));
			if (this.apostleData.Count == 0)
			{
				string path = string.Concat(new object[]
				{
					Application.persistentDataPath,
					"/creatureData/",
					"100014",
					".dat"
				});
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(path, FileMode.Open);
				Dictionary<string, object> dic = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();
				this.LoadData(dic);
			}
			List<ApostleGenData> adeventTargets;
			if (this.apostles.Count == 0)
			{
				adeventTargets = DeathAngel.GetAdeventTargets(this.apostleData);
			}
			else
			{
				adeventTargets = this.genDataSave;
			}
			this.GenApostle(adeventTargets);
			this.AnimScript.AdventClockUI.StartSimpleAdventEvent();
			this.AnimScript.AdventClockUI.SimpleAdventStart(adeventTargets);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000FD914 File Offset: 0x000FBB14
		public override void OnFixedUpdate(CreatureModel creature)
		{
			if (this._qliphothSubTimer.started)
			{
				if (this.model.IsEscaped())
				{
					this._qliphothSubTimer.StopTimer();
				}
				if (this._qliphothSubTimer.RunTimer())
				{
					this.model.SubQliphothCounter();
					if (this.model.qliphothCounter > 0)
					{
						this._qliphothSubTimer.StartTimer(90f);
					}
				}
			}
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000FD988 File Offset: 0x000FBB88
		public override void Escape()
		{
			MapNode centerNode = this.CurrentSefira.sefiraPassage.centerNode;
			this.movable.SetCurrentNode(centerNode);
			this.model.Escape();
			this.AnimScript.SetTransform(DeathAngel._outPos, DeathAngel._outScale);
			this.AnimScript.OnEscape();
			this._escapeSkillTimer.StartTimer(DeathAngel._escapeSkillFreq);
			if (this.blockUI == null)
			{
				GameObject gameObject = Prefab.LoadPrefab("Effect/Creature/DeathAngel/DeathAngelBlock");
				this.blockUI = gameObject.GetComponent<DeathAngelPlaySpeedBlockUI>();
				this.blockUI.SetAngel(this);
				PlaySpeedSettingUI.instance.AddBlockedEvent(PlaySpeedSettingBlockType.WHITENIGHT, this.blockUI);
			}
			this._escapeLoop = this.MakeSoundLoop("creature/deathangel/Lucifer_standbg0");
			this._escapeLoop.transform.SetParent(CameraMover.instance.gameObject.transform);
			this._escapeLoop.transform.localPosition = new Vector3(0f, 0f, 0f);
			if (!this._isPrevEscaped)
			{
				this._isPrevEscaped = true;
			}
			else
			{
				foreach (DeathAngelApostle deathAngelApostle in this.apostles)
				{
					deathAngelApostle.ReEscaped();
				}
			}
			this.RoomEscapeSpriteOff();
			PlaySpeedSettingUI.instance.SetNormalSpeedForcely();
			PlaySpeedSettingUI.instance.UpdateButton();
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000FDB08 File Offset: 0x000FBD08
		public override void OnSuppressed()
		{
			foreach (DeathAngelApostle deathAngelApostle in this.apostles)
			{
				deathAngelApostle.OnDeathAngelSuppressed();
			}
			this._escapeSkillTimer.StopTimer();
			if (this.betrayer != null)
			{
				this.betrayer.OnDeathAngelSuppressed();
			}
			PlaySpeedSettingUI.instance.RemoveBlockedEvent(PlaySpeedSettingBlockType.WHITENIGHT);
			this.blockUI = null;
			this.AnimScript.OnSetSuppressEffect(false);
			this.ClearEscapeLoop();
			BgmManager.instance.ReleaseRecoverBlock();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000FDBB4 File Offset: 0x000FBDB4
		public void OnSuppressedByDamage()
		{
			if (InventoryModel.Instance.CheckEquipmentCount(200015))
			{
				EquipmentModel equipmentModel = InventoryModel.Instance.CreateEquipment(200015);
				DeathAngel.Log("Make Weapon", true);
			}
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000224FA File Offset: 0x000206FA
		public void OnSuppressedByConfess(OneBadManyGood oneBad)
		{
			this._confessDead.StartTimer(0.3f);
			this._escapeSkillTimer.StopTimer();
			this.AnimScript.OnSetSuppressEffect(true);
			this.oneBadManyGood = oneBad;
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000FDBF0 File Offset: 0x000FBDF0
		private void ActivateWorkSkill(CreatureFeelingState feelingState)
		{
			if (feelingState != CreatureFeelingState.GOOD)
			{
				if (feelingState != CreatureFeelingState.NORM)
				{
					if (feelingState == CreatureFeelingState.BAD)
					{
						this.AnimScript.StartWorkSkill(new DeathAngelVoidAction(this.DamageSefira));
					}
				}
				else
				{
					this.AnimScript.StartWorkSkill(new DeathAngelVoidAction(this.RecoverSefira));
					this.MakeSound("creature/WhiteNight/WhiteNight_Heal");
				}
			}
			else
			{
				this.AnimScript.StartWorkSkill(new DeathAngelVoidAction(this.RecoverAllFacility));
			}
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000FDC78 File Offset: 0x000FBE78
		private void RecoverList(List<AgentModel> targets, float factor)
		{
			foreach (AgentModel agentModel in targets)
			{
				agentModel.RecoverMental((float)agentModel.maxMental * factor);
				agentModel.RecoverHP((float)agentModel.maxHp * factor);
			}
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000FDCE8 File Offset: 0x000FBEE8
		private void RecoverAllFacility()
		{
			List<AgentModel> list = new List<AgentModel>();
			foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
			{
				if (agentModel.IsAttackTargetable())
				{
					if (!agentModel.IsDead())
					{
						if (!agentModel.IsPanic() && !agentModel.IsCrazy())
						{
							if (agentModel.unconAction == null)
							{
								list.Add(agentModel);
							}
						}
					}
				}
			}
			this.RecoverList(list, 1f);
			this.model.AddQliphothCounter();
			this.MakeSound("creature/WhiteNight/WhiteNight_Heal");
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000FDDBC File Offset: 0x000FBFBC
		private void RecoverSefira()
		{
			List<AgentModel> list = new List<AgentModel>();
			foreach (AgentModel agentModel in this.CurrentSefira.agentList)
			{
				if (agentModel.IsAttackTargetable())
				{
					if (!agentModel.IsDead())
					{
						if (!agentModel.IsPanic() && !agentModel.IsCrazy())
						{
							if (agentModel.unconAction == null)
							{
								list.Add(agentModel);
							}
						}
					}
				}
			}
			this.RecoverList(list, 0.5f);
			if (DeathAngel.Prob(0.4f))
			{
				this.model.AddQliphothCounter();
			}
			this.MakeSound("creature/WhiteNight/WhiteNight_Heal");
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000FDEA0 File Offset: 0x000FC0A0
		private void DamageSefira()
		{
			List<AgentModel> list = new List<AgentModel>();
			foreach (AgentModel agentModel in this.CurrentSefira.agentList)
			{
				if (agentModel.IsAttackTargetable())
				{
					if (!agentModel.IsDead())
					{
						if (!agentModel.IsPanic() && !agentModel.IsCrazy())
						{
							if (agentModel.unconAction == null)
							{
								list.Add(agentModel);
							}
						}
					}
				}
			}
			foreach (AgentModel agentModel2 in list)
			{
				agentModel2.TakeDamage(this.model, DeathAngel._badDamageInfo);
			}
			this.model.SubQliphothCounter();
			this.MakeSound("creature/WhiteNight/WhiteNight_Shout");
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x000FDFBC File Offset: 0x000FC1BC
		private void EscapeSkill()
		{
			foreach (DeathAngelApostle deathAngelApostle in this.apostles)
			{
				deathAngelApostle.Resurrect();
			}
			this.AnimScript.ActivateSkill();
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000FE024 File Offset: 0x000FC224
		private void MakeOverload(int overloadCount = 12)
		{
			CreatureOverloadManager.instance.ActivateOverload(overloadCount, OverloadType.DEFAULT, 60f, true, true, false, new long[]
			{
				100009L
			});
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000FE058 File Offset: 0x000FC258
		public List<string> GetApostleNames()
		{
			List<string> list = new List<string>();
			foreach (ApostleData apostleData in this.apostleData)
			{
				try
				{
					AgentName name = AgentNameList.instance.GetName(apostleData.NameId);
					list.Add(name.GetName());
				}
				catch (NullReferenceException message)
				{
					Debug.LogError(message);
					list.Add(apostleData.Name);
				}
			}
			return list;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000FE0FC File Offset: 0x000FC2FC
		public void GenApostle(List<ApostleGenData> genDataList)
		{
			if (this.apostles.Count == 0)
			{
				this.genDataSave = genDataList;
				int count = genDataList.Count;
				for (int i = 0; i < count; i++)
				{
					ApostleGenData apostleGenData = genDataList[i];
					ApostleType apostleType = ApostleStaticInfo.GetApostleType(apostleGenData.index);
					string empty = string.Empty;
					string empty2 = string.Empty;
					ApostleStaticInfo.GetApostleGenInfo(apostleType, out empty, out empty2);
					if (apostleType == ApostleType.BETRAYER)
					{
						WorkerModel target = apostleGenData.target;
						DeathAngelBetrayerBuf buf = new DeathAngelBetrayerBuf(this, apostleGenData.data);
						target.AddUnitBuf(buf);
						this.betrayer = buf;
					}
					else
					{
						long instID = this.model.AddChildCreatureModel(empty, empty2);
						ChildCreatureModel childCreatureModel = this.model.GetChildCreatureModel(instID);
						childCreatureModel.Unit.gameObject.SetActive(false);
						childCreatureModel.GetMovableNode().Assign(apostleGenData.target.GetMovableNode());
						childCreatureModel.Unit.transform.position = apostleGenData.target.GetWorkerUnit().transform.position;
						childCreatureModel.Unit.transform.localScale = Vector3.one * apostleGenData.target.GetMovableNode().currentScale;
						apostleGenData.AposlteModel = childCreatureModel;
						DeathAngelApostle deathAngelApostle = childCreatureModel.script as DeathAngelApostle;
						if (i == ApostleStaticInfo.GuardApostleAry[0] || i == ApostleStaticInfo.GuardApostleAry[1])
						{
							deathAngelApostle.statType = ApostleStatType.GUARDIAN;
						}
						this.apostles.Add(deathAngelApostle);
						deathAngelApostle.SetAngel(this);
						deathAngelApostle.data = apostleGenData.data;
						deathAngelApostle.Index = i;
					}
				}
			}
			else
			{
				this.SecondAdvent();
			}
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x0002252A File Offset: 0x0002072A
		public void GiveGlobalDamage(UnitModel target)
		{
			if (target.IsAttackTargetable())
			{
				target.TakeDamage(this.model, DeathAngel._escapeSkillDamage.Copy());
			}
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x0002254D File Offset: 0x0002074D
		public override void OnTakeDamage(UnitModel actor, DamageInfo dmg, float value)
		{
			if (this.oneBadManyGood != null)
			{
				return;
			}
			if (this.model.hp <= 0f)
			{
				this.OnSuppressedByDamage();
			}
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x00022576 File Offset: 0x00020776
		public bool IsKilledByConfess()
		{
			return this.oneBadManyGood != null;
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000FE2A0 File Offset: 0x000FC4A0
		public string GetFunctionDesc(string key)
		{
			string empty = string.Empty;
			CreatureStaticData.ParameterData param = this.model.metaInfo.creatureStaticData.GetParam(key);
			return param.desc;
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x00022584 File Offset: 0x00020784
		public override void OnGamemanagerInit()
		{ // <Mod>
			IList<AgentModel> agentList = AgentManager.instance.GetAgentList();
			foreach (ApostleData apostleData in apostleData)
			{
				foreach (AgentModel agentModel in agentList)
				{
					if (apostleData.instId == agentModel.instanceId && apostleData.NameId == agentModel._agentName.id)
					{
						if (!agentModel.HasEquipment(PlagueDoctor.giftId))
						{
							EGOgiftModel gift = InventoryModel.Instance.CreateEquipmentForcely(PlagueDoctor.giftId) as EGOgiftModel;
							agentModel.AttachEGOgift(gift);
						}
						break;
					}
				}
			}
			if (PlayerModel.instance.GetOpenedAreaCount() > 1 && PlayerModel.instance.GetDay() > 5)
			{
				this.ChangeIsolateRoom();
			}
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000FE2D4 File Offset: 0x000FC4D4
		public void ChangeIsolateRoom()
		{ // <Mod>
			if (SpecialModeConfig.instance.GetValue<bool>("WhiteNightImmobilize"))
			{
				return;
			}
			Sefira sefira = this.model.sefira;
			CreatureModel creatureModel = CreatureManager.instance.PickOtherSefiraCreatureByRandom(this.model);
			if (creatureModel != null)
			{
				Sefira sefira2 = creatureModel.sefira;
				CreatureManager.instance.ChangeCreaturePos(this.model, creatureModel);
				Sefira sefira3 = sefira2;
				creatureModel.sefira = sefira;
				this.model.sefira = sefira3;
				base.Unit.room.TurnOffRoomLight();
				creatureModel.Unit.room.TurnOffRoomLight();
				DeathAngel.Log("Change Room With " + creatureModel.metaInfo.name, false);
			}
			else
			{
				DeathAngel.Log("Error To Pick Creature", true);
			}
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x000225AC File Offset: 0x000207AC
		public override void AddedQliphothCounter()
		{
			this.CheckStateSprite();
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000225AC File Offset: 0x000207AC
		public override void ReducedQliphothCounter()
		{
			this.CheckStateSprite();
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000FE380 File Offset: 0x000FC580
		private void CheckStateSprite()
		{
			int qliphothCounter = this.model.qliphothCounter;
			if (qliphothCounter != 3)
			{
				if (qliphothCounter != 2)
				{
					if (qliphothCounter == 1)
					{
						base.Unit.room.StateFilter.renderSprite = this.badState;
					}
				}
				else
				{
					base.Unit.room.StateFilter.renderSprite = this.normalState;
				}
			}
			else
			{
				base.Unit.room.StateFilter.renderSprite = this.goodState;
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000FE414 File Offset: 0x000FC614
		public override void RoomSpriteInit()
		{
			Sprite sprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/Escape/" + this.model.metadataId);
			Sprite sprite2 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/Skill/" + this.model.metadataId);
			Sprite sprite3 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/State/" + this.model.metadataId + "/bad");
			Sprite sprite4 = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/State/" + this.model.metadataId + "/normal");
			Sprite renderSprite = Resources.Load<Sprite>("Sprites/CreatureSprite/Isolate/State/" + this.model.metadataId + "/good");
			this.badState = sprite3;
			this.normalState = sprite4;
			this.goodState = renderSprite;
			base.Unit.room.StateFilter.spriteRenderer.sortingOrder = 12;
			try
			{
				if (sprite != null)
				{
					base.Unit.room.EscapeFilter.renderSprite = sprite;
				}
				if (sprite2 != null)
				{
					base.Unit.room.StateFilter.renderSprite = sprite2;
				}
				base.Unit.room.StateFilter.renderSprite = renderSprite;
			}
			catch (Exception ex)
			{
			}
			this.RoomStateSpriteOn();
		}

		// <Patch>
		public void LoadData(Dictionary<string, object> dic)
		{
			int num = -1;
			this.apostleData.Clear();
			if (!GameUtil.TryGetValue<int>(dic, "apostleListCount", ref num))
			{
				PlagueDoctor.Log("Failed To ApostleData", true);
				return;
			}
			if (num != -1)
			{
				Dictionary<int, Dictionary<string, object>> data = null;
				if (GameUtil.TryGetValue<Dictionary<int, Dictionary<string, object>>>(dic, "apostleList", ref data))
				{
					this.LoadApostleSaveData(num, data);
				}
			}
		}

		// <Patch>
		private void LoadApostleSaveData(int max, Dictionary<int, Dictionary<string, object>> data)
		{
			for (int i = 0; i < max; i++)
			{
				Dictionary<string, object> data2 = null;
				if (data.TryGetValue(i, out data2))
				{
					ApostleData apostleData = new ApostleData(data2);
					this.apostleData.Add(apostleData);
					PlagueDoctor.Log("Load Apostle : " + apostleData.NameId.ToString(), false);
				}
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000FE580 File Offset: 0x000FC780
		// Note: this type is marked as 'beforefieldinit'.
		static DeathAngel()
		{
		}

		// Token: 0x040020D0 RID: 8400
		private const float _normalQliphothIncreaseProb = 0.4f;

		// Token: 0x040020D1 RID: 8401
		private const float _goodRecoverFactor = 1f;

		// Token: 0x040020D2 RID: 8402
		private const float _normRecoverFactor = 0.5f;

		// Token: 0x040020D3 RID: 8403
		private const float _qliphothSubTime = 90f;

		// Token: 0x040020D4 RID: 8404
		private static MinMax _escapeFreqRange = new MinMax(60f, 60f);

		// Token: 0x040020D5 RID: 8405
		private const float _confessDeadFreq = 0.3f;

		// Token: 0x040020D6 RID: 8406
		private const string _blockUI = "Effect/Creature/DeathAngel/DeathAngelBlock";

		// Token: 0x040020D7 RID: 8407
		public const string healSound = "creature/WhiteNight/WhiteNight_Heal";

		// Token: 0x040020D8 RID: 8408
		public const string damageSound = "creature/WhiteNight/WhiteNight_Shout";

		// Token: 0x040020D9 RID: 8409
		public const string outSound = "creature/WhiteNight/WhiteNight_Atk";

		// Token: 0x040020DA RID: 8410
		public const string suppressByHit = "creature/deathangel/Lucifer_yaduafinish_poof";

		// Token: 0x040020DB RID: 8411
		public const string suppress_confess_0 = "creature/WhiteNight/WhiteNight_Dead1";

		// Token: 0x040020DC RID: 8412
		public const string suppress_confess_1 = "creature/WhiteNight/WhiteNight_Dead2";

		// Token: 0x040020DD RID: 8413
		public const string suppress_confess_2 = "creature/WhiteNight/WhiteNight_Dead3";

		// Token: 0x040020DE RID: 8414
		public const string bgm_escape = "creature/deathangel/Lucifer_standbg0";

		// Token: 0x040020DF RID: 8415
		private const int QliphothMax = 3;

		// Token: 0x040020E0 RID: 8416
		private const int apostleCount = 12;

		// Token: 0x040020E1 RID: 8417
		private static DamageInfo _badDamageInfo = new DamageInfo(RwbpType.W, 50, 60);

		// Token: 0x040020E2 RID: 8418
		private static Vector3 _roomPos = new Vector3(0.06f, -0.59f, 0f);

		// Token: 0x040020E3 RID: 8419
		private static Vector3 _roomScale = new Vector3(0.85f, 0.85f, 1f);

		// Token: 0x040020E4 RID: 8420
		private static Vector3 _outPos = new Vector3(0.06f, 1.5f, 0f);

		// Token: 0x040020E5 RID: 8421
		private static Vector3 _outScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040020E6 RID: 8422
		private static DamageInfo _confessDeadDamage = new DamageInfo(RwbpType.P, 3330f);

		// Token: 0x040020E7 RID: 8423
		private static DamageInfo _escapeSkillDamage = new DamageInfo(RwbpType.P, 40f);

		// Token: 0x040020E8 RID: 8424
		private Sefira _currentSefira;

		// Token: 0x040020E9 RID: 8425
		private List<ApostleData> apostleData = new List<ApostleData>();

		// Token: 0x040020EA RID: 8426
		private List<DeathAngelApostle> apostles = new List<DeathAngelApostle>();

		// Token: 0x040020EB RID: 8427
		private List<ApostleGenData> genDataSave;

		// Token: 0x040020EC RID: 8428
		private Timer _qliphothSubTimer = new Timer();

		// Token: 0x040020ED RID: 8429
		private UnscaledTimer _confessDead = new UnscaledTimer();

		// Token: 0x040020EE RID: 8430
		private OneBadManyGood oneBadManyGood;

		// Token: 0x040020EF RID: 8431
		private DeathAngelPlaySpeedBlockUI blockUI;

		// Token: 0x040020F0 RID: 8432
		private DeathAngelBetrayerBuf betrayer;

		// Token: 0x040020F1 RID: 8433
		private Sprite badState;

		// Token: 0x040020F2 RID: 8434
		private Sprite normalState;

		// Token: 0x040020F3 RID: 8435
		private Sprite goodState;

		// Token: 0x040020F4 RID: 8436
		private CreatureCameraUtil.CreatureCameraUtil _escapeSense;

		// Token: 0x040020F5 RID: 8437
		private Timer _escapeSkillTimer = new Timer();

		// Token: 0x040020F6 RID: 8438
		private bool bgmChanged;

		// Token: 0x040020F7 RID: 8439
		private SoundEffectPlayer _escapeLoop;

		// Token: 0x040020F8 RID: 8440
		private Timer _bgmFadeTime = new Timer();

		// Token: 0x040020F9 RID: 8441
		private bool _isPrevEscaped;

		// Token: 0x040020FA RID: 8442
		private DeathAngelAnim _animScript;

		// Token: 0x040020FB RID: 8443
		[CompilerGenerated]
		private static Comparison<ApostleGenData> cache0;
	}
}
