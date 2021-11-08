using System.Collections.Generic;
using TtWork.ProjectName.Entities.Place;

namespace TtWork.ProjectName.Apis.Client.Dtos
{
    public class PlaceSuggestionDto
    {
        public string message { get; set; }
        public int state { get; set; }
        public List<Place> result { get; set; }
    }
}