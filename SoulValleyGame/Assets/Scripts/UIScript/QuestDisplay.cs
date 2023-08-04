using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDisplay : MonoBehaviour
{
    public TextMeshProUGUI title, detail;
    Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
    int max = 0, current = 0;
    Animator animator;
    string id;
    private void Start() {
        animator = GetComponent<Animator>();
        QuestData[] allQuests = Resources.LoadAll<QuestData>("Quests"); 
        foreach(QuestData questData in allQuests){
            idToQuestMap.Add(questData.id,new Quest(questData));
        }

    }
    void OnEnable()
    {
        GameEventManager.instance.questEvent.onSpawnQuest += SpawnQuest;
        GameEventManager.instance.questEvent.onImproveQuest += SetCurrentAmount;
        GameEventManager.instance.questEvent.onFinishQuest += finishQuest;
    }
    void OnDisable()
    {
        GameEventManager.instance.questEvent.onSpawnQuest -= SpawnQuest;
        GameEventManager.instance.questEvent.onImproveQuest -= SetCurrentAmount;
        GameEventManager.instance.questEvent.onFinishQuest -= finishQuest;
    }
    
    void SpawnQuest(string ids)
    {
        Debug.Log(ids);
        this.id = ids;
        SetQuestDisplay();
        animator.SetTrigger("start");
    }
    void finishQuest(string id)
    {
        animator.SetTrigger("finish");
        current = 0;
    }

    void SetQuestDisplay(){
        Quest quest = idToQuestMap[id];
        max = quest.GetCurrentQuestStepPrefab().GetComponent<CollectQuestStep>().amountRequire;
        title.SetText(quest.data.displayName);
        detail.SetText("Progress "+current.ToString()+" / "+max.ToString());
    }
    void SetCurrentAmount(int amount){
        current = amount;
        SetQuestDisplay();
    }
}
