using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Employee_Dept_API.Models
{
    public class Employee
    {
       public int EmployeeId { get; set; }
        
       public string EmployeeName { get; set; }

       public string Department { get; set; }

       public string DateofJoining { get; set; }

       public int MyProperty { get; set; }

       public string PhotoFileName { get; set; }

    }
}

