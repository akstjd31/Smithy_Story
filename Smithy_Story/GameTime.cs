using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 게임 시간 클래스
    public class GameTime
    {
        // 상수
        // 변수
        private int day;    // 일
        private int hour;   // 시간
        private int minute; // 분

        // 프로퍼티
        public int Day
        { get; private set; }

        public int Hour
        { get; private set; }

        public int Minute
        { get; private set; }

        // 생성자
        public GameTime()
        {
            day = 0;
            hour = 8;
            minute = 0;
        }

        // 커스텀 시간 설정
        public GameTime(int day, int hour, int minute)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }

        // 메소드

        // 시간 증가 메소드
        public void AddMinutes(int min)
        {
            minute += min;

            // minute 계산
            while (minute >= 60)
            {
                minute -= 60;
                hour++;
            }

            // hour 계산
            while (hour >= 24)
            {
                hour -= 24;
            }
        }

        // 일 추가 메소드
        public void AddDays(int days) => this.day += days;

        // 출력 ?일 ??:??
        public string GetFormattedTime() => $"Day {day}, {hour:D2}:{minute:D2}";
    }
}
