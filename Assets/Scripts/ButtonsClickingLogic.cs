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

    public bool gameFinished;  // For the GAME MANAGER to set it and tell ButtonsClickingLogic that the game is FINISHED - it doesn't need to loop forever
    
    [SerializeField] private GameObject buttonsHolder;
    [SerializeField] private GameObject buttonPrefab;

    private Dictionary<char, Sprite> defaultSprites;
    private Dictionary<char, Sprite> correctSprites;
    private Dictionary<char, Sprite> wrongSprites;
    
    char[] directions = { 'u', 'd', 'l', 'r' };  // For selecting a random direction when instantiating new sprites for the new level

    private TimerBar timerBarScript;
    private bool timerOut;
    
    // Variables I get from LEVEL DATA - LevelSetup()
    private LevelData currentLevelData;  // Recording the object itself
    private int numberOfButtons;  // This will get assigned in the method
    private int numberOfRounds;
    private float animationSpeed;

    private int roundsPassed;  // This I change MANUALLY keeping track of how many rounds we completed
    // once it's == numberOfRounds, we call the ProgressToNextLevel() from Game Manager

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
    
    public void LevelSetup(LevelData levelData)  // Accepts LEVEL DATA type from the GAME MANAGER - this is where all the details about the current level are stored
    {
        currentLevelData = levelData;
        this.numberOfButtons = currentLevelData.numberOfButtons;
        this.numberOfRounds = currentLevelData.numberOfRounds;
        this.animationSpeed = currentLevelData.animationSpeed;

        roundsPassed = 0;

        timerBarScript.SetTimerDuration(currentLevelData.timerDuration);  // Passing the time for THIS LEVEL to the timer script
        
        // Debug.Log(buttonsHolder.transform.childCount);

        StartCoroutine(ResetButtons());  // CREATES THE NEEDED BUTTONS - from this moment the ButtonsClickingLogic.cs starts WORKING
    }

    void Update()
    {
        float upAxis = Input.GetAxis("Up");
        float downAxis = Input.GetAxis("Down"); 
        float leftAxis = Input.GetAxis("Left");
        float rightAxis = Input.GetAxis("Right");
        
        if (!gameFinished)  // If the GAME MANAGER didn't tell us the game is finished, ButtonsClickingLogic.cs KEEPS WORKING
        {
            timerOut = timerBarScript.getTimerOut;
        
            if (!timerOut)  // If the timer has NOT FINISHED yet,
            {
                LevelLogic_and_Functions(upAxis, downAxis, leftAxis, rightAxis);  // Keep playing - checking buttons, creating new levels etc
            }
            else
            {
                Debug.Log(
                    "TIMER FINISHED - LOST THE GAME"); // Acc stops the game automatically bc we don't land into the "if" part anymore so we can't press any buttons 

                // -----------------------------------------------------------------
                // Add some message of loosing here - maybe the menu screen, reset to the start level
                // -----------------------------------------------------------------
            }
        }
    }
    
    void LevelLogic_and_Functions(float upAxis, float  downAxis, float leftAxis, float rightAxis)
    {
        if (!waitingForLevelReset)
        {
            GameObject
                button = buttonsHolder.transform.GetChild(i).gameObject; // i - current child button number we need to press
            
            Transform buttonTransform = button.transform;  // For using DOTween and making the BOUNCE animation
            RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();  // For SHAKE animation - DOShakeAnchorPos works only with Rect Transform
    
            SpriteRenderer
                buttonSpriteRenderer = button.GetComponent<SpriteRenderer>(); // Get Sprite RENDERER - this is the COMPONENT
            Sprite buttonSprite = buttonSpriteRenderer.sprite; // Get the SPRITE itself
            char spriteName = buttonSprite.name[0]; // Gets the NAME of the sprite
    
            if (!isWaiting)  // In case the coroutine for resetting the buttons is running
            {
                if (upAxis > 0)
                {
                    CheckPressedDirection(ref alreadyClickedUp, spriteName, buttonSpriteRenderer, 'u', buttonTransform, buttonRectTransform);
                    
                }
                else
                {
                    alreadyClickedUp = false;  // Resets the Up axis flag - meaning we can register a new press
                }
    
                if (downAxis > 0)
                {
                    CheckPressedDirection(ref alreadyClickedDown, spriteName, buttonSpriteRenderer, 'd', buttonTransform, buttonRectTransform);
                }
                else
                {
                    alreadyClickedDown = false;
                }
    
                if (leftAxis > 0)
                {
                    CheckPressedDirection(ref alreadyClickedLeft, spriteName, buttonSpriteRenderer, 'l', buttonTransform, buttonRectTransform);
                }
                else
                {
                    alreadyClickedLeft = false;
                }
    
                if (rightAxis > 0)
                {
                    CheckPressedDirection(ref alreadyClickedRight, spriteName, buttonSpriteRenderer, 'r', buttonTransform, buttonRectTransform);
                }
                else
                {
                    alreadyClickedRight = false;
                }
            }
            
            if (i == numberOfButtons)
            {
                roundsPassed++;
                if (roundsPassed >= numberOfRounds)
                {
                    // DON'T ResetButtons() here! Just tell the manager we are done
                    FindObjectOfType<GameManager>().ProgressToNextLevel();
                }
                else 
                {
                    // Still have rounds left in this level? Reset for the next sequence
                    StartCoroutine(ResetButtons());
                }
            }
        }
    }
    
    void CheckPressedDirection(ref bool alreadyClickedDirection, char spriteName, SpriteRenderer buttonSpriteRenderer, char userPress, Transform buttonTransform, RectTransform buttonRectTransform)
    {
        if (!alreadyClickedDirection)
        {
            alreadyClickedDirection = true;
            Sprite correctSprite = correctSprites[userPress];  // correctSprites and wrongSprites are HASH MAPS so it's easy to get the correct sprite since we know the KEY for both 
            Sprite wrongSprite = wrongSprites[spriteName];
            if (spriteName == userPress)
            {
                if (i <= numberOfButtons)
                {
                    buttonSpriteRenderer.sprite = correctSprite;
                    CorrectButtonAnimation(buttonTransform);
                    i++;
                }
            }
            else
            {
                WrongButtonAnimation(buttonRectTransform);
                StartCoroutine(WrongButtonWaitTime(buttonSpriteRenderer, wrongSprite, defaultSprites[spriteName]));
            }
        }
    }
    
    IEnumerator WrongButtonWaitTime(SpriteRenderer buttonSpriteRenderer, Sprite wrongSprite, Sprite defaultSprite)
    {
        isWaiting = true;  // Stop receiving button presses from the user while we reset the buttons
        
        buttonSpriteRenderer.sprite = wrongSprite;
        yield return new WaitForSeconds(0.7f);
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
    
    IEnumerator ResetButtons() 
    {
        waitingForLevelReset = true;  // A flag to NOT be able to click any buttons while resetting the level

        while (buttonsHolder.transform.childCount > 0)
        {
            Transform child = buttonsHolder.transform.GetChild(0);
            // We set the parent to null immediately so childCount drops NOW
            child.SetParent(null); 
            Destroy(child.gameObject);
        }

        yield return null;
        
        // Reset i (current child index)
        i = 0;
    
        // Go through each, instantiate and pick a random default sprite
        for (int j = 0; j < numberOfButtons; j++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonsHolder.transform);  // buttonsHolder.transform tells Unity it's the PARENT object and newButton is supposed to be inside it
            char randomDefaultDirectionSprite = directions[UnityEngine.Random.Range(0, 4)];
            newButton.GetComponent<SpriteRenderer>().sprite = defaultSprites[randomDefaultDirectionSprite];
        }
    
        waitingForLevelReset = false;
        
        timerBarScript.ResetTimerBar();
    }
    
    void CorrectButtonAnimation(Transform buttonTransform)
    {
        buttonTransform.DOScale(Vector3.one * 1.12f, animationSpeed)
            .OnComplete(() =>
            {
                buttonTransform.DOScale(Vector3.one, animationSpeed);
            });
    }
    
    void WrongButtonAnimation(RectTransform buttonTransform)
    {
        buttonTransform.DOShakeAnchorPos(animationSpeed, new Vector3(0.10f, 0, 0), 10, 0, false, true);
    }
}
