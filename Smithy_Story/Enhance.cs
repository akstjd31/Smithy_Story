using System;
using System.Threading;

namespace Smithy_Story
{
    public enum EnhanceResult
    {
        Success,    // 성공
        Fail,       // 실패
        Destroyed   // 파괴
    }
    public class Enhance
    {
        // 프로퍼티
        public double SuccessRate { get; set; }    
        public double FailRate { get; set; }
        public double DestroyRate { get; set; }

        // 생성자
        public Enhance(double successRate, double failRate, double destroyRate)
        {
            SuccessRate = successRate; 
            FailRate = failRate;
            DestroyRate = destroyRate;
        }
    }
}
