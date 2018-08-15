using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSystem.Core.Model
{
    public class ComputerDetails
    {
        #region Public Members
        public string NameComputer { get; set; }
        public string Ipv4 { get; set; }
        public string UserName { get; set; }
        public object Processes { get; set; }

        #endregion

    }
}
