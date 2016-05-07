using UnityEngine;

namespace Assets.Scripts
{
    public class Bug : MonoBehaviour
    {
        private const float Speed = 1.8f;

        private Rect _levelBoundaries;
        private Vector3 _startPosition;
        private Vector3 _destination;
        private float _lerpPosition;
        private float _lerpLength;

        protected virtual void Start()
        {
            var gameManager = FindObjectOfType<GameManager>();
            _levelBoundaries = gameManager.LevelBoundaries;
            SetNewDestination();
        }

        protected virtual void Update()
        {
            transform.rotation = Quaternion.LookRotation(_destination - _startPosition);
            _lerpPosition += (Speed * Time.deltaTime) / _lerpLength;
            transform.position = Vector3.Lerp(_startPosition, _destination, _lerpPosition);
            if (_lerpPosition >= 1)
            {
                SetNewDestination();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
            {
                var ant = other.gameObject.GetComponent<Ant>();
                if (ant != null)
                {
                    ant.Kill();
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var levelBoundary = collision.gameObject.GetComponentInParent<LevelBoundary>();
            if (levelBoundary != null)
            {
                SetNewDestination();
            }
        }

        private Vector3 GetRandomDestination()
        {
            var position = new Vector3(
                Random.Range(_levelBoundaries.xMin, _levelBoundaries.xMax),
                0f,
                Random.Range(_levelBoundaries.yMin, _levelBoundaries.yMax));
            return position;
        }

        private void SetNewDestination()
        {
            var destination = GetRandomDestination();

            Debug.DrawLine(transform.position, destination, Color.blue, 1f);

            _destination = destination;
            _lerpPosition = 0;
            _lerpLength = (destination - transform.position).magnitude;
            _startPosition = transform.position;
        }
    }
}
