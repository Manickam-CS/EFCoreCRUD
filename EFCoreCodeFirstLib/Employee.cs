using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreCodeFirstLib
{
    public class Employee
    {
        //EmpId, Name, Address, DeptId, ImagePath
        [Key]
        public int EmpId { get; set; }

        [Column(TypeName = "varchar(500)")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        [ForeignKey("Department")]
        public int DeptId { get; set; }

        //Navigation Property
        public virtual Department Department { get; set; }
    }
}
