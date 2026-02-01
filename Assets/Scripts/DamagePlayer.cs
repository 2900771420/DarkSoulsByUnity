using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LostLight { 
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStatu playerStatu = other.GetComponent<PlayerStatu>();
            EnemyStatu enemyStatu = other.GetComponent<EnemyStatu>();

            if(playerStatu != null)
            {
                playerStatu.TakeDamage(damage);
            }
            if(enemyStatu != null)
            {
                enemyStatu.TakeDamage(damage);
            }
        }
    }

}