using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSound : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] SatchelController satchelController;
    [SerializeField] WellController wellController;

    public bool disableOnce;

    void PlaySound(AudioClip whichSound)
    {
        if (!disableOnce)
        {
            if(menuButtonController != null)
            {
                Debug.Log("the sound should be playing");

                menuButtonController.audioSource.PlayOneShot(whichSound);
            }

            if(satchelController != null)
            {
                satchelController.audioSource.PlayOneShot(whichSound);
            }

            if(wellController != null)
            {
                wellController.audioSource.PlayOneShot(whichSound);

                Debug.Log("the sound should be playing");

            }

        }      
        else
        {
            disableOnce = false;
        }

    }
}
