using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Grade
{
    Common, Rare, Epic, Legendary
}

public interface IItem : ICloneable
{
    int ID { get; }
    string Name { get; }
    int Price { get; set; }
    int Quantity { get; set; }  // 양, 개수
    bool IsStackable { get; }   // 스택 가능? (2개 이상을 지닐 수 있는 아이템 == 재료)
    Grade Grade { get; }        // 등급
}
