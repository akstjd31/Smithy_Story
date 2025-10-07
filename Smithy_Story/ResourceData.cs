using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class ResourceData
    {
        // 상수
        // 변수
        private static DataManager<Resource> manager = new DataManager<Resource>();

        // 프로퍼티

        // 생성자
        static ResourceData()
        {
            manager.AddItem(new Resource(id: 1001, name: "돌",         price: 1,  grade: Grade.Common));
            manager.AddItem(new Resource(id: 1002, name: "나무",       price: 2,  grade: Grade.Common));
            manager.AddItem(new Resource(id: 1003, name: "금광석",     price: 8,  grade: Grade.Rare));
            manager.AddItem(new Resource(id: 1004, name: "철", price: 5, grade: Grade.Epic));
            manager.AddItem(new Resource(id: 1005, name: "실", price: 4, grade: Grade.Rare));
            manager.AddItem(new Resource(id: 1006, name: "다이아몬드", price: 20, grade: Grade.Legendary));
        }

        // 메소드
        public static Resource GetById(int id) => manager.GetById(id);
        public static Resource GetByName(string name) => manager.GetByName(name);
        public static IEnumerable<Resource> GetAll() => manager.GetAll();
    }
}
