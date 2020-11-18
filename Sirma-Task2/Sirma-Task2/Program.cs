using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sirma_Task
{
    class Program
    {
        /// <summary>
        /// Method to calculate the common period of time between two sets of dates.
        /// </summary>
        /// <param name="s1">First set date from</param>
        /// <param name="e1">First set date to</param>
        /// <param name="s2">Second set date from</param>
        /// <param name="e2">Second set date to</param>
        /// <returns>Returns the calculated days.</returns>
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

       /// <summary>
       /// Method to convert the date if needed to today's date if its NULL.
       /// </summary>
       /// <param name="date">Date to be converted</param>
       /// <returns>Returns the converted or non-converted date.</returns>
        private static string ConvertDate(string date)
        {
            if(date == "NULL")
            {
                return date = DateTime.Today.ToString();
            }

            return date;
        }

        /// <summary>
        /// Method to calculate the total amount of days worked between two employees on common projects.
        /// It uses ConvertDate method to get the correct date and InclusiveDays method to find the common days between two sets of dates.
        /// </summary>
        /// <param name="emps1">List of first employee common projects</param>
        /// <param name="emps2">List of second employee common projects</param>
        /// <returns> Return days worked on common projects between two employees. </returns>
        private static int CommonEmployeesDays(List <ProjectWorked> emps1, List <ProjectWorked> emps2)
        {
            var totalDays = 0;
            foreach (var emp1 in emps1)
            {
                foreach (var emp2 in emps2)
                {
                    emp1.DateFrom = ConvertDate(emp1.DateFrom);
                    emp1.DateTo = ConvertDate(emp1.DateTo);
                    emp2.DateFrom = ConvertDate(emp2.DateFrom);
                    emp2.DateTo = ConvertDate(emp2.DateTo);

                    var calcInterDays = InclusiveDays(Convert.ToDateTime(emp1.DateFrom), Convert.ToDateTime(emp1.DateTo),
                    Convert.ToDateTime(emp2.DateFrom), Convert.ToDateTime(emp2.DateTo));

                    totalDays += calcInterDays;
                }
            }
            return totalDays;
        }


        static void Main(string[] args)
        {
            var counter = 0;
            string line;

            var employees = new Dictionary<int, List<ProjectWorked>>();

            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            //file in main directory with the records
            var file =
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
            var projectsWorkedOn = new List<int>();
            var totalDaysWorked = 0;
            var topEmp1Id = 0;
            var topEmp2Id = 0;
            var daysWorked = 0;

            /*         
             * First we find the common project ID's between different employees added by ID in the dictionary for key.
             * Then we get the list of employees from the list with their projects and dates who are already added in the dictionary.
             */
            foreach (var id in employees.Keys)
            {
                foreach (var id2 in employees.Keys)
                {
                    if (id != id2)
                    {
                        var commonListOfProjects = employees[id].Select(e => e.ProjectId).ToList()
                            .Intersect(employees[id2].Select(e2 => e2.ProjectId).ToList()).ToList();

                        foreach (var commonProjectId in commonListOfProjects)
                        {
                            var firstEmps = employees[id].Where(e => e.ProjectId == commonProjectId).ToList();
                            var secondEmps = employees[id2].Where(e => e.ProjectId == commonProjectId).ToList();

                            daysWorked = CommonEmployeesDays(firstEmps, secondEmps);
                            totalDaysWorked += daysWorked;
                        
                        }
                        if (totalDaysWorked > maxDays)
                        {
                            projectsWorkedOn = commonListOfProjects;
                            maxDays = totalDaysWorked;
                            topEmp1Id = id;
                            topEmp2Id = id2;
                        }
                    }
                    totalDaysWorked = 0;
                }
            }
            Console.WriteLine("Employee " + topEmp1Id + " and employee "  + topEmp2Id + " worked on projects: " + (string.Join("," , projectsWorkedOn))
                + " for a total of " + maxDays + " days maximum.");

        }
    }
}
