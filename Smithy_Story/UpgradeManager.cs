using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 업그레이드 매니저(등급/레벨별 확률 테이블 정리) 클래스
    public class UpgradeManager
    {
        // 상수
        // 변수
        Dictionary<(Grade grade, int level), Upgrade> rateTable;

        // 프로퍼티
        // 생성자
        public UpgradeManager()
        {
             rateTable = new Dictionary<(Grade grade, int level), Upgrade>();
        }

        // 메소드
    }
}
