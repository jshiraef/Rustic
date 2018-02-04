using UnityEngine;
using System.Collections;

public class smartBubbleTrail : MonoBehaviour {

    public GameObject bubbleTrail;
    private ParticleSystem bubbleEmitter;

    // Use this for initialization
    void Start () {

        bubbleTrail = GameObject.Find("bubbleTrail");
        bubbleEmitter = bubbleTrail.GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {

        if (GetComponentInParent<PlayerControl>().inWater)
        {
            if(!bubbleEmitter.isPlaying)
            {
                bubbleEmitter.Play();
            }
        }
        else
        {
            if(bubbleEmitter.isPlaying)
            {
                bubbleEmitter.Stop();
            }
        }
	}
}
