using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControl : MonoBehaviour
{
    [SerializeField]
    private bool shakeCam;
    [SerializeField]
    private bool shakePlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);

            Debug.Log("the raycast hit the object " + rayHit.collider.gameObject.name);

            if(rayHit.collider != null)
            {
                if (shakeCam)
                {
                    interactThisObject(Camera.main.gameObject);
                }

                if (shakePlayer)
                {
                    Debug.Log("this happened");
                    interactThisObject(rayHit.collider.gameObject);
                   
                }

            }


        }
    }

    private void interactThisObject(GameObject interactObject)
    {
        interactObject.GetComponent<ObjectShake>().Begin();
    }
}
