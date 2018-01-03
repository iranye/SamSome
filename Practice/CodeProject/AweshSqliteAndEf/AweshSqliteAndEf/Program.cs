using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweshSqliteAndEf
{
    class Program
    {
        static void Main()
        {
            try
            {
                DatabaseContext context = new DatabaseContext();
                EmployeeMaster employee = new EmployeeMaster()
                {
                    EmpName = "Foobar",
                    Salary = 656.12,
                    Designation = "Dingaling"
                };
                context.EmployeeMaster.Add(employee);
                context.SaveChanges();

                var data = context.EmployeeMaster.ToList();
                foreach (var item in data)
                {
                    Console.Write(string.Format("Id : {0}  Name : {1}  Salary : {2}   Designation : {3}{4}", item.Id, item.EmpName, item.Salary, item.Designation, Environment.NewLine));
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + Environment.NewLine + ex.Message);
            }
            Console.ReadKey();
        }
    }
}
