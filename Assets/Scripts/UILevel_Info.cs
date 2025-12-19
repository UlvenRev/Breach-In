using UnityEngine;
using TMPro;

public class UILevel_Info : MonoBehaviour
{

    [SerializeField] private TMP_Text levelName;
    [SerializeField] private TMP_Text roundName;
    [SerializeField] private TMP_Text roundNumber;

    public void setLevelName(string levelName)
    {
        this.levelName.text = levelName;
    }

    public void setRoundName(string roundName)
    {
        this.roundName.text = roundName;
    }

    public void setRoundNumber(string roundNumber)
    {
        this.roundNumber.text = "Clearance tier: " + roundNumber;
    }
}
