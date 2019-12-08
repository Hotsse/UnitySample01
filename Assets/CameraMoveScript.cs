using UnityEngine;

public class CameraMoveScript : MonoBehaviour {

	[SerializeField]
	private GameObject _player;

	[SerializeField]
	private float offsetX;
	[SerializeField]
	private float offsetY;
	[SerializeField]
	private float offsetZ;
	[SerializeField]
	private float followSpeed;

	private Vector3 cameraPosition;

	private float shakeDuration = 0.0f;
	private float shakeAmount = 0.1f;
	private float decreaseFactor = 1.0f;

	void LateUpdate () {

		cameraPosition.x = _player.transform.position.x + offsetX;
		cameraPosition.y = _player.transform.position.y + offsetY;
		cameraPosition.z = _player.transform.position.z + offsetZ;

		cameraPosition += _player.transform.forward * 5f;

		this.transform.position = Vector3.Lerp(this.transform.position, cameraPosition, followSpeed * Time.deltaTime);

		if (shakeDuration <= 0.0f)
		{
			this.transform.position = cameraPosition;
		}
		else
		{
			this.transform.position = cameraPosition + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
	}

	public void CreateShake(float duration, float amount = 0.1f)
	{
		shakeDuration = duration;
		shakeAmount = amount;
	}
}
