using UnityEngine;

namespace Game.Player.Scripts.Managers
{
    public class PlayerManager: MonoBehaviour
    {
        public static PlayerManager Instance;
        public Player Player;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else
                Instance = this;
        }
    }
}