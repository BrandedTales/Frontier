using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health target;
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
            if (target.IsDead()) return;

            if (!InRange())
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            if ((targetToTest != null) && (!targetToTest.IsDead())) return true;
            else return false;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);

            if ((!target.IsDead()) && (timeSinceLastAttack >= timeBetweenAttacks))
            {
                timeSinceLastAttack = 0;
                //This will trigger "Hit()"
                StartAttack();
            }

        }

        private void StartAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool InRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //Animation Event
        void Hit() 
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
    }
}