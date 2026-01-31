using System.Collections;
using System.Collections.Generic;
using LostLight;
using UnityEngine;

namespace LostLight {
    public class DamageCollider : MonoBehaviour
    {

        Collider damageCollider;

        public int currentWeaponDamage = 25;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hittalbe")
            {
                PlayerStatu playerStatus = other.GetComponent<PlayerStatu>();

                if (playerStatus != null)
                {
                    playerStatus.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }

}

