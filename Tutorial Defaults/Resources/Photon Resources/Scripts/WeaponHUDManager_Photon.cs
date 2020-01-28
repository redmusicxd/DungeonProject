using System.Collections.Generic;
using UnityEngine;

public class WeaponHUDManager_Photon : MonoBehaviour
{

    public GameFlowManager_Photon manager;
    [Tooltip("UI panel containing the layoutGroup for displaying weapon ammos")]
    public RectTransform ammosPanel;
    [Tooltip("Prefab for displaying weapon ammo")]
    public GameObject ammoCounterPrefab;

    PlayerWeaponsManager_Photon m_PlayerWeaponsManager;
    List<AmmoCounter_Photon> m_AmmoCounters = new List<AmmoCounter_Photon>();

    void Start()
    {

        m_PlayerWeaponsManager = manager.m_Player.m_WeaponsManager;
     
        WeaponController_Photon activeWeapon = m_PlayerWeaponsManager.GetActiveWeapon();
        if (activeWeapon)
        {
            AddWeapon(activeWeapon, m_PlayerWeaponsManager.activeWeaponIndex);
            ChangeWeapon(activeWeapon);
        }

        m_PlayerWeaponsManager.onAddedWeapon += AddWeapon;
        m_PlayerWeaponsManager.onRemovedWeapon += RemoveWeapon;
        m_PlayerWeaponsManager.onSwitchedToWeapon += ChangeWeapon;



    }

    void AddWeapon(WeaponController_Photon newWeapon, int weaponIndex)
    {
        GameObject ammoCounterInstance = Instantiate(ammoCounterPrefab, ammosPanel);
        AmmoCounter_Photon newAmmoCounter = ammoCounterInstance.GetComponent<AmmoCounter_Photon>();
        DebugUtility.HandleErrorIfNullGetComponent<AmmoCounter_Photon, WeaponHUDManager_Photon>(newAmmoCounter, this, ammoCounterInstance.gameObject);

        newAmmoCounter.Initialize(newWeapon, weaponIndex);

        m_AmmoCounters.Add(newAmmoCounter);
    }

    void RemoveWeapon(WeaponController_Photon newWeapon, int weaponIndex)
    {
        int foundCounterIndex = -1;
        for (int i = 0; i < m_AmmoCounters.Count; i++)
        {
            if (m_AmmoCounters[i].weaponCounterIndex == weaponIndex)
            {
                foundCounterIndex = i;
                Destroy(m_AmmoCounters[i].gameObject);
            }
        }

        if (foundCounterIndex >= 0)
        {
            m_AmmoCounters.RemoveAt(foundCounterIndex);
        }
    }

    void ChangeWeapon(WeaponController_Photon weapon)
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ammosPanel);
    }
}
