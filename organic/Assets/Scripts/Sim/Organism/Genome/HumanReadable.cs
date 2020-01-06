using System.Collections.Generic;

namespace Sim.Organism.Genome
{
    public class HumanReadable
    {
        public string CapabilityName { get; }
        public string Gene { get; }
        public string GeneParameters { get; }
        public IEnumerable<string> Parameters { get; }

        public HumanReadable(string capabilityName, string gene, IEnumerable<string> parameters = null, string geneParameters = "")
        {
            CapabilityName = capabilityName;
            Gene = gene;
            Parameters = parameters ?? new string[0];
            GeneParameters = geneParameters;
        }

        public override string ToString()
        {
            return $"{Gene} {GeneParameters}: {ToReadableString()}";
        }

        public string ToReadableString()
        {
            return $"{CapabilityName}({string.Join(", ", Parameters)})";
        }
    }
}