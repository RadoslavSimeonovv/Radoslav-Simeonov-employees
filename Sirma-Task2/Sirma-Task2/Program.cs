using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sirma_Task
{
    class Program
    {

        //method to calculate the common period between two sets of dates
        private static int InclusiveDays(DateTime s1, DateTime e1, DateTime s2, DateTime e2)
        {
            // If they don't intersect return 0.
            if (!(s1 <= e2 && e1 >= s2))
            {
                return 0;
            }
            // Take the highest start date and the lowest end date.
            DateTime start = s1 > s2 ? s1 : s2;
            DateTime end = e1 > e2 ? e2 : e1;

            // Add one to the time range since its inclusive.
            return (int)(end - start).TotalDays + 1;
        }

        static void Main(string[] args)
        {
            var counter = 0;
            string line;
      
            Dictionary<int, List<ProjectWorked>> employees = new Dictionary<int, List<ProjectWorked>>();

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            //file in main directory with the records
            StreamReader file =
                new StreamReader(projectDirectory + "\\employees.txt");

            //filling the dictionary with key - Employee ID and value - list of ProjectWorked - Project ID, DateFrom and DateTo
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',').Select(w => w.Trim()).ToArray();

                ProjectWorked pj = new ProjectWorked(Convert.ToInt32(words[1]), words[2], words[3]);
                if (employees.ContainsKey(Convert.ToInt32(words[0])))
                {
                    employees[Convert.ToInt32(words[0])].Add(pj);
                }
                else
                {
                    employees.Add(Convert.ToInt32(words[0]), new List<ProjectWorked> { pj });
                }

                counter++;
            }

            file.Close();

            var maxDays = 0;
            var topEmp1Id = 0;
            var topEmp2Id = 0;

            /*
             * First we find the common project ID's between different employees added by ID in the dictionary for key.
             * Then we get the two employees from the list with their projects and dates who are already added in the dictionary.
             * We check if some of their dates are NULL and if they are, we make them the day today.
             * Then we use the private method who finds the number of common days between two sets of dates and add them into the total days.
             * We repeat the same for every two different employees and always store the current biggest number of days and the two employee ID's
             * And we print them to the console at the end.
             */
            foreach (var id in employees.Keys)
            {
                foreach (var id2 in employees.Keys)
                {
                    var totalDays = 0;
                    if (id != id2)
                    {
                        var commons = employees[id].Select(e => e.ProjectId).ToList()
                            .Intersect(employees[id2].Select(e2 => e2.ProjectId).ToList()).ToList();

                        foreach (var commonProjectId in commons)
                        {
                            var firstEmp = employees[id].FirstOrDefault(e => e.ProjectId == commonProjectId);
                            var secondEmp = employees[id2].FirstOrDefault(e => e.ProjectId == commonProjectId);
                            List<ProjectWorked> bothEmps = new List<ProjectWorked> { firstEmp, secondEmp };

                            if (firstEmp.DateTo == "NULL")
                            {
                                firstEmp.DateTo = DateTime.Today.ToString();
                            }
                            if (firstEmp.DateFrom == "NULL")
                            {
                                firstEmp.DateFrom = DateTime.Today.ToString();
                            }
                            if (secondEmp.DateTo == "NULL")
                            {
                                secondEmp.DateTo = DateTime.Today.ToString();
                            }
                            if (secondEmp.DateFrom == "NULL")
                            {
                                secondEmp.DateFrom = DateTime.Today.ToString();
                            }

                            var calcInterDays = InclusiveDays(Convert.ToDateTime(firstEmp.DateFrom), Convert.ToDateTime(firstEmp.DateTo),
                                Convert.ToDateTime(secondEmp.DateFrom), Convert.ToDateTime(secondEmp.DateTo));

                            totalDays += calcInterDays;
             
                        }
                        if (totalDays > maxDays)
                        {
                            maxDays = totalDays;
                            topEmp1Id = id;
                            topEmp2Id = id2;
                        }
                    }
                    totalDays = 0;
                }
            }
            Console.WriteLine("Longest days worked on common projects: " + maxDays + " First employee ID=" + topEmp1Id + " Second employee ID=" + topEmp2Id);

        }
    }
}
