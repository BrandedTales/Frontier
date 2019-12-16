using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5.0f;
        [SerializeField] float suspicionTimer = 3.0f;
        [SerializeField] PatrolPath patrolpath;
        [SerializeField] float waypointTolerance = 1.0f;
        [SerializeField] float dwellTime = 3f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        int currentWaypointIndex = 0;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;


        private void Start() 
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if ((InAttackRangeOfPlayer()) && (fighter.CanAttack(player)))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTimer)
            {
                SuspicionBehavior();
            }
            else
            {
                GuardBehavior();
            }

            UpdateTimers();

        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void GuardBehavior()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolpath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > dwellTime)
                mover.StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolpath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolpath.GetNextIndex(currentWaypointIndex, patrolpath.transform.childCount);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            if (distanceToWaypoint < waypointTolerance)
                return true;
            else
                return false;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            return distance < chaseDistance;
        }

        //Called by Unity
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}