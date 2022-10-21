using System;
using Turnero.Models;

namespace TurneroAPI.DTO
{
    public class TimeDTO
    {
        public Guid Id { get; set; }
        public string Time { get; set; }

        public TimeDTO(TimeTurnViewModel time) 
        {
            Id = time.Id;
            Time = time.Time;
        }
    }
}
