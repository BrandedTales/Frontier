using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        Transform target;
        Mover mover;

        [SerializeField] float weaponRange = 2.0f;

        private void Start() 
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (target == null) return;
            
            if ((target != null) && !GetInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Stop();
            }
        }

        private bool GetInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Cancel()
        {
            target = null;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }
    }
}