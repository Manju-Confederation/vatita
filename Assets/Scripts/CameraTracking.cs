using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public GameObject leftBound;
    public GameObject rightBound;

    float leftBoundX;
    float rightBoundX;
    new Camera camera;

    public Vector2 Origin
    {
        get => new(
            transform.position.x - camera.orthographicSize * camera.aspect,
            transform.position.y - camera.orthographicSize
        );
    }
    public Vector2 Size
    {
        get => new(
            camera.orthographicSize * camera.aspect * 2,
            camera.orthographicSize * 2
        );
    }

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Start()
    {
        float cameraHalfWidth = camera.orthographicSize * camera.aspect;
        if (leftBound)
        {
            leftBoundX = leftBound.transform.position.x + leftBound.transform.localScale.x * 0.5f + cameraHalfWidth;
        }
        if (rightBound)
        {
            rightBoundX = rightBound.transform.position.x - rightBound.transform.localScale.x * 0.5f - cameraHalfWidth;
        }
        Update();
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
        transform.position = new Vector3(targetX, targetY, -20);
    }
}
