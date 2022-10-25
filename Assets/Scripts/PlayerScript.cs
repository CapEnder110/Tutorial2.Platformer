using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text health;
    public GameObject win;
    public GameObject lose;
    private int scoreValue = 0;
    private int healthValue = 3;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;
    Animator anim;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        health.text = "Health: " + healthValue.ToString();
        win.SetActive(false);
        lose.SetActive(false);
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }
        if (healthValue == 0)
        {
            anim.SetInteger("State", 3);
        }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        
        if (scoreValue == 10)
        {
            win.SetActive(true);
        }
        if (healthValue == 0)
        {
            lose.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            musicSource.clip = musicClipTwo;
            musicSource.Play();

            if (scoreValue == 5)
            {
                transform.position = new Vector3(55.0f, -2.0f, 0.0f);
                healthValue = 3;
                health.text = "Health: " + healthValue.ToString();
            }
            if (scoreValue == 10)
            {
                musicSource.clip = musicClipThree;
                musicSource.Play();
            }
        }
        else if (collision.collider.tag == "Enemy")
        {
            healthValue -= 1;
            health.text = "Health: " + healthValue.ToString();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
    
    void Flip()
    {
         facingRight = !facingRight;
         Vector2 Scaler = transform.localScale;
         Scaler.x = Scaler.x * -1;
         transform.localScale = Scaler;
     }
}
