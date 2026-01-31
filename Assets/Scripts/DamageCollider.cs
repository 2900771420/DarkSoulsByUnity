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

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStatu playerStatu = collision.GetComponent<PlayerStatu>();

                if (playerStatu != null)
                {
                    playerStatu.TakeDamage(currentWeaponDamage);
                }
            }

            if(collision.tag == "Enemy")
            {
                EnemyStatu enemyStatu = collision.GetComponent<EnemyStatu>();

                if (enemyStatu != null)
                {
                    enemyStatu.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }

}

