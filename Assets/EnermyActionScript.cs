using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyActionScript : MonoBehaviour {

	private enum EnermyState
	{
		None,
		Trace,
		Attack			
	}
	static float enermySpeed = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GameObject player = GameObject.FindGameObjectWithTag("Player");

		Vector3 dir = (player.transform.position - this.transform.position).normalized;

		Quaternion q = Quaternion.identity;
		q.SetLookRotation(dir, Vector3.up);

		q = Quaternion.Slerp(this.transform.rotation, q, Time.deltaTime * 50.0f);
		q.x = 0.0f; q.z = 0.0f;
		this.transform.rotation = q;

		float dist = Vector3.Distance(this.transform.position, player.transform.position);
		if(dist > 1.5f)
		{
			float speed = enermySpeed;
			this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime * 1.0f);
		}
		
	}
}
