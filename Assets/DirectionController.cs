using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	[SerializeField] private RectTransform background;
	[SerializeField] private RectTransform joystick;

	private float radius;

	[SerializeField] private GameObject player;
	private PlayerMoveScript playerMove;

	private Vector3 inputDirection = Vector3.zero;
	private bool isTouch = false;


	// Use this for initialization
	void Start () {
		radius = background.rect.width * 0.5f;
		playerMove = player.GetComponent<PlayerMoveScript>();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!"Left".Equals(eventData.button.ToString())) return;
		Vector2 value = eventData.position - (Vector2)background.position;

		value = Vector2.ClampMagnitude(value, radius);
		joystick.localPosition = value;

		value = value.normalized;
		inputDirection = new Vector3(value.x, 0f, value.y);
		playerMove.SetInputDirection(inputDirection);

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!"Left".Equals(eventData.button.ToString())) return;
		isTouch = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!"Left".Equals(eventData.button.ToString())) return;
		isTouch = false;
		joystick.localPosition = Vector3.zero;

		inputDirection = Vector3.zero;
		playerMove.SetInputDirection(inputDirection);
	}
}
