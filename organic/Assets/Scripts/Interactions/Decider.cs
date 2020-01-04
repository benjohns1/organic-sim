using Interactions.Util;
using Sim;
using Input = Sim.Input;

namespace Interactions
{
    public class DeciderFactory : InteractionFactory
    {
        public override Interaction Create(string dnaParameters)
        {
            return new Decider();
        } 
    }
    
    public class Decider : Interaction
    {
        private const long EnergyCost = 1;

        public override int GetInputCount()
        {
            return 2;
        }

        public override Output Run(Input input)
        {
            // Return the second input value if the first input evaluates to true
            return new Output
            {
                Data = Convert.Bool(input.Data[0]) ? input.Data[1] : 0,
                Energy = -EnergyCost,
            };
        }
    }

}