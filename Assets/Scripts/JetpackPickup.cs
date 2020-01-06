using UnityEngine;

public class JetpackPickup : MonoBehaviour
{
    public bool Online;
    Pickup m_Pickup;

    void Start()
    {


        m_Pickup = GetComponent<Pickup>();
        Online = m_Pickup.Online;
        DebugUtility.HandleErrorIfNullGetComponent<Pickup, JetpackPickup>(m_Pickup, this, gameObject);

        // Subscribe to pickup action
        m_Pickup.onPick += OnPicked;
        m_Pickup.onPick_Photon += OnPickedOnline;
    }

    void OnPicked(PlayerCharacterController byPlayer)
    {
        if(Online)
        { }

        var jetpack = byPlayer.GetComponent<Jetpack>();
        if (!jetpack)
            return;

        if (jetpack.TryUnlock())
        {
            m_Pickup.PlayPickupFeedback();

            Destroy(gameObject);
        }
    }

    void OnPickedOnline(PlayerCharacterController_Photon byPlayer)
    {
        var jetpack = byPlayer.GetComponent<JetPack_Photon>();
        if (!jetpack)
            return;

        if (jetpack.TryUnlock())
        {
            m_Pickup.PlayPickupFeedback();

            Destroy(gameObject);
        }
    }

}
