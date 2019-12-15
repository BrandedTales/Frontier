using System;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100.0f;
        [SerializeField] float health;

        bool isAlive = true;

        public bool IsDead()
        {
            return (!isAlive);
        }

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
            if (!isAlive) return;

            GetComponent<Animator>().SetTrigger("die");
            isAlive = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();

        }
    }
}