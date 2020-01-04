using Interactions;

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

        public void Tick()
        {
            if (dead)
            {
                return;
            }

            energy = brain.Tick(energy);
            
            if (energy <= 0)
            {
                Die();
            }
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