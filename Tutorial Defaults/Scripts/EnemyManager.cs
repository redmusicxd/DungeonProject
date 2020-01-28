using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{


    public bool online;
    PlayerCharacterController m_PlayerController;
    PlayerCharacterController_Photon m_PlayerController_Photon;

    public List<EnemyController> enemies { get; private set; }
    public int numberOfEnemiesTotal { get; private set; }
    public int numberOfEnemiesRemaining => enemies.Count;
    
    public UnityAction<EnemyController, int> onRemoveEnemy;

    private void Awake()
    {
        if (online)
        {

            m_PlayerController_Photon = FindObjectOfType<PlayerCharacterController_Photon>();
          //  DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController_Photon, EnemyManager>(m_PlayerController_Photon, this);
        }
        else
        {
            m_PlayerController = FindObjectOfType<PlayerCharacterController>();
           // DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);
        }
        enemies = new List<EnemyController>();
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);

        numberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesRemaining - 1;

        onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        enemies.Remove(enemyKilled);
    }
}
