using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachController : MonoBehaviour
{
    public float approachSpeed = 0.5f;

    public bool Reflected { get; set; } = false;

    void Update()
    {
        float scaleChange = (Reflected ? 1.5f : -1f) * approachSpeed * Time.deltaTime;
        transform.localScale += (Vector3)Vector2.one * scaleChange;
        Color color = GetComponent<SpriteRenderer>().color;
        float before = color.a;
        if (!Reflected && transform.localScale.x < 0.3f)
        {
            scaleChange *= -2f;
        }
        color.a = Util.ToBounds(color.a - scaleChange * 2f, 0f, 1f);
        GetComponent<SpriteRenderer>().color = color;
        if (color.a == 0f)
        {
            Object.Destroy(gameObject);
        }
    }
}
