using UnityEngine;

namespace Game.Managers
{
    public class AutoRegisterToActivationManager : MonoBehaviour
    {
        private void Start()
        {
            if (ObjectActivationManager.Instance != null)
                ObjectActivationManager.Instance.RegisterObject(gameObject);
        }

        private void OnDestroy()
        {
            if (ObjectActivationManager.Instance != null)
                ObjectActivationManager.Instance.UnregisterObject(gameObject);
        }
    }
}