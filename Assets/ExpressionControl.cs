using UnityEngine;

public class ExpressionControl : MonoBehaviour {
	[SerializeField] private Animator animator;

	public void ExpressionDefault() {
		Debug.Log("Default expression");
		animator.SetTrigger("Default");
	}
	public void ExpressionJoy() {
		Debug.Log("Joyful expression");
		animator.SetTrigger("Joy");
	}
	public void ExpressionAngry() {
		Debug.Log("Angry expression");
		animator.SetTrigger("Angry");
	}
}
