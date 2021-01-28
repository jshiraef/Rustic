using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAgate : MonoBehaviour
{

    public AudioSource collectSound; 

    void OnTriggerEnter2D(Collider2D other)
    {
        collectSound.Play();

        ScoringSystem.theScore += 50;

        Destroy(gameObject);
    }
}
