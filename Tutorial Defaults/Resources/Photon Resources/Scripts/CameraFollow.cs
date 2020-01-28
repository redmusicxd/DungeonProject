using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;


    void FixedUpdate()
    {

        transform.rotation = player.rotation;
        transform.position = player.position * Time.deltaTime;

    }
}
