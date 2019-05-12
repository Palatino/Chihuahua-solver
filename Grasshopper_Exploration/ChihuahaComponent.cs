using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Chihuahua
{
    public class ChihuahuaComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        /// 

        public ChihuahuaComponent()
          : base("Chihuahua Solver", "Chihuahuha",
              "Basic Evolutive Solver",
              "Useless Components", "Evolutive Solver")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Genes", "Genes", "Genes to optimize, must be number sliders", GH_ParamAccess.list);
            pManager.AddNumberParameter("Fitness", "Fitness", "Fitness Value", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Maximize (T), Minimize (F)", "M/m", "Choose  whether to maximize or minimize goal ", GH_ParamAccess.item, true);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
           ///Nothing here
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Do nothing here to avoid recomputing of this component
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.Chihuahua;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("cb95d98b-2ab0-4b93-8062-bb1b2dada82d"); }
        }

        
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Run Chihuahua", SolverClicked);
        }

        private void SolverClicked(object sender, EventArgs eventArgs)
        {
            Generation gen1 = Chihuahua.CreateRandomGeneration(this, 50);
            for (int i =0; i<22; i++)
            {
                Generation next = Chihuahua.EvolveGeneration(this, gen1, 5, 50, 0.1);
                gen1 = next;
            }

            Rhino.RhinoApp.WriteLine(string.Format("Best fitness: {0}", gen1.bestFitness));

            foreach( decimal gen in gen1.bestIndividual.Genome)
            {
                Rhino.RhinoApp.WriteLine(gen.ToString());

            }

            //Reinstate the best solution back the grasshopper canvas

            Chihuahua.ReinstateGenome(this, gen1.bestIndividual.NormalizeGenome);
            


        }

    }

}
