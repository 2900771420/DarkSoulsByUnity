using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LostLight
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
        public void SetCurrentHealth(int health)
        {
            slider.value = health;
        }
    }
}

