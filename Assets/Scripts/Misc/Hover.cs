using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

    public GameObject AlertIcon;
    private bool scaleDown;
    public float scaleRange;
    public float scaleFactor;

    public Vector3 IconScale;

	// Use this for initialization
	void Start ()
    {
        if(scaleRange == 0)
        {
            scaleRange = .1f;
        }

        if(scaleFactor == 0)
        {
            scaleFactor = .003f;
        }

        IconScale = new Vector3((1 - scaleRange) + .01f, (1 - scaleRange) + .01f, 1f);

        this.transform.localScale = IconScale;
	}
	
	// Update is called once per frame
	void Update ()
    {

        // this makes the gameObject grow and shrink slightly as if it is breathing
	    if(scaleDown == false)
        {
            if(transform.localScale.x > (1 - scaleRange) && transform.localScale.x < 1f)
            {
                this.transform.localScale += new Vector3(scaleFactor, scaleFactor, 0);
            }

            if(transform.localScale.x > 1f)
            {
                scaleDown = true;
            }
        }

        if(scaleDown)
        {

            this.transform.localScale -= new Vector3(scaleFactor, scaleFactor, 0);

            if(transform.localScale.x < (1 - scaleRange) + .01f)
            {
                scaleDown = false;
            }
        }
	}
}
