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
		currentExpression = "neutral";
		currentBlendshape = "Fcl_BRW_Angry";
		for (int i = 0; i < face.sharedMesh.blendShapeCount; i++) {
			targetWeights.Add(face.GetBlendShapeWeight(i));
		}
	}
	void Update() {
		for (int i = 0; i < targetWeights.Count; i++) {
			face.SetBlendShapeWeight(i, Mathf.MoveTowards(face.GetBlendShapeWeight(i), targetWeights[i], Time.deltaTime * speed));
		}
		ExpressionStatus(currentExpression, currentBlendshape);
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
				ResetBlendshapeWeights();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				//joy
				ResetBlendshapeWeights();
				SetBlendshapeWeight("Fcl_ALL_Joy", 100);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				//angry
				ResetBlendshapeWeights();
				SetBlendshapeWeight("Fcl_ALL_Angry", 100);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				//surprised
				ResetBlendshapeWeights();
				SetBlendshapeWeight("Fcl_ALL_Surprised", 100);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				//surprised
				ResetBlendshapeWeights();
				SetBlendshapeWeight("Fcl_ALL_Sorrow", 100);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5)) {
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
		}
	}

	void ResetBlendshapeWeights() {
		for (int i = 0; i < targetWeights.Count; i++) {
			targetWeights[i] = 0;
		}
		currentExpression = "neutral";
	}

	void SetBlendshapeWeight(string blendShapeName, float weight) {
		SetBlendshapeWeight(face.sharedMesh.GetBlendShapeIndex(blendShapeName), weight);
	}

	void ChangeBlendshapeWeight(string blendShapeName, float weight) {
		SetBlendshapeWeight(face.sharedMesh.GetBlendShapeIndex(blendShapeName), GetBlendshapeWeight(blendShapeName) + weight);
	}

	float GetBlendshapeWeight(string blendShapeName) {
		return face.GetBlendShapeWeight(face.sharedMesh.GetBlendShapeIndex(blendShapeName));
	}

	void SetBlendshapeWeight(int blendShapeIndex, float weight) {
		targetWeights[blendShapeIndex] = Mathf.Clamp(weight, 0, 100);
		currentBlendshape = face.sharedMesh.GetBlendShapeName(blendShapeIndex);
		//Debug.Log(currentBlendshape + " " + blendShapeIndex);
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

	void ExpressionStatus(string expression, string blendshape) {
		if (expression == "neutral") {
			status.text = "Current expression: " + expression + " " + (100 - GetBlendshapeWeight(blendshape)) + "%";
		}
		else {
			status.text = "Current expression: " + expression + " " + GetBlendshapeWeight(blendshape) + "%";
		}
	}
}
