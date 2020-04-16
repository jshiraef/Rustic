using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUponPlayerDistance : MonoBehaviour
{
    Animator anim;
    Animation actualAnimation;

    public bool onlyForObjectFromInspectorDragAndDrop;
    public GameObject objectToAnimate;


    // note: if you want the Animation to only play once go to animation window
    //       in the Unity Editor and click on the animation state and uncheck the box that say "Loop Time"
    // Start is called before the first frame update
    void Start()
    {
        if (onlyForObjectFromInspectorDragAndDrop || objectToAnimate == null)
        {
            anim = objectToAnimate.GetComponent<Animator>();
            actualAnimation = objectToAnimate.GetComponent<Animation>();
        }
        else
        {
            anim = GetComponent<Animator>();
            actualAnimation = GetComponent<Animation>();
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "player")
        {
            anim.Play(0);
        }
    }


    public bool animationHasPlayedOnce()
    {
        return anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0);
    }

    public bool animatorIsPlaying(string stateName)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
