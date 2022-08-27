using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpVel = 5.0f;

    new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 vel = rigidbody.velocity;
        vel.x = Input.GetAxis("Move") * speed;
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            vel.y = jumpVel;
        }
        rigidbody.velocity = vel;
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
