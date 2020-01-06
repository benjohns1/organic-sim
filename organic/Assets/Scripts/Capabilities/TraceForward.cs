using System;
using System.Collections.Generic;
using System.IO;
using Sim;
using Sim.Organism.Genome;
using UnityEngine;
using Convert = Capabilities.Util.Convert;
using Input = Sim.Organism.Genome.Input;

namespace Capabilities
{
    public class TraceForwardFactory : CapabilityFactory
    {
        public override string HumanReadableName => $"{GetType().Name}";
        private readonly Transform transform;

        public TraceForwardFactory(Transform transform)
        {
            this.transform = transform;
        }

        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new TraceForward(hr, transform);
        } 
    }
    
    public class TraceForward : Capability
    {
        private readonly Transform transform;
        private const float DistanceScalar = 10f;
        private const float EnergyScalar = 5f;
        private const long MinCost = 1;
        
        public TraceForward(HumanReadable hr, Transform transform)
        {
            HumanReadable = hr;
            this.transform = transform;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = new ulong[]{0},
                Energy = -1,
            };
            
            var magnitude = Convert.ScaledFloat(input.Data[0]);
            var cost = Math.Max(MinCost, (long) (magnitude * EnergyScalar));
            if ((ulong) cost > input.AvailableEnergy)
            {
                return output;
            }
            
            var position = transform.position;
            var direction = transform.forward;
            var distance = magnitude * DistanceScalar;
            var hit = Physics.Raycast(position, direction, distance);
            
            output.Data = new[]{hit ? input.Data[0] : 0};
            output.Energy = -cost;
            return output;
        }
    }
}