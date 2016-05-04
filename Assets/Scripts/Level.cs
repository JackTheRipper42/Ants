using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class Level : MonoBehaviour
    {
        public Transform Ground;

        public Rect GetLevelBounds()
        {
            var scale = CalculateTotalScale(Ground);
            return Rect.MinMaxRect(
                Ground.position.x - scale.x/2,
                Ground.position.z - scale.y/2,
                Ground.position.x + scale.x/2,
                Ground.position.z + scale.y/2);
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
