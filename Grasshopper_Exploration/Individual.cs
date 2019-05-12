using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chihuahua
{
    public struct Individual
    {
        public readonly decimal Fitness;
        public readonly List<decimal> Genome;
        public readonly List<decimal> NormalizeGenome;
        public Individual(decimal fitness, List<decimal> genome, List<decimal> Ngenome)
        {
            Fitness = fitness;
            Genome = genome;
            NormalizeGenome = Ngenome;
        }
    }
}
