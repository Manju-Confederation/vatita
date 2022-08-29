using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public TextAsset source;

    RhythmController rhythm;

    void Start()
    {
        rhythm = GetComponent<RhythmController>();
        if (rhythm != null && source != null) StartCoroutine(ProcessSource());
    }

    IEnumerator ProcessSource()
    {
        string[] lines = source.text.Split("\n");
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
