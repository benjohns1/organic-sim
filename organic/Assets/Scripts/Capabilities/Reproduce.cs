using System;
using System.Collections.Generic;
using System.IO;
using Sim;
using Unity;
using UnityEngine;
using Convert = Capabilities.Util.Convert;
using Input = Sim.Input;
using Object = UnityEngine.Object;

namespace Capabilities
{
    public class ReproduceFactory : CapabilityFactory
    {
        private readonly Transform transform;
        private readonly long copyCost;

        public ReproduceFactory(Transform transform, long copyCost)
        {
            this.transform = transform;
            this.copyCost = copyCost;
        }

        public override Capability Create(StringReader genome)
        {
            return new Reproduce(transform, copyCost);
        } 
    }
    
    public class Reproduce : Capability
    {
        private readonly Transform t;
        private readonly ulong copyCost;
        private const long BaseEnergyCost = 1000;
        private const long MinEnergyAmount = 5000;
        
        public Reproduce(Transform t, long copyCost)
        {
            this.t = t;
            this.copyCost = (ulong) Math.Abs(copyCost);
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

            var child = Object.Instantiate(t.gameObject, t.position + new Vector3(0,0,5f), t.rotation, t);
            if (child == null)
            {
                return output;
            }

            var go = child.GetComponent<Organism>();
            if (go == null)
            {
                return output;
            }

            go.Initialize(t.gameObject.GetComponent<Organism>().Config);
            go.Birth(energyAmount, true);

            output.Data = new []{ulong.MaxValue};
            output.Energy = -(long) cost;
            return output;
        }
    }
}