using UnityEngine;

namespace Assets.Scripts
{
    public class Sugar : MonoBehaviour
    {
        public int InitialCapacity = 100;

        private int _capacity;

        protected virtual void Start()
        {
            _capacity = InitialCapacity;
        }

        protected virtual void Update()
        {
            if (_capacity <= 0)
            {
                Destroy(gameObject);
            }
        }

        public bool PickSugar()
        {
            if (_capacity <= 0)
            {
                return false;
            }
            _capacity--;
            return true;
        }
    }
}
