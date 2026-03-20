using DG.Tweening;
using UnityEngine;

public class falling_spike_trap : MonoBehaviour
{
    [SerializeField] private float speed;
    private Collider2D trigger;
    private GameObject spike;

    void Start()
    {
        trigger = GetComponent<Collider2D>();
        spike = transform.GetChild(0).gameObject;
        spike.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            spike.SetActive(true);
            spike.transform.DOMoveY(transform.position.y, Mathf.Abs(transform.position.y - spike.transform.position.y) * 1/speed);
            trigger.enabled = false;
        }
    }
}
