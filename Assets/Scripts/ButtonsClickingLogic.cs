using UnityEngine;
using System.Collections.Generic;

public class ButtonsClickingLogic : MonoBehaviour
{
    // Having a separate variable for each let the button be pressed only during the FIRST frame and not during as many frames as you hold the button for
    // If you had only one variable for each it wouldn't work correctly
    private bool alreadyClickedUp = false;
    private bool alreadyClickedDown = false;
    private bool alreadyClickedLeft = false;
    private bool alreadyClickedRight = false;
    private int i = 0;
    private string userPress = "";

    [SerializeField] private GameObject buttonsHolder;
    [SerializeField] private List<GameObject> defaultPrefabs;  // Blue buttons prefabs (top, down, left, right)
    [SerializeField] private List<GameObject> correctPrefabs;  // Green buttons prefabs (top, down, left, right)
    [SerializeField] private List<GameObject> wrongPrefabs;  // Red buttons prefabs (top, down, left, right) 
    
    void Update()
    {
        float upAxis = Input.GetAxis("Up");
        float downAxis = Input.GetAxis("Down");
        float leftAxis = Input.GetAxis("Left");
        float rightAxis = Input.GetAxis("Right");

        GameObject button = buttonsHolder.transform.GetChild(i).gameObject;  // i - current child button number we need to press
        Debug.Log("Next direction: " + button.name);

        if (upAxis > 0)
        {
            if (!alreadyClickedUp)
            {
                alreadyClickedUp = true;
                userPress = "up";
                if (button.CompareTag(userPress))
                {
                    Debug.Log("Correct");

                    // SetSiblingIndex is used to place the NEW PREFAB in the correct place - since I have prefabs and not just sprites of diff color, I have to do it this way 
                    GameObject newCorrectButton = Instantiate(correctPrefabs[0], buttonsHolder.transform);
                    newCorrectButton.transform.SetSiblingIndex(i);
                    
                    Destroy(button);
                    if (i <= buttonsHolder.transform.childCount)
                    {
                        i++;
                    }
                }
                else
                {
                    Debug.Log("Incorrect --------------------------------");
                    Debug.Log("i: " + i);  // The number of the current button we WERE standing on and pressed wrong
                    for (int j = 0; j < i; j++)
                    {
                        Debug.Log("j: " + j);  // Starting from button 0 I take each one up until the button I'm standing on
                        GameObject prevButton = buttonsHolder.transform.GetChild(j).gameObject;  // Get this jth button
                        
                        int defPrefabIndex = 0;
                        Debug.Log("Prev button name: " + prevButton.name);
                        if (prevButton.name == "up_correct" ||  prevButton.name == "up_correct(Clone)")
                        {
                            defPrefabIndex = 0;
                        } else if (prevButton.name == "down_correct" || prevButton.name == "down_correct(Clone)")
                        {
                            defPrefabIndex = 1;
                        } else if (prevButton.name == "left_correct" || prevButton.name == "left_correct(Clone)")
                        {
                            defPrefabIndex = 2;
                        } else if (prevButton.name == "right_correct" || prevButton.name == "right_correct(Clone)")
                        {
                            defPrefabIndex = 3;
                        }
                    
                        Destroy(prevButton);
                        GameObject newDefButton = Instantiate(defaultPrefabs[defPrefabIndex], buttonsHolder.transform);
                        Debug.Log("BUTTON TO CREATE RN: " + newDefButton + " at " + j);
                        newDefButton.transform.SetSiblingIndex(j);
                    }
                    i = 0;
                }
            }
        }
        else
        {
            alreadyClickedUp = false;
        }
        
        if (downAxis > 0)
        {
            if (!alreadyClickedDown)
            {
                alreadyClickedDown = true;
                userPress = "down";
                Debug.Log("Pressed Down");
                if (button.CompareTag(userPress))
                {
                    Debug.Log("Correct");
                    
                    int siblingIndex = button.transform.GetSiblingIndex();
                    GameObject newCorrectButton = Instantiate(correctPrefabs[1], buttonsHolder.transform);
                    newCorrectButton.transform.SetSiblingIndex(siblingIndex);
                    
                    Destroy(button);
                    
                    if (i < buttonsHolder.transform.childCount)
                    {
                        i++;
                    } else
                    {
                        Debug.Log("Incorrect");
                        i = 0;
                    }
                }
            }
        }
        else
        {
            alreadyClickedDown = false;
        }
        
        if (leftAxis > 0)
        {
            if (!alreadyClickedLeft)
            {
                alreadyClickedLeft = true;
                userPress = "left";
                Debug.Log("Pressed Left");
                if (button.CompareTag(userPress))
                {
                    Debug.Log("Correct");
                    
                    int siblingIndex = button.transform.GetSiblingIndex();
                    GameObject newCorrectButton = Instantiate(correctPrefabs[2], buttonsHolder.transform);
                    newCorrectButton.transform.SetSiblingIndex(siblingIndex);
                    
                    Destroy(button);
                    
                    if (i < buttonsHolder.transform.childCount)
                    {
                        i++;
                    } else
                    {
                        Debug.Log("Incorrect");
                        i = 0;
                    }
                }
            }
        }
        else
        {
            alreadyClickedLeft = false;
        }
        
        if (rightAxis > 0)
        {
            if (!alreadyClickedRight)
            {
                alreadyClickedRight = true;
                userPress = "right";
                Debug.Log("Pressed Right");
                if (button.CompareTag(userPress))
                {
                    Debug.Log("Correct");
                    
                    int siblingIndex = button.transform.GetSiblingIndex();
                    GameObject newCorrectButton = Instantiate(correctPrefabs[3], buttonsHolder.transform);
                    newCorrectButton.transform.SetSiblingIndex(siblingIndex);
                    
                    Destroy(button);
                    
                    if (i < buttonsHolder.transform.childCount)
                    {
                        i++;
                    } else
                    {
                        Debug.Log("Incorrect");
                        i = 0;
                    }
                }
            }
        }
        else
        {
            alreadyClickedRight = false;
        }
    }
}
