using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float speed = 10f;

    private float time;
    private bool isRotating;
    private Quaternion baseRotation;

    private void Start()
    {
        baseRotation = transform.rotation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isRotating = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isRotating = false;
        transform.rotation = baseRotation;
    }

    private void Update()
    {
        if (!isRotating) return;

        time += Time.deltaTime * speed;

        float angle = Mathf.Sin(time) * amplitude;

        transform.rotation = baseRotation * Quaternion.Euler(0f, 0f, angle);
    }
}