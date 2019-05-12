using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chihuahua
{
    static class Helpers
    {
        public static decimal Remap(this decimal value, decimal from1, decimal to1, decimal from2, decimal to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
