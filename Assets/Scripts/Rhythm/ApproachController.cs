using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class ApproachController : MonoBehaviour
{
    public RhythmController controller;
    public RhythmController.HitData hitData;

    SpriteRenderer spriteRenderer;
    float scale;
    float spriteWidth;
    float spriteHeight;
    float hitPos = -1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteWidth = 2.5f / hitData.sprite.bounds.size.x;
        spriteHeight = 2.5f / hitData.sprite.bounds.size.y;
    }

    void Update()
    {
        float currentTime = controller.CurrentTime;
        if (hitPos == -1)
        {
            if (currentTime <= hitData.time + controller.window)
            {
                scale = Mathf.Max(
                    Mathf.LerpUnclamped(
                        4, 1,
                        (currentTime - hitData.showTime) / (hitData.time - hitData.showTime)
                    ),
                    0
                );
                spriteRenderer.color = new(1f, 1f, 1f, Mathf.InverseLerp(4, 2, scale));
            }
            else
            {
                transform.SetAsLastSibling();
                scale = Mathf.MoveTowards(scale, 0, 1 / controller.BeatTime(1) * Time.deltaTime);
                spriteRenderer.color = new(1f, 1f, 1f, Mathf.InverseLerp(0, 0.5f, scale));
                if (scale <= 0)
                {
                    controller.ProcessMiss(hitData);
                    Destroy(gameObject);
                }
            }
            if (
                currentTime.InDelta(hitData.time, controller.window)
                && transform.GetSiblingIndex() == 1
                && Input.GetButtonDown(hitData.spriteId)
            )
            {
                controller.ProcessHit(hitData, currentTime);
                hitPos = scale;
                transform.SetAsLastSibling();
            }
        }
        else if (scale < 2)
        {
            scale = Mathf.MoveTowards(scale, 2, (2 - hitPos) / controller.BeatTime(1) * Time.deltaTime);
        }
        else
        {
            scale = Mathf.MoveTowards(scale, 3, 1 / controller.BeatTime(0.5f) * Time.deltaTime);
            spriteRenderer.color = new(1f, 1f, 1f, Mathf.InverseLerp(3, 2, scale));
            if (scale >= 3)
            {
                Destroy(gameObject);
            }
        }
        transform.localScale = new(scale * spriteWidth, scale * spriteHeight, 1);
    }
}
