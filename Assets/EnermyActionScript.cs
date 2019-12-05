using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnermyActionScript : MonoBehaviour {

	private enum EnermyState
	{
		None,
		Trace,
		Attack			
	}
	static float enermySpeed = 2.0f;

	private NavMeshAgent nav;
	private GameObject player;

	void Awake()
	{
		nav = GetComponent<NavMeshAgent>();
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		/*
		Vector3 dir = (player.transform.position - this.transform.position).normalized;

		Quaternion q = Quaternion.identity;
		q.SetLookRotation(dir, Vector3.up);

		q = Quaternion.Slerp(this.transform.rotation, q, Time.deltaTime * 50.0f);
		q.x = 0.0f; q.z = 0.0f;
		this.transform.rotation = q;

		float dist = Vector3.Distance(this.transform.position, player.transform.position);
		*/

		nav.SetDestination(player.transform.position);
		
	}
}
