using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Transform target;
        Mover mover;

        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] float timeBetweenAttacks = 1.0f;

        [SerializeField] float weaponDamage = 5.0f;

        float timeSinceLastAttack = 0;

        private void Start() 
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {

            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            
            if (!InRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if (target.GetComponent<Health>().isAlive && (timeSinceLastAttack >= timeBetweenAttacks))
            {
                timeSinceLastAttack = 0;
                //This will trigger "Hit()"
                GetComponent<Animator>().SetTrigger("attack");

            }

        }

        private bool InRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Cancel()
        {
            target = null;
        }

        //Animation Event
        void Hit() 
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }
    }
}