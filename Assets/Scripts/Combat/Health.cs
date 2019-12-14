using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100.0f;
        [SerializeField] float health;

        public bool isAlive = true;

        private void Start() 
        {
            health = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Clamp(health - damage, 0, maxHealth);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            isAlive = false;

        }
    }
}