using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject target;

	public GameObject player;

	public float LockedY = -10.5f; public float LockedZ = -20f;

	public float distance;

	public float height;

	public float movement;

	public Transform targetPosition;

    protected Cinemachine.CinemachineVirtualCamera _virtualCamera;

    // Use this for initialization
    void Start () 
	{
//		player = GameObject.Find ("HorseCart");
        player = GameObject.Find("player");

        _virtualCamera = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();

    }
	
	// Update is called once per frame
	void Update () 
	{
		//transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -300f);

		if(Input.GetButton ("PS4_R1"))
		{
            if(_virtualCamera.m_Lens.OrthographicSize < 9)
            {
                _virtualCamera.m_Lens.OrthographicSize += .2f;
            }        
            
		}
        else
        {
            if(_virtualCamera.m_Lens.OrthographicSize > 7)
            {
                _virtualCamera.m_Lens.OrthographicSize -= .5f;
            }
            
            if(_virtualCamera.m_Lens.OrthographicSize < 7)
            {
                _virtualCamera.m_Lens.OrthographicSize = 7;
            }
        }

        //if (Input.GetAxis ("PS4_R2") > 0) {
        //	camera.orthographicSize -= .001f;
        //}

        if (Input.GetButton ("PS4_L1"))
		{
			//camera.orthographicSize -= .015f;
		}



//		if (Input.GetAxis ("PS4_R2") > 0) {
//			movement += .005f;
//			transform.position = new Vector3 (transform.position.x + movement, transform.position.y, transform.position.z);
//		} 



		if(Input.GetAxis ("PS4_L2") > 0)
		{
			movement += .002f;
			transform.position = new Vector3 (transform.position.x, transform.position.y + movement, transform.position.z);
		}

		if (Input.GetAxis ("PS4_L2") <= 0 && Input.GetAxis ("PS4_R2") <= 0) 
		{
			movement = 0;
		}

//		movement -= .0015f;
//		transform.position = new Vector3 (transform.position.x - movement, transform.position.y, transform.position.z);
//		print ("movement is " + movement);

	}
}
