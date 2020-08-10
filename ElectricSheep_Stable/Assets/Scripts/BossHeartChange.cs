using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeartChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite bossHeartCurrent;
    public bool inRoom = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        bossHeartCurrent = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = bossHeartCurrent;
    }
}
