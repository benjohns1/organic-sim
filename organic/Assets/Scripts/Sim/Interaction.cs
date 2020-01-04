namespace Sim
{
    public abstract class InteractionFactory
    {
        public abstract Interaction Create(string dnaParameters);
    }
    
    public abstract class Interaction
    {
        public abstract Output Run(Input input);

        public virtual int GetInputCount()
        {
            return 1;
        }
    }
    
    public struct Input
    {
        public ulong[] Data;
        public ulong AvailableEnergy;
    }

    public struct Output
    {
        public ulong Data;
        public long Energy;
    }
}