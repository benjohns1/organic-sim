using UnityEngine;

namespace Sim.Organism
{
    public class Organism
    {
        private readonly Brain brain;
            
        private ulong energy;
        private bool dead;

        public Organism(ulong startingEnergy, Brain brain)
        {
            energy = startingEnergy;
            this.brain = brain;
        }

        public object Tick()
        {
            if (dead)
            {
                return "dead";
            }

            var brainOut = brain.Tick(energy);
            energy = brainOut.RemainingEnergy;
            
            if (energy <= 0)
            {
                Die();
            }
            
            return string.Join("\n", brainOut.Debug);
        }

        private void Die()
        {
            dead = true;
            energy = 0;
        }

        public bool IsDead()
        {
            return dead;
        }

        public ulong GetEnergy()
        {
            return energy;
        }
    }
}