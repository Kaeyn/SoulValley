using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Music2D")]
    [field: SerializeField] public EventReference music2D { get; private set; }

    [field: Header("LobbyMusic2D")]
    [field: SerializeField] public EventReference lobbymusic2D { get; private set; }
    // Start is called before the first frame update
    [field : Header("Collect SFX")]
    [field: SerializeField] public EventReference itemCollected { get; private set; }
    [field: Header("Clicked SFX")]
    [field: SerializeField] public EventReference clickedSound { get; private set; }

    [field: Header("Footsteps SFX")]
    [field: SerializeField] public EventReference footSteps { get; private set; }

    [field: Header("FootstepsSprint SFX")]
    [field: SerializeField] public EventReference footStepsSprint { get; private set; }

    [field: Header("Hoeing SFX")]
    [field: SerializeField] public EventReference hoeingSound { get; private set; }

    [field: Header("Watering SFX")]
    [field: SerializeField] public EventReference wateringSound { get; private set; }

    [field: Header("Harvest SFX")]
    [field: SerializeField] public EventReference harvestSound { get; private set; }

    [field: Header("TabSound SFX")]
    [field: SerializeField] public EventReference tabSound { get; private set; }

    [field: Header("QuestAccepted SFX")]
    [field: SerializeField] public EventReference questAcceptedSound { get; private set; }

    [field: Header("QuestCompleted SFX")]
    [field: SerializeField] public EventReference questCompletedSound { get; private set; }

    [field: Header("EmptyBucket SFX")]
    [field: SerializeField] public EventReference emptyBucketSound { get; private set; }

    [field: Header("FillWaterBucket SFX")]
    [field: SerializeField] public EventReference fillWaterBucketSound { get; private set; }

    [field: Header("SlimeWalk SFX")]
    [field: SerializeField] public EventReference slimeWalkSound { get; private set; }
    public static FMODEvents instance { get; private set; }
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager");
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
