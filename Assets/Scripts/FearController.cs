using UnityEngine;
using UnityEngine.UI;

public class FearController : MonoBehaviour
{
    GameObject fearLevel;

    public float Fear
    { get; set; }

    void Awake()
    {
        fearLevel = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        fearLevel.GetComponent<Image>().fillAmount = Fear;
    }
}
