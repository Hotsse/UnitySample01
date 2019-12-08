using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{

	private float walkSpeed = 3.0f;
	private float sprintSpeed = 6.0f;
	
	private RaycastHit hit;
	private Animator anim;

	private LineRenderer line;
	private Transform linetr;
	public Material linemt;

	[SerializeField]
	private LayerMask layerMask;

	public GameObject ShootEffect;
	public GameObject PistolEffect;
	public GameObject bloodEffect;

	private Vector3 inputPosition = Vector3.zero;
	private Vector3 inputDirection = Vector3.zero;
	private bool isPlayerSprint = false;

	private float delayTimer = 0.0f;

	private Vector2 currentLeftAxis = Vector2.zero;
	private Vector2 currentRightAxis = Vector2.zero;

	// Use this for initialization
	void Start()
	{
		anim = this.GetComponent<Animator>();

		line = GameObject.Find("Liner").GetComponent<LineRenderer>();
		linetr = GameObject.Find("Liner").GetComponent<Transform>();
		
		line.useWorldSpace = false;
		line.enabled = false;
		line.SetWidth(0.05f, 0.01f);
	}

	// Update is called once per frame
	void Update()
	{
		delayTimer -= Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.T))
		{
			Vector3 pos = transform.position + transform.forward * 5.0f;
			Quaternion rot = transform.rotation;

			MainEngineScript.GetInstance().CreateEnermy(pos, rot);
		}

		if (Input.GetAxisRaw("RightTrigger") != 0)
		{
			this.Shoot();
		}

		this.InputMoveKey();
		this.InputDirectionKey();

		this.Move();
		this.Rotate();
	}

	public void SetInputPosition(Vector3 pos)
	{
		inputPosition = pos;
		if (inputDirection == Vector3.zero && Vector3.Distance(Vector3.zero, inputPosition) >= 0.9f)
		{
			isPlayerSprint = true;
		}
		else
		{
			isPlayerSprint = false;
		}
	}

	public void SetInputDirection(Vector3 dir)
	{
		inputDirection = dir;
	}

	private void Move()
	{
		float speed = walkSpeed;		
		if(isPlayerSprint) speed = sprintSpeed;

		transform.Translate(inputPosition.normalized * speed * Time.deltaTime, Space.World);

		if(inputPosition != Vector3.zero)
		{
			anim.SetBool("Walk", true);
			if (isPlayerSprint) anim.SetBool("Sprint", true);
			else anim.SetBool("Sprint", false);
		}
		else
		{
			anim.SetBool("Walk", false);
			anim.SetBool("Sprint", false);
		}
	}

	private void Rotate()
	{
		Vector3 dir = inputDirection;
		if (dir == Vector3.zero) dir = inputPosition;

		if (dir != Vector3.zero)
		{
			Quaternion q = Quaternion.identity;
			q.SetLookRotation(dir, Vector3.up);

			q = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5.0f);
			q.x = 0.0f; q.z = 0.0f;
			transform.rotation = q;
		}

		if(inputDirection != Vector3.zero)
		{
			anim.SetInteger("WeaponAim", 2);

			Ray rays = new Ray(transform.position + transform.up * 2.1f + transform.forward * 1.4f + transform.right * 0.1f, transform.forward);

			line.SetPosition(0, linetr.InverseTransformPoint(rays.origin));
			
			Color c = linemt.color;
			c.r = 1.0f;
			linemt.color = c;
			linemt.SetColor("_EmissionColor", c);

			if (Physics.Raycast(rays, out hit, 10f))
			{
				line.SetPosition(1, linetr.InverseTransformPoint(hit.point));

				if("Enermy".Equals(hit.transform.gameObject.tag)) this.Shoot();
			}
			else
			{
				line.SetPosition(1, linetr.InverseTransformPoint(rays.GetPoint(10f)));
			}
			line.enabled = true;
		}
		else
		{
			anim.SetInteger("WeaponAim", 0);
			line.enabled = false;
		}
	}
		
	public void Shoot()
	{
		if (delayTimer > 0.0f) return;

		delayTimer = 0.35f;
		GameObject.Find("Main Camera").GetComponent<CameraMoveScript>().CreateShake(0.3f);

		Vector3 pos = transform.position + transform.up * 2.1f + transform.forward * 1.4f + transform.right * 0.1f;

		anim.Play("Character_Handgun_Shoot");
		PlaySound("Sound/firePistol");
		CreateShootEffect(pos, this.transform.rotation, 0);

		Debug.DrawRay(this.transform.position + Vector3.up * 2.1f, this.transform.forward * 10f, Color.red, 0.3f);
		if(Physics.Raycast(pos, this.transform.forward, out hit, 10f, layerMask)){

			hit.transform.gameObject.GetComponent<EnermyActionScript>().Hit(30, hit.point, this.transform.rotation);

		}
	}

	void CreateShootEffect(Vector3 pos, Quaternion rot, int size)
	{
		GameObject tmp = ShootEffect;
		if (size == 0) tmp = PistolEffect;
		else if (size == 1) tmp = ShootEffect;
		GameObject shoot1 = (GameObject)Instantiate(tmp, pos, rot);
		shoot1.transform.Rotate(0, 150, 0);
		Destroy(shoot1, 0.5f);
	}

	public void PlaySound(string tmp)
	{
		GameObject prefab = Resources.Load(tmp) as GameObject;
		GameObject instance = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
		Destroy(instance, 3.0f);
	}

	private void InputMoveKey()
	{
		Vector3 pos = Vector3.zero;
		bool isMoveKeyUp = false;

		/*
		if (Input.GetKey(KeyCode.W)) pos.z = 0.5f;
		else if (Input.GetKey(KeyCode.S)) pos.z = -0.5f;

		if (Input.GetKey(KeyCode.A)) pos.x = -0.5f;
		else if (Input.GetKey(KeyCode.D)) pos.x = 0.5f;

		if (Input.GetKey(KeyCode.LeftShift)) pos *= 2;

		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
		{
			if (pos == Vector3.zero) isMoveKeyUp = true;
		}
		*/

		pos.x = Input.GetAxisRaw("Horizontal");
		pos.z = Input.GetAxisRaw("Vertical");

		if (currentLeftAxis != Vector2.zero && pos == Vector3.zero) isMoveKeyUp = true;

		currentLeftAxis = new Vector2(pos.x, pos.z);

		if (pos != Vector3.zero || isMoveKeyUp) SetInputPosition(pos);
	}

	private void InputDirectionKey()
	{
		Vector3 dir = Vector3.zero;

		bool isDirectionKeyUp = false;

		/*
		if (Input.GetKey(KeyCode.UpArrow)) dir.z = 1.0f;
		else if (Input.GetKey(KeyCode.DownArrow)) dir.z = 1.0f;

		if (Input.GetKey(KeyCode.LeftArrow)) dir.x = -1.0f;
		else if (Input.GetKey(KeyCode.RightArrow)) dir.x = 1.0f;

		if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			if (dir == Vector3.zero) isDirectionKeyUp = true;
		}
		*/

		dir.z = Input.GetAxisRaw("RightHorizontal");
		dir.x = Input.GetAxisRaw("RightVertical");

		if (currentRightAxis != Vector2.zero && dir == Vector3.zero) isDirectionKeyUp = true;

		currentRightAxis = new Vector2(dir.x, dir.z);

		if (dir != Vector3.zero || isDirectionKeyUp) SetInputDirection(dir);
	}
}
