using UnityEngine;
using System.Collections.Generic;

// alright buckle up, this is the character select screen.
// to get this roulette thing working. dont touch it. 
// this whole thing is a miracle of duct tape and prayers. mostly duct tape.
public class CharacterSelectScript : MonoBehaviour
{
    // --- INSPECTOR STUFF, anYONE TOUCHING THIS BUYS ME COFFEE ---
    [SerializeField] private GameObject[] characters; // drag all the character prefabs here. if you miss one, it's not my problem.
    [SerializeField] private Vector2 leftPosition = new Vector2(-715f, -182f); // where the left guy stands awkwardly
    [SerializeField] private Vector2 middlePosition = new Vector2(31f, 12f); // the star of the show
    [SerializeField] private Vector2 rightPosition = new Vector2(715f, -182f); // where the right guy stands awkwardly
    [SerializeField] private float transitionDuration = 0.25f; // how fast they yeet across the screen. 0.25f feels snappy enough. dont make it too slow.
    [SerializeField] private float offscreenBufferX = 400f; // how far off the screen we chuck em so nobody sees them spawning in. magic.

    // --- INTERNAL STATE, AKA THE MESS ---
    private bool isTransitioning = false; // are we currently in the middle of the big shuffle? yes/no
    private float transitionElapsed = 0f; // timer for the shuffle. goes from 0 to... well, to transitionDuration.
    private Vector2[] transitionStartPositions; // WHERE ARE THEY NOW
    private Vector2[] transitionTargetPositions; // WHERE ARE THEY GOING. deep questions.
    private List<int> toDeactivateOnCompletion = new List<int>(); // list of poor souls to banish to the void after the transition is done.

    private int currentIndex = 0; // who's the main character right now. literally.
    private readonly Vector2 offscreenSpawn = new Vector2(2000f, 2000f); // basically Narnia. where we hide the characters that arent on screen.

    void Start()
    {
        // just throw everyone where they're supposed to be at the start. the calm before the storm.
        // middle guy in the middle, left on left, right on right. everyone else? NARNIA.
        for (int i = 0; i < characters.Length; i++)
        {
            if (i == currentIndex)
                SetLocalPosition(characters[i], middlePosition);
            else if (i == GetLeftIndex(currentIndex))
                SetLocalPosition(characters[i], leftPosition);
            else if (i == GetRightIndex(currentIndex))
                SetLocalPosition(characters[i], rightPosition);
            else
            {
                SetLocalPosition(characters[i], offscreenSpawn);
                characters[i].SetActive(false); // poof. gone.
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
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
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
        }
    }

    private void HandleTransition()
    {
        // the main dance routine. this is where the magic happens. or the bugs. usually both.
        transitionElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(transitionElapsed / transitionDuration); // get a 0-1 value for how far along we are
        t = Mathf.SmoothStep(0f, 1f, t); // smoothstep is my secret sauce. makes the movement less... robotic. more... buttery.

        // move everyone based on the timer
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null) continue;
            Vector2 pos = Vector2.Lerp(transitionStartPositions[i], transitionTargetPositions[i], t);
            SetLocalPosition(characters[i], pos);
        }

        // if the dance is over...
        if (t >= 1f)
        {
            isTransitioning = false; // we can stop now. please.
            // okay now GET RID OF THE ONES THAT LEFT. send them to the shadow realm.
            foreach (int index in toDeactivateOnCompletion)
            {
                if (characters[index] != null)
                {
                    characters[index].SetActive(false);
                    SetLocalPosition(characters[index], offscreenSpawn);
                }
            }
        }
    }

    private void PrepareTransition(System.Func<int, Vector2> getTarget)
    {
        // THIS. this function. this is the hellmouth. this is where we figure out who goes where and how they get there.
        // it's the choreographer for the dance of the damned characters.
        int n = characters.Length;
        if (transitionStartPositions == null || transitionStartPositions.Length != n)
        {
            transitionStartPositions = new Vector2[n];
            transitionTargetPositions = new Vector2[n];
        }
        
        toDeactivateOnCompletion.Clear(); // forget who we banished last time. new round, new banishments.

        for (int i = 0; i < n; i++)
        {
            if (characters[i] == null) continue;

            Vector2 currentPos = GetLocalPosition(characters[i]);
            Vector2 finalPos = getTarget(i);

            bool isStartingOnScreen = characters[i].activeSelf;
            bool isEndingOnScreen = finalPos.x < 1000f; // lazy check for "is it going to Narnia?"

            // spent way too long on this logic. basically a state machine from hell. here are the cases.
            // case 1: they're on screen and they're staying on screen. easy. just lerp it. the good case.
            if (isStartingOnScreen && isEndingOnScreen)
            {
                transitionStartPositions[i] = currentPos;
                transitionTargetPositions[i] = finalPos;
            }
            // case 2: they're in narnia and need to come on stage. this is the BLINK. remember the damn blink ability?
            // yeah this is that but for a menu. teleport them just off the edge so they can make a grand entrance. NO LERPING FROM NARNIA.
            // steam project flash backs right here
            else if (!isStartingOnScreen && isEndingOnScreen)
            {
                float spawnX = (finalPos.x > middlePosition.x)
                    ? rightPosition.x + offscreenBufferX
                    : leftPosition.x - offscreenBufferX;

                Vector2 spawnPos = new Vector2(spawnX, finalPos.y);
                SetLocalPosition(characters[i], spawnPos); // BAMF. they appear.
                characters[i].SetActive(true);

                transitionStartPositions[i] = spawnPos; // start lerping from the edge
                transitionTargetPositions[i] = finalPos;
            }
            // case 3: they were the star, now they're getting kicked off stage. lerp them to the edge then deactivate them later. dramatic exit.
            else if (isStartingOnScreen && !isEndingOnScreen)
            {
                float exitX = (currentPos.x > middlePosition.x)
                   ? rightPosition.x + offscreenBufferX
                   : leftPosition.x - offscreenBufferX;

                transitionStartPositions[i] = currentPos;
                transitionTargetPositions[i] = new Vector2(exitX, currentPos.y); // target is just off screen
                toDeactivateOnCompletion.Add(i); // mark this one for death
            }
            // case 4: they're in narnia and they're staying in narnia. who cares. leave em there.
            else
            {
                transitionStartPositions[i] = offscreenSpawn;
                transitionTargetPositions[i] = offscreenSpawn;
            }
        }

        transitionElapsed = 0f; // reset the clock
        isTransitioning = true; // AND... ACTION!
    }

    // --- UTILITIES, AKA THE BORING MATH STUFF ---

    // modulo math, my beloved enemy and friend. wraps around the array so we dont go out of bounds.
    private int GetLeftIndex(int index) => (index - 1 + characters.Length) % characters.Length;
    private int GetRightIndex(int index) => (index + 1) % characters.Length;

    // had to do this just in case some of them are UI elements and some are GameObjects.
    // why? dont ask me, i just do the work. this handles both so i dont have to think about it.
    private Vector2 GetLocalPosition(GameObject obj)
    {
        if (obj == null) return offscreenSpawn;
        return obj.GetComponent<RectTransform>()?.anchoredPosition ?? (Vector2)obj.transform.localPosition;
    }

    private void SetLocalPosition(GameObject obj, Vector2 pos)
    {
        if (obj == null) return;
        var rt = obj.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = pos;
        else
            obj.transform.localPosition = new Vector3(pos.x, pos.y, obj.transform.localPosition.z);
    }
}
