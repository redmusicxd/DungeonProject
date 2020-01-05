using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public PlayerCharacterController playerCharacter;
    public bool Active;
    public float maxDashTime = 1.0f;
    public float dashStoppingSpeed = 0.1f;
    private float currentDashTime;

    private void Start()
   {
       playerCharacter = GetComponent<PlayerCharacterController>();
       currentDashTime = maxDashTime;
    }
    void Update()
    {
        if (Input.GetButton(GameConstants.k_ButtonNameDash) && Active)
        {
            currentDashTime = 0.0f;
        }
        if (currentDashTime < maxDashTime)
        {
            playerCharacter.anspeed = 3f;
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            playerCharacter.anspeed = 1f;
        }
    }

}