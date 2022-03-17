using UnityEngine;

namespace Modula.Common
{
    public class SimpleFollow : MonoBehaviour
    {
        public Transform target;

        private void Update()
        {
            if (target == null) return;
            transform.position = target.position;
        }
    }
}