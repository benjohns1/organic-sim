using System;
using System.Collections.Generic;
using Capabilities;
using Capabilities.Util;
using JetBrains.Annotations;
using Sim;
using Sim.Organism;
using Sim.Organism.Genome;
using UnityEngine;

namespace Unity
{

    public class OrganismConfig
    {
        public int? Seed;
        public char[] Dialect;
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

        [SerializeField] private string genome = "1|TrFwd|Fan|MvFwd.Rnd.~|Eat.Dec|H.TrqHrz.1|H.MvFwd";
        
        [SerializeField]
        private List<string> genomeAncestors = new List<string>();

        public Factory factory;

        public OrganismConfig Config { get; private set; }

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(OrganismConfig config)
        {
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
                DeleteBase = 0.1f,
                AddBase = 0.2f,
            };
            factory = new Factory(mapper, mutationRates, config.Seed);
        }

        public void Birth(ulong newEnergy, bool mutate, [CanBeNull] string newGenome = null)
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

        public void Start()
        {
            if (Config == null)
            {
                throw new Exception("organism has not been initialized");
            }

            if (organism == null)
            {
                throw new Exception("organism has not been birthed");
            }
        }

        public void Update()
        {
            if (organism == null) return;
            
            var debug = organism.Tick();
            Debug.Log(debug);
            isDead = organism.IsDead();
            energy = organism.GetEnergy();
            if (isDead)
            {
                enabled = false;
            }
        }
    }
}
