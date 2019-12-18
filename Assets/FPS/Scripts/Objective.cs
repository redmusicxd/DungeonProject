using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Objective : MonoBehaviour
{
    [Tooltip("Short text explaining the objective that will be shown on screen")]
    public string title;
    [Tooltip("Short text explaining the objective that will be shown on screen")]
    public string description;
    [Tooltip("Whether the objective is required to win or not")]
    public bool isOptional;
    [Tooltip("Delay before theobjective becomes visible")]
    public float delayVisible;
    List<OpenDoor> Doors = new List<OpenDoor>();
    public bool rewardable;
    public List<WeaponController> rewards = new List<WeaponController>();
    PlayerWeaponsManager player;
    Collider other;
    public bool isCompleted { get; private set; }
    public bool isBlocking() => !(isOptional || isCompleted);

    public UnityAction<UnityActionUpdateObjective> onUpdateObjective;

    NotificationHUDManager m_NotificationHUDManager;
    ObjectiveHUDManger m_ObjectiveHUDManger;

    void Start()
    {
        // add this objective to the list contained in the objective manager
        ObjectiveManager objectiveManager = FindObjectOfType<ObjectiveManager>();
        DebugUtility.HandleErrorIfNullFindObject<ObjectiveManager, Objective>(objectiveManager, this);
        objectiveManager.RegisterObjective(this);

        // register this objective in the ObjectiveHUDManger
        m_ObjectiveHUDManger = FindObjectOfType<ObjectiveHUDManger>();
        DebugUtility.HandleErrorIfNullFindObject<ObjectiveHUDManger, Objective>(m_ObjectiveHUDManger, this);
        m_ObjectiveHUDManger.RegisterObjective(this);

        // register this objective in the NotificationHUDManager
        m_NotificationHUDManager = FindObjectOfType<NotificationHUDManager>();
        DebugUtility.HandleErrorIfNullFindObject<NotificationHUDManager, Objective>(m_NotificationHUDManager, this);
        m_NotificationHUDManager.RegisterObjective(this);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerWeaponsManager>();
        
    }

    public void UpdateObjective(string descriptionText, string counterText, string notificationText)
    {
        onUpdateObjective.Invoke(new UnityActionUpdateObjective(this, descriptionText, counterText, false, notificationText));
    }
    void DoorOpen(OpenDoor door)
    {
        door.open = true;
    }



    public void CompleteObjective(string descriptionText, string counterText, string notificationText)
    {
        isCompleted = true;
        foreach(var door in Doors)
        {
            DoorOpen(door);
        }
        foreach(var weapon in rewards)
        {
          if(rewardable)
            {
                player.AddWeapon(weapon);
            }
         
        }
        onUpdateObjective.Invoke(new UnityActionUpdateObjective(this, descriptionText, counterText, true, notificationText));

        // unregister this objective form both HUD managers
        m_ObjectiveHUDManger.UnregisterObjective(this);
        m_NotificationHUDManager.UnregisterObjective(this);
    }
}

public class UnityActionUpdateObjective
{
    public Objective objective;
    public string descriptionText;
    public string counterText;
    public bool isComplete;
    public string notificationText;

    public UnityActionUpdateObjective(Objective objective, string descriptionText, string counterText, bool isComplete, string notificationText)
    {
        this.objective = objective;
        this.descriptionText = descriptionText;
        this.counterText = counterText;
        this.isComplete = isComplete;
        this.notificationText = notificationText;
    }
}