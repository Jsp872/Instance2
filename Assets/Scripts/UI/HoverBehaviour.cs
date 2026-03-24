using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float rotation = 0.2f;
    private float speed = 1.5f;
    private float abs = Mathf.PI;
    private float rot;
    private bool isRotating = false;
    private float sin;
    private bool turn = true;
    private float min;
    private float max;

    private void Start()
    {
        rot = rotation/2;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isRotating = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isRotating = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            sin = Mathf.Sin(abs) + 1;
            abs += Time.deltaTime*speed;

            if (turn &&  sin <= 0.001)
            {
                StartCoroutine(Turn());
                if (rot > -rotation)
                {
                    rot = - rotation;
                    abs = Mathf.PI;
                }
                else
                {
                    rot = rotation;
                    abs = Mathf.PI;
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + sin*rot);
        }
    }

    private IEnumerator Turn()
    {
        turn = false;
        yield return new WaitForSeconds(0.1f);
        turn = true;
    }
}
