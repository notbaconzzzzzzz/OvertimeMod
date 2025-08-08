using System;

// Token: 0x02000378 RID: 888
public class NoticeName
{
	// Token: 0x06001B52 RID: 6994 RVA: 0x00004230 File Offset: 0x00002430
	public NoticeName()
	{
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000E46BC File Offset: 0x000E28BC
	public static string MakeName(string noticeName, params string[] param)
	{
		string text = noticeName;
		foreach (string str in param)
		{
			text = text + "_" + str;
		}
		return text;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x000E46F0 File Offset: 0x000E28F0
	static NoticeName()
	{
	}

	// Token: 0x04001BD4 RID: 7124
	public static string OnChangeCameraSize = "OnChangeCameraSize";

	// Token: 0x04001BD5 RID: 7125
	public static string FixedUpdate = "FixedUpdate";

	// Token: 0x04001BD6 RID: 7126
	public static string Update = "Update";

	// Token: 0x04001BD7 RID: 7127
	public static string MoveUpdate = "MoveUpdate";

	// Token: 0x04001BD8 RID: 7128
	public static string OnStageStart = "OnStageStart";

	// Token: 0x04001BD9 RID: 7129
	public static string OnStageEnd = "OnStageEnd";

	// Token: 0x04001BDA RID: 7130
	public static string OnLateUpdateCamera = "OnLateUpdateCamera";

	// Token: 0x04001BDB RID: 7131
	public static string OnInitGameManager = "OnInitGameManager";

	// Token: 0x04001BDC RID: 7132
	public static string OnReleaseGameManager = "OnReleaseGameManager";

	// Token: 0x04001BDD RID: 7133
	public static string EnergyTimer = "EnergyTimer";

	// Token: 0x04001BDE RID: 7134
	public static string AutoSaveTimer = "AutoSaveTimer";

	// Token: 0x04001BDF RID: 7135
	public static string AddNarrationLog = "AddNarrationLog";

	// Token: 0x04001BE0 RID: 7136
	public static string AreaOpenUpdate = "AreaOpenUpdate";

	// Token: 0x04001BE1 RID: 7137
	public static string AddPlayerLog = "AddPlayerLog";

	// Token: 0x04001BE2 RID: 7138
	public static string AddSystemLog = "AddSystemLog";

	// Token: 0x04001BE3 RID: 7139
	public static string UpdateEnergy = "UpdateEnergy";

	// Token: 0x04001BE4 RID: 7140
	public static string UpdateDay = "UpdateDay";

	// Token: 0x04001BE5 RID: 7141
	public static string AddNewAgent = "AddNewAgent";

	// Token: 0x04001BE6 RID: 7142
	public static string DeployAgent = "DeployAgent";

	// Token: 0x04001BE7 RID: 7143
	public static string AddOfficer = "AddOfficer";

	// Token: 0x04001BE8 RID: 7144
	public static string AddExternalWorker = "AddExternalWorker";

	// Token: 0x04001BE9 RID: 7145
	public static string RemoveAgent = "RemoveAgent";

	// Token: 0x04001BEA RID: 7146
	public static string RemoveOfficer = "RemoveOfficer";

	// Token: 0x04001BEB RID: 7147
	public static string RemoveExternalWorker = "RemoveExternalWorker";

	// Token: 0x04001BEC RID: 7148
	public static string ClearAgent = "ClearAgent";

	// Token: 0x04001BED RID: 7149
	public static string ClearOfficer = "ClearOfficer";

	// Token: 0x04001BEE RID: 7150
	public static string InitAgent = "InitAgent";

	// Token: 0x04001BEF RID: 7151
	public static string OnInitBufEffect = "OnInitBufEffect";

	// Token: 0x04001BF0 RID: 7152
	public static string OnDestroyBufEffect = "OnDestroyBufEffect";

	// Token: 0x04001BF1 RID: 7153
	public static string WorkEndReport = "WorkEndReport";

	// Token: 0x04001BF2 RID: 7154
	public static string ChangeAgentSefira = "ChangeAgentSefira";

	// Token: 0x04001BF3 RID: 7155
	public static string ChangeAgentState = "ChangeAgentState";

	// Token: 0x04001BF4 RID: 7156
	public static string AddCreature = "AddCreature";

	// Token: 0x04001BF5 RID: 7157
	public static string RemoveCreature = "RemoveCreature";

	// Token: 0x04001BF6 RID: 7158
	public static string ClearCreature = "ClearCreature";

	// Token: 0x04001BF7 RID: 7159
	public static string AddOrdealCreature = "AddOrdealCreature";

	// Token: 0x04001BF8 RID: 7160
	public static string RemoveOrdealCreature = "RemoveOrdealCreature";

	// Token: 0x04001BF9 RID: 7161
	public static string ClearOrdealCreature = "ClearOrdealCreature";

	// Token: 0x04001BFA RID: 7162
	public static string AddEventCreature = "AddEventCreature";

	// Token: 0x04001BFB RID: 7163
	public static string RemoveEventCreature = "RemoveEventCreature";

	// Token: 0x04001BFC RID: 7164
	public static string ClearEventCreature = "ClearEventCreature";

	// Token: 0x04001BFD RID: 7165
	public static string AddRabbit = "AddRabbit";

	// Token: 0x04001BFE RID: 7166
	public static string RemoveRabbit = "RemoveRabbit";

	// Token: 0x04001BFF RID: 7167
	public static string ClearRabbit = "ClearRabbit";

	// Token: 0x04001C00 RID: 7168
	public static string AddSefiraBossCreature = "AddSefiraBossCreature";

	// Token: 0x04001C01 RID: 7169
	public static string RemoveSefiraBossCreature = "RemoveSefiraBossCreature";

	// Token: 0x04001C02 RID: 7170
	public static string OnCreatureFeverStart = "OnCreatureFeverStart";

	// Token: 0x04001C03 RID: 7171
	public static string OnCreaturePhysicsAttack = "OnCreaturePhysicsAttack";

	// Token: 0x04001C04 RID: 7172
	public static string OnCreatureMentalAttack = "OnCreatureMentalAttack";

	// Token: 0x04001C05 RID: 7173
	public static string OnCreatureComplexAttack = "OnCreatureComplexAttack";

	// Token: 0x04001C06 RID: 7174
	public static string LoadMapGraphComplete = "LoadMapGraphComplete";

	// Token: 0x04001C07 RID: 7175
	public static string ResetMapGraph = "ResetMapGraph";

	// Token: 0x04001C08 RID: 7176
	public static string EscapeCreature = "EscapeCreature";

	// Token: 0x04001C09 RID: 7177
	public static string OnEscape = "OnEscape";

	// Token: 0x04001C0A RID: 7178
	public static string ReportAgentSuccess = "ReportAgentSuccess";

	// Token: 0x04001C0B RID: 7179
	public static string OnLoadingEnd = "OnLoadingEnd";

	// Token: 0x04001C0C RID: 7180
	public static string AddPassageObject = "AddPassageObject";

	// Token: 0x04001C0D RID: 7181
	public static string CreatureObserveLevelAdded = "CreatureObserveLevelAdded";

	// Token: 0x04001C0E RID: 7182
	public static string AddBloodMapObject = "AddBloodMapObject";

	// Token: 0x04001C0F RID: 7183
	public static string SetTeleportNode = "SetTeleportNode";

	// Token: 0x04001C10 RID: 7184
	public static string RemoveTeleportNode = "RemoveTeleportNode";

	// Token: 0x04001C11 RID: 7185
	public static string PassageBlackOut = "PassageBlackOut";

	// Token: 0x04001C12 RID: 7186
	public static string PassageWhitle = "PassageWhite";

	// Token: 0x04001C13 RID: 7187
	public static string PassageAlpha = "PassageAlpha";

	// Token: 0x04001C14 RID: 7188
	public static string LanaguageChange = "LanguageChange";

	// Token: 0x04001C15 RID: 7189
	public static string SefiraEnabled = "SefiraEnabled";

	// Token: 0x04001C16 RID: 7190
	public static string SefiraDisabled = "SefiraDisabled";

	// Token: 0x04001C17 RID: 7191
	public static string TutorialElementEnable = "TutorialElementEnable";

	// Token: 0x04001C18 RID: 7192
	public static string TutorialElementDisable = "TutorialElementDisable";

	// Token: 0x04001C19 RID: 7193
	public static string AddEtcUnit = "AddEtcUnit";

	// Token: 0x04001C1A RID: 7194
	public static string RemoveEtcUnit = "RemoveEtcUnit";

	// Token: 0x04001C1B RID: 7195
	public static string ClearEtcUnit = "ClearEtcUnit";

	// Token: 0x04001C1C RID: 7196
	public static string UnconWorkerDead = "UnconWorkerDead";

	// Token: 0x04001C1D RID: 7197
	public static string CreatureSuppressCancel = "CreatureSuppressCancel";

	// Token: 0x04001C1E RID: 7198
	public static string TurnOffUI = "TurnOffUI";

	// Token: 0x04001C1F RID: 7199
	public static string TurnOnUI = "TurnOnUI";

	// Token: 0x04001C20 RID: 7200
	public static string ManageCancel = "ManageCancel";

	// Token: 0x04001C21 RID: 7201
	public static string InitResearchItem = "InitResearchItem";

	// Token: 0x04001C22 RID: 7202
	public static string UpdateResearchItem = "UpdateResearchItem";

	// Token: 0x04001C23 RID: 7203
	public static string OpenArea = "OpenArea";

	// Token: 0x04001C24 RID: 7204
	public static string OnAgentPanic = "OnAgentPanic";

	// Token: 0x04001C25 RID: 7205
	public static string OnAgentPanicReturn = "OnAgentPanicReturn";

	// Token: 0x04001C26 RID: 7206
	public static string OnAgentHairChanged = "OnAgentHairChanged";

	// Token: 0x04001C27 RID: 7207
	public static string OnEventTimeReached = "OnEventTimeReached";

	// Token: 0x04001C28 RID: 7208
	public static string OnFailStage = "OnFailStage";

	// Token: 0x04001C29 RID: 7209
	public static string UpdateAgentState = "UpdateAgentState_";

	// Token: 0x04001C2A RID: 7210
	public static string OnAgentPromote = "OnAgentPromote";

	// Token: 0x04001C2B RID: 7211
	public static string OnMaxObserveSuccess = "OnMaxObserveSuccess";

	// Token: 0x04001C2C RID: 7212
	public static string OnCreatureSuppressed = "OnCreatureSuppressed";

	// Token: 0x04001C2D RID: 7213
	public static string OnAgentDead = "OnAgentDead";

	// Token: 0x04001C2E RID: 7214
	public static string OnOrdealActivated = "OnOrdealActivated";

	// Token: 0x04001C2F RID: 7215
	public static string OnEmergencyLevelChanged = "OnEmergencyLevelChanged";

	// Token: 0x04001C30 RID: 7216
	public static string OnDragStart = "OnDragStart";

	// Token: 0x04001C31 RID: 7217
	public static string OnWorkCoolTimeEnd = "OnWorkCoolTimeEnd";

	// Token: 0x04001C32 RID: 7218
	public static string OnReleaseWork = "OnReleaseWork";

	// Token: 0x04001C33 RID: 7219
	public static string OnMissionProgressed = "OnMissionProgressed";

	// Token: 0x04001C34 RID: 7220
	public static string OnNextDay = "OnNextDay";

	// Token: 0x04001C35 RID: 7221
	public static string OnInventoryAgentChanged = "OnInventoryAgentChanged";

	// Token: 0x04001C36 RID: 7222
	public static string OnWorkStart = "OnWorkStart";

	// Token: 0x04001C37 RID: 7223
	public static string OnChangeGift = "OnChangeGift";

	// Token: 0x04001C38 RID: 7224
	public static string OnOfficerDie = "OnOfficerDie";

	// Token: 0x04001C39 RID: 7225
	public static string OnOfficerPanic = "OnOfficerPanic";

	// Token: 0x04001C3A RID: 7226
	public static string OnQliphothOverloadLevelChanged = "OnQliphothOverloadLevelChanged";

	// Token: 0x04001C3B RID: 7227
	public static string OnIsolateOverloaded = "OnIsloateOverloaded";

	// Token: 0x04001C3C RID: 7228
	public static string OnIsolateOverloadCanceled = "OnIsolateOverloadCanceled";

	// Token: 0x04001C3D RID: 7229
	public static string OnDestroyBossCore = "OnDestroyBossCore";

	// Token: 0x04001C3E RID: 7230
	public static string WorkToOverloaded = "WorkToOverloaded";

	// Token: 0x04001C3F RID: 7231
	public static string OrdealEnd = "OrdealEnd";

	// Token: 0x04001C40 RID: 7232
	public static string MakeEquipment = "MakeEquipment";

	// Token: 0x04001C41 RID: 7233
	public static string RemoveEquipment = "RemoveEquipment";

	// Token: 0x04001C42 RID: 7234
	public static string RabbitCaptainConversation = "RabbitCaptainConversation";

	// Token: 0x04001C43 RID: 7235
	public static string RabbitProtocolActivated = "RabbitProtocolActivated";

	// Token: 0x04001C44 RID: 7236
	public static string UnmuteSefiraConversation = "UnmuteSefiraConversation";

	// Token: 0x04001C45 RID: 7237
	public static string OnProcessWorkTick = "OnProcessWorkTick";

	// Token: 0x04001C46 RID: 7238
	public static string OnResearchEnd = "OnResearchEnd";

	// Token: 0x04001C47 RID: 7239
	public static string OnWorkerReinforcementAccepted = "OnWorkerReinforcementAccepted";

	// Token: 0x04001C48 RID: 7240
	public static string OnChangeInventoryTap = "OnChangeInventoryTap";

	// Token: 0x04001C49 RID: 7241
	public static string OnClickStartGame = "OnClickStartGame";

	// Token: 0x04001C4A RID: 7242
	public static string OnResearchPanelDropped = "OnResearchPanelDropped";

	// Token: 0x04001C4B RID: 7243
	public static string OnTutorialCreatureArrived = "OnTutorialCreatureArrived";

	// Token: 0x04001C4C RID: 7244
	public static string OnCommandSuppress = "OnCommandSuppress";

	// Token: 0x04001C4D RID: 7245
	public static string HorrorDamage = "HorrorDamage";

	// Token: 0x04001C4E RID: 7246
	public static string WorkerAttackEnd = "WorkerAttackEnd";

	// Token: 0x04001C4F RID: 7247
	public static string OnClickRecallButton = "OnClickRecallButton";

	// Token: 0x04001C50 RID: 7248
	public static string OnAgentMoveCommand = "OnAgentMoveCommand";

	// Token: 0x04001C51 RID: 7249
	public static string OnClickNextDayAcceptInResult = "OnClickNextDayAcceptInResult";

	// Token: 0x04001C52 RID: 7250
	public static string KetherConversation = "KetherConversation";

	// Token: 0x04001C53 RID: 7251
	public static string ChangeKetherImage = "ChangeKetherImage";

	// Token: 0x04001C54 RID: 7252
	public static string OnOrdealStarted = "OnOrdealStarted";

	// Token: 0x04001C55 RID: 7253
	public static string OnGetEGOgift = "OnGetEGOgift";

	//> <Mod>
	public static string OnOpenNameplate = "OnOpenNameplate";

	public static string RecoverByRegenerator = "RecoverByRegenerator";

	public static string RecoverByBullet = "RecoverByBullet";

	public static string BlockDamageByShield = "BlockDamageByShield";

	public static string AddExcessEnergy = "AddExcessEnergy";

	public static string CreatureDamagedByAgent = "CreatureDamagedByAgent";

	public static string OnUseBullet = "OnUseBullet";

	public static string OnPause = "OnPause";

	public static string CreatureHitWorker = "CreatureHitWorker";

	public static string SetSystemLogSize = "SetSystemLogSize";
}
