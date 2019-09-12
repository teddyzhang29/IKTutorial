using UnityEngine;

namespace LH
{
    public class FABRIK : MonoBehaviour
    {
        public bool useConstraint = true;
        public Transform target;
        public IKChain[] chains;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            foreach (var chain in chains)
            {
                chain.Init(this);
            }
        }

        private void Update()
        {
            foreach (var chain in chains)
            {
                chain.SolveFABRIK(target.position);
            }
        }
    }
}