using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grasshopper_Exploration
{
    public class Generation
    {
        public readonly List<Individual> populations;
        readonly decimal bestFitness;
        readonly Individual bestIndividual;

        public Generation(List<Individual> unordered)
        {
            populations = unordered.OrderByDescending(x => x.Fitness).ToList();
            bestFitness = populations[0].Fitness;
            bestIndividual = populations[0];
        }
    }
}
