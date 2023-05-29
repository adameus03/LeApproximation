/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationData
{
    internal class ComputationDumpEventArgs
    {
    }
}*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximationData
{
    public class ComputationDumpEventArgs : EventArgs
    {
        private string line;
        public ComputationDumpEventArgs(string line)
        {
            this.line = line;
        }
        public string Line => this.line;
    }
}

