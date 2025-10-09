using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 시간 UI 클래스
    public class TimeUI
    {
        // 변수
        private GameTime gameTime;

        // 생성자
        public TimeUI(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        // 메소드
        // 출력
        public void Update()
        {
            Console.Write(gameTime.ToString() + "\t\t");
        }
    }
}
