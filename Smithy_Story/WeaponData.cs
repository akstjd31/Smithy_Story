using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Smithy_Story
{
    public class WeaponData
    {
        // 상수
        // 변수
        private static DataManager<Weapon> manager = new DataManager<Weapon>();

        // 프로퍼티

        // 생성자
        static WeaponData()
        {
            manager.AddItem(new Weapon(id: 1, name: "곡괭이", price: 5, grade: Grade.Common, craftMinutes: 10));
            manager.AddItem(new Weapon(id: 2, name: "목검", price: 15, grade: Grade.Common, craftMinutes: 15));
            manager.AddItem(new Weapon(id: 3, name: "나무활", price: 20, grade: Grade.Common, craftMinutes: 20));
            manager.AddItem(new Weapon(id: 4, name: "패월도", price: 40, grade: Grade.Epic, craftMinutes: 80));
            manager.AddItem(new Weapon(id: 5, name: "천총운검", price: 5, grade: Grade.Common, craftMinutes: 140));
        }

        // 메소드
        public static Weapon GetById(int id) => manager.GetById(id);  
        public static Weapon GetByName(string name) => manager.GetByName(name);
        public static IEnumerable<Weapon> GetAll() => manager.GetAll();
    }
}
