using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity
{
    public class GameManager : MonoBehaviour
    {
        public GameObject foodPrefab;
        public GameObject organismPrefab;

        [SerializeField] private long foodCount = 1000;
        [SerializeField] private long organismCount = 10;
        [SerializeField] private float spawnRadius = 20;
        [SerializeField] private int tickCheckInterval = 100;

        [SerializeField] private int startingGenomeLength = 5;
        [SerializeField] private int startingGenomeCount = 1;
        [SerializeField] private string[] genomes;
        [SerializeField] private string dialect = "ABCD";

        [SerializeField] private ulong startingEnergy = 10000;

        [SerializeField] private bool useRandomSeed = true;
        [SerializeField] private int staticSeed = 1;
        [SerializeField] private int seed;

        public void Start()
        {
            seed = useRandomSeed ? Random.Range(int.MinValue, int.MaxValue) : staticSeed;
            Random.InitState(seed);
            Debug.Log($"seed: {seed}");
            
            SpawnFood(foodCount);

            genomes = RandomGenomes();
            for (var i = 0; i < organismCount; i++)
            {
                var go = Instantiate(organismPrefab, Random.insideUnitSphere * spawnRadius, Random.rotation);
                var organism = go.GetComponent<Organism>();
                organism.Initialize(new OrganismConfig
                {
                    Seed = seed,
                    Dialect = dialect.ToCharArray(),
                });
                organism.Birth(startingEnergy, false, genomes[Random.Range(0, genomes.Length)]);
            }
        }

        public void Update()
        {
            if (Time.frameCount % tickCheckInterval != 0)
            {
                return;
            }

            var food = FindObjectsOfType(typeof(Food));
            var newCount = foodCount - food.Length;
            if (newCount <= 0)
            {
                return;
            }
            
            Debug.Log($"spawning {newCount} food");
            SpawnFood(newCount);
        }

        private void SpawnFood(long count)
        {
            for (var i = 0; i < count; i++)
            {
                Instantiate(foodPrefab, Random.insideUnitSphere * spawnRadius, Random.rotation, transform);
            }
        }

        private string[] RandomGenomes()
        {
            var newGenomes = new List<string>();
            var random = new System.Random(seed);
            for (var i = 0; i < startingGenomeCount; i++)
            {
                var genome = new StringBuilder();
                for (var j = 0; j < startingGenomeLength; j++)
                {
                    var index = random.Next(0, dialect.Length);
                    genome.Append(dialect[index]);
                }

                newGenomes.Add(genome.ToString());
            }

            return newGenomes.ToArray();
        }
    }
}
