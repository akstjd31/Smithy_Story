using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Resource : IItem
    {
        // 상수
        //const int Max_Quantity = 10;    // 최대 개수 (인벤토리 한 칸 기준)
        // 변수
        // 프로퍼티
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Price { get; set; }

        public int Quantity
        {
            get; set;
        }
        
        // 재료는 스택 가능 (개수 추가)
        public bool IsStackable => true;
        public Grade Grade { get; private set; }

        // 생성자
        public Resource(int id, string name, int price, Grade grade, int quantity = 5)  // 수량 = 임시
        {
            ID = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Grade = grade;
        }

        // 메소드

        // 출력문 재정의
        public override string ToString() => $"[{ID}] {Name}\t(개수: {Quantity}개, 개당 가격: {Price}, 등급: {Grade})\n";
    }
}
