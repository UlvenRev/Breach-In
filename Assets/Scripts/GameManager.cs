using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    [SerializeField] public List<LevelData> allLevels;
    private int currentLevelIndex;
    
    private ButtonsClickingLogic buttonsClickingLogic;
    void Awake()
    {
        buttonsClickingLogic = GetComponent<ButtonsClickingLogic>();
    }

    void Start()
    {
        buttonsClickingLogic.LevelSetup(allLevels[0]);
    }

    public void ProgressToNextLevel()  // Use this ONLY when you've finished all sequences from the previous level
    // This way I will give ButtonsClickingLogic.cs the NEXT level data
    {
        currentLevelIndex++;
        if (currentLevelIndex < allLevels.Count)
        {
            // .SetupLevel() - method from the ButtonsClickingLogic.cs to start the level WITH THE INFO we PASS IN from LEVEL DATA
            buttonsClickingLogic.LevelSetup(allLevels[currentLevelIndex]);
        }
        else
        {
            buttonsClickingLogic.gameFinished = true; 
            Debug.Log("YOU WIN THE WHOLE GAME!");
        }
    }
}
