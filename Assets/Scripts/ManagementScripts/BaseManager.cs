namespace ManagementScripts
{
    using UnityEngine;

    public class BaseManager : MonoBehaviour
    {
        public HealthManager healthManager;

        private void Start()
        {
            healthManager = GetComponentInChildren<HealthManager>();
            if (healthManager == null)
            {
                Debug.LogError("HealthManager component not found in child objects.");
            }
        }
    }
}