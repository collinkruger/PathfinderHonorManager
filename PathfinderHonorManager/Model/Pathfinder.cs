﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PathfinderHonorManager.Model
{
    [Table("pathfinder")]
    [Index(nameof(Email), IsUnique = true)]

    public class Pathfinder
    {
        [Key]
        [Column("pathfinder_id")]
        public Guid PathfinderID { get; set; }
        [Column("first_name")]
        public String FirstName { get; set; }
        [Column("last_name")]
        public String LastName { get; set; }
        [Column("email")]

        public String Email { get; set; }
        [Column("create_timestamp"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }
        [Column("update_timestamp"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated { get; set; }


        [ForeignKey("PathfinderID")]
        public ICollection<PathfinderHonor> PathfinderHonors { get; set; }

    }
}
