using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAttack : MonoBehaviour
{
    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerAttack.healthCount == 1)
        {
            audioData.Play();
        }
    }
}
