using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.5f;
        private void OnDrawGizmos() 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i, transform.childCount);
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i, int maxIndex)
        {
            if ((i+1) == maxIndex)
                return 0;
            else
                return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}