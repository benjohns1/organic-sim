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
    public class TorqueHorizontalFactory : CapabilityFactory
    {
        public override string HumanReadableName => $"{GetType().Name}";
        private readonly Rigidbody rb;
        public TorqueHorizontalFactory(Rigidbody rb)
        {
            this.rb = rb;
        }

        public override Capability Create(string gene, StringReader genome)
        {
            var hr = new HumanReadable(HumanReadableName, gene);
            return new TorqueHorizontal(hr, rb);
        } 
    }
    
    public class TorqueHorizontal : Capability
    {
        private readonly Rigidbody rb;
        private const float TorqueScalar = 5f;
        private const float EnergyScalar = 10f;
        private const long MinCost = 1;
        
        public TorqueHorizontal(HumanReadable hr, Rigidbody rb)
        {
            HumanReadable = hr;
            this.rb = rb;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = new[]{input.Data[0]},
                Energy = -1,
            };
            
            var multiplier = Convert.ScaledSignedFloat(input.Data[0]);
            var cost = Math.Max(MinCost, (long) (Math.Abs(multiplier) * EnergyScalar));
            if ((ulong) cost > input.AvailableEnergy)
            {
                return output;
            }

            var rotationAxis = rb.transform.up;
            var torque = rotationAxis * (multiplier * TorqueScalar * Time.deltaTime);
            rb.AddTorque(torque);

            output.Energy = -cost;
            return output;
        }
    }

}