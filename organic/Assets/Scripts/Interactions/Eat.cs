using Interactions.Util;
using Sim;
using UnityEngine;
using Input = Sim.Input;

namespace Interactions
{
    public class EatFactory : InteractionFactory
    {
        private readonly Transform transform;

        public EatFactory(Transform transform)
        {
            this.transform = transform;
        }

        public override Interaction Create(string dnaParameters)
        {
            return new Eat(transform);
        } 
    }

    public interface IEdible
    {
        long Eat();
    }
    
    public class Eat : Interaction
    {
        private readonly Transform t;
        private const float MaxAllowableDistance = 2f;
        private const float DistanceScalar = 5f;
        private const float EnergyScalar = 0.001f;
        
        public Eat(Transform t)
        {
            this.t = t;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = 0,
                Energy = -1,
            };
            
            var position = t.position;
            var direction = Convert.Vector3(input.Data[0]);
            var mag = direction.magnitude;
            
            var cost = (long) (mag * EnergyScalar);
            if (cost < 1 || (ulong) cost > input.AvailableEnergy)
            {
                return output;
            }
            
            var distance = mag * DistanceScalar;
            var hit = Physics.Raycast(position, direction, out var hitInfo, distance);
            if (!hit || hitInfo.transform == null || Vector3.Distance(t.position, hitInfo.transform.position) > MaxAllowableDistance)
            {
                output.Energy = -cost;
                return output;
            }

            var energy = hitInfo.transform.GetComponent<IEdible>()?.Eat() ?? 0;
            
            output.Data = (ulong)energy;
            output.Energy = energy - cost;
            return output;
        }
    }
}