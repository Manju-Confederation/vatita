using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RhythmController))]
public class MusicController : MonoBehaviour
{
    public TextAsset source;

    RhythmController rhythm;

    void Awake()
    {
        rhythm = GetComponent<RhythmController>();
    }

    void Start()
    {
        StartCoroutine(ProcessSource());
    }

    IEnumerator<YieldInstruction> ProcessSource()
    {
        string[] lines = source ? source.text.Split("\n") : new string[0];
        Dictionary<string, int> labels = new();
        for (int i = 0; i < lines.Length; i++)
        {
            string command = lines[i];
            switch (command[0])
            {
                case '&':
                    float duration;
                    try
                    {
                        duration = float.Parse(command[1..]);
                    }
                    catch
                    {
                        Debug.LogWarning($"Invalid time {command[1..]}.");
                        yield break;
                    }
                    yield return new WaitForSeconds(duration);
                    break;
                case '$':
                    string[] args = command[1..].Split(" ");
                    switch (args[0])
                    {
                        case "label":
                            if (args.Length == 1)
                            {
                                Debug.LogWarning($"Invalid instruction '{command}'.");
                                yield break;
                            }
                            foreach (string label in args[1..])
                            {
                                labels.Add(label, i);
                            }
                            break;
                        case "jump":
                            if (args.Length != 2)
                            {
                                Debug.LogWarning($"Invalid instruction '{command}'.");
                                yield break;
                            }
                            int target;
                            if (labels.TryGetValue(args[1], out target))
                            {
                                i = target;
                            }
                            else
                            {
                                Debug.LogWarning($"Label '{args[1]}' not found.");
                                yield break;
                            }
                            break;
                        default:
                            Debug.LogWarning($"Unknown instruction '{args[0]}'.");
                            yield break;
                    }
                    break;
                case '#': break;
                default:
                    rhythm.Spawn(command);
                    break;
            }
        }
        yield return null;
    }
}
