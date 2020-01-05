using System;
using System.Collections.Generic;
using System.IO;
using Sim;
using UnityEngine;
using Convert = Capabilities.Util.Convert;
using Input = Sim.Input;

namespace Capabilities
{
    public class MoveForwardFactory : CapabilityFactory
    {
        private readonly Rigidbody rb;
        public MoveForwardFactory(Rigidbody rb)
        {
            this.rb = rb;
        }

        public override Capability Create(StringReader genome)
        {
            return new MoveForward(rb);
        } 
    }
    
    public class MoveForward : Capability
    {
        private readonly Rigidbody rb;
        private const float ForceScalar = 10f;
        private const float EnergyScalar = 10f;
        private const long MinCost = 1;
        
        public MoveForward(Rigidbody rb)
        {
            this.rb = rb;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = new []{input.Data[0]},
                Energy = -1,
            };
            
            var magnitude = Convert.ScaledFloat(input.Data[0]);
            var cost = Math.Max(MinCost, (long) (magnitude * EnergyScalar));
            if ((ulong) cost > input.AvailableEnergy)
            {
                return output;
            }

            var direction = rb.transform.forward;
            var force = direction * (magnitude * ForceScalar * Time.deltaTime);
            rb.AddForce(force);

            output.Energy = -cost;
            return output;
        }
    }

}