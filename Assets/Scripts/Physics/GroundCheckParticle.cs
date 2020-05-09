using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckParticle : MonoBehaviour {

	[SerializeField]
	GameObject dustCloud;

	
	public GameObject extraParticleSpray;

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag.Equals ("movable"))
			Instantiate (dustCloud, transform.position, dustCloud.transform.rotation);

		if (col.gameObject.tag.Equals("movable"))
        {
			//Instantiate(extraParticleSpray, transform.position, extraParticleSpray.transform.rotation);


			if(extraParticleSpray != null)
			{
				extraParticleSpray.SetActive(true);

				if (!extraParticleSpray.GetComponent<ParticleSystem>().isPlaying)
				{
					extraParticleSpray.GetComponent<ParticleSystem>().Play();
					Debug.Log("the purple particles should be flying");
				}
			}
			
			//Debug.Log("this purple thing should be spraying");
		}
	}

}
