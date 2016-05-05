using UnityEngine;

namespace Assets.Scripts
{
    public class Anthill : MonoBehaviour
    {
        public Transform AntContainer;

        private GameManager _gameManager;

        public void CollectSugar()
        {
            if (gameObject.tag == "Player")
            {
                _gameManager.PlayerCollectSugar();
            }
        }

        public void CollectApple()
        {
            if (gameObject.tag == "Player")
            {
                _gameManager.PlayerCollectApple();
            }
        }

        protected virtual void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }
}
