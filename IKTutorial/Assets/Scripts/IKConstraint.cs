using UnityEngine;

namespace LH
{
    public class IKConstraint : MonoBehaviour
    {
        public IKJoint joint;
        public Transform target;

        private void Awake()
        {
            joint = GetComponent<IKJoint>();
        }

        private void OnDrawGizmos()
        {
            if (joint == null)
            {
                joint = GetComponent<IKJoint>();
            }

            Vector3 constraintPosition = Constraint();
            Gizmos.DrawWireCube(constraintPosition, Vector3.one * 0.05f);
        }

        private Vector3 Constraint()
        {
            if (joint.useConstraint)
            {
                Vector3 toTarget = target.position - joint.position;
                Vector3 up = Vector3.Project(toTarget, joint.transform.up);
                float h = up.magnitude;
                Vector3 centerToTarget = target.position - (joint.position + up);
                float size = Mathf.Abs(h * Mathf.Tan(Mathf.Deg2Rad * joint.constrainAngle));
                if (centerToTarget.magnitude < size)
                {
                    return target.position;
                }
                else
                {
                    return centerToTarget.normalized * size + up + transform.position;
                }
            }
            return target.position;
        }
    }
}