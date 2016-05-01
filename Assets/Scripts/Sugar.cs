using UnityEngine;

namespace Assets.Scripts
{
    public class Sugar : MonoBehaviour
    {
        public int InitialCapacity = 100;

        public int Capacity { get; private set; }

        protected virtual void Start()
        {
            Capacity = InitialCapacity;
        }

        protected virtual void Update()
        {
            if (Capacity <= 0)
            {
                Destroy(gameObject);
            }
        }

        public bool PickSugar()
        {
            if (Capacity <= 0)
            {
                return false;
            }
            Capacity = Capacity - 1;
            return true;
        }
    }
}
