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
        // 변수
        // 프로퍼티
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }
        public Grade Grade { get; private set; }

        // 생성자
        public Resource(int id, string name, int price, Grade grade)
        {
            ID = id;
            Name = name;
            Price = price;
            Grade = grade;
        }

        // 메소드

        // 출력문 재정의
        public override string ToString()
        {
            return $"[{ID}] {Name}\t(등급: {Grade}, 가격: {Price}\n";
        }
    }
}
