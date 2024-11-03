using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlendshapeController : MonoBehaviour {
	[SerializeField] private SkinnedMeshRenderer face;
	[SerializeField] private TextMeshProUGUI status;
	[SerializeField] float speed;
	private List<float> targetWeights = new List<float>();
	private string currentBlendshape, currentExpression;

	private void Start() {
		//set default values
		currentExpression = "neutral";
		currentBlendshape = "Fcl_BRW_Angry";
		for (int i = 0; i < face.sharedMesh.blendShapeCount; i++) {
			targetWeights.Add(face.GetBlendShapeWeight(i));
		}
	}
	void Update() {
		//move towards target weight
		for (int i = 0; i < targetWeights.Count; i++) {
			face.SetBlendShapeWeight(i, Mathf.MoveTowards(face.GetBlendShapeWeight(i), targetWeights[i], Time.deltaTime * speed));
		}

		//update current expression status
		ExpressionStatus(currentExpression, currentBlendshape);

		//process inputs
		if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				//increase joy
				ChangeBlendshapeWeight("Fcl_ALL_Joy", 10);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				//increase angry
				ChangeBlendshapeWeight("Fcl_ALL_Angry", 10);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				//increase surprise
				ChangeBlendshapeWeight("Fcl_ALL_Surprised", 10);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				//increase sorrow
				ChangeBlendshapeWeight("Fcl_ALL_Sorrow", 10);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5) && GetBlendshapeWeight("Fcl_BRW_Angry") < 100) {
				//increase scream
				ChangeBlendshapeWeight("Fcl_EYE_Close", 5.1f);
				ChangeBlendshapeWeight("Fcl_EYE_Sorrow", 10);
				ChangeBlendshapeWeight("Fcl_MTH_Up", 3.3f);
				ChangeBlendshapeWeight("Fcl_MTH_Sorrow", 1);
				ChangeBlendshapeWeight("Fcl_MTH_Surprised", 10);
				ChangeBlendshapeWeight("Fcl_MTH_A", 0.2f);
				ChangeBlendshapeWeight("Fcl_BRW_Angry", 10);
			}
		}
		else {
			if (Input.GetKeyDown(KeyCode.Alpha0)) {
				ExpressionNeutral();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				ExpressionJoy();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				ExpressionAngry();
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				ExpressionSurprised();
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				ExpressionSorrow();
			}
			if (Input.GetKeyDown(KeyCode.Alpha5)) {
				ExpressionScream();
			}
		}
	}

	public void ExpressionNeutral() {
		ResetBlendshapeWeights();
	}
	public void ExpressionJoy() {
		//joy
		ResetBlendshapeWeights();
		SetBlendshapeWeight("Fcl_ALL_Joy", 100);
	}
	public void ExpressionAngry() {
		//angry
		ResetBlendshapeWeights();
		SetBlendshapeWeight("Fcl_ALL_Angry", 100);
	}
	public void ExpressionSurprised() {
		//surprised
		ResetBlendshapeWeights();
		SetBlendshapeWeight("Fcl_ALL_Surprised", 100);
	}
	public void ExpressionSorrow() {
		//sorrow
		ResetBlendshapeWeights();
		SetBlendshapeWeight("Fcl_ALL_Sorrow", 100);
	}
	public void ExpressionScream() {
		//screaming
		ResetBlendshapeWeights();
		SetBlendshapeWeight("Fcl_EYE_Close", 51);
		SetBlendshapeWeight("Fcl_EYE_Sorrow", 100);
		SetBlendshapeWeight("Fcl_MTH_Up", 33);
		SetBlendshapeWeight("Fcl_MTH_Sorrow", 10);
		SetBlendshapeWeight("Fcl_MTH_Surprised", 100);
		SetBlendshapeWeight("Fcl_MTH_A", 2);
		SetBlendshapeWeight("Fcl_BRW_Angry", 100);
	}

	//returns target weights to zero and sets expression to neutral
	void ResetBlendshapeWeights() {
		for (int i = 0; i < targetWeights.Count; i++) {
			targetWeights[i] = 0;
		}
		currentExpression = "neutral";
	}

	//sets blendshape weight given blendshape name with value
	void SetBlendshapeWeight(string blendShapeName, float weight) {
		SetBlendshapeWeight(face.sharedMesh.GetBlendShapeIndex(blendShapeName), weight);
	}

	//modifies blendshape weight given blendshape name with value
	void ChangeBlendshapeWeight(string blendShapeName, float weight) {
		SetBlendshapeWeight(face.sharedMesh.GetBlendShapeIndex(blendShapeName), GetBlendshapeWeight(blendShapeName) + weight);
	}

	//returns weight of blendshape given blendshape name
	float GetBlendshapeWeight(string blendShapeName) {
		return face.GetBlendShapeWeight(face.sharedMesh.GetBlendShapeIndex(blendShapeName));
	}

	//sets blendshape weight given blendshape index with value
	void SetBlendshapeWeight(int blendShapeIndex, float weight) {
		//limits weight to a range of 0 to 100 so it doesn't become deformed
		targetWeights[blendShapeIndex] = Mathf.Clamp(weight, 0, 100);

		//sets current expression and weight value for the expression status
		currentBlendshape = face.sharedMesh.GetBlendShapeName(blendShapeIndex);
		switch (blendShapeIndex) {
			case 1:
				currentExpression = "angry";
				break;
			case 3:
				currentExpression = "joy";
				break;
			case 4:
				currentExpression = "sorrow";
				break;
			case 5:
				currentExpression = "surprised";
				break;
			case 6:
				currentExpression = "scream";
				break;
			default:
				currentExpression = "neutral";
				break;
		}

	}

	//shows current expression status
	void ExpressionStatus(string expression, string blendshape) {
		if (expression == "neutral") {
			status.text = "Current expression: " + expression + " " + (100 - GetBlendshapeWeight(blendshape)) + "%";
		}
		else {
			status.text = "Current expression: " + expression + " " + GetBlendshapeWeight(blendshape) + "%";
		}
	}
}
