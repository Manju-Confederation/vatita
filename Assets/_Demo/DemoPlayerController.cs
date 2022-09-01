using UnityEngine;

public class DemoPlayerController : MonoBehaviour
{
    public float speed = 8.0f;
    public float jumpVel = 15.5f;

    new Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 vel = rigidbody.velocity;
        vel.x = Input.GetAxis("Move") * speed;
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            vel.y += jumpVel;
        }
        rigidbody.velocity = vel;
        if (vel.x != 0f)
        {
            transform.GetComponentInChildren<SpriteRenderer>().flipX = rigidbody.velocity.x < 0;
        }
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
        float distance = transform.localScale.y * GetComponent<CapsuleCollider2D>().size.y * 0.6f;
        return Physics2D.Raycast(
            transform.position,
            Vector2.down,
            distance,
            LayerMask.GetMask("Ground")
        );
    }
}
