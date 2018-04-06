using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafEmitter : MonoBehaviour {

    public bool jostled = false;

    private float leafEmitterCoolDown;

    private ParticleSystem leafEmitter;

	// Use this for initialization
	void Start () {
        leafEmitter = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(jostled)
        {
            if(leafEmitterCoolDown <= 0)
            {
                leafEmitterCoolDown = 1f;
            }
            
            if(!leafEmitter.isPlaying)
            {
                leafEmitter.Play();
            }
        }

        if(leafEmitterCoolDown > 0)
        {
            leafEmitterCoolDown -= Time.deltaTime;
        }

        if(leafEmitterCoolDown > 0f && leafEmitterCoolDown < .1f)
        {
            leafEmitter.Stop();

            jostled = false;
        }

        if(!jostled && leafEmitter.isPlaying)
        {
            leafEmitter.Stop();
        }

        //Debug.Log("jostled is " + jostled);
	}
}
