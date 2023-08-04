using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class QuestStep :MonoBehaviourPunCallbacks
{
    bool isFinished = false;
    protected string questID;

    public void InstantiateQuestStep(string questID){
        this.questID = questID;
        GameEventManager.instance.questEvent.SpawnQuest(questID);
    } 
    protected void FinishQuestStep(){
        if(!isFinished){
            isFinished = true;
            GameEventManager.instance.questEvent.AdvanceQuest(questID);
            Destroy(this.gameObject);
        }
    }
    
}
