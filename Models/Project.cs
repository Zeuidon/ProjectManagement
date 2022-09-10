using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }

    }
}