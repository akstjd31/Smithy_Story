using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Weapon : IItem
    {
        // 상수
        const int MaxEnhanceLevel = 15;

        // 변수
        private int enhanceLevel;   // 강화 수치
        private int craftMinutes;   // 제작 시간

        // 프로퍼티
        public int ID { get; private set; }
        public string Name { get; private set; }                
        public int EnhanceLevel                         
        {
            get => enhanceLevel;
            private set
            {
                if (value <= 1) enhanceLevel = 1;
                else if (value > MaxEnhanceLevel) enhanceLevel = MaxEnhanceLevel;
                else enhanceLevel = value;
            }
        }
        
        public int Price
        {
            get => Price;
            private set
            {
                if (value <= 0) Price = 0;
                else Price = value;
            }
        }
        
        public Grade Grade { get; private set; }

        public int CraftMinutes
        {
            get => craftMinutes;
            private set { }
        }
        // 생성자
        public Weapon(int id, string name, int price, Grade grade, int craftMinutes, int enhanceLevel = 0)
        {
            ID = id;
            Name = name;
            Price = price;
            Grade = grade;
            this.craftMinutes = craftMinutes;
            this.enhanceLevel = enhanceLevel;
        }

        // 메소드

        // 출력문 재정의
        public override string ToString()
        {
            return $"[{ID}] {Name}\t{enhanceLevel}강\t(등급: {Grade}, 가격: {Price}\n";
        }
    }
}
