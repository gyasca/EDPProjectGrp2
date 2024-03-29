﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDPProjectGrp2.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a user name.")]
        [StringLength(50, ErrorMessage = "User name must be less than 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        [StringLength(50, ErrorMessage = "Subject must be less than 50 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter a comment.")]
        [StringLength(1000, ErrorMessage = "Comment must be less than 1000 characters.")]
        public string Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Please enter an eventID.")]
        public int EventId { get; set; }
    }
}

