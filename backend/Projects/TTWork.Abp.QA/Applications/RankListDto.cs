using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace TTWork.Abp.QA.Applications
{
    public class RankListDto
    {
        public DateTime CreationTime { get; set; }

        public List<RankListItem> Items { get; set; } = new();

        public List<RankShareListItem> ShareItems { get; set; } = new();

        public int Total { get; set; }
    }

    public class RankShareListItem
    {
        public string ImageUrl { get; set; }

        [CanBeNull] public string Name { get; set; }

        public string PhoneNumber { get; set; }

        [CanBeNull] public string Town { get; set; }

        public long UserId { get; set; }

        public int Count { get; set; }
    }
}