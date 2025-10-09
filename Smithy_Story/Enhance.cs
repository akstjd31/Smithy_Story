using System;
using System.Threading;

namespace Smithy_Story
{
    // 강화 결과 열거형 클래스
    public enum EnhanceResult
    {
        Success,    // 성공
        Fail,       // 실패
        Destroyed   // 파괴
    }

    // 강화 클래스
    public class Enhance
    {
        // 상수
        // 변수
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
