using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingCamera : MonoBehaviour {


	#region Inspector Fields
	[SerializeField] private GameObject player;
	[SerializeField] private float m_distanceFromPlayer;
	[SerializeField] private float m_cameraChanseTime;
	[SerializeField] private Vector3 m_cameraOffset;
	[SerializeField] private float m_xAxisOffset;

	#endregion

	#region Class Members 
	private Transform m_playerTransform;
	private Transform m_cameraTransform;
	private Vector3 m_cameraPosition;
	private Vector3 m_playerPosition;
	private Vector2 m_playerVelocity;
	private Vector3 m_leftCameraOffset;
	private Vector3 m_rightCameraOffset;


	#endregion

	#region Engine Methods
	private void Start(){
		m_playerTransform = player.transform; 
		m_cameraTransform = this.transform;
		m_playerPosition = player.transform.position; 
		m_cameraPosition = this.transform.position;
		m_playerVelocity = player.GetComponent<Rigidbody2D> ().velocity;

	}


	private void LateUpdate () {
		m_leftCameraOffset = new Vector3 (m_cameraOffset.x - m_xAxisOffset, m_cameraOffset.y, m_cameraOffset.z);
		m_rightCameraOffset = new Vector3 (m_cameraOffset.x + m_xAxisOffset, m_cameraOffset.y, m_cameraOffset.z);

		if (Mathf.Abs(Vector2.Distance (this.transform.position - m_cameraOffset , player.transform.position - m_cameraOffset)) > m_distanceFromPlayer) {
			if (m_playerVelocity.x < 0) {
				StartCoroutine(MoveCamera(this.transform.position - m_leftCameraOffset,player.transform.position - m_leftCameraOffset, m_cameraChanseTime));

			} else if (m_playerVelocity.x > 0) {
				StartCoroutine(MoveCamera(this.transform.position - m_rightCameraOffset,player.transform.position - m_cameraOffset, m_cameraChanseTime));

			} else {
				StartCoroutine(MoveCamera(this.transform.position - m_cameraOffset,player.transform.position - m_cameraOffset, m_cameraChanseTime));
			}

		} 

	}
	#endregion

	#region Corouteins

	private IEnumerator MoveCamera (Vector3 startPosition, Vector3 endPosition, float movementTime ){
		float t = 0f;
		while (t <= 1f) {
			t += Time.deltaTime/movementTime;
			transform.position = Vector3.Lerp(startPosition, endPosition,t);
			yield return null;
		}

	}
	#endregion
}
