using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    public bool rhythmActive = true;

    readonly Dictionary<string, Sprite> sprites = new();

    void Start()
    {
        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Rhythm"))
        {
            sprites.Add(sprite.name, sprite);
        }
    }

    void Update()
    {
        if (rhythmActive != gameObject.activeSelf)
        {
            gameObject.SetActive(rhythmActive);
        }
        if (rhythmActive)
        {
            if (transform.childCount != 0)
            {
                GameObject next = transform.GetChild(0).transform.gameObject;
                if (Input.GetButtonUp(next.name))
                {
                    next.GetComponent<ApproachController>().Reflected = true;
                    next.transform.SetAsLastSibling();
                }
            }
        }
    }

    public void Spawn(string id)
    {
        if (sprites.TryGetValue(id, out Sprite sprite))
        {
            GameObject approach = new(id, typeof(SpriteRenderer), typeof(ApproachController));
            approach.transform.SetParent(transform);
            approach.transform.localPosition = Vector2.zero;
            approach.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"Rhythm Sprite Resource {id} not found.");
        }
    }
}
