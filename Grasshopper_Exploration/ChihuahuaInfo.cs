using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Chihuahua
{
    public class ChihuahaSolver : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Chihuaha Solver";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e2372beb-91a0-48cb-a0d0-f58617e9e889");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
