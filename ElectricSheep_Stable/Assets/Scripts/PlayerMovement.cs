using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Color old;
    public float moveSpeed = 50.0f;
    public Camera cam;
    private Rigidbody2D rb;
    Vector2 mousePos;
    //private static bool playerExists;
    public Sprite[] spritesIdle;
    public Sprite[] spritesMoving;
    public float framesPerSecond;
    public float framesPerSecond2;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSrc;
    public AudioSource gameMusic;
    public GameObject musicSource;

    public AudioClip hurtClip;
    public AudioClip chargeGrab;
    public AudioClip balloon;
    public AudioClip[] music;
    public AudioClip bossSong;
    
    public string startPoint;
    public static bool bossMusic;

    private float moveThreshold = 0.5f;
    private bool horMove, vertMove;
    public static int seedIndex;


    private void Start()
    {
        bossMusic = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        old = GetComponent<SpriteRenderer>().color;
        spriteRenderer.enabled = true;
        audioSrc = GetComponent<AudioSource>();
        gameMusic = musicSource.GetComponent<AudioSource>();
        gameMusic.volume = 0.5f;
        seedIndex = (int)System.DateTime.Now.Ticks % RoomSpawner.seeds.Length;
        if (seedIndex < 0)
        {
            seedIndex = seedIndex * -1;
        }
        if (!gameMusic.playOnAwake)
        {
            int num = 0;
            Debug.Log(num);
            gameMusic.clip = music[num];
            gameMusic.Play();
        }
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FindObjectOfType<PauseMenu>().GameIsPaused = false;
        Time.timeScale = 1f;
        //Time.timeScale = 12.0f;
    }

    // Update is called once per frame
    void Update()
    {
        MusicManager();
        horMove = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > moveThreshold;
        vertMove = Mathf.Abs(Input.GetAxisRaw("Vertical")) > moveThreshold;
        if(!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            if (horMove)
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }

            if (vertMove)
            {
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }

            if (horMove || vertMove)
            {
                AnimateMoving();
            }
            else
            {
                //spriteRenderer.sprite = sprites[0];
                AnimateIdle();
            }
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void MusicManager()
    {
        if (FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            Debug.Log("bbbbbbbbbbbbbbbbbbb");
            gameMusic.Pause();
        }

        if (!bossMusic)
        {
            if (!gameMusic.isPlaying && !FindObjectOfType<PauseMenu>().GameIsPaused)
            {
                /*
                Debug.Log("eeeeeeeeeeeeeeee");
                AudioClip musicTemp = music[Random.Range(0, music.Length)];
                while (musicTemp == gameMusic.clip)
                {
                    musicTemp = music[Random.Range(0, music.Length)];
                }
                gameMusic.clip = musicTemp;
                */
                gameMusic.Play();
            }
        
        } else
        {            
            if (!gameMusic.isPlaying && !FindObjectOfType<PauseMenu>().GameIsPaused)
            {
                Debug.Log("WOW");
                gameMusic.clip = bossSong;
                gameMusic.Play();
            }
        }        
    }

    private void FixedUpdate()
    {
        if (!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    void AnimateIdle()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond2);
        index %= spritesIdle.Length;
        spriteRenderer.sprite = spritesIdle[index];
    }

    void AnimateMoving()
    {
        int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
        index %= spritesMoving.Length;
        spriteRenderer.sprite = spritesMoving[index];
    }

    public void Hit(Collider2D other)
    {
        audioSrc.PlayOneShot(hurtClip);
        StartCoroutine(Flasher(other));
    }

    public void ChargePickUp()
    {
        audioSrc.PlayOneShot(chargeGrab);
    }

    public void BalloonPop()
    {
        audioSrc.PlayOneShot(balloon);
    }

    IEnumerator Flasher(Collider2D other) 
    {
        for (int i = 0; i < 2; i++)
        {
            other.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(.1f);
            other.GetComponent<SpriteRenderer>().color = old;
            yield return new WaitForSeconds(.1f);
        }
    }


}
