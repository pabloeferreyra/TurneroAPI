using System;
using System.ComponentModel.DataAnnotations;
using Turnero.Models;

namespace TurneroAPI.DTO
{
    public class TurnDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Dni { get; set; }
        public string? Medic { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTurn { get; set; }
        public string? Time { get; set; }
        public string? SocialWork { get; set; }
        public string? Reason { get; set; }
        public bool Accessed { get; set; }

        public TurnDTO(Turn turn)
        {
            Id = turn.Id;
            Name = turn.Name;
            Dni = turn.Dni;
            Medic = turn.Medic?.Name;
            DateTurn = turn.DateTurn;
            Time = turn.Time?.Time;
            SocialWork = turn.SocialWork;
            Reason = turn.Reason;
            Accessed = turn.Accessed;
        }
    }

    public class TurnAddDTO
    {
        public string? Name { get; set; }
        public string? Dni { get; set; }
        public Guid Medic { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTurn { get; set; }
        public Guid Time { get; set; }
        public string? SocialWork { get; set; }
        public string? Reason { get; set; }
    }
}
