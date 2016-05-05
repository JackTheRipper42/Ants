using UnityEngine;

namespace Assets.Scripts
{
    public class Sugar : MonoBehaviour
    {
        public int InitialCapacity = 100;
        private int _capacity;

        public int Capacity
        {
            get { return _capacity; }
            private set
            {
                if (value != _capacity)
                {
                    _capacity = value;
                    OnCapacityChanged();
                }
            }
        }

        public bool PickSugar()
        {
            if (Capacity <= 0)
            {
                return false;
            }
            Capacity--;
            return true;
        }

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

        private void OnCapacityChanged()
        {
            var scale = Mathf.Pow((float) Capacity/InitialCapacity, 1/3f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
