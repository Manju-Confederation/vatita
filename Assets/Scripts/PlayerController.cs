using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpVel = 5.0f;

    new Rigidbody2D rigidbody;

    void Awake()
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
