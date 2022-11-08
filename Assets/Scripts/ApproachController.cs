using UnityEngine;

public class ApproachController : MonoBehaviour
{
    public RhythmController controller;
    public RhythmController.HitData hitData;
    public bool hit = false;


    void Start()
    {
        
    }

    void Update()
    {
        if (hit)
        {
            // TODO: animate out via time
        }
        else
        {
            // TODO: animate interpolate
        }
    }

    public void Hit()
    {
        hit = true;
        // TODO: calc score|fear
    }
}
