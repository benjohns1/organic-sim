using UnityEngine;

namespace Unity
{
    [RequireComponent(typeof(Rigidbody))]
    public class Initializer : MonoBehaviour
    {
        public GameObject foodPrefab;
        public GameObject organismPrefab;
        
        [SerializeField]
        private long foodCount = 1000;

        [SerializeField]
        private long organismCount = 10;

        [SerializeField] private float spawnRadius = 20;

        public void Start()
        {
            for (var i = 0; i < foodCount; i++)
            {
                Instantiate(foodPrefab, UnityEngine.Random.insideUnitSphere * spawnRadius, Quaternion.identity, transform);
            }
            
            for (var i = 0; i < organismCount; i++)
            {
                Instantiate(organismPrefab, UnityEngine.Random.insideUnitSphere * spawnRadius, Quaternion.identity, transform);
            }
        }
    }
}
