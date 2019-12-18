using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkSystemManager : MonoBehaviour
{

     public List<EnemyMobile> Enemy_HoverBot;
     public List<EnemyTurret> Enemy_Turret;



    public void ghostOn()
    {
        foreach (EnemyMobile enemymobile in Enemy_HoverBot)
        {
            enemymobile.enabled = false;
        } 

        foreach (EnemyTurret enemyturret in Enemy_Turret)
        {
            enemyturret.enabled = false;
        }
    }
    public void ghostOff()
    {
        foreach (EnemyMobile enemymobile in Enemy_HoverBot)
        {
            enemymobile.enabled = true;
        }
        foreach (EnemyTurret enemyturret in Enemy_Turret)
        {
            enemyturret.enabled = true;
        }
    }




}
