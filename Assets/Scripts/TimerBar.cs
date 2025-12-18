using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{

    [SerializeField] private GameObject maskObject;
    [SerializeField] private float maskFill;
    private Image mask;
    private float barTimerDuration = 7f;  // This DOES NOT relate to Timer.cs - I'm not using a timer here since I can just 
    // tell how many seconds I need for the bar to run out and do 1f/7 e.g. - in 7 seconds

    private bool timerOut;

    void Start()
    {
        mask = maskObject.GetComponent<Image>();

        maskFill = 1f;  // Start at full
    }
    
    void Update()
    {
        if (maskFill > 0)
        {
            maskFill -= (1f / barTimerDuration) * Time.deltaTime;
            if (maskFill < 0) maskFill = 0;
            mask.fillAmount = maskFill;  // Assign the new amount to the fill property of the mask object   
        }
        else
        {
            OnBarTimerFinished();
        }
    }

    void OnBarTimerFinished()
    {
        Debug.Log("timer finished!");
        timerOut = true;
    }

    public bool getTimerOut
    {
        get { return timerOut; }
    }

    public void ResetTimerBar()
    {
        maskFill = 1f;
    }
}
