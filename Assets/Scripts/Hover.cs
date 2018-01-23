using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

    public GameObject AlertIcon;
    private bool scaleDown;

    public Vector3 IconScale = new Vector3(.41f, .41f, .41f);

	// Use this for initialization
	void Start ()
    {
        AlertIcon.transform.localScale = IconScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(scaleDown == false)
        {
            if(transform.localScale.x > .4f && transform.localScale.x < .6f)
            {
                AlertIcon.transform.localScale += new Vector3(0.003f, 0.003f, 0);
            }

            if(transform.localScale.x > .6f)
            {
                scaleDown = true;
            }
        }

        if(scaleDown)
        {

            AlertIcon.transform.localScale -= new Vector3(0.003f, 0.003f, 0);

            if(transform.localScale.x < 0.42f)
            {
                scaleDown = false;
            }
        }
	}
}
