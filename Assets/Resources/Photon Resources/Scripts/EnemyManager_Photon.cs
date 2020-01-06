using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager_Photon : MonoBehaviour
{


    public bool online;
    PlayerCharacterController_Photon m_PlayerController_Photon;

    public List<EnemyController_Photon> enemies { get; private set; }
    public int numberOfEnemiesTotal { get; private set; }
    public int numberOfEnemiesRemaining => enemies.Count;

    public UnityAction<EnemyController_Photon, int> onRemoveEnemy;

    private void Awake()
    {

            m_PlayerController_Photon = FindObjectOfType<PlayerCharacterController_Photon>();
            //  DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController_Photon, EnemyManager>(m_PlayerController_Photon, this);
        
        enemies = new List<EnemyController_Photon>();
    }

    public void RegisterEnemy(EnemyController_Photon enemy)
    {
        enemies.Add(enemy);

        numberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(EnemyController_Photon enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesRemaining - 1;

        onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        enemies.Remove(enemyKilled);
    }
}
