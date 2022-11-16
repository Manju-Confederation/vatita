using UnityEngine;
using System.Collections.Generic;

public class RhythmController : MonoBehaviour
{
    public TextAsset source;
    public float bpm = 100;
    public float advanceBeats = 4;
    public float window = 0.5f;

    Queue<HitData> hitQueue;
    readonly Dictionary<string, Sprite> sprites = new();
    FearController fear;
    float advanceTime;
    float startTime;

    public float CurrentTime
    {
        get => (float) AudioSettings.dspTime - startTime;
    }

    void Awake()
    {
        fear = transform.parent.GetComponentInChildren<FearController>();
        advanceTime = BeatTime(advanceBeats);
        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Rhythm"))
        {
            sprites.Add(sprite.name, sprite);
        }
    }

    void Start()
    {
        List<string> lines = new(source ? source.text.Split("\n") : new string[0]);
        List<HitData> hits = lines.ConvertAll((line) => HitData.Parse(this, bpm, line));
        hits.RemoveAll((hit) => hit == null);
        hits.Sort((a, b) => (int) (a.beat - b.beat));
        hitQueue = new(hits);
        startTime = (float) AudioSettings.dspTime;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (transform.childCount != 0)
            {
                Transform next = transform.GetChild(0);
                ApproachController approach = next.GetComponent<ApproachController>();
            }
            if (hitQueue.TryPeek(out HitData hitData))
            {
                if (hitData.time - advanceTime <= CurrentTime)
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
        approach.transform.localPosition = Vector3.zero;
        approach.GetComponent<SpriteRenderer>().sprite = hitData.sprite;
        approach.GetComponent<SpriteRenderer>().color = new(1f, 1f, 1f, 0f);
        approach.GetComponent<ApproachController>().controller = this;
        approach.GetComponent<ApproachController>().hitData = hitData;
    }

    public void ProcessHit(HitData hitData, float time)
    {
        float delta = hitData.time.Delta(time);
    }

    public void ProcessMiss(HitData hitData)
    {
    }

    public float BeatTime(float beats) => beats * 60 / bpm;

    public class HitData
    {
        public readonly Sprite sprite;
        public readonly string spriteId;
        public readonly float beat;
        public readonly float time;
        public readonly float showTime;

        HitData(RhythmController controller, string spriteId, float beat)
        {
            this.spriteId = spriteId;
            this.beat = beat;
            this.time = controller.BeatTime(beat);
            this.showTime = this.time - controller.advanceTime;
            if (controller.sprites.TryGetValue(spriteId, out Sprite sprite))
            {
                this.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"Rhythm Sprite Resource '{spriteId}' not found.");
            }
        }

        public static HitData Parse(RhythmController controller, float bpm, string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.StartsWith("#")) return null;
            string[] data = text.Split(" ");
            if (data.Length == 2 && float.TryParse(data[0], out float beat))
            {
                return new(controller, data[1], beat + controller.advanceBeats);
            }
            else
            {
                Debug.LogWarning($"Rhythm Hit Data '{text}' invalid.");
                return null;
            }
        }
    }
}
