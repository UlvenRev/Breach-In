using UnityEngine;

public class Timer
{
    public float RemainingTime { get; private set; } // Shorthand way to say: get method for this variable is public
                                                     // (can do Timer.RemainingTime from any script), but set is
                                                     // private (can be done only from Timer.cs)
    public bool IsRunning { get; private set; }
    
    // Start the timer with a specific duration
    public void Start(float duration)
    {
        RemainingTime = duration;
        IsRunning = true;
    }
    
    // Call this every Update 
    public void Tick(float deltaTime)
    {
        if (!IsRunning) return;
        
        RemainingTime -= deltaTime;  // Time.deltaTime is how much time it took for the PREVIOUS frame to execute

        if (RemainingTime <= 0)
        {
            RemainingTime = 0;
            IsRunning = false;
        }
    }
    
    // Checking if the timer is finished
    public bool IsFinished()
    {
        return !IsRunning && RemainingTime <= 0;
    }
}