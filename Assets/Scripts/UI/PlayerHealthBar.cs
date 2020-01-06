using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public bool online;
    public GameFlowManager_Photon managerOnline;
    public GameFlowManager managerOffile;

    [Tooltip("Image component dispplaying current health")]
    public Image healthFillImage;

    Health m_PlayerHealth;

    private void Start()
    {
        if (online)
        {


            m_PlayerHealth = managerOnline.m_Player.GetComponent<Health>();

        }
        else
        {

            m_PlayerHealth = managerOffile.m_Player.GetComponent<Health>();

        }
    }

    void Update()
    {

        healthFillImage.fillAmount = m_PlayerHealth.currentHealth;
    }
}
