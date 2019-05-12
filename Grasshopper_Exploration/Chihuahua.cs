using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

namespace Chihuahua
{
    public class Chihuahua
    {
        public static Random random = new Random();

        public static Individual CreateRandomIndividual(ChihuahuaComponent comp)
        {
            GH_Document docu = comp.OnPingDocument();

            //Change value of sliders
            List<decimal> genome = new List<decimal>();
            List<decimal> Ngenome = new List<decimal>();
            IList<IGH_Param> sliders = comp.Params.Input[0].Sources;
            foreach (IGH_Param param in sliders)
            {

                Grasshopper.Kernel.Special.GH_NumberSlider slider = param as Grasshopper.Kernel.Special.GH_NumberSlider;
                if (slider != null)
                {
                    decimal N_gene_value = (decimal)random.NextDouble();
                    decimal gene_value = N_gene_value.Remap(0, 1, slider.Slider.Minimum, slider.Slider.Maximum);
                    slider.SetSliderValue(gene_value);
                    Ngenome.Add(N_gene_value);
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

            Individual ind = new Individual(fitness, genome, Ngenome);

            return ind;


        }

        public static Generation CreateRandomGeneration(ChihuahuaComponent comp, int numberOfIndividuals)
        {
            List<Individual> tempIndi = new List<Individual>();
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                Individual ind = Chihuahua.CreateRandomIndividual(comp);
                tempIndi.Add(ind);
            }

            Generation gen = new Generation(tempIndi);
            return gen;

        }

        public static Generation EvolveGeneration(ChihuahuaComponent comp, Generation previousgen, int survivors, int GenerationSize, double mutationPercentage)
        {
            

            // Get onlt the fittest individuals

            List<Individual> validIndividuals = previousgen.populations.GetRange(0, survivors);
            List<Individual> nextGenerationList = new List<Individual>();

            for (int i = 0; i<GenerationSize; i++)
            {
                //Randomly select two individuals
                int firstIndividualIndx = random.Next(0, survivors);
                int scondIndividualIndx = random.Next(0, survivors);

                //Make sure the same individual is not being chosen twice
                while (firstIndividualIndx == scondIndividualIndx)
                {
                    scondIndividualIndx = random.Next(0, survivors);
                }

                Individual firstIndividual = validIndividuals[firstIndividualIndx];
                Individual secondtIndividual = validIndividuals[scondIndividualIndx];

                //Mix individuals and add mutations
                Individual newIndividual = mixTwoIndividuals(comp, firstIndividual, secondtIndividual, mutationPercentage);

                //Add new individual to new generation list

                nextGenerationList.Add(newIndividual);

            }


            //Create new generation

            Generation newGeneration = new Generation(nextGenerationList);

            return newGeneration;

        }

        public static Individual CreateIndividualFromNGenome(ChihuahuaComponent comp, List<decimal> Ngenome)
        {
            GH_Document docu = comp.OnPingDocument();

            //Change value of sliders
            IList<IGH_Param> sliders = comp.Params.Input[0].Sources;

            List<decimal> genome = new List<decimal>();

            if (sliders.Count != Ngenome.Count)
            {
                throw new System.ArgumentException("Numbers of genes not equal to number of sliders", "Genome");
            }

            for (int i = 0; i < sliders.Count; i++)
            {

                Grasshopper.Kernel.Special.GH_NumberSlider slider = sliders[i] as Grasshopper.Kernel.Special.GH_NumberSlider;
                if (slider != null)
                {
                    decimal N_gene_value = Ngenome[i];
                    decimal gene_value = N_gene_value.Remap(0, 1, slider.Slider.Minimum, slider.Slider.Maximum);
                    slider.SetSliderValue(gene_value);
                    genome.Add(gene_value);
                }
            }


            // Get a new solution

            docu.NewSolution(false);

            //Read new fitness value

            decimal fitness = 0;

            IGH_Param param = comp.Params.Input[1].Sources[0];
            GH_Structure<GH_Number> outcome = comp.Params.Input[1].Sources[0].VolatileData as GH_Structure<GH_Number>;

            if (outcome == null)
            {
                throw new System.ArgumentException("Fitness input is not a number", "Fitness");
            }
            if (outcome != null)
            {
                fitness = (decimal)outcome.Branches[0][0].Value;
            }

            Individual ind = new Individual(fitness, genome, Ngenome);

            return ind;
        }
        
        private static Individual mixTwoIndividuals(ChihuahuaComponent comp,Individual ind1, Individual ind2, double mutationPercentage)
        {

            if (mutationPercentage > 1 || mutationPercentage < 0)
            {
                throw new System.ArgumentException("The mutation percentage must be betwen 0 and 1", "Mutation percentage");
            }

            //Get both genomes
            List<decimal> gen1 = ind1.NormalizeGenome;
            List<decimal> gen2 = ind2.NormalizeGenome;

            //Creat a new genome by randomly chosing of the genes

            List<decimal> newNGen = new List<decimal>();

            for (int i=0;i<gen1.Count; i++)
            {
                if (random.NextDouble() < 0.5) newNGen.Add(gen1[i]);
                else newNGen.Add(gen2[i]);
            }

            //Iterate over genes and mutate then

            //First mutation type will set the gene to a competely new random value
            for (int i=0; i<newNGen.Count; i++)
            {
                if(random.NextDouble()<= mutationPercentage)
                {
                    newNGen[i] = (decimal)random.NextDouble();

                }
            }

            //Second type of mutation will increase/decrese the gene value by a small percentage

            for (int i = 0; i < newNGen.Count; i++)
            {
                if (random.NextDouble() <= 0.15)
                {

                    decimal factor = ((decimal)random.NextDouble()).Remap(0, 1, 0.85m, 1.15m);
                    newNGen[i] *= factor;

                    //Make sure the modification does not take the value out of range
                    if (newNGen[i] < 0) newNGen[i] = 0;
                    if (newNGen[i] > 1) newNGen[i] = 1;

                }
            }

            //Create a new individual with the given genome

            Individual newInd = CreateIndividualFromNGenome(comp, newNGen);
            return newInd;


        }

        public static void ReinstateGenome(ChihuahuaComponent comp, List<decimal> Ngenome)
        {
            GH_Document docu = comp.OnPingDocument();

            //Change value of sliders
            IList<IGH_Param> sliders = comp.Params.Input[0].Sources;

            List<decimal> genome = new List<decimal>();

            if (sliders.Count != Ngenome.Count)
            {
                throw new System.ArgumentException("Numbers of genes not equal to number of sliders", "Genome");
            }

            for (int i = 0; i < sliders.Count; i++)
            {

                Grasshopper.Kernel.Special.GH_NumberSlider slider = sliders[i] as Grasshopper.Kernel.Special.GH_NumberSlider;
                if (slider != null)
                {
                    decimal N_gene_value = Ngenome[i];
                    decimal gene_value = N_gene_value.Remap(0, 1, slider.Slider.Minimum, slider.Slider.Maximum);
                    slider.SetSliderValue(gene_value);
                    genome.Add(gene_value);
                }
            }


            // Get a new solution

            docu.NewSolution(false);
        }

    }

}
