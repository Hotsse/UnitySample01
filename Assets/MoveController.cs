using UnityEngine;
using UnityEngine.EventSystems;

public class MoveController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	[SerializeField] private RectTransform background;
	[SerializeField] private RectTransform joystick;

	private float radius;

	[SerializeField] private GameObject player;
	private PlayerMoveScript playerMove;

	private Vector3 movePosition = Vector3.zero;
	private bool isTouch = false;


	// Use this for initialization
	void Start () {
		radius = background.rect.width * 0.5f;
		playerMove = player.GetComponent<PlayerMoveScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 value = eventData.position - (Vector2)background.position;

		value = Vector2.ClampMagnitude(value, radius);
		joystick.localPosition = value;

		float dist = Vector2.Distance(background.position, joystick.position) / radius;

		value = value.normalized;
		movePosition = new Vector3(value.x * dist, 0f, value.y * dist);
		playerMove.SetInputPosition(movePosition);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isTouch = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isTouch = false;
		joystick.localPosition = Vector3.zero;

		movePosition = Vector3.zero;
		playerMove.SetInputPosition(movePosition);
	}
}
