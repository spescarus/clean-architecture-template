using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Application.Base.Models
{
    public class TimeStampedExtendModel : TimeStampedModel
    {
        public UserId  CreatedBy { get; set; }
        public UserId  UpdatedBy { get; set; }
        public UserId? DeletedBy { get; set; }

    }
}
