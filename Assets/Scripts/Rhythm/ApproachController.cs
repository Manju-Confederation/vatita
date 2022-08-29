using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachController : MonoBehaviour
{
    public float approachSpeed = 0.5f;

    public bool Reflected { get; set; } = false;

    void Update()
    {
        float scaleChange = (Reflected ? 1 : -1) * approachSpeed * Time.deltaTime;
        transform.localScale += (Vector3) Vector2.one * scaleChange;
        bool shouldDestroy = Reflected
            ? transform.localScale.x > 0.7f
            : transform.localScale.x < 0.2f;
        if (shouldDestroy)
        {
            Object.Destroy(gameObject);
        }
        
    }
}
