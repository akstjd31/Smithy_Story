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
        private int enhanceLevel;                               // 강화 수치
        private int craftMinutes;                               // 제작 시간

        // 프로퍼티
        public Dictionary<Resource, int> RequiredResources { get; private set; }
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

        public int Price { get; set; }

        public int Quantity
        {
            get; set;
        }

        public int CraftMinutes
        {
            get => craftMinutes;
            private set { }
        }

        // 무기는 스택 불가 (개수 항상 1)
        public bool IsStackable => false;
        public Grade Grade { get; private set; }



        // 생성자
        // 무기는 무조건 개수 1 고정!!!!!!!!
        public Weapon(int id, string name, int price, Grade grade, int craftMinutes,
            Dictionary<Resource, int> requiredResources,
            int quantity = 1, int enhanceLevel = 0)
        {
            ID = id;
            Name = name;
            Price = price;
            Grade = grade;
            Quantity = quantity;
            this.craftMinutes = craftMinutes;
            this.enhanceLevel = enhanceLevel;
            RequiredResources = requiredResources;
        }

        // 메소드
        // 출력문 재정의
        public override string ToString() => $"[{ID}] {enhanceLevel}강 {Name}\t(등급: {Grade}, 가격: {Price})\n";
    }
}
