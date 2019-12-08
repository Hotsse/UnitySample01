using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnController : MonoBehaviour, IPointerDownHandler {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		MainEngineScript.GetInstance().CreateEnermy(player.transform.position + player.transform.forward * 5.0f, player.transform.rotation);
	}
}
