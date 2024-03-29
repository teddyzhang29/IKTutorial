﻿using UnityEngine;

namespace LH
{
    public class IKJoint : MonoBehaviour
    {
        public Vector3 position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public float length;
        public bool isEndEffector;
        public Vector3 solvePosition;
        public IKChain ikChain;

        [Space]
        public bool useConstraint = true;
        public float constrainHeight = 2;
        public float constrainAngle = 30;

        public void Init(IKChain ikChain)
        {
            this.ikChain = ikChain;
        }

        private void OnDrawGizmos()
        {
            if (useConstraint)
            {
                const int segmentCount = 36;
                Gizmos.color = Color.green;

                Quaternion headRotation = Quaternion.FromToRotation(Vector3.down, -transform.up);
                Vector3 heightCenter = transform.position - transform.up * constrainHeight;

                Quaternion rotationPerSegment = Quaternion.AngleAxis(360f / segmentCount, -transform.up);
                Vector3 point = heightCenter + transform.forward * constrainHeight * Mathf.Tan(Mathf.Deg2Rad * constrainAngle);
                for (int i = 0; i < segmentCount; i++)
                {
                    Vector3 centerToPoint = point - heightCenter;
                    Vector3 nextPoint = heightCenter + rotationPerSegment * centerToPoint;
                    Gizmos.DrawLine(point, nextPoint);

                    if (i % 4 == 0)
                    {
                        Gizmos.DrawLine(transform.position, point);
                    }

                    point = nextPoint;
                }
            }
        }

        public Vector3 Constraint(Vector3 targetPosition)
        {
            if (useConstraint && ikChain.useConstraint)
            {
                Vector3 toTarget = targetPosition - position;
                Vector3 up = Vector3.Project(toTarget, transform.up);
                float h = up.magnitude;
                Vector3 centerToTarget = targetPosition - (position + up);
                float size = Mathf.Abs(h * Mathf.Tan(Mathf.Deg2Rad * constrainAngle));
                if (centerToTarget.magnitude < size)
                {
                    return targetPosition;
                }
                else
                {
                    return centerToTarget.normalized * size + up + transform.position;
                }
            }
            return targetPosition;
        }
    }
}