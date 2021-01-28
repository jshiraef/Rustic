using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlucked : MonoBehaviour
{
    private PlayerControl playerControl;
    private GameObject flowerOutline;
    private bool plucked;
    private int pluckedTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.Find("player").GetComponent<PlayerControl>();
        flowerOutline = transform.Find("Outline").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerControl.transform.position, this.transform.position) < 1)
        {
            flowerOutline.SetActive(true);

            if (Input.GetKeyDown(KeyCode.X))
            {
                playerControl.GetComponent<Animator>().SetFloat("animationSpeed", .7f);
                playerControl.setPlucking(true, .7f);

            }

            if (Input.GetKey(KeyCode.X) && playerControl.plucking && playerControl.animatorIsPlaying("PluckingEast") && playerControl.getAnimatorNormalizedTime() > .5f && !plucked)
            {
                playerControl.setPlucking(true, 0f);
                pluckedTimer += Mathf.RoundToInt(Time.deltaTime * 100);
            }

            if (Input.GetKeyUp(KeyCode.X) && playerControl.getAnimatorNormalizedTime() >= .5f && !plucked)
            {
                plucked = true;
                playerControl.setPlucking(true, 1f);
                GetComponent<AudioSource>().Play();

            }


            if (pluckedTimer > 50)
            {
                if (!plucked)
                {
                    plucked = true;
                    GetComponent<AudioSource>().Play();
                    playerControl.setPlucking(true, 1f);
                    pluckedTimer = 0;
                }
            }

            if (plucked)
            {

                if (transform.GetChild(1).name == "groundCheck")
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                    //Debug.Log("Oh! It happened!");
                    //GetComponent<SpriteOutline2>().enabled = false;
                    flowerOutline.GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            if (playerControl.plucking)
            {
                if (playerControl.animatorIsPlaying("PluckingEast"))
                {
                    if (playerControl.animationHasPlayedOnce())
                    {
                        playerControl.setPlucking(false, 1f);
                    }
                }
            }
        }
        else
        {
            flowerOutline.SetActive(false);
        }

        //Debug.Log("the plucked Timer is at " + pluckedTimer);

    }
}
