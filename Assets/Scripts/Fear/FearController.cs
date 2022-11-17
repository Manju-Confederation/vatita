using UnityEngine;
using UnityEngine.UI;

public class FearController : MonoBehaviour
{
    public float rate;

    GameObject fearLevel;

    private float _fear;

    public float Fear
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
        Add(rate * Time.deltaTime);
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
