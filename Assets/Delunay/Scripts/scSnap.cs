using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class scSnap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3((int) transform.position.x, (int) transform.position.y, (int) transform.position.z);
	}
}
