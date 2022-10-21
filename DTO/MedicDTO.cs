using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using Turnero.Models;

namespace TurneroAPI.DTO
{
    public class MedicDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public MedicDTO(Medic medic)
        {
            Id = medic.Id;
            Name = medic.Name;
        }
    }
}
