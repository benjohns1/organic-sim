using System;
using System.Collections.Generic;
using Interactions;
using Interactions.Util;
using Sim;
using Sim.Organism;
using UnityEngine;

namespace Unity
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(LineRenderer))]
    public class Organism : MonoBehaviour
    {
        private Sim.Organism.Organism organism;
        private Rigidbody rb;
        
        [SerializeField]
        private bool isDead;
        
        [SerializeField]
        private ulong energy = 100000;

        [SerializeField]
        private string genome = "R.R.L.R|D.D|T.T|S.M|E.R.E.R|D.D|S.P";
        
        [SerializeField]
        private List<string> genomeAncestors = new List<string>();

        public GeneFactory geneFactory;
        public GameObject carcassPrefab;
        private Collider[] colliders;

        public void Awake()
        {
            colliders = GetComponents<Collider>();
            rb = GetComponent<Rigidbody>();
            var mem1 = new Memory();
            var t = transform;
            var interactionFactories = new Dictionary<char, InteractionFactory>
            {
                {'R', new RandomFactory()},
                {'M', new MoveFactory(rb)},
                {'T', new TraceFactory(t)},
                {'D', new DeciderFactory()},
                {'L', new MemoryLoadFactory(mem1)},
                {'S', new MemorySaveFactory(mem1)},
                {'E', new EatFactory(t)},
                {'P', new ReproduceFactory(t, genome.Length)},
            };
            geneFactory = new GeneFactory(interactionFactories, '.', '|');
            if (carcassPrefab == null)
            {
                throw new Exception("carcassPrefab must be set");
            }
        }

        public void Birth(ulong newEnergy, bool mutate)
        {
            genomeAncestors.Add(genome);
            if (mutate)
            {
                genome = geneFactory.Mutate(genome);
            }
            var brain = new Brain(genome, geneFactory);
            organism = new Sim.Organism.Organism(newEnergy, brain);
        }

        public void Start()
        {
            if (organism != null) return;
            Birth(energy, false);
        }

        public void Update()
        {
            if (organism == null) return;
            
            organism.Tick();
            isDead = organism.IsDead();
            energy = organism.GetEnergy();
            
            if (isDead)
            {
                Death();
            }
        }

        private void Death()
        {
            if (gameObject == null)
            {
                return;
            }
            rb.isKinematic = false;
            foreach (var c in colliders)
            {
                c.enabled = false;
            }

            var t = transform;
            var go = Instantiate(carcassPrefab, t.position, t.rotation);
            if (go == null)
            {
                return;
            }

            var carcass = go.GetComponent<ICarcass>();
            carcass?.SetEnergy(1000);
            Destroy(gameObject);
        }
    }

    public interface ICarcass
    {
        void SetEnergy(long energy);
    }
}
