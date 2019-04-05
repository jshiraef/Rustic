using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{

    // initialization
    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

    public string whichLevel = "Village Demo";

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0)
        {      
            if (!keyDown)
            {
                if(Input.GetAxis("Vertical") < 0)
                {
                    if(index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }          
        }
        else
        {
            keyDown = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0 && (Input.GetKey(KeyCode.X) || Input.GetButton("PS4_X")))
        {
            //SceneManager.LoadScene(whichLevel);
            Debug.Log("the next level should be loading now but it is commented out");
        }
        

    }
    


}
