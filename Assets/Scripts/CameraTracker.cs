using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public GameObject leftBound;
    public GameObject rightBound;

    float leftBoundX;
    float rightBoundX;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float cameraHalfWidth = camera.orthographicSize * camera.aspect;
        if (leftBound)
        {
            leftBoundX = leftBound.transform.position.x + leftBound.transform.localScale.x * 0.5f + cameraHalfWidth;
        }
        if (rightBound)
        {
            rightBoundX = rightBound.transform.position.x - rightBound.transform.localScale.x * 0.5f - cameraHalfWidth;
        }
    }

    void Update()
    {
        Vector2 subjectPos = transform.parent.position;
        float targetX = subjectPos.x + 5;
        if (rightBound)
        {
            targetX = Mathf.Min(targetX, rightBoundX);
        }
        if (leftBound)
        {
            targetX = Mathf.Max(targetX, leftBoundX);
        }
        float targetY = subjectPos.y;
        RaycastHit2D groundRaycast = Physics2D.Raycast(subjectPos, Vector2.down, 5f, LayerMask.GetMask("Ground"));
        if (groundRaycast)
        {
            targetY = groundRaycast.point.y + 2;
        }
        transform.position = new Vector3(targetX, targetY, -10);
    }
}
