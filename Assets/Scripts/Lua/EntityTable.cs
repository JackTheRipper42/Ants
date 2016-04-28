using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class EntityTable
    {
        public Vector2 position;

        public EntityTable(MonoBehaviour entity)
        {
            position = new Vector2(
                entity.transform.position.x,
                entity.transform.position.z);
        }
    }
}