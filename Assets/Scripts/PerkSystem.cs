using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : MonoBehaviour
{
    [Header("Objects")]
    public Health healthCS;
    public PlayerCharacterController playerCharacterControllerCS;
    public PerkSystemManager perkSystemManagerCS;


    [Header("Ghost Ability")]
    public bool ghostingAbility;
    public int ghostingAbilityTime;
    [Header("Dodge Ability")]
    public bool dodgeAbility;
    [Header("Invincibility Ability")]
    public bool invincibilityAbility;
    public int invincibilityAbilityTime;
    [Header("SpeedBoost Ability")]
    public bool speedBoost;

    int ghostTime;
    int invTime;


    void Update()
    {
        if (ghostingAbility) { ghost(); }
        if (dodgeAbility) { dodge(); }
        if (invincibilityAbility) { invincibility(); }
        if (speedBoost) { speedboost(); }

    }

    public void ghost()
    {

        ghostTime++;
        if (ghostTime == ghostingAbilityTime)
        {
            perkSystemManagerCS.ghostOff();
            ghostingAbility = false;
            ghostTime = 0;
        }
        else { perkSystemManagerCS.ghostOn(); }

    }   
    public void dodge()
    { 

    
    }  
    public void invincibility()
    {

        invTime++;
        if (invTime == invincibilityAbilityTime)
        {
            healthCS.invincible = false;
            invincibilityAbility = false;
            invTime = 0;
        }
        else { healthCS.invincible = true; }

    }   
    public void speedboost()
    { 

    
    }

}
