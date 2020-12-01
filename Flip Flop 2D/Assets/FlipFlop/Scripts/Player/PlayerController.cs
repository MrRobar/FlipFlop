using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    public GameObject startText;

    public Level level;

    public Sprite deathSprite;

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    public SoundManager soundManager;

    public static PlayerController instance;

    private bool top;

    private bool facingRight = true;

    private State state;

    private enum State 
    {
        WaitingToStart,
        Playing,
        Dead
    }
    
    void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
        startText.SetActive(true);

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    
    void Update()
    {
        switch (state) 
        {
            default:
            case State.WaitingToStart:
                if (Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    startText.SetActive(false);
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    ChangeGravity();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                    level.PlayMusic();
                }
                break;
            case State.Playing:
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeGravity();
                }
                break;
            case State.Dead:
                
                break;
        }
        
        
    }

    void Rotation() 
    {
            if (top == false)
            {
                transform.eulerAngles = new Vector3(0, 0, 180f);
            }
            else
            {
                transform.eulerAngles = Vector3.zero;
            }
            Flip();
            top = !top;
        
    }

    private void Flip() 
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void ChangeGravity() 
    {
       
        rb.gravityScale *= -1f;
        soundManager.PlaySound(SoundManager.Sounds.Jump);
        Rotation();
        
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.tag == "Obstacle") 
        {
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<SpriteRenderer>().sprite = deathSprite;
            GetComponent<PlayerController>().enabled = false;
            soundManager.PlaySound(SoundManager.Sounds.Lose);
            if (OnDied != null) OnDied(this, EventArgs.Empty);
            level.StopMusic();
        }
        if(collider.tag == "Crystall") 
        {
            soundManager.PlaySound(SoundManager.Sounds.Crystall);
            level.crystallCount++;
            level.crystallList.Remove(collider.transform);
            Destroy(collider.gameObject);
        }
        
    }


}
