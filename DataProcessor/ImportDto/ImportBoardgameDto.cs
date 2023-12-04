﻿using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(10)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [Range(1,10)]
        [XmlElement("Rating")]
        public double Rating { get; set; }
        [Required]
        [Range(0,4)]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }
        [Required]
        [Range (2018,2023)]
        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }
        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; }
    }
}
