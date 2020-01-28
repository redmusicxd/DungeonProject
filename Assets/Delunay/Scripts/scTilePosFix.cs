using UnityEngine;
using System.Collections;

public class scTilePosFix : MonoBehaviour {
	
	
	public float yShift;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y + yShift, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
