using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

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
    private bool waitingForLevelReset;  // Default - false
    
    [SerializeField] private GameObject buttonsHolder;
    [SerializeField] private GameObject buttonPrefab;

    private Dictionary<char, Sprite> defaultSprites;
    private Dictionary<char, Sprite> correctSprites;
    private Dictionary<char, Sprite> wrongSprites;
    
    char[] directions = { 'u', 'd', 'l', 'r' };  // For selecting a random direction when instantiating new sprites for the new level

    private TimerBar timerBarScript;
    private bool timerOut;

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
        
        timerBarScript = GameObject.FindGameObjectWithTag("TimerBar").GetComponent<TimerBar>();
        timerOut = timerBarScript.getTimerOut;

    }

    void Update()
    {
        // float upAxis = Input.GetAxis("Up");
        // float downAxis = Input.GetAxis("Down");
        // float leftAxis = Input.GetAxis("Left");
        // float rightAxis = Input.GetAxis("Right");
        //
        // timerOut = timerBarScript.getTimerOut;
        //
        // if (!timerOut)  // If the timer has NOT FINISHED yet,
        // {
        //     LevelLogic_and_Functions(upAxis, downAxis, leftAxis, rightAxis);  // Keep playing - checking buttons, creating new levels etc
        // }
        // else
        // {
        //     Debug.Log("TIMER FINISHED - STOP THE GAME");  // Acc stops the game automatically bc we don't land into the "if" part anymore so we can't press any buttons 
        //     
        //     // -----------------------------------------------------------------
        //     // Add some message of loosing here - maybe the menu screen, reset to the start level
        //     // -----------------------------------------------------------------
        // }

    }

    public void SetupLevel(LevelData levelData)  // Accepts LEVEL DATA type from the GAME MANAGER - this is where all the details about the current level are stored
    {
        Debug.Log(levelData.numberOfButtons);
    }
    
    // void LevelLogic_and_Functions(float upAxis, float  downAxis, float leftAxis, float rightAxis)
    // {
    //     if (!waitingForLevelReset)
    //     {
    //         GameObject
    //             button = buttonsHolder.transform.GetChild(i).gameObject; // i - current child button number we need to press
    //         
    //         Transform buttonTransform = button.transform;  // For using DOTween and making the BOUNCE animation
    //         RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();  // For SHAKE animation - DOShakeAnchorPos works only with Rect Transform
    //
    //         SpriteRenderer
    //             buttonSpriteRenderer = button.GetComponent<SpriteRenderer>(); // Get Sprite RENDERER - this is the COMPONENT
    //         Sprite buttonSprite = buttonSpriteRenderer.sprite; // Get the SPRITE itself
    //         char spriteName = buttonSprite.name[0]; // Gets the NAME of the sprite
    //
    //         if (!isWaiting)  // In case the coroutine for resetting the buttons is running
    //         {
    //             if (upAxis > 0)
    //             {
    //                 CheckPressedDirection(upAxis, ref alreadyClickedUp, spriteName, buttonSpriteRenderer, 'u', buttonTransform, buttonRectTransform);
    //                 
    //             }
    //             else
    //             {
    //                 alreadyClickedUp = false;  // Resets the Up axis flag - meaning we can register a new press
    //             }
    //
    //             if (downAxis > 0)
    //             {
    //                 CheckPressedDirection(downAxis, ref alreadyClickedDown, spriteName, buttonSpriteRenderer, 'd', buttonTransform, buttonRectTransform);
    //             }
    //             else
    //             {
    //                 alreadyClickedDown = false;
    //             }
    //
    //             if (leftAxis > 0)
    //             {
    //                 CheckPressedDirection(leftAxis, ref alreadyClickedLeft, spriteName, buttonSpriteRenderer, 'l', buttonTransform, buttonRectTransform);
    //             }
    //             else
    //             {
    //                 alreadyClickedLeft = false;
    //             }
    //
    //             if (rightAxis > 0)
    //             {
    //                 CheckPressedDirection(rightAxis, ref alreadyClickedRight, spriteName, buttonSpriteRenderer, 'r', buttonTransform, buttonRectTransform);
    //             }
    //             else
    //             {
    //                 alreadyClickedRight = false;
    //             }
    //         }
    //         
    //         if (i == totalNumOfButtons)
    //         {
    //             StartCoroutine(ResetButtons());
    //         }
    //     }
    // }
    //
    // void CheckPressedDirection(float axisPressed, ref bool alreadyClickedDirection, char spriteName, SpriteRenderer buttonSpriteRenderer, char userPress, Transform buttonTransform, RectTransform buttonRectTransform)
    // {
    //     if (!alreadyClickedDirection)
    //     {
    //         Debug.Log(userPress);
    //         alreadyClickedDirection = true;
    //         Sprite correctSprite = correctSprites[userPress];  // correctSprites and wrongSprites are HASH MAPS so it's easy to get the correct sprite since we know the KEY for both 
    //         Sprite wrongSprite = wrongSprites[spriteName];
    //         if (spriteName == userPress)
    //         {
    //             if (i <= buttonsHolder.transform.childCount)
    //             {
    //                 buttonSpriteRenderer.sprite = correctSprite;
    //                 CorrectButtonAnimation(buttonTransform);
    //                 i++;
    //             }
    //         }
    //         else
    //         {
    //             WrongButtonAnimation(buttonRectTransform);
    //             StartCoroutine(WrongButtonWaitTime(buttonSpriteRenderer, wrongSprite, defaultSprites[spriteName]));
    //         }
    //     }
    // }
    //
    // IEnumerator WrongButtonWaitTime(SpriteRenderer buttonSpriteRenderer, Sprite wrongSprite, Sprite defaultSprite)
    // {
    //     isWaiting = true;  // Stop receiving button presses from the user while we reset the buttons
    //     
    //     buttonSpriteRenderer.sprite = wrongSprite;
    //     yield return new WaitForSeconds(0.7f);
    //     buttonSpriteRenderer.sprite = defaultSprite;
    //     
    //     // Now start from the first button, go over all of them and change to default colour
    //     for (int j = 0; j < i; j++)
    //     {
    //         GameObject prevButton = buttonsHolder.transform.GetChild(j).gameObject;
    //         SpriteRenderer prevButtonSpriteRenderer = prevButton.GetComponent<SpriteRenderer>();  // Get Sprite RENDERER - this is the COMPONENT
    //         Sprite prevButtonSprite = prevButtonSpriteRenderer.sprite;  // Get the SPRITE itself
    //         char prevSpriteName = prevButtonSprite.name[0];  // Gets the NAME of the sprite
    //         prevButtonSpriteRenderer.sprite = defaultSprites[prevSpriteName];
    //     }
    //     i = 0;  // Coming back to the very first button in the sequence
    //     
    //     isWaiting = false;  // Now we unlock the input receiving
    // }
    //
    // IEnumerator ResetButtons()
    // {
    //     waitingForLevelReset = true;
    //     
    //     for (int j = buttonsHolder.transform.childCount - 1; j >= 0; j--)
    //     {
    //         Destroy(buttonsHolder.transform.GetChild(j).gameObject);
    //     }
    //
    //     yield return null;  // WAIT FOR ONE FRAME so that the Destroy() actually works - THEN COUNT THE CHILDREN AND INSTANTIATE NEW ONES
    //     
    //     // Reset i
    //     i = 0;
    //     
    //     // Select a random number of buttons in a level's range - LATER, for now the amount is fixed
    //     int nextNumOfButtons = 5;
    //
    //     // Go through each, instantiate and pick a random default sprite
    //     for (int j = 0; j < nextNumOfButtons; j++)
    //     {
    //         GameObject newButton = Instantiate(buttonPrefab, buttonsHolder.transform);  // buttonsHolder.transform tells Unity it's the PARENT object and newButton is supposed to be inside it
    //         char randomDefaultDirectionSprite = directions[UnityEngine.Random.Range(0, 4)];
    //         newButton.GetComponent<SpriteRenderer>().sprite = defaultSprites[randomDefaultDirectionSprite];
    //     }
    //
    //     // Record the new total number of buttons, since we won't enter this piece of code again until we get i equal to this new total number of buttons
    //     totalNumOfButtons = buttonsHolder.transform.childCount; 
    //
    //     waitingForLevelReset = false;
    //     
    //     timerBarScript.ResetTimerBar();
    // }
    //
    // void CorrectButtonAnimation(Transform buttonTransform)
    // {
    //     buttonTransform.DOScale(Vector3.one * 1.12f, 0.05f)
    //         .OnComplete(() =>
    //         {
    //             buttonTransform.DOScale(Vector3.one, 0.05f);
    //         });
    // }
    //
    // void WrongButtonAnimation(RectTransform buttonTransform)
    // {
    //     buttonTransform.DOShakeAnchorPos(0.6f, new Vector3(0.10f, 0, 0), 10, 0, false, true);
    // }
}
