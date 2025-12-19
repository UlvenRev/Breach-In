using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{

    [SerializeField] private GameObject maskObject;
    [SerializeField] private float maskFill;
    private Image mask;
    private float barTimerDuration;

    private bool timerOut;
    private bool timerRunning;

    void Start()
    {
        mask = maskObject.GetComponent<Image>();

        maskFill = 1f;  // Start at full
    }
    
    void Update()
    {
        if (timerRunning)
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

    public void SetTimerDuration(int timerDuration)
    {
        barTimerDuration = timerDuration;
    }

    public void ToggleTimerState()
    {
        timerRunning = !timerRunning;
    }

    public bool GetTimerRunning
    {
        get { return timerRunning; }
    }
}
