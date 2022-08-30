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
                Transform next = transform.GetChild(0);
                ApproachController approach = next.GetComponent<ApproachController>();
                if (Input.GetButtonUp(next.name) && !approach.Reflected && next.localScale.x.InBounds(.2f, .5f))
                {
                    approach.Reflected = true;
                    next.SetAsLastSibling();
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
            approach.transform.localPosition = new Vector3(0, 0, -1);
            approach.GetComponent<SpriteRenderer>().sprite = sprite;
            approach.GetComponent<SpriteRenderer>().color = new(1f, 1f, 1f, 0f);
        }
        else
        {
            Debug.LogWarning($"Rhythm Sprite Resource {id} not found.");
        }
    }
}
