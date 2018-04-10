using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadNewSceneCollision : MonoBehaviour {

    public string newScene;

	// Use this for initialization
	void Start () {
        //newScene = "HubTown";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
       SceneManager.LoadScene(newScene);

       Debug.Log("this definitely happened");
    }

}
