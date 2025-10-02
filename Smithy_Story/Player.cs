using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Player
    {
        // 상수
        const int MaxFatigue = 100; // 최대 피로도

        // 변수
        private string name;
        private int fatigue;        // 피로도
        private int money;          // 자산

        // 프로퍼티
        public string Name
        {
            get => name;
            private set
            { name = value; }   
        }

        public int Fatigue
        {
            get => fatigue;
            private set
            {
                if (value <= 0) fatigue = 0;
                else if (value > MaxFatigue) fatigue = MaxFatigue;
                else fatigue = value;
            }
        }

        public int Money
        {
            get => money;
            private set
            {
                if (value <= 0) money = 0;
                else money = value;
            }
        }

        // 생성자
        public Player(string name, int fatigue = 0, int money = 0)
        {
            this.name = name;
            this.fatigue = fatigue;
            this.money = money;
        }

        // 메소드
        public override string ToString()
        {
            return $"이름: {Name}\t피로도: {Fatigue}\n";
        }
    }
}
