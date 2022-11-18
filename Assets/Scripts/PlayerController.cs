using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpVel = 5.0f;
    public bool spriteFacingRight = true;

    new Rigidbody2D rigidbody;
    GameObject sprite;
    Animator animator;
    float jumpDelay = -1;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").gameObject;
        animator = sprite.GetComponent<Animator>();
    }

    void Start()
    {
        if (!spriteFacingRight)
        {
            FlipSprite();
        }
    }

    void Update()
    {
        Vector2 vel = rigidbody.velocity;
        float move = Input.GetAxis("Move");
        vel.x = move * speed;
        if (move != 0 && Mathf.Sign(move) != (spriteFacingRight ? 1 : -1))
        {
            FlipSprite();
        }
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            vel.y += jumpVel;
            jumpDelay = 20;
            animator.SetBool("Airborne", true);
        }
        else if (jumpDelay == 0 && CanJump())
        {
            jumpDelay--;
        }
        if (jumpDelay > 0 || jumpDelay == 0 && CanJump())
        {
            jumpDelay--;
        }
        rigidbody.velocity = vel;
        animator.SetBool("Moving", move != 0);
        animator.SetFloat("YVel", vel.y);
        animator.SetBool("Airborne", jumpDelay != -1);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        FadeController fade = collision.GetComponent<FadeController>();
        if (fade)
        {
            fade.Hide();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        FadeController fade = collision.GetComponent<FadeController>();
        if (fade)
        {
            fade.Show();
        }
    }

    bool CanJump()
    {
        return Physics2D.Raycast(
            transform.position,
            Vector2.down,
            transform.localScale.y * 0.6f,
            LayerMask.GetMask("Ground")
        );
    }

    void FlipSprite()
    {
        Vector3 scale = sprite.transform.localScale;
        sprite.transform.localScale = new(-scale.x, scale.y, 1);
        spriteFacingRight = !spriteFacingRight;
    }
}
