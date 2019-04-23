using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] SatchelController satchelController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorSound animatorSound;
    [SerializeField] int thisIndex;


 

    // Update is called once per frame
    void Update()
    {
        if(menuButtonController != null)
        {
            if (menuButtonController.index == thisIndex)
            {
                animator.SetBool("selected", true);
                if (Input.GetKey(KeyCode.X))
                {
                    animator.SetBool("pressed", true);
                }
                else if (animator.GetBool("pressed"))
                {
                    animator.SetBool("pressed", false);
                    animatorSound.disableOnce = true;
                }
            }
            else
            {
                animator.SetBool("selected", false);
            }
        }

        if (satchelController != null)
        {
            if (satchelController.index == thisIndex)
            {
                //animator.SetBool("selected", true);
                transform.GetChild(0).gameObject.SetActive(true);
                //if (Input.GetKey(KeyCode.X))
                //{
                //    animator.SetBool("pressed", true);
                //}
                //else if (animator.GetBool("pressed"))
                //{
                //    animator.SetBool("pressed", false);
                //    animatorSound.disableOnce = true;
                //}
            }
            else
            {
                //animator.SetBool("selected", false);              
                    transform.GetChild(0).gameObject.SetActive(false);               
            }
        }


    }
}
