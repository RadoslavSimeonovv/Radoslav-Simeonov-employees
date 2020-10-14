using System;
using System.Collections.Generic;
using System.Text;

namespace Sirma_Task
{
    public class ProjectWorked
    {
        //ProjectWorked class with project and his dates - used in the dictionary
        public ProjectWorked(int projId, string dFrom, string dTo)
        {
            this.ProjectId = projId;
            this.DateFrom = dFrom;
            this.DateTo = dTo;
        }
        public int ProjectId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

    }
}
