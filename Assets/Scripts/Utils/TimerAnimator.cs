using System;
using TMPro;
using UnityEngine;

public class TimerAnimator : MonoBehaviour
{
    private Chronometer chronometer;
    
    private TMP_Text timerText;

    [SerializeField] private int minSize;
    [SerializeField] private int maxSize;
    [SerializeField] private float sizeChangeSpeed;
    [SerializeField] private float LowPos;
    [SerializeField] private float HighPos;
    [SerializeField] private float PosChangeSpeed;

    private float size;
    private bool sizePositive = true;
    private float position;
    private bool positionPositive = true;
    private float originalPos;

    void Start()
    {
        chronometer = GetComponent<Chronometer>();
        timerText = chronometer.TimerText;
        size = timerText.fontSize;
        position = timerText.rectTransform.position.y;
        originalPos = timerText.rectTransform.position.y;
    }

    private void Update()
    {
        if (chronometer.TimeLeft <= 10 && chronometer.TimeLeft > 0)
        {
            
                    if (sizePositive)
                    {
                        if (size < maxSize)
                        {
                            size += sizeChangeSpeed*Time.deltaTime;
                        }
                        else
                        {
                            size -= sizeChangeSpeed*Time.deltaTime;
                            sizePositive = false;
                        }
                    }
                    else
                    {
                        if (size > minSize)
                        {
                            size -= sizeChangeSpeed*Time.deltaTime;
                        }
                        else
                        {
                            size += sizeChangeSpeed*Time.deltaTime;
                            sizePositive = true;
                        }
                    }
            
                    if (positionPositive)
                    {
                        if (position < originalPos + HighPos)
                        {
                            position += PosChangeSpeed*Time.deltaTime;
                        }
                        else
                        {
                            position -= PosChangeSpeed*Time.deltaTime;
                            positionPositive = false;
                        }
                    }
                    else
                    {
                        if (position > originalPos + LowPos)
                        {
                            position -= PosChangeSpeed*Time.deltaTime;
                        }
                        else
                        {
                            position += PosChangeSpeed*Time.deltaTime;
                            positionPositive = true;
                        }
                    }
                    timerText.fontSize = size;
                    timerText.rectTransform.position = new Vector2(timerText.rectTransform.position.x, position);
        }
    }
}
