using System;
using System.ComponentModel.DataAnnotations;

namespace BloggApp.Models;
    public class Article
    {
        public int ArticleId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        public DateTime CreatDate { get; set; } = DateTime.Now;
        
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string ContributorUsername { get; set; }
    }
