using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableManager : MonoBehaviour {

	public DraggableGeneric[] draggers;
	public enum PickMode { CAMERA, MOUSE, TOUCH };
	public PickMode pickMode = PickMode.CAMERA;
	public float zPos = 1f;

	[HideInInspector] public bool blocked = false;

	private void Start() {
		for (int i=0; i<draggers.Length; i++) {
			draggers[i].pickMode = (DraggableGeneric.PickMode) pickMode;
			draggers[i].zPos = zPos;
		}
	}

	private void FixedUpdate() {
		if (pickMode != PickMode.CAMERA) {
			blocked = false;
			for (int i=0; i<draggers.Length; i++) {
				if (draggers[i].dragging) {
					blocked = true;
				}
			}
		}
	}

}
