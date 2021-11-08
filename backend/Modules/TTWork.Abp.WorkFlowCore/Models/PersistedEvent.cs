using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedEvent : Entity<Guid>
    {
        [MaxLength(200)]
        public string EventName { get; set; }

        [MaxLength(200)]
        public string EventKey { get; set; }

        public string EventData { get; set; }

        public DateTime EventTime { get; set; }

        public bool IsProcessed { get; set; }
    }
}