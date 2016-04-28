using UnityEngine;

namespace Assets.Scripts
{
    public class Ant : MonoBehaviour
    {
        private AntScript _antScript;

        protected virtual void Start()
        {
            _antScript = new AntScript();
        }

        protected virtual void Update()
        {
            _antScript.Update();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
            {
                var ant = other.gameObject.GetComponentInParent<Ant>();
                if (ant != null)
                {
                    _antScript.EnterView(ant);
                }
                var sugar = other.gameObject.GetComponentInParent<Sugar>();
                if (sugar != null)
                {
                    _antScript.EnterView(sugar);
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger)
            {
                var ant = other.gameObject.GetComponentInParent<Ant>();
                if (ant != null)
                {
                    _antScript.ExitView(ant);
                }
                var sugar = other.gameObject.GetComponentInParent<Sugar>();
                if (sugar != null)
                {
                    _antScript.ExitView(sugar);
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var sugar = collision.gameObject.GetComponentInParent<Sugar>();
            if (sugar != null)
            {
                _antScript.Reach(sugar);
            }
            var apple = collision.gameObject.GetComponentInParent<Apple>();
            if (apple != null)
            {
                _antScript.Reach(apple);
            }
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            var sugar = collision.gameObject.GetComponentInParent<Sugar>();
            if (sugar != null)
            {
                _antScript.Leave(sugar);
            }
            var apple = collision.gameObject.GetComponentInParent<Apple>();
            if (apple != null)
            {
                _antScript.Leave(apple);
            }
        }
    }
}
