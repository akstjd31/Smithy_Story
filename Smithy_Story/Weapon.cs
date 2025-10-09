using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 무기 클래스
    public class Weapon : IItem
    {
        // 상수
        const int MaxEnhanceLevel = 15;
        public static readonly int MaxDurability = 100;

        // 변수
        private int enhanceLevel;   // 강화 수치
        private int craftMinutes;   // 제작 시간

        // 프로퍼티
        public int Durability { get; private set; }

        public Dictionary<Resource, int> RequiredResources { get; private set; }
        public int ID { get; private set; }
        public string Name { get; private set; }                
        public int EnhanceLevel                         
        {
            get => this.enhanceLevel;
            private set
            {
                if (value <= 1) this.enhanceLevel = 1;
                else if (value > MaxEnhanceLevel) this.enhanceLevel = MaxEnhanceLevel;
                else this.enhanceLevel = value;
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
        public bool IsItemDeposited { get; set; } = false;   // 의뢰자한테 맡겨진 물건인가?

        // 생성자
        public Weapon(){ }
        // 무기는 무조건 개수 1 고정!!!!!!!!
        public Weapon(int id, string name, int price, Grade grade, int craftMinutes,
            Dictionary<Resource, int> requiredResources,
            int quantity = 1, int enhanceLevel = 0, int durability = 50, bool isDeposited = false)
        {
            ID = id;
            Name = name;
            Price = price;
            Grade = grade;
            Quantity = quantity;
            this.craftMinutes = craftMinutes;
            this.enhanceLevel = enhanceLevel;
            Durability = durability;
            RequiredResources = requiredResources;
            IsItemDeposited = isDeposited;
        }

        // 메소드
        // 무기 레벨 업 (강화수치 업)
        public void LevelUp() => EnhanceLevel++;

        // 수리
        public void Repair() => Durability = MaxDurability;

        // 내구도 설정
        public void SetDurability(int durability) => Durability = durability;


        // 현재 무기의 가치 (단순 재료비)
        public double BaseValue()
        {
            double value = 0;

            foreach (var resource in RequiredResources.Keys)
                value += resource.Price * resource.Quantity;
            
            return value;
        }

        // 복사
        public IItem Clone()
        {
            // RequiredResources 딥 카피
            var newResources = new Dictionary<Resource, int>();
            foreach (var kvp in RequiredResources)
            {
                // Resource도 Clone()이 구현되어 있어야 함
                newResources.Add((Resource)kvp.Key.Clone(), kvp.Value);
            }

            // 새로운 Weapon 객체 생성
            return new Weapon(
                id: this.ID,
                name: this.Name,
                price: this.Price,
                grade: this.Grade,
                craftMinutes: this.CraftMinutes,
                requiredResources: newResources,
                quantity: this.Quantity,
                enhanceLevel: this.EnhanceLevel,
                durability: this.Durability,
                isDeposited: this.IsItemDeposited
            );
        }

        object ICloneable.Clone() => Clone();

        // 출력문 재정의
        public override string ToString() => $"[{ID}] {EnhanceLevel}강 {Name}\t(등급: {Grade}, 가격: {Price})\t내구도 [{Durability}/{MaxDurability}]\n";
    }
}
