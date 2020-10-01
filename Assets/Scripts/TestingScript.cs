using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TestingScript : MonoBehaviour
{

    [SerializeField] private AnimationClip Idle_anim;
    [SerializeField] private List<PlayableCharacter> players;
    [SerializeField] private List<Animator> animators;
    [SerializeField] private GameObject entities = null;
    [SerializeField] private Vector2 referenceVector;
    private const float SPACE_BETWEEN = 69.6f;
    public bool canAnimate = true;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(PlayableCharacter p in players)
        {
            GameObject g = Instantiate(p.BattleVersionPrefab);
            animators.Add(g.GetComponent<Animator>());
            g.transform.SetParent(entities.transform);
            RectTransform rect = g.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(referenceVector.x, referenceVector.y);
            referenceVector.y -= SPACE_BETWEEN;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canAnimate)//if you can animate check if a key is pressed
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))//if the 1 key is pressed start animation and make sure no other animations can run
            {
                animators[0].SetTrigger("Fight");
                canAnimate = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animators[1].SetTrigger("Fight");
                canAnimate = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                animators[2].SetTrigger("Fight");
                canAnimate = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                animators[3].SetTrigger("Fight");
                canAnimate = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (!canAnimate)//if animations cannot run
        {
            foreach(Animator a in animators)//for every animator
            {
                if (!a.GetCurrentAnimatorStateInfo(0).IsName(Idle_anim.name))//if the current state of animation is not the idle animation. meaning another animation is playing
                {
                    return;//leave the method
                }
            }
            canAnimate = true;//otherwise you can play another animation
            Debug.Log("Run anim");
        }
    }
}
