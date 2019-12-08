using System.Collections;
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

	private NavMeshAgent _nav;
	private GameObject _player;
	private Animator _anim;

	[SerializeField] private GameObject bloodEffect;

	private bool isAlive = false;
	private int healthPoint = 0;


	void Awake()
	{
		_nav = GetComponent<NavMeshAgent>();
		_player = GameObject.FindGameObjectWithTag("Player");
		_anim = this.GetComponent<Animator>();

		isAlive = true;
		healthPoint = Random.Range(100, 150);
	}
	
	// Update is called once per frame
	void Update () {

		if (!isAlive) return;

		Vector3 dir = (_player.transform.position - this.transform.position).normalized;
		float dist = Vector3.Distance(this.transform.position, _player.transform.position);


		if(dist <= 1.3)
		{
			// 공격
			_anim.SetBool("Walk", true);
			_anim.SetBool("Eating", true);
		}
		else
		{
			// 이동
			_nav.SetDestination(_player.transform.position);
			_anim.SetBool("Walk", true);
		}
	}

	public void Hit(int damage, Vector3 pos, Quaternion rot)
	{
		CreateBloodEffect(pos, rot);

		healthPoint -= damage;
		if (healthPoint <= 0) Die();
	}

	private void CreateBloodEffect(Vector3 pos, Quaternion rot)
	{
		// PlaySound("Sound/hitBullet");
		GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, rot);
		blood1.transform.Rotate(0, 90, 0);
		Destroy(blood1, 1.0f);
	}

	public void Die()
	{
		isAlive = false;
		int r = Random.Range(0, 100);
		if (r <= 50) _anim.Play("Death_01");
		else _anim.Play("Death_02");

		PlaySound("Sound/zDeath_1");

		this.transform.Translate(Vector3.zero);
		_nav.enabled = false;
		this.GetComponent<Rigidbody>().isKinematic = true;
		this.GetComponent<BoxCollider>().enabled = false;
		Destroy(this.gameObject, 6.0f);
	}

	public void PlaySound(string tmp)
	{
		GameObject prefab = Resources.Load(tmp) as GameObject;
		GameObject instance = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		Destroy(instance, 3.0f);
	}
}
