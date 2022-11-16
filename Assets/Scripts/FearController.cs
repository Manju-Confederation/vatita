using UnityEngine;
using UnityEngine.UI;

public class FearController : MonoBehaviour
{
    GameObject fearLevel;

    private float _fear;

    float Fear
    {
        get => _fear;
        set {
            _fear = Mathf.Clamp01(value);
        }
    }

    void Awake()
    {
        fearLevel = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        fearLevel.GetComponent<Image>().fillAmount = Fear;
    }

    public void Add(float val)
    {
        Fear += val / 100;
    }

    public void Sub(float val)
    {
        Add(-val);
    }
}
