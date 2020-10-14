using System;
using System.Collections.Generic;
using System.Text;

namespace Sirma_Task
{
    public class Employee
    {
        //Employee class with his id and project
        public Employee(int empId, int projId, string dFrom, string dTo)
        {
            this.EmpId = empId;
            this.ProjectId = projId;
            this.DateFrom = dFrom;
            this.DateTo = dTo;
        }
        public int EmpId { get; set; }
        public int ProjectId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
