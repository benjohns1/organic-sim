using System;
using System.IO;
using Sim.Organism.Genome;
using Unity;
using UnityEngine;
using Convert = Capabilities.Util.Convert;
using Input = Sim.Organism.Genome.Input;
using Object = UnityEngine.Object;

namespace Capabilities
{
    public class ReproduceFactory : CapabilityFactory
    {
        public override string HumanReadableName { get; }
        private readonly Transform transform;
        private readonly long copyCost;
        private readonly Random random;

        public ReproduceFactory(Transform transform, long copyCost)
        {
            this.transform = transform;
            this.copyCost = copyCost;
            HumanReadableName = $"{GetType().Name}({copyCost})";
            
        }

        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new Reproduce(hr, transform, copyCost);
        } 
    }
    
    public class Reproduce : Capability
    {
        private readonly Transform t;
        private readonly ulong copyCost;
        private const long BaseEnergyCost = 1000;
        private const long MinEnergyAmount = 5000;
        private readonly Transform genGroup;
        private readonly Organism parent;
        
        public Reproduce(HumanReadable hr, Transform t, long copyCost)
        {
            HumanReadable = hr;
            this.t = t;
            this.copyCost = (ulong) Math.Abs(copyCost);
            parent = t.gameObject.GetComponent<Organism>();
            var groupName = $"Gen{parent.Config.Generation + 1}";
            var genGo = GameObject.Find(groupName);
            if (genGo != null)
            {
                genGroup = genGo.transform;
                return;
            }
            var go = new GameObject {name = groupName};
            go.transform.SetParent(t.parent.parent);
            genGroup = go.transform;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = new ulong[]{0},
                Energy = -BaseEnergyCost,
            };
            
            if (input.Data[0] == 0 || input.AvailableEnergy <= BaseEnergyCost)
            {
                return output;
            }

            var energyAmount = (ulong) (Convert.ScaledFloat(input.Data[0]) * (input.AvailableEnergy - BaseEnergyCost));
            if (energyAmount < MinEnergyAmount || energyAmount < BaseEnergyCost)
            {
                return output;
            }

            var cost = energyAmount + copyCost + BaseEnergyCost;
            if (cost > input.AvailableEnergy || cost < energyAmount)
            {
                return output;
            }

            var child = Object.Instantiate(t.gameObject, t.position + new Vector3(0,0,5f), t.rotation, genGroup);
            if (child == null)
            {
                return output;
            }

            var go = child.GetComponent<Organism>();
            if (go == null)
            {
                return output;
            }

            var config = parent.Config;
            config.Seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            config.StartingEnergy = energyAmount;
            config.Mutate = true;
            config.NewGenome = null;
            config.Generation++;
            config.Born = Time.frameCount;

            go.Initialize(config);

            output.Data = new []{ulong.MaxValue};
            output.Energy = -(long) cost;
            return output;
        }
    }
}