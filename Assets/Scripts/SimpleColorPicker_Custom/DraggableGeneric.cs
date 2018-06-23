// https://stackoverflow.com/questions/39437044/how-to-pick-color-from-raycast-hit-point-in-unity
// https://www.youtube.com/watch?v=wysIsMEQ3_Y

using UnityEngine;

public class DraggableGeneric : MonoBehaviour {

	public enum PickMode { CAMERA, MOUSE, TOUCH };
	public PickMode pickMode = PickMode.CAMERA;
	public float zPos = 1f;
	public ShowHideGeneric showHideGeneric;
	public Transform pointer;
	public Transform minBound;
	public bool fixX = false;
	public bool fixY = false;
	public bool fixZ = false;
	public Transform thumb;	

	[HideInInspector] public bool dragging;

	void FixedUpdate() {
		bool touchActive = false;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && GUIUtility.hotControl == 0) { 
			touchActive = true;
		} else if (Input.touchCount < 1 || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) {
			touchActive = false;
		}

		bool mouseActive = Input.GetMouseButton(0);
			
		Vector3 rayPos = Vector3.zero;

		if (pickMode == PickMode.CAMERA) {
			rayPos = pointer.position;
		} else if (pickMode == PickMode.MOUSE) {
			rayPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPos));
		} else if (pickMode == PickMode.TOUCH) {
			rayPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, zPos));
		}

		Vector3 rayDir = pointer.forward;

		if (showHideGeneric.isTracking) {//(sixCtl.menuDown) {
			dragging = false;
			Ray ray = new Ray(rayPos, rayDir);
			//var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
				if (pickMode == PickMode.CAMERA || ((touchActive || mouseActive))) dragging = true;
			}
		}

		if (!showHideGeneric.isTracking) dragging = false;

		if (dragging && showHideGeneric.isTracking) {
			Ray ray = new Ray(rayPos, rayDir);
			//var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity)) {
				//Camera.main.ScreenToWorldPoint(Input.mousePosition);
				var point = hit.point;//col.ClosestPointOnBounds(hit.point);
				SetThumbPosition(point); 
				//~
				//Vector3 oldMessage = Vector3.one - (thumb.position - GetComponent<Collider>().bounds.min) / GetComponent<Collider>().bounds.size.x;
				Vector3 message = Vector3.one - (thumb.localPosition - minBound.localPosition) / GetComponent<BoxCollider>().size.x;

				SendMessage("OnDrag", message);
				//Debug.Log("oldMessage: " + oldMessage + "   message: " + message);
			}
		}
	}

	void SetThumbPosition(Vector3 point) {
		Vector3 temp = thumb.localPosition;
		thumb.position = point;
		thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? temp.y : thumb.localPosition.y, thumb.localPosition.z-1);
	}

	/*
	void SetDragPoint(Vector3 point) {
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		SetThumbPosition(point);
	}
	*/

	/*
	void SetThumbPosition(Vector3 point) {
		thumb.position = new Vector3(fixX ? thumb.position.x : point.x, fixY ? thumb.position.y : point.y, fixZ ? thumb.position.z : point.z);
	}
	*/

}
