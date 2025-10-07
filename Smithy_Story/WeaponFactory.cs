//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Smithy_Story
//{
//    // 메소드
//    public static class WeaponFactory
//    {
//        public static Weapon CreateWeaponById(int id)
//        {
//            switch (id)
//            {
//                case 1:
//                    return new Weapon(id: 1, name: "곡괭이", price: 3, grade: Grade.Common, craftMinutes: 6);
//                case 2:
//                    return new Weapon(id: 2, name: "삽",     price: 2, grade: Grade.Common, craftMinutes: 4);
//                case 3:
//                    return new Weapon(id: 3, name: "목검",   price: 5, grade: Grade.Common, craftMinutes: 15);
//                case 4:
//                    return new Weapon(id: 4, name: "나무활", price: 7, grade: Grade.Common, craftMinutes: 20);
//                default:
//                    throw new ArgumentException("잘못된 무기 ID 입니다!");
//            }
//        }

//        public static Weapon CreateWeaponByName(string name)
//        {
//            switch (name)
//            {
//                case "곡괭이":
//                    return new Weapon(id: 1, name: "곡괭이", price: 3, grade: Grade.Common, craftMinutes: 6);
//                case "삽":
//                    return new Weapon(id: 2, name: "삽",     price: 2, grade: Grade.Common, craftMinutes: 4);
//                case "목검":
//                    return new Weapon(id: 3, name: "목검",   price: 5, grade: Grade.Common, craftMinutes: 15);
//                case "나무활":
//                    return new Weapon(id: 4, name: "나무활", price: 7, grade: Grade.Common, craftMinutes: 20);
//                default:
//                    throw new ArgumentException("잘못된 무기 ID 입니다!");
//            }
//        }
//    }
//}