using UnityEngine;

namespace Assets.Scripts
{
    public class Level : MonoBehaviour
    {
        public Transform Ground;
        public Transform FoodContainer;

        public Rect GetLevelBounds()
        {
            var bounds = Ground.GetComponent<Renderer>().bounds;

            return Rect.MinMaxRect(
                bounds.min.x,
                bounds.min.z,
                bounds.max.x,
                bounds.max.z);
        }
    }
}
