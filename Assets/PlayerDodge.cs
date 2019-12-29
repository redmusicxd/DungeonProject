using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public PlayerCharacterController playerCharacter;
    public bool Active;

    private void Start()
   {
       playerCharacter = GetComponent<PlayerCharacterController>();
    }
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Z) && Active)
       {
           playerCharacter.maxSpeedOnGround = 200f;
       }
       else
       {
           if(playerCharacter.maxSpeedOnGround > 20f)
           {
           playerCharacter.maxSpeedOnGround = 13f;
           }
       }
    }

}