using UnityEngine;
using System.Collections;
using Cinemachine;


public class BlurControl : MonoBehaviour {
	
	float value;

    public float blurThreshold = -50f;
    GameObject player;
    SpriteRenderer sprite;
    Material currentMaterial;
    public Material blurMaterial;

    private Vector3 originalPosition;

    private bool toggle;

    public GameObject Camera;

    private Vector3 camLastPosition;
    private Vector3 camNextPosition;


    float v, h;


    // Use this for initialization
    void Start () {

        player = GameObject.Find("player");
        sprite = GetComponent<SpriteRenderer>();
        currentMaterial = sprite.material;

        value = 800f;
		transform.GetComponent<Renderer>().material.SetFloat("resolution",value);

        originalPosition = this.transform.position;

        Camera = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update () {
        
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        // check to see if the camera is moving
        toggle = !toggle;

        if (toggle)
            camLastPosition = Camera.transform.position;

        if (!toggle)
            camNextPosition = Camera.transform.position;

        //Debug.Log("the camera's last position was " + camLastPosition);
        //Debug.Log("the camera's next position is " + camNextPosition);

        if (player.transform.position.y > blurThreshold && (player.transform.position.x < 24 || player.transform.position.x > 31 ))
        {
            sprite.material = blurMaterial;
            value = 2000 - ((player.transform.position.y + 50) * 50);
            transform.GetComponent<Renderer>().material.SetFloat("resolution", value);

        }
        else
        {
            value = 2000;
            transform.GetComponent<Renderer>().material.SetFloat("resolution", value);
            sprite.material = currentMaterial;
        }

        if(player.transform.position.y > -25)
        {
            if(player.transform.position.x > 28)
            {
                if(Input.GetAxisRaw("Horizontal") > 0)
                {
                    if((double) camLastPosition.x > (double)camNextPosition.x || (double) camLastPosition.x < (double) camNextPosition.x )
                    {
                        if(this.transform.position.x > originalPosition.x - 10)
                        {
                            transform.Translate(-(player.transform.position.x - 28) * .0012f, 0, 0);
                        }
                    }

                }
                else if(Input.GetAxisRaw("Horizontal") < 0 && !(this.transform.position == originalPosition))
                {
                    if ((double)camLastPosition.x > (double)camNextPosition.x || (double)camLastPosition.x < (double)camNextPosition.x)
                    {
                        if(this.transform.position.x < originalPosition.x + 10)
                        {
                            transform.Translate((player.transform.position.x - 28) * .0015f, 0, 0);
                        }
                    }

                    
                }

                //if (Input.GetAxisRaw("Horizontal") < 0)
                //{
                //    transform.Translate(h * -.05f, 0, 0);
                //}

                //if (Input.GetAxisRaw("Horizontal") > 0)
                //{
                //    transform.Translate(h * -.05f, 0, 0);
                //}

                //if (Input.GetAxisRaw("Vertical") < 0)
                //{
                //    transform.Translate(0, v * -.005f, 0);
                //}

                //if (Input.GetAxisRaw("Vertical") > 0)
                //{
                //    transform.Translate(0, v * -.005f, 0);
                //}
            }        
            
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, originalPosition, Time.deltaTime);
        }

       

        //      if (Input.GetButton("PS4_R1"))
        //{
        //	value = value + Time.deltaTime * 100;
        //	if (value>2000f) value = 2000f;
        //	transform.GetComponent<Renderer>().material.SetFloat("resolution", value);
        //}
        //else if(Input.GetButton("PS4_L1"))
        //{
        //	value = (value - Time.deltaTime * 100) % 2000.0f;
        //	if (value<200f) value = 200f;
        //	transform.GetComponent<Renderer>().material.SetFloat("resolution", value);
        //}


    }
	
	void OnGUI () {
	//	GUI.TextArea(new Rect(10f,10f,200f,50f), "Press the 'Up' and 'Down' arrows \nto interact with the blur plane\nCurrent value: "+value);
		}
}
