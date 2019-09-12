using UnityEngine;

namespace LH
{
    public class IKChain : MonoBehaviour
    {
        public float length;
        public IKJoint[] joints;

        public void Init()
        {
            for (int i = 0; i < joints.Length - 1; i++)
            {
                IKJoint joint = joints[i];
                IKJoint jointNext = joints[i + 1];
                joint.length = Vector3.Distance(joint.position, jointNext.position);
                length += joint.length;
            }

            if (joints.Length > 0)
            {
                joints[joints.Length - 1].isEndEffector = true;
                joints[0].useConstraint = false;
            }
        }

        public void SolveFABRIK(Vector3 target)
        {
            if (joints.Length == 0)
                return;

            float distFromRootToTarget = Vector3.Distance(joints[0].position, target);
            if (distFromRootToTarget > length)
            {
                //can't reach
                for (int i = 0; i < joints.Length; i++)
                {
                    IKJoint joint = joints[i];
                    if (i > 0)
                    {
                        IKJoint parent = joints[i - 1];
                        float distFromParentToTarget = Vector3.Distance(parent.position, target);
                        float t = parent.length / distFromParentToTarget;
                        joint.position = Vector3.Lerp(parent.position, target, t);
                    }
                    joint.rotation = Quaternion.FromToRotation(Vector3.up, target - joint.position);
                }
            }
            else
            {
                //can reach
                Vector3 origin = joints[0].position;

                // forward interation
                Vector3 newPos = target;
                for (int i = joints.Length - 1; i > 0; i--)
                {
                    IKJoint joint = joints[i];
                    joint.position = newPos;
                    if (i < joints.Length - 1)
                    {
                        IKJoint next = joints[i + 1];
                        joint.rotation = Quaternion.FromToRotation(Vector3.up, next.position - joint.position);
                    }
                    IKJoint parent = joints[i - 1];
                    float distFromParentToCurrent = Vector3.Distance(parent.position, joint.position);
                    float t = parent.length / distFromParentToCurrent;
                    newPos = Vector3.Lerp(joint.position, parent.position, t);
                }
                joints[0].position = newPos;
                //if (joints.Length >= 2)
                //{
                //    joints[0].rotation = Quaternion.FromToRotation(Vector3.up, joints[1].position - joints[0].position);
                //}

                // backward interation
                newPos = origin;
                for (int i = 0; i < joints.Length - 1; i++)
                {
                    IKJoint joint = joints[i];
                    IKJoint next = joints[i + 1];
                    joint.position = newPos;

                    float distFromCurrentToNext = Vector3.Distance(joint.position, next.position);
                    float t = joint.length / distFromCurrentToNext;
                    newPos = Vector3.Lerp(joint.position, next.position, t);
                    joint.rotation = Quaternion.FromToRotation(Vector3.up, newPos - joint.position);
                }
                if (joints.Length >= 2)
                {
                    joints[joints.Length - 1].rotation = joints[joints.Length - 2].rotation;
                }
                joints[joints.Length - 1].position = newPos;
            }
        }
    }
}