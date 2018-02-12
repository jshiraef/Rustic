using UnityEngine;
using System.Collections;

public class smartBubbleTrail : MonoBehaviour {

    public GameObject bubbleTrail;
    private ParticleSystem bubbleEmitter;
    private GameObject player;

    // Use this for initialization
    void Start () {

        bubbleTrail = GameObject.Find("bubbleTrail");
        bubbleEmitter = bubbleTrail.GetComponent<ParticleSystem>();
        player = GameObject.Find("player");

    }
	
	// Update is called once per frame
	void Update () {

        if (player.GetComponent<PlayerControl>().inWater)
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
