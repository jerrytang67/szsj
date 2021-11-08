using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TTWork.Abp.QA
{
    public class EnumClass
    {
        public enum QAPlanType
        {
            未设置 = 0,
            积分奖励 = 1,
            抽奖 = 2
        }


        public enum QAPlanState
        {
            关闭 = 0,
            开启 = 1
        }

        public enum QAQuestionState
        {
            关闭 = 0,
            开启 = 1
        }
    }
}