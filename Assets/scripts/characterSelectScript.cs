using UnityEngine;
using System.Collections.Generic;

// alright buckle up, this is the character select screen.
// to get this roulette thing working. dont touch it. 
// this whole thing is a miracle of duct tape and prayers. mostly duct tape.
public class CharacterSelectScript : MonoBehaviour
{
    // --- INSPECTOR STUFF, anYONE TOUCHING THIS BUYS ME COFFEE ---
    [SerializeField] private GameObject[] charactersFixed; // drag all the character prefabs here. if you miss one, it's not my problem.
    [SerializeField] private GameObject[] charactersBlurred; // if the characters aren't in the same order, your issue LLLL
    [SerializeField] private Vector2 leftPosition = new Vector2(-715f, -182f); // where the left guy stands awkwardly
    [SerializeField] private Vector2 middlePosition = new Vector2(31f, 12f); // the star of the show
    [SerializeField] private Vector2 rightPosition = new Vector2(715f, -182f); // where the right guy stands awkwardly
    [SerializeField] private float transitionDuration = 0.25f; // how fast they yeet across the screen. 0.25f feels snappy enough. dont make it too slow.
    [SerializeField] private float offscreenBufferX = 400f; // how far off the screen we chuck em so nobody sees them spawning in. magic.
    [SerializeField] private CharacterButtonHandler characterButtonHandler;

    // --- INTERNAL STATE, AKA THE MESS ---
    private bool isTransitioning = false; // are we currently in the middle of the big shuffle? yes/no
    private float transitionElapsed = 0f; // timer for the shuffle. goes from 0 to... well, to transitionDuration.
    private Vector2[] transitionStartPositions; // WHERE ARE THEY NOW
    private Vector2[] transitionTargetPositions; // WHERE ARE THEY GOING. deep questions.
    // toDeactivateOnCompletion is no longer needed. we handle this in HandleTransition's final loop.

    private int currentIndex = 0; // who's the main character right now. literally.
    private readonly Vector2 offscreenSpawn = new Vector2(2000f, 2000f); // basically Narnia. where we hide the characters that arent on screen.

    void Start()
    {
        // quick safety check
        if (charactersFixed.Length != charactersBlurred.Length)
        {
            Debug.LogError("CRITICAL ERROR: 'charactersFixed' and 'charactersBlurred' arrays are not the same length. This will explode. Go fix it. Now.");
            return;
        }

        if (!characterButtonHandler)
        {
            Debug.LogError("Character Button Handler not set.");
            return;
        }

        // just throw everyone where they're supposed to be at the start. the calm before the storm.
        // middle guy in the middle, left on left, right on right. everyone else? NARNIA.
        for (int i = 0; i < charactersFixed.Length; i++)
        {
            charactersFixed[i].SetActive(true);
            charactersBlurred[i].SetActive(true);
            //turn them on if they aren't on

            if (i == currentIndex)
            {
                // Middle: Show FIXED
                SetLocalPosition(i, middlePosition);
                SetActiveState(i, true, false);
            }
            else if (i == GetLeftIndex(currentIndex))
            {
                // Left: Show BLURRED
                SetLocalPosition(i, leftPosition);
                SetActiveState(i, false, true);
            }
            else if (i == GetRightIndex(currentIndex))
            {
                // Right: Show BLURRED
                SetLocalPosition(i, rightPosition);
                SetActiveState(i, false, true);
            }
            else
            {
                // Narnia: Show NOTHING
                SetLocalPosition(i, offscreenSpawn);
                SetActiveState(i, false, false); // poof. gone.
            }
        }
    }

    void Update()
    {
        // if we're in the middle of a beautiful choreographed dance, DONT read input. let them dance.
        if (isTransitioning)
        {
            HandleTransition();
            return;
        }

        // otherwise, check if the player is mashing buttons
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Just call the new public method. Clean.
            NavigateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Same here. Nice.
            NavigateRight();
        }
    }

    // --- NEW PUBLIC METHODS ---
    // you can call these from other scripts
    // my gawt reusable code.
    
    /// <summary>
    /// Moves the character select roulette to the left (showing the character from the right).
    /// </summary>
    public void NavigateLeft()
    {
        // if we're busy, don't do anything. spam-clicking protection.
        if (isTransitioning) return;

        // go left. which actually means we move everything right. my brain hurts.
        int nextIndex = GetRightIndex(currentIndex); // yeah get the RIGHT one to move to the MIDDLE. intuitive.
        PrepareTransition((i) =>
        {
            if (i == currentIndex) return leftPosition; // current guy moves left
            if (i == nextIndex) return middlePosition; // right guy moves to middle
            if (i == GetRightIndex(nextIndex)) return rightPosition; // the guy even further right slides in
            return offscreenSpawn; // everyone else, NARNIA.
        });
        currentIndex = nextIndex;
        ChangeDisplayedCharacterName();
    }

    /// <summary>
    /// Moves the character select roulette to the right (showing the character from the left).
    /// </summary>
    public void NavigateRight()
    {
        // if we're busy, don't do anything. spam-clicking protection.
        if (isTransitioning) return;

        // go right. move everything left. of course.
        int nextIndex = GetLeftIndex(currentIndex);
        PrepareTransition((i) =>
        {
            if (i == currentIndex) return rightPosition; // current guy moves right
            if (i == nextIndex) return middlePosition; // left guy moves to middle
            if (i == GetLeftIndex(nextIndex)) return leftPosition; // guy from the far left slides in
            return offscreenSpawn; // NARNIA.
        });
        currentIndex = nextIndex;

        // change whatever the current displayed name is
        ChangeDisplayedCharacterName();
    }

    // ----------------------------


    private void HandleTransition()
    {
        // the main dance routine. this is where the magic happens. or the bugs. usually both.
        transitionElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(transitionElapsed / transitionDuration); // get a 0-1 value for how far along we are
        t = Mathf.SmoothStep(0f, 1f, t); // smoothstep is my secret sauce. makes the movement less... robotic. more... buttery.

        // move everyone based on the timer. this now moves both fixed and blurred objects.
        for (int i = 0; i < charactersFixed.Length; i++)
        {
            if (charactersFixed[i] == null) continue;
            Vector2 pos = Vector2.Lerp(transitionStartPositions[i], transitionTargetPositions[i], t);
            SetLocalPosition(i, pos); // Use the new helper
        }

        // if the dance is over...
        if (t >= 1f)
        {
            isTransitioning = false; // we can stop now. please.
            
            // okay now the dance is *really* over, do a final pass
            // to set the final active states. this is the "truth".
            for (int i = 0; i < charactersFixed.Length; i++)
            {
                // Check the *intended* final position
                Vector2 finalPos = transitionTargetPositions[i];
                
                if (finalPos == middlePosition)
                {
                    // End in Middle: Show FIXED
                    SetActiveState(i, true, false);
                }
                else if (finalPos == leftPosition || finalPos == rightPosition)
                {
                    // End on Sides: Show BLURRED
                    SetActiveState(i, false, true);
                }
                else
                {
                    // End in Narnia: Show NOTHING
                    SetActiveState(i, false, false);
                    SetLocalPosition(i, offscreenSpawn); // just in case
                }
            }
        }
    }

    private void PrepareTransition(System.Func<int, Vector2> getTarget)
    {
        // THIS. this function. this is the hellmouth. this is where we figure out who goes where and how they get there.
        // it's the choreographer for the dance of the damned characters.
        int n = charactersFixed.Length;
        if (transitionStartPositions == null || transitionStartPositions.Length != n)
        {
            transitionStartPositions = new Vector2[n];
            transitionTargetPositions = new Vector2[n];
        }

        for (int i = 0; i < n; i++)
        {
            if (charactersFixed[i] == null) continue;

            Vector2 currentPos = GetLocalPosition(i); // new helper
            Vector2 finalPos = getTarget(i);

            bool isStartingOnScreen = IsActive(i); // new helper
            bool isEndingOnScreen = finalPos.x < 1000f; // lazy check for "is it going to Narnia?"

            // spent way too long on this logic. basically a state machine from hell. here are the cases.
            // case 1: they're on screen and they're staying on screen. easy. just lerp it. the good case.
            if (isStartingOnScreen && isEndingOnScreen)
            {
                transitionStartPositions[i] = currentPos;
                transitionTargetPositions[i] = finalPos;
                
                // --- NEW LOGIC ---
                // Set the active state based on *destination*.
                // This makes it "pop" to fixed or blurred *before* moving.
                if(finalPos == middlePosition) SetActiveState(i, true, false);
                else SetActiveState(i, false, true);
            }
            // case 2: they're in narnia and need to come on stage. this is the BLINK.
            else if (!isStartingOnScreen && isEndingOnScreen)
            {
                float spawnX = (finalPos.x > middlePosition.x)
                    ? rightPosition.x + offscreenBufferX
                    : leftPosition.x - offscreenBufferX;

                Vector2 spawnPos = new Vector2(spawnX, finalPos.y);
                SetLocalPosition(i, spawnPos); // BAMF. they appear (both of them)

                // --- NEW LOGIC ---
                // Activate the correct one *before* it slides in.
                if(finalPos == middlePosition) SetActiveState(i, true, false);
                else SetActiveState(i, false, true);

                transitionStartPositions[i] = spawnPos; // start lerping from the edge
                transitionTargetPositions[i] = finalPos;
            }
            // case 3: they were the star, now they're getting kicked off stage. dramatic exit.
            else if (isStartingOnScreen && !isEndingOnScreen)
            {
                float exitX = (currentPos.x > middlePosition.x)
                   ? rightPosition.x + offscreenBufferX
                   : leftPosition.x - offscreenBufferX;

                transitionStartPositions[i] = currentPos;
                transitionTargetPositions[i] = new Vector2(exitX, currentPos.y); // target is just off screen
                
                // --- NEW LOGIC ---
                // Ensure the *correct* one is active *while* it exits.
                // It should already be in this state, but better safe than sorry.
                if(currentPos == middlePosition) SetActiveState(i, true, false);
                else SetActiveState(i, false, true);
                // no more toDeactivateOnCompletion list. HandleTransition does this now.
            }
            // case 4: they're in narnia and they're staying in narnia. who cares. leave em there.
            else
            {
                transitionStartPositions[i] = offscreenSpawn;
                transitionTargetPositions[i] = offscreenSpawn;
                SetActiveState(i, false, false); // make sure they're off
            }
        }

        transitionElapsed = 0f; // reset the clock
        isTransitioning = true; // AND... ACTION!
    }

    // --- UTILITIES, AKA THE BORING MATH STUFF ---
    
    // --- NEW HELPERS ---
    // these helpers now manage BOTH the fixed and blurred objects for a given index.
    
    // sets the active state for the fixed/blurred pair
    private void SetActiveState(int index, bool isFixedActive, bool isBlurredActive)
    {
        if (index < 0 || index >= charactersFixed.Length) return;
        
        if (charactersFixed[index] != null)
            charactersFixed[index].SetActive(isFixedActive);
            
        if (charactersBlurred[index] != null)
            charactersBlurred[index].SetActive(isBlurredActive);
    }
    
    // checks if *either* object in the pair is active
    private bool IsActive(int index)
    {
        if (index < 0 || index >= charactersFixed.Length) return false;
        
        bool fixedActive = charactersFixed[index] != null && charactersFixed[index].activeSelf;
        bool blurredActive = charactersBlurred[index] != null && charactersBlurred[index].activeSelf;
        
        return fixedActive || blurredActive;
    }

    // gets the position from the "master" (fixed) object
    private Vector2 GetLocalPosition(int index)
    {
        if (index < 0 || index >= charactersFixed.Length || charactersFixed[index] == null) return offscreenSpawn;
        
        return charactersFixed[index].GetComponent<RectTransform>()?.anchoredPosition ?? (Vector2)charactersFixed[index].transform.localPosition;
    }
    
    // moves *both* objects in the pair to the same spot
    private void SetLocalPosition(int index, Vector2 pos)
    {
        if (index < 0 || index >= charactersFixed.Length) return;
        
        // Move the fixed one
        if (charactersFixed[index] != null)
            SetPositionInternal(charactersFixed[index], pos);
            
        // Move the blurred one
        if (charactersBlurred[index] != null)
            SetPositionInternal(charactersBlurred[index], pos);
    }

    // this is the original SetLocalPosition logic, just renamed
    private void SetPositionInternal(GameObject obj, Vector2 pos)
    {
        if (obj == null) return;
        var rt = obj.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = pos;
        else
            obj.transform.localPosition = new Vector3(pos.x, pos.y, obj.transform.localPosition.z);
    }

    public void ChangeDisplayedCharacterName()
    {
        //no need for a check, checking in start alr
        characterButtonHandler.ChangeDisplayedName(GetCurrentCharacterName());
    }

    public string GetCurrentCharacterName()
    {
        return charactersFixed[currentIndex].name.Replace("Fixed", "");
    }
    
    // --- ORIGINAL HELPERS ---

    // modulo math, my beloved enemy and friend. wraps around the array so we dont go out of bounds.
    private int GetLeftIndex(int index) => (index - 1 + charactersFixed.Length) % charactersFixed.Length;
    private int GetRightIndex(int index) => (index + 1) % charactersFixed.Length;
    
    // these two are no longer used, kept here for posterity, or in case i need them later.
    // private Vector2 GetLocalPosition(GameObject obj)
    // {
    //     if (obj == null) return offscreenSpawn;
    //     return obj.GetComponent<RectTransform>()?.anchoredPosition ?? (Vector2)obj.transform.localPosition;
    // }
    // private void SetLocalPosition(GameObject obj, Vector2 pos)
    // {
    //      SetPositionInternal(obj, pos);
    // }
}