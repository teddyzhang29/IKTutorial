using UnityEngine;

namespace LH
{
    public class FABRIK : MonoBehaviour
    {
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
                chain.Init();
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