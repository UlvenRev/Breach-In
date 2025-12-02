using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ButtonsClickingLogic : MonoBehaviour
{
    // Having a separate variable for each let the button be pressed only during the FIRST frame and not during as many frames as you hold the button for
    // If you had only one variable for each it wouldn't work correctly
    private bool alreadyClickedUp;  // Default value - false
    private bool alreadyClickedDown;
    private bool alreadyClickedLeft;
    private bool alreadyClickedRight;
    
    private int i;  // Default value - 0
    private bool isWaiting;

    [SerializeField] private GameObject buttonsHolder;

    private Dictionary<char, Sprite> defaultSprites;
    private Dictionary<char, Sprite> correctSprites;
    private Dictionary<char, Sprite> wrongSprites;

    void Start()
    {
        // Populating all the dictionaries with the correct sprites from Resources folder
        defaultSprites = new Dictionary<char, Sprite>()
        {
            {'u', Resources.Load<Sprite>("Sprites/up_default")},
            {'d', Resources.Load<Sprite>("Sprites/down_default")},
            {'l', Resources.Load<Sprite>("Sprites/left_default")},
            {'r', Resources.Load<Sprite>("Sprites/right_default")}
        };
        correctSprites = new Dictionary<char, Sprite>()
        {
            {'u', Resources.Load<Sprite>("Sprites/up_correct")},
            {'d', Resources.Load<Sprite>("Sprites/down_correct")},
            {'l', Resources.Load<Sprite>("Sprites/left_correct")},
            {'r', Resources.Load<Sprite>("Sprites/right_correct")}
        };
        wrongSprites = new Dictionary<char, Sprite>()
        {
            { 'u', Resources.Load<Sprite>("Sprites/up_wrong") },
            { 'd', Resources.Load<Sprite>("Sprites/down_wrong") },
            { 'l', Resources.Load<Sprite>("Sprites/left_wrong") },
            { 'r', Resources.Load<Sprite>("Sprites/right_wrong") }
        };
    }

    void Update()
    {
        float upAxis = Input.GetAxis("Up");
        float downAxis = Input.GetAxis("Down");
        float leftAxis = Input.GetAxis("Left");
        float rightAxis = Input.GetAxis("Right");

        GameObject
            button = buttonsHolder.transform.GetChild(i).gameObject; // i - current child button number we need to press

        SpriteRenderer
            buttonSpriteRenderer = button.GetComponent<SpriteRenderer>(); // Get Sprite RENDERER - this is the COMPONENT
        Sprite buttonSprite = buttonSpriteRenderer.sprite; // Get the SPRITE itself
        char spriteName = buttonSprite.name[0]; // Gets the NAME of the sprite

        if (!isWaiting)  // In case the coroutine for resetting the buttons is running
        {
            if (upAxis > 0)
            {
                CheckPressedDirection(upAxis, ref alreadyClickedUp, spriteName, buttonSpriteRenderer, 'u');
            }
            else
            {
                alreadyClickedUp = false;  // Resets the Up axis flag - meaning we can register a new press
            }

            if (downAxis > 0)
            {
                CheckPressedDirection(downAxis, ref alreadyClickedDown, spriteName, buttonSpriteRenderer, 'd');
            }
            else
            {
                alreadyClickedDown = false;
            }

            if (leftAxis > 0)
            {
                CheckPressedDirection(leftAxis, ref alreadyClickedLeft, spriteName, buttonSpriteRenderer, 'l');
            }
            else
            {
                alreadyClickedLeft = false;
            }

            if (rightAxis > 0)
            {
                CheckPressedDirection(rightAxis, ref alreadyClickedRight, spriteName, buttonSpriteRenderer, 'r');
            }
            else
            {
                alreadyClickedRight = false;
            }
        }
    }
    
    void CheckPressedDirection(float axisPressed, ref bool alreadyClickedDirection, char spriteName, SpriteRenderer buttonSpriteRenderer, char userPress)
    {
        if (!alreadyClickedDirection)
        {
            Debug.Log(userPress);
            alreadyClickedDirection = true;
            Sprite correctSprite = correctSprites[userPress];  // correctSprites and wrongSprites are HASH MAPS so it's easy to get the correct sprite since we know the KEY for both 
            Sprite wrongSprite = wrongSprites[spriteName];
            if (spriteName == userPress)
            {
                Debug.Log("Correct " + i);
                if (i <= buttonsHolder.transform.childCount)
                {
                    buttonSpriteRenderer.sprite = correctSprite;
                    i++;
                }
            }
            else
            {
                Debug.Log("Incorrect " + i);
                StartCoroutine(WrongButtonWaitTime(buttonSpriteRenderer, wrongSprite, defaultSprites[spriteName]));
            }
        }
    }

    IEnumerator WrongButtonWaitTime(SpriteRenderer buttonSpriteRenderer, Sprite wrongSprite, Sprite defaultSprite)
    {
        isWaiting = true;  // Stop receiving button presses from the user while we reset the buttons
        
        buttonSpriteRenderer.sprite = wrongSprite;
        yield return new WaitForSeconds(2f);
        buttonSpriteRenderer.sprite = defaultSprite;
        
        // Now start from the first button, go over all of them and change to default colour
        for (int j = 0; j < i; j++)
        {
            GameObject prevButton = buttonsHolder.transform.GetChild(j).gameObject;
            SpriteRenderer prevButtonSpriteRenderer = prevButton.GetComponent<SpriteRenderer>();  // Get Sprite RENDERER - this is the COMPONENT
            Sprite prevButtonSprite = prevButtonSpriteRenderer.sprite;  // Get the SPRITE itself
            char prevSpriteName = prevButtonSprite.name[0];  // Gets the NAME of the sprite
            prevButtonSpriteRenderer.sprite = defaultSprites[prevSpriteName];
        }
        i = 0;  // Coming back to the very first button in the sequence
        
        isWaiting = false;  // Now we unlock the input receiving
    }
}
