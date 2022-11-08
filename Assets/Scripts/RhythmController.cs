using UnityEngine;
using System;
using System.Collections.Generic;

public class RhythmController : MonoBehaviour
{
    public bool active = false;
    public TextAsset source;
    public float bpm = 100;
    public float advanceBeats = 4;
    public float window = 0.5f;

    Queue<HitData> hitQueue;
    readonly Dictionary<string, Sprite> sprites = new();

    float advanceTime;

    void Awake()
    {
        advanceTime = advanceBeats / bpm * 60;
        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Rhythm"))
        {
            sprites.Add(sprite.name, sprite);
        }
    }

    void Start()
    {
        List<string> lines = new(source ? source.text.Split("\n") : new string[0]);
        List<HitData> hits = lines.ConvertAll((line) => HitData.Parse(this, bpm, line));
        hits.Sort((a, b) => (int) (a.beat - b.beat));
        hitQueue = new(hits);
    }

    void Update()
    {
        if (active != gameObject.activeSelf)
        {
            gameObject.SetActive(active);
        }
        if (active)
        {
            if (transform.childCount != 0)
            {
                Transform next = transform.GetChild(0);
                ApproachController approach = next.GetComponent<ApproachController>();
                if (Input.GetButtonDown(approach.hitData.spriteId) && !approach.hit && ((float) AudioSettings.dspTime).InDelta(approach.hitData.time, window) || next.localScale.x.InBounds(.2f, .5f))
                {
                    approach.Hit();
                    next.SetAsLastSibling();
                }
            }
            if (hitQueue.TryPeek(out HitData hitData))
            {
                if (hitData.time <= AudioSettings.dspTime + advanceTime)
                {
                    Spawn(hitData);
                    hitQueue.Dequeue();
                }
            }
        }
    }

    void Spawn(HitData hitData)
    {
        GameObject approach = new($"Approach{hitData.spriteId}", typeof(SpriteRenderer), typeof(ApproachController));
        approach.transform.SetParent(gameObject.transform);
        approach.GetComponent<SpriteRenderer>().sprite = hitData.sprite;
        approach.GetComponent<SpriteRenderer>().color = new(1f, 1f, 1f, 0f);
        approach.GetComponent<ApproachController>().controller = this;
        approach.GetComponent<ApproachController>().hitData = hitData;
    }

    public class HitData
    {
        public readonly Sprite sprite;
        public readonly string spriteId;
        public readonly float beat;
        public readonly float time;

        HitData(RhythmController controller, float bpm, string spriteId, float beat)
        {
            if (controller.sprites.TryGetValue(spriteId, out Sprite sprite))
            {
                this.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"Rhythm Sprite Resource '{spriteId}' not found.");
            }
            this.beat = beat;
            this.time = beat / bpm * 60;
        }

        public static HitData Parse(RhythmController controller, float bpm, string text)
        {
            string[] data = text.Split(" ");
            if (data.Length == 2 && float.TryParse(data[0], out float beat))
            {
                return new(controller, bpm, data[1], beat);
            }
            else
            {
                Debug.LogWarning($"Rhythm Hit Data '{text}' invalid.");
                return null;
            }
        }
    }
}
