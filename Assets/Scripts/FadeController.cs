using UnityEngine;
using UnityEngine.U2D;

public class FadeController : MonoBehaviour
{
    public float fade = 1.5f;

    SpriteShapeRenderer sprite;

    bool hide = false;

    void Awake()
    {
        sprite = GetComponent<SpriteShapeRenderer>();
    }

    void Start()
    {
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    void Update()
    {
        Color color = sprite.color;
        color.a = (color.a + (hide ? -1f : 1.5f) * fade * Time.deltaTime).ToBounds(0f, 1f);
        sprite.color = color;
    }

    public void Show() => hide = false;
    public void Hide() => hide = true;
}
