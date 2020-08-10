using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretDeath : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float framesPerSecond;

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
        index %= sprites.Length;
        spriteRenderer.sprite = sprites[index];

    }
}
