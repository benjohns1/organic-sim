namespace Sim
{
    public class Food
    {
        public long Energy { get; private set; }

        public Food(long energy)
        {
            Energy = energy;
        }

        public long Eat()
        {
            var eaten = Energy;
            Energy = 0;
            return eaten;
        }
    }
}