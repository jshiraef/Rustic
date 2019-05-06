using UnityEngine;
using System.Collections;
using Cinemachine;


public class BlurControl : MonoBehaviour {
	
	float value;

    public float blurThreshold = -50f;
    GameObject player;
    GameObject bigTree;
    SpriteRenderer sprite;
    Material currentMaterial;
    public Material blurMaterial;

    private Vector3 originalPosition;

    private bool toggle;

    public GameObject Camera;

    private Vector3 camLastPosition;
    private Vector3 camNextPosition;
    private Vector3 camLeftPosition;


    float v, h;


    // Use this for initialization
    void Start () {

        player = GameObject.Find("player");
        bigTree = GameObject.Find("giantTree_VillageTopLeft").gameObject;
        sprite = bigTree.GetComponent<SpriteRenderer>();
        currentMaterial = sprite.material;

        value = 800f;
        bigTree.transform.GetComponent<Renderer>().material.SetFloat("resolution",value);

        originalPosition = bigTree.transform.position;
        camLeftPosition = new Vector3(bigTree.transform.position.x - 6.5f, bigTree.transform.position.y, bigTree.transform.position.z);

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
            bigTree.transform.GetComponent<Renderer>().material.SetFloat("resolution", value);

        }
        else
        {
            value = 2000;
            bigTree.transform.GetComponent<Renderer>().material.SetFloat("resolution", value);
            sprite.material = currentMaterial;
        }

        if(player.transform.position.y > -25 && player.transform.position.y < -10)
        {
            if(player.transform.position.x > 28)
            {
                if(Input.GetAxisRaw("Horizontal") > 0)
                {
                    if((double) camLastPosition.x > (double)camNextPosition.x || (double) camLastPosition.x < (double) camNextPosition.x )
                    {
                        if(bigTree.transform.position.x > originalPosition.x - 10)
                        {
                            bigTree.transform.Translate(-(player.transform.position.x - 28) * .0012f, 0, 0);
                        }
                    }

                }
                else if(Input.GetAxisRaw("Horizontal") < 0 && !(bigTree.transform.position == originalPosition))
                {
                    if ((double)camLastPosition.x > (double)camNextPosition.x || (double)camLastPosition.x < (double)camNextPosition.x)
                    {
                        if(bigTree.transform.position.x < originalPosition.x + 10)
                        {
                            bigTree.transform.Translate((player.transform.position.x - 28) * .0015f, 0, 0);
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
        else if(player.transform.position.y > -10 || player.transform.position.x > 50)
        {
            bigTree.transform.position = Vector3.Lerp(bigTree.transform.position, camLeftPosition, Time.deltaTime);
          
        }
        else
        {

            bigTree.transform.position = Vector3.Lerp(bigTree.transform.position, originalPosition, Time.deltaTime);
        }


        if (player.transform.position.y > -25 && player.transform.position.x < 37 && bigTree.transform.position.x < camLeftPosition.x + 1)
        {
            Color tmpColor = sprite.color;
            sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 0f);

            bigTree.SetActive(false);
        }

        if (!bigTree.activeSelf && player.transform.position.x > 37)
        {
            bigTree.SetActive(true);

            if (sprite.color.a < 1f)
            {
                Color tmpColor = sprite.color;
                sprite.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(sprite.color.a, 1f, Time.deltaTime));
            }
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
