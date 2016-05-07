using UnityEngine;

namespace Assets.Scripts
{
    public class Anthill : MonoBehaviour
    {
        public Transform AntContainer;

        private GameManager _gameManager;

        public void CollectSugar()
        {
            _gameManager.CollectSugar(this);
        }

        public void CollectApple()
        {
            _gameManager.CollectApple(this);
        }

        public void AntDied()
        {
            _gameManager.AntDied(this);
        }

        protected virtual void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }
}
