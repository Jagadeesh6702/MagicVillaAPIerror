﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_Villa2.Model.DTO
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
      
        public double Rate { get; set; }

        public int Occupancy { get; set; }

        public int Sqft { get; set; }
        public string ImageUrl { get; set; }

        public string Amenity { get; set; }
    }
}
