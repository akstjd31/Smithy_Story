using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Grade
{
    Common, Rare, Epic, Legendary
}

public interface IItem
{
    int ID { get; }
    string Name { get; }
    int Price { get; }
    int Quantity { get; }
    Grade Grade { get; }
}
