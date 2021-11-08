using System.ComponentModel.DataAnnotations;

namespace TtWork.ProjectName.Apis.Organizations.Dtos
{
    public class ApplyInputDto
    {
        public long Id { get; set; }
        [StringLength(512, ErrorMessage = "字数不能超过512字")]
        public string RefuseContent { get; set; }
    }
}