using UnityEngine;
using System.Collections;

public class SimpleController : MonoBehaviour {
	public float speed;
	public float angularSpeed;

	void Update() {
		float mouseInputX = Input.GetAxis ("Mouse X");
		Vector3 forward = Quaternion.AngleAxis (mouseInputX, Vector3.up) * transform.forward;
		transform.rotation = Quaternion.LookRotation (forward);

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		transform.position += transform.forward * vertical * speed + transform.right * horizontal * speed;
	}
}
