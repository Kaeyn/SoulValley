using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueTrigger trigger;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") == true && Input.GetKeyDown(KeyCode.F))
            trigger.StartDialogue();
    }
}
