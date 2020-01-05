using System.Collections.Generic;
using System.IO;
using System.Linq;
using Capabilities.Util;
using Sim;
using UnityEngine;
using Input = Sim.Input;

namespace Capabilities
{
    public class EatFactory : CapabilityFactory
    {
        private readonly Transform transform;
        private readonly long maxSensitivity;
        private readonly List<char> dialect;

        public EatFactory(IEnumerable<char> dialect, Transform transform, long maxSensitivity)
        {
            this.dialect = dialect.ToList();
            this.transform = transform;
            this.maxSensitivity = maxSensitivity;
        }

        public override Capability Create(StringReader genome)
        {
            var c = genome.Read();
            float multiplier = 0;
            if (c != -1)
            {
                multiplier = (dialect.IndexOf((char) c) / (float) dialect.Count);
            }
            
            var sensitivity = (long) (maxSensitivity * multiplier);
            return new Eat(transform, sensitivity);
        } 
    }

    public interface IEdible
    {
        long Eat();
    }
    
    public class Eat : Capability
    {
        private readonly Transform t;
        private readonly long sensitivity;
        private const float MaxAllowableDistance = 2f;
        private const float DistanceScalar = 5f;
        private const float EnergyScalar = 0.001f;
        
        public Eat(Transform t, long sensitivity)
        {
            this.t = t;
            this.sensitivity = sensitivity;
        }
        
        public override Output Run(Input input)
        {
            var output = new Output
            {
                Data = new ulong[]{0},
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

            // If the energy received from food is greater than sensitivity value, output max value; otherwise output ratio
            var result = energy >= sensitivity ? ulong.MaxValue : (ulong.MaxValue / (ulong) sensitivity) * (ulong) energy;
            
            output.Data = new []{result};
            output.Energy = energy - cost;
            return output;
        }
    }
}