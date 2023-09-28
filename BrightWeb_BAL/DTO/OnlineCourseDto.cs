﻿using BrightWeb_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.DTO
{
    public class OnlineCourseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Objectives { get; set; }
        public required double Price { get; set; }
        public double Discount { get; set; }
        public bool HasDiscount { get; set; }
        public required int Enrollments { get; set; }
        public required string Hours { get; set; }
        public string? IntructorName { get; set; }
        public string? IntructorDescription { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
        public string? Link { get; set; }
    }
}
