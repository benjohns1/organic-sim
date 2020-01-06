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

        [SerializeField] private int randomGenomeLength = 5;
        [SerializeField] private int randomGenomeCount = 1;
        [SerializeField] private List<string> genomes = new List<string>();
        
        // ReSharper disable once StringLiteralTypo
        [SerializeField] private string dialect = "ABCD";
        [SerializeField] private Transform organismParent;
        [SerializeField] private Transform foodParent;

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

            genomes.AddRange(RandomGenomes(randomGenomeCount));
            for (var i = 0; i < organismCount; i++)
            {
                var go = Instantiate(organismPrefab, Random.insideUnitSphere * spawnRadius, Random.rotation, organismParent);
                var organism = go.GetComponent<Organism>();
                organism.Initialize(new OrganismConfig
                {
                    Seed = Random.Range(int.MinValue, int.MaxValue),
                    Dialect = dialect.ToCharArray(),
                    StartingEnergy = startingEnergy,
                    Mutate = false,
                    NewGenome = genomes[Random.Range(0, genomes.Count)],
                    Generation = 1,
                    Born = Time.frameCount,
                });
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
                Instantiate(foodPrefab, Random.insideUnitSphere * spawnRadius, Random.rotation, foodParent);
            }
        }

        private IEnumerable<string> RandomGenomes(int count)
        {
            var newGenomes = new List<string>();
            var random = new System.Random(seed);
            for (var i = 0; i < count; i++)
            {
                var genome = new StringBuilder();
                for (var j = 0; j < randomGenomeLength; j++)
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
