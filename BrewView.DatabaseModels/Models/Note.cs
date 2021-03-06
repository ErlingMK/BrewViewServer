﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewView.DatabaseModels.Models
{
    public class Note
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? NoteId { get; set; }
        public DateTime? Date { get; set; }
        public string Text { get; set; }
        public int? Rating { get; set; }
        public UserBrew UserBrew { get; set; }
    }
}