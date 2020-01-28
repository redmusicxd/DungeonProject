using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    Health m_health;
    TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {   
        m_health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = m_health.currentHealth.ToString();
        textMesh.SetText(m_health.currentHealth.ToString());
    }
}
