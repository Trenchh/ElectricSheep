using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeTrigger : MonoBehaviour
{
    private bool done = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!done && collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<DialogueTrigger>().TriggerDialogue(1);
            done = true;
        }
    }
}
