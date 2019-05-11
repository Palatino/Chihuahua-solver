using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

namespace Grasshopper_Exploration
{
    public class Chihuaha
    {
        public static Random random = new Random();

        public static Individual CreateRandomIndividual(ChihuahaComponent comp)
        {
            GH_Document docu = comp.OnPingDocument();
            //Change value of sliders
            List<decimal> genome = new List<decimal>();
            IList<IGH_Param> sliders = comp.Params.Input[0].Sources;
            List<string> strings = new List<string>();
            foreach (IGH_Param param in sliders)
            {

                Grasshopper.Kernel.Special.GH_NumberSlider slider = param as Grasshopper.Kernel.Special.GH_NumberSlider;
                if (slider != null)
                {
                    decimal gene_value = (decimal)random.NextDouble() * (slider.Slider.Maximum);
                    slider.SetSliderValue(gene_value);
                    genome.Add(gene_value);
                }

                else
                {
                    Rhino.RhinoApp.WriteLine("At least one of the input parameters is not a number slider" + Environment.NewLine);
                    throw new System.ArgumentException("At least one of the input parameters is not a number slider", "Genome");
                }
            }

            // Get a new solution

            docu.NewSolution(false);

            //Read new fitness value

            decimal fitness = 0;

            if (comp.Params.Input[1].SourceCount != 1)
            {
                Rhino.RhinoApp.Write("Fitness input must be a unique value" + Environment.NewLine);
                throw new System.ArgumentException("Fitness input must be a unique value", "Fitness");
            }

            else
            {
                IGH_Param param = comp.Params.Input[1].Sources[0];
                GH_Structure<GH_Number> outcome = comp.Params.Input[1].Sources[0].VolatileData as GH_Structure<GH_Number>;

                if (outcome == null)
                {
                    Rhino.RhinoApp.WriteLine("Fitness input is not a number" + Environment.NewLine);
                    throw new System.ArgumentException("Fitness input is not a number", "Fitness");
                }
                if (outcome != null)
                {
                    fitness = (decimal)outcome.Branches[0][0].Value;
                }
            }

            Individual ind = new Individual(fitness, genome);

            return ind;


        }

        public static Generation CreateRandomGeneration(ChihuahaComponent comp, int numberOfIndividuals)
        {
            List<Individual> tempIndi = new List<Individual>();
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                Individual ind = Chihuaha.CreateRandomIndividual(comp);
                tempIndi.Add(ind);
            }

            Generation gen = new Generation(tempIndi);
            return gen;

        }
    }

}
