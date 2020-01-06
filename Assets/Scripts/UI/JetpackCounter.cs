using UnityEngine;
using UnityEngine.UI;

public class JetpackCounter : MonoBehaviour
{
    public bool online;
    public GameFlowManager_Photon managerOnline;
    public GameFlowManager managerOffile;
    [Tooltip("Image component representing jetpack fuel")]
    public Image jetpackFillImage;
    [Tooltip("Canvas group that contains the whole UI for the jetack")]
    public CanvasGroup mainCanvasGroup;
    [Tooltip("Component to animate the color when empty or full")]
    public FillBarColorChange fillBarColorChange;

    Jetpack m_Jetpack;
    JetPack_Photon m_Jetpack_Photon;

    void Awake()
    {
        if (online)
        {
            m_Jetpack_Photon = managerOnline.m_Player.GetComponent<JetPack_Photon>();

        }
        else
        {
            m_Jetpack = managerOffile.m_Player.GetComponent<Jetpack>();
        }

        fillBarColorChange.Initialize(1f, 0f);
    }

    void Update()
    {

        if (online)
        {
            mainCanvasGroup.gameObject.SetActive(m_Jetpack_Photon.isJetpackUnlocked);

            if (m_Jetpack_Photon.isJetpackUnlocked)
            {
                jetpackFillImage.fillAmount = m_Jetpack_Photon.currentFillRatio;
                fillBarColorChange.UpdateVisual(m_Jetpack_Photon.currentFillRatio);
            }
        }

        else
        {
            mainCanvasGroup.gameObject.SetActive(m_Jetpack.isJetpackUnlocked);

            if (m_Jetpack.isJetpackUnlocked)
            {
                jetpackFillImage.fillAmount = m_Jetpack.currentFillRatio;
                fillBarColorChange.UpdateVisual(m_Jetpack.currentFillRatio);
            }
        }

    }
}
