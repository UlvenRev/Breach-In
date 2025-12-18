using UnityEngine;

public class Timer : MonoBehaviour
{
    private float totalSeconds = 0f;
    private float elapsedSeconds = 0f;
    private bool running = false;
    private bool started = false;

    public float Duration  // This will allow to SET a new timer for e.g. 5 seconds in total (like a setter in Java, but here it's timer.Duration = 5)
    {
        set
        {
            if (!running)
            {
                totalSeconds = value;
            }
        }
    }

    public float getDuration
    {
        get { return totalSeconds; }
    }

    public bool Finished
    {
        get { return started && !running; }  // Just do timer.Finished to GET the property (like a getter in Java)
    }

    public bool Running
    {
        get { return running; }
    }

    public float ElapsedSeconds  // To just check in the other scripts if the timer is working, printing the elapsed seconds in the console
    {
        get { return elapsedSeconds; }
    }

    void Update()
    {
        if (running)
        {
            // Time.deltaTime - it's NOT a literal 1 second that passed, it's HOW LONG it took for the PREV FRAME TO
            // EXECUTE, e.g. 0.18924 seconds, but we still just sum it up to the time that passed, and eventually
            // we'll have our elapsedSeconds = or > totalSeconds, meaning the timer ended
            elapsedSeconds += Time.deltaTime;  // Accumulate the time that passed
            if (elapsedSeconds >= totalSeconds)  // Once the time elapsed, 
            {
                running = false;  // Stop running the timer
            }
        }
    }

    public void Run()
    {
        if (totalSeconds > 0)
        {
            started = true;
            running = true;
            elapsedSeconds = 0f;
        }
    }
}
