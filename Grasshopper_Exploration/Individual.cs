using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grasshopper_Exploration
{
    public struct Individual
    {
        public readonly decimal Fitness;
        public readonly List<decimal> Genome;
        public Individual(decimal fitness, List<decimal> genome)
        {
            Fitness = fitness;
            Genome = genome;
        }
    }
}
