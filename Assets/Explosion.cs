using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Explosion : MonoBehaviour
{
    public GameObject bomb;
    public bool active;
    public float power = 10.0f;  //power of explosion force.
    public float radius = 5.0f;  //radius of the explosion force.
    public float upforce = 1.0f; //upforce lifts the gameobject off the ground. 
    void FixedUpdate()
    {
        if(active)
        {
            if (Input.GetKeyDown("b"))
            { // This checks once bomb is in scene or gameobject gets enabled by adding it to the scene.
                SendMessage("Detonate"); //Calls Detonate() after 5 seconds 
            }
        }

    }
   //private void Start()
   //{
   //    bomb = GameObject.FindWithTag("Player");
   //}
    void Detonate()
    {
        Vector3 explosionPosition = bomb.transform.position; //Grabs position of bomb and stores it in a Vector3 explosionPosition
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);  // Stores an array of colliders hit by the OverlapSphere at position explosionPosition and with a radius of radius.
        foreach (Collider hit in colliders) // colliders is array name 
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>(); //get rigibody and store in rb from hit
            if (rb != null) //Won't throw error if hit gameobject doesn't have a rigidbody
            {
                rb.AddExplosionForce(power, explosionPosition, radius, upforce, ForceMode.Impulse); //This is where force gets applied to the rigidbody grabbed from each foreach.
            }
        }
    }
}