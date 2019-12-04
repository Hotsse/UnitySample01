using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEngineScript : MonoBehaviour {

	public GameObject Enermy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateEnermy(Vector3 pos, Quaternion rot)
	{
		GameObject e = Instantiate(Enermy);
		e.name = "Enermy";
		e.transform.SetPositionAndRotation(pos, rot);
	}
}
