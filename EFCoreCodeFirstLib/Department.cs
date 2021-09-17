using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreCodeFirstLib
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required]
        public string Name { get; set; }
        
        //navigation property
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
