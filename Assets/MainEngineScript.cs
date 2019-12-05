using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEngineScript : MonoBehaviour {

	[SerializeField]
	private GameObject _enermy;

	private static MainEngineScript _instance;
	public static MainEngineScript GetInstance()
	{
		return _instance;
	}


	// Use this for initialization
	void Start () {
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateEnermy(Vector3 pos, Quaternion rot)
	{
		GameObject e = Instantiate(_enermy);
		e.name = "Enermy";
		e.transform.SetPositionAndRotation(pos, rot);
	}
}
