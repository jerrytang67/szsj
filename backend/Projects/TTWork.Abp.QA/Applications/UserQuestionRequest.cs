namespace TTWork.Abp.QA.Applications
{
    public class UserQuestionRequest
    {
        public long Id { get; set; }

        public int QuestionIndex { get; set; }

        public int AnswerIndex { get; set; }
    }
}