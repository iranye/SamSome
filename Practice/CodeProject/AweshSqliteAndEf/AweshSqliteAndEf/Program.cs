using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweshSqliteAndEf
{
    class Program
    {
        private static DatabaseContext context = new DatabaseContext();
        static void Main()
        {
            int continueFlag = 1;

            while (continueFlag != 0)
            {
                Console.WriteLine("Enter a to add, p to print.");
                string menuOption = Console.ReadLine();

                if (menuOption.ToUpper().StartsWith("A"))
                {
                    PerformAdd();
                }
                else
                {
                    foreach (var item in context.EmployeeMaster.ToList())
                    {
                        Console.WriteLine("Id : {0}  Name : {1}  Salary : {2}   Designation : {3}", item.Id, item.EmpName,
                            item.Salary, item.Designation);
                    }
                }
                Console.WriteLine("Enter 0 to stop");
                string input = Console.ReadLine();
                if (!Int32.TryParse(input, out continueFlag))
                {
                    continueFlag = 1;
                }
            }
        }

        private static void PerformAdd()
        {
            try
            {
                Console.Write("name? ");
                var name = Console.ReadLine().Trim();
                Console.Write("salary? ");
                var salary = Console.ReadLine().Trim();
                Console.Write("designation? ");
                var designation = Console.ReadLine().Trim();

                Double salaryParsed;
                if (!Double.TryParse(salary, out salaryParsed))
                {
                    Console.Error.WriteLine("Could not parse salary from '{0}'", salary);
                }
                else
                {
                    EmployeeMaster employee = new EmployeeMaster()
                    {
                        EmpName = name,
                        Salary = salaryParsed,
                        Designation = designation
                    };
                    context.EmployeeMaster.Add(employee);
                    context.SaveChanges();
                }

                //var existingEmployee = context.EmployeeMaster.Where(e => e.Id == 1)

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + Environment.NewLine + ex.Message);
            }
        }
    }
}
