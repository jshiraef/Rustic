using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAgate : MonoBehaviour
{

    public AudioSource collectSound;

    void Start()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(collectSound != null)
        {
            collectSound.Play();
        }

        if(collectSound == null)
        {
            other.transform.Find("collectAgateSound").gameObject.GetComponent<AudioSource>().Play();
            Debug.Log("it should have played the collect sound");
        }        

        ScoringSystem.theScore += 50;

        Destroy(gameObject);
    }
}
