using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    int count = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (count == 0)
        {
            GetComponent<RhythmController>().Spawn("Ching0");
        }
        count++;
        if (count > 5f/Time.deltaTime)
        {
            count = 0;
        }
    }
}
