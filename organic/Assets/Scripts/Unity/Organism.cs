using System;
using System.Collections.Generic;
using System.Linq;
using Capabilities;
using Capabilities.Util;
using Sim.Organism;
using Sim.Organism.Genome;
using UnityEngine;
using Util;

namespace Unity
{
    public struct OrganismConfig
    {
        public int? Seed;
        public char[] Dialect;
        public ulong StartingEnergy;
        public bool Mutate;
        public string NewGenome;
        public int Generation;
        public int Born;
    }
    
    [RequireComponent(typeof(Rigidbody))]
    public class Organism : MonoBehaviour
    {
        private Sim.Organism.Organism organism;
        private Rigidbody rb;
        
        [SerializeField]
        private bool isDead;
        
        [SerializeField]
        private ulong energy = 100000;

        [SerializeField] private string genome;
        
        [SerializeField]
        private List<string> genomeAncestors = new List<string>();

        [SerializeField] private bool debug = false;

        public Factory factory;

        public OrganismConfig Config { get; private set; }

        [SerializeField] private int seed;
        [SerializeField] private int born;
        
        
        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(OrganismConfig config)
        {
            if (config.Dialect == null || config.Dialect.Length <= 0)
            {
                throw new Exception("invalid organism config");
            }

            name = $"Gen{config.Generation}";
            seed = config.Seed ?? 0;
            born = config.Born;
            
            Config = config;
            var mem1 = new Memory();
            var t = transform;

            var dialect = config.Dialect;
            var capabilityFactories = new CapabilityFactory[]
            {
                new RandomFactory(config.Seed),
                new StaticFactory(ulong.MaxValue),
                new StaticFactory(0),
                new MoveForwardFactory(rb),
                new TorqueHorizontalFactory(rb),
                new TraceForwardFactory(t),
                new DecideFactory(),
                new FanOutFactory(dialect, 5),
                new InvertFactory(),
                new HaltFactory(),
                new MemoryLoadFactory(mem1),
                new MemorySaveFactory(mem1),
                new EatFactory(dialect, t, 20000),
                new ReproduceFactory(t, genome.Length),
            };
            var mapper = new Mapper(dialect, capabilityFactories);
            
            var mutationRates = new MutationRates
            {
                DeleteBase = 0.05f,
                AddBase = 0.1f,
            };
            factory = new Factory(mapper, mutationRates, config.Seed);

            // if (mapper.GeneMap != null)
            // {
            //     Debug.Log(string.Join("\n", mapper.GeneMap.Select(g => $"{g.Key}: {g.Value?.HumanReadableName}")));
            // }
            
            Birth(config.StartingEnergy, config.Mutate, config.NewGenome);
        }

        private void Birth(ulong newEnergy, bool mutate, string newGenome)
        {
            if (newGenome != null)
            {
                // Organism initialized externally (init script)
                genome = newGenome;
            }
            else
            {
                // Organism initialized naturally
                genomeAncestors.Add(genome);
            }
            
            if (mutate)
            {
                genome = factory.Mutate(genome);
            }
            var brain = new Brain(genome, factory);
            organism = new Sim.Organism.Organism(newEnergy, brain);
        }

        public void Update()
        {
            if (organism == null) return;
            
            var debugValue = organism.Tick();
            if (debug)
            {
                Debug.Log($"--- {name}({GetInstanceID()}) ---\n{debugValue}");
            }
            isDead = organism.IsDead();
            energy = organism.GetEnergy();
            if (!isDead) return;
            enabled = false;
            Destroy(gameObject);
        }
    }
}
