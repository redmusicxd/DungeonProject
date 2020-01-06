using UnityEngine;
using UnityEngine.UI;

public class StanceHUD : MonoBehaviour
{
    public bool online;

    [Tooltip("Image component for the stance sprites")]
    public Image stanceImage;
    [Tooltip("Sprite to display when standing")]
    public Sprite standingSprite;
    [Tooltip("Sprite to display when crouching")]
    public Sprite crouchingSprite;

    private void Start()
    {
        if (online)
        {
            PlayerCharacterController_Photon character = FindObjectOfType<PlayerCharacterController_Photon>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController_Photon, StanceHUD>(character, this);
            character.onStanceChanged += OnStanceChanged;

            OnStanceChanged(character.isCrouching);
        }
        else
        {
            PlayerCharacterController character = FindObjectOfType<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, StanceHUD>(character, this);
            character.onStanceChanged += OnStanceChanged;

            OnStanceChanged(character.isCrouching);
        }


    }

    void OnStanceChanged(bool crouched)
    {
        stanceImage.sprite = crouched ? crouchingSprite : standingSprite;
    }
}
