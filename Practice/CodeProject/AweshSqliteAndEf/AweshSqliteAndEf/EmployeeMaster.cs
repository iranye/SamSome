using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AweshSqliteAndEf
{
    [Table("EmployeeMaster")]
    public class EmployeeMaster
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }

        [Column("EmpName")]
        public string EmpName { get; set; }

        [Column("Salary")]
        public double Salary { get; set; }

        [Column("Designation")]
        public string Designation { get; set; }
    }
}
