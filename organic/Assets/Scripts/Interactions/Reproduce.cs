using System;
using Sim;
using Unity;
using UnityEngine;
using Convert = Interactions.Util.Convert;
using Input = Sim.Input;
using Object = UnityEngine.Object;

namespace Interactions
{
    public class ReproduceFactory : InteractionFactory
    {
        private readonly Transform transform;
        private readonly long copyCost;

        public ReproduceFactory(Transform transform, long copyCost)
        {
            this.transform = transform;
            this.copyCost = copyCost;
        }

        public override Interaction Create(string dnaParameters)
        {
            return new Reproduce(transform, copyCost);
        } 
    }
    
    public class Reproduce : Interaction
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
                Data = 0,
                Energy = -BaseEnergyCost,
            };

            var energyAmount = (ulong) (Convert.ScaledFloat(input.Data[0]) * input.AvailableEnergy);
            if (energyAmount < MinEnergyAmount || energyAmount < BaseEnergyCost)
            {
                return output;
            }

            var cost = energyAmount + copyCost + BaseEnergyCost;
            if (cost > input.AvailableEnergy || cost < energyAmount)
            {
                return output;
            }

            var child = Object.Instantiate(t.gameObject, t.position + new Vector3(0,0,5f), t.rotation);
            if (child == null)
            {
                return output;
            }

            var go = child.GetComponent<Organism>();
            if (go == null)
            {
                return output;
            }

            go.Birth(energyAmount, true);

            output.Data = input.Data[0];
            output.Energy = -(long) cost;
            return output;
        }
    }
}