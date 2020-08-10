using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float framesPerSecond;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update 
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.enabled = true;
    }

    // Update is called once per frame 
    void Update()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index = index % sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }
}
