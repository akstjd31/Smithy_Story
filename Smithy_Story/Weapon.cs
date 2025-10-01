using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 무기 등급
    public enum Grade
    {
        Common, Rare, Epic, Legendary
    }

    public class Weapon
    {
        // 상수
        const int MaxEnhanceLevel = 15;

        // 변수
        private string name;
        private int enhanceLevel;   // 강화 수치
        private int price;
        private int craftMinutes;   // 제작 시간
        private Grade grade;        // 무기 등급 

        // 프로퍼티
        public string Name { get; set; }                
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
            get => price;
            private set
            {
                if (value <= 0) price = 0;
                else price = value;
            }
        }          

        public int CraftMinutes
        {
            get => craftMinutes;
            private set { }
        }
        
        // 메소드

    }
}
