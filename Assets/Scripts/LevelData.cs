using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject  // This is a SCRIPTABLE OBJECT - it simply HOLDS information, it DOESN'T HAVE ANY FUNCTIONS
{
    public string levelName;  // Main thing we're hacking, e.g. CCTV cameras
    public int numberOfRounds;
    public string[] roundsNames;  // The sub things we're hacking to finish the level, e.g. getting coords, breaching security and other smart words
    public int numberOfButtons;  // Will stay the same for all rounds
    public int timerDuration;  // The same for all rounds
    public float animationSpeed = 0.05f;
}
