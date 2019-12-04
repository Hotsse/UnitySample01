using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour {

	private float PlayerSpeed = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		#region 캐릭터 이동
		string moveKey = "";
		
		// 달리기 키
		if (Input.GetKey(KeyCode.LeftShift)) moveKey += "E";
		
		// 이동 키
		if (Input.GetKey(KeyCode.W)) moveKey += "W";
		else if (Input.GetKey(KeyCode.S)) moveKey += "S";

		if (Input.GetKey(KeyCode.A)) moveKey += "A";
		else if (Input.GetKey(KeyCode.D)) moveKey += "D";
		
		// 이동 실행
		if (!string.IsNullOrEmpty(moveKey))
		{
			this.Move(moveKey);
		}

		#endregion

		if (Input.GetKeyDown(KeyCode.T))
		{
			Vector3 pos = transform.position + transform.forward * 5.0f;
			Quaternion rot = transform.rotation;
			GameObject.Find("GameController").GetComponent<MainEngineScript>().CreateEnermy(pos, rot);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			this.Shoot();
		}
	}

	public void Move(string key)
	{
		if (!"".Equals(key))
		{
			float speed = PlayerSpeed;
			if (key.Contains("E")) speed *= 2;

			Vector3 pos = transform.position;

			bool isPlayerMoving = false;

			if (key.Contains("W"))
			{
				transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
				pos += Vector3.forward;
				isPlayerMoving = true;
			}
			if (key.Contains("S"))
			{
				transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
				pos += Vector3.back;
				isPlayerMoving = true;
			}
			if (key.Contains("A"))
			{
				transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
				pos += Vector3.left;
				isPlayerMoving = true;
			}
			if (key.Contains("D"))
			{
				transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
				pos += Vector3.right;
				isPlayerMoving = true;
			}

			if (isPlayerMoving)
			{
				Vector3 dir = (pos - transform.position).normalized;

				Quaternion q = Quaternion.identity;
				q.SetLookRotation(dir, Vector3.up);

				q = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5.0f);
				q.x = 0.0f; q.z = 0.0f;
				transform.rotation = q;
			}
		}
	}

	public void Shoot()
	{
		Vector3 pos = transform.position + transform.up * 2.1f + transform.forward * 1.4f + transform.right * 0.1f;
	}
}
