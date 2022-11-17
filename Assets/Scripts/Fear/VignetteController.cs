using UnityEngine;

public class VignetteController : MonoBehaviour
{
    SpriteRenderer vignette;
    FearController fear;

    void Awake()
    {
        vignette = GetComponent<SpriteRenderer>();
        fear = transform.parent.GetComponentInChildren<FearController>();
    }

    void Update()
    {
        if (fear != null)
        {
            vignette.color = new(1f, 1f, 1f, Mathf.Lerp(0.5f, 1, fear.Fear));
        }
    }
}
