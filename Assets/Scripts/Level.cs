using UnityEngine;

namespace Assets.Scripts
{
    public class Level : MonoBehaviour
    {
        public Transform Ground;

        public Rect GetLevelBounds()
        {
            var scale = CalculateTotalScale(Ground);
            return new Rect(
                Ground.transform.position.x,
                Ground.transform.position.z,
                scale.x,
                scale.z);
        }

        private static Vector3 CalculateTotalScale(Transform transform)
        {
            if (transform == null)
            {
                return new Vector3(1f, 1f, 1f);
            }

            var scale = transform.localScale;
            var parentScale = CalculateTotalScale(transform.parent);
            return new Vector3(scale.x*parentScale.x, scale.y*parentScale.y, scale.z*parentScale.z);
        }
    }
}
