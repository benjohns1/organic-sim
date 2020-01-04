using Interactions.Util;
using Sim;
using UnityEngine;
using Input = Sim.Input;

namespace Interactions
{
    public class TraceFactory : InteractionFactory
    {
        private readonly Transform transform;

        public TraceFactory(Transform transform)
        {
            this.transform = transform;
        }

        public override Interaction Create(string dnaParameters)
        {
            return new Trace(transform);
        } 
    }
    
    public class Trace : Interaction
    {
        private readonly Transform t;
        private const float DistanceScalar = 10f;
        private const float EnergyScalar = 0.0001f;
        
        public Trace(Transform t)
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
            var hit = Physics.Raycast(position, direction, distance);
            
            output.Data = hit ? input.Data[0] : 0;
            output.Energy = -cost;
            return output;
        }
    }
}