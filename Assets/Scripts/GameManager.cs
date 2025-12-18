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
        buttonsClickingLogic.SetupLevel(allLevels[0]);
    }

    public void ProgressToNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < allLevels.Count)
        {
            // .SetupLevel() - method from the ButtonsClickingLogic.cs to start the level WITH THE INFO we PASS IN from LEVEL DATA
            buttonsClickingLogic.SetupLevel(allLevels[currentLevelIndex]);
        }
    }
}
