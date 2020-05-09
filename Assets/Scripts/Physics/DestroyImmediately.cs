using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImmediately : MonoBehaviour
{
    public float timeUntilDestruction;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeUntilDestruction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
