using TMPro;
using UnityEngine;

public class deathPrint : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Deaths : " + DeathCount.Instance.Deaths;
    }
}
