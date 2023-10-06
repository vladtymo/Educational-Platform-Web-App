using SS_EDUP.Core.DTO_s;


namespace SS_EDUP.Web.ViewModels
{
    public class CourseCardViewModel
    {
        public CourseDetailDto? CourseDto { get; set; }
        public bool IsInCart { get; set; }
        public bool IsLearning { get; set; }
    }
}
