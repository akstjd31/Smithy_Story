using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 업그레이드 결과 열거형 클래스
    public enum UpgradeResult
    {
        Success, Fail, Destroyed
    }

    // 업그레이드 클래스
    public class Upgrade
    {
        // 상수
        // 변수
        double successRate; // 성공 확률
        double failRate;    // 실패 확률
        double destroyRate; // 파괴 확률
                            // 프로퍼티

        // 생성자
        public Upgrade(double successRate, double failRate, double destroyRate)
        {
            this.successRate = successRate;
            this.failRate = failRate;
            this.destroyRate = destroyRate;
        }

        // 메소드
    }
}
