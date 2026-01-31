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

            if(playerStatu != null)
            {
                playerStatu.TakeDamage(damage);
            }
        }
    }

}