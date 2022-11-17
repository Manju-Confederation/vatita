using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpVel = 5.0f;
    public float spriteScale = 0.2f;
    public bool spriteFacingRight = true;

    new Rigidbody2D rigidbody;
    GameObject sprite;
    Animator animator;
    bool jumping;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").gameObject;
        animator = sprite.GetComponent<Animator>();
    }

    void Start()
    {
        sprite.transform.localScale = new(spriteScale * (spriteFacingRight ? 1 : -1), spriteScale, 1);
    }

    void Update()
    {
        Vector2 vel = rigidbody.velocity;
        float move = Input.GetAxis("Move");
        vel.x = move * speed;
        if (move != 0)
        {
            sprite.transform.localScale = new(spriteScale * (spriteFacingRight ? 1 : -1) * Mathf.Sign(move), spriteScale, 1);
        }
        animator.SetBool("Moving", move != 0);
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            vel.y += jumpVel;
            jumping = true;
            animator.SetBool("Jumping", true);
        }
        else if (jumping && CanJump())
        {
            jumping = false;
            animator.SetBool("Jumping", false);
        }
        rigidbody.velocity = vel;
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
}
