using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace TTWork.Abp.WorkFlowCore.Models
{
    public class PersistedSubscription : Entity<Guid>
    {
        public string WorkflowId { get; set; }

        public int StepId { get; set; }

        public string ExecutionPointerId { get; set; }

        [MaxLength(200)]
        public string EventName { get; set; }

        [MaxLength(200)]
        public string EventKey { get; set; }

        public DateTime SubscribeAsOf { get; set; }

        public string SubscriptionData { get; set; }

        [MaxLength(200)]
        public string ExternalToken { get; set; }

        [MaxLength(200)]
        public string ExternalWorkerId { get; set; }

        public DateTime? ExternalTokenExpiry { get; set; }
    }
}