using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Image health_bar;
    float current_health;
    float max_health = 50;
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        health_bar = GetComponent<Image>();
        pc = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        current_health = pc.health;
        health_bar.fillAmount = current_health / max_health;
    }
}
