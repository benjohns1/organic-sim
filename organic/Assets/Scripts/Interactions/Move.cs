using Interactions.Util;
using Sim;
using UnityEngine;
using Input = Sim.Input;

namespace Interactions
{
    public class MoveFactory : InteractionFactory
    {
        private readonly Rigidbody rb;
        public MoveFactory(Rigidbody rb)
        {
            this.rb = rb;
        }

        public override Interaction Create(string dnaParameters)
        {
            return new Move(rb);
        } 
    }
    
    public class Move : Interaction
    {
        private readonly Rigidbody rb;
        private const float ForceScalar = 0.005f;
        private const float EnergyScalar = 0.001f;
        
        public Move(Rigidbody rb)
        {
            this.rb = rb;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = input.Data[0],
                Energy = -1,
            };
            
            var vector = Convert.Vector3(input.Data[0]);
            var mag = vector.magnitude;
            var cost = (long) (mag * EnergyScalar);
            
            if (cost < 1 || (ulong) cost > input.AvailableEnergy)
            {
                return output;
            }

            var force = vector * (ForceScalar * Time.deltaTime);
            rb.AddForce(force);

            output.Energy = -cost;
            return output;
        }
    }

}