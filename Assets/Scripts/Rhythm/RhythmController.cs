using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    public bool rhythmActive = true;

    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

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
    }

    public void Spawn(string id)
    {
        Sprite sprite;
        if (sprites.TryGetValue(id, out sprite))
        {
            GameObject approach = new GameObject("ApproachCircle", typeof(SpriteRenderer), typeof(ApproachController));
            approach.transform.SetParent(transform);
            approach.transform.localPosition = Vector2.zero;
            approach.GetComponent<SpriteRenderer>().sprite = sprite;
            Object.Destroy(approach, 1.5f);
        }
        else
        {
            Debug.LogWarning($"Rhythm Sprite Resource {id} not found.");
        }
    }
}
