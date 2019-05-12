using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chihuahua
{
    public class Generation
    {
        public readonly List<Individual> populations;
        public readonly decimal bestFitness;
        public readonly Individual bestIndividual;

        public Generation(List<Individual> unordered)
        {
            populations = unordered.OrderByDescending(x => x.Fitness).ToList();
            bestFitness = populations[0].Fitness;
            bestIndividual = populations[0];
        }
    }
}
