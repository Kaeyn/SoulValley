using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class QuestManager : MonoBehaviourPunCallbacks
{
    Dictionary<string,Quest> questMap;
    int playerCurrentGold;
    Dictionary<string, Quest> CreateQuestMap(){
        QuestData[] allQuests = Resources.LoadAll<QuestData>("Quests"); 
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach(QuestData questData in allQuests){
            idToQuestMap.Add(questData.id,new Quest(questData));
        }
        return idToQuestMap;
    }
    private void OnEnable() {
        GameEventManager.instance.questEvent.onStartQuest += StartQuest;
        GameEventManager.instance.questEvent.onAdvanceQuest += AdvanceQuest;
        GameEventManager.instance.questEvent.onFinishQuest += FinishQuest;
    }
    private void OnDisable() {
        GameEventManager.instance.questEvent.onStartQuest -= StartQuest;
        GameEventManager.instance.questEvent.onAdvanceQuest -= AdvanceQuest;
        GameEventManager.instance.questEvent.onFinishQuest -= FinishQuest;
    }
    
    void ChangeQuestState(string id, QuestState state){
        Quest quest = GetQuestByID(id);
        quest.state = state;
        GameEventManager.instance.questEvent.QuestStateChange(quest);
    }
    public Quest GetQuestByID(string id){
        return questMap[id];
    }
    void StartQuest(string id){
        Quest quest = GetQuestByID(id);
        GameObject questStepPrefab = quest.GetCurrentQuestStepPrefab();
        if(questStepPrefab != null){
            GameObject questStep = PhotonNetwork.Instantiate(questStepPrefab.name,transform.position,Quaternion.identity);
            photonView.RPC("SetUpQuestStep", RpcTarget.AllBufferedViaServer, 
                    questStep.GetComponent<PhotonView>().ViewID, gameObject.GetComponent<PhotonView>().ViewID,id);
        }
        ChangeQuestState(id,QuestState.In_Progress);
    }
    void AdvanceQuest(string id){
        Quest quest = GetQuestByID(id);
        quest.MoveToNextStep();
        if(quest.CurrentStepExists()) 
        {
            GameObject questStepPrefab = quest.GetCurrentQuestStepPrefab();
            if(questStepPrefab != null){
                GameObject questStep = PhotonNetwork.Instantiate(questStepPrefab.name,transform.position,Quaternion.identity);
                photonView.RPC("SetUpQuestStep", RpcTarget.AllBufferedViaServer, 
                        questStep.GetComponent<PhotonView>().ViewID, gameObject.GetComponent<PhotonView>().ViewID,id);
            }
        }
        else
            ChangeQuestState(id,QuestState.Can_Finish);

    }
    void FinishQuest(string id){
        Quest quest = GetQuestByID(id);
        ClaimReward(quest);
        ChangeQuestState(id,QuestState.Finished);
    }
    void ClaimReward(Quest quest){
        Debug.Log("Congrate! U earn "+ quest.data.goldReward+" gold.");
    }
    bool checkRequirements(Quest quest){
        bool meetRequirement = true;
        // if(playerCurrentGold < quest.data.goldRequirement){
        //     meetRequirement = false; 
        // }
        foreach (QuestData prerequisiteQuestData in quest.data.questPrerequisites)
        {
            if(GetQuestByID(prerequisiteQuestData.id).state != QuestState.Finished){
                meetRequirement = false; 
                break;
            }
        }
        return meetRequirement;
    }
    private void Awake() {
        questMap = CreateQuestMap();
    }
    private void Start() {
        foreach (Quest quest in questMap.Values)
        {
            GameEventManager.instance.questEvent.QuestStateChange(quest);
        }
    }
    private void Update() {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.Requirement_Not_Met && checkRequirements(quest)){
                ChangeQuestState(quest.data.id,QuestState.Can_Start);
            }
        }
    }
    [PunRPC]
    public void SetUpQuestStep(int ViewID,int ViewIDTranform, string questID)
    {
        // Set the parent for the instantiated GameObject on non-master clients
        GameObject questStep = PhotonView.Find(ViewID).gameObject;
        questStep.GetComponent<QuestStep>().InstantiateQuestStep(questID);
        questStep.transform.SetParent(PhotonView.Find(ViewIDTranform).transform);
        
    }
    
}

