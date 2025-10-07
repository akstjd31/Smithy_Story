//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Smithy_Story
//{
//    // 메소드
//    public static class ResourceFactory
//    {
//        public static Resource CreateWeaponById(int id)
//        {
//            switch (id)
//            {
//                case 1001:
//                    return new Resource(id: 1001, name: "돌", price: 1, grade: Grade.Common);
//                case 1002:
//                    return new Resource(id: 1002, name: "나무", price: 2, grade: Grade.Common);
//                case 1003:
//                    return new Resource(id: 1003, name: "금광석", price: 5, grade: Grade.Rare);
//                case 1005:
//                    return new Resource(id: 1005, name: "양털", price: 8, grade: Grade.Rare);
//                case 1006:
//                    return new Resource(id: 1006, name: "a", price: 20, grade: Grade.Legendary);
//                default:
//                    throw new ArgumentException("잘못된 무기 ID 입니다!");
//            }
//        }

//        public static Resource CreateWeaponByName(string name)
//        {
//            switch (name)
//            {
//                case "돌":
//                    return new Resource(id: 1001, name: "돌", price: 1, grade: Grade.Common);
//                case "나무":
//                    return new Resource(id: 1002, name: "나무", price: 2, grade: Grade.Common);
//                case "금광석":
//                    return new Resource(id: 1003, name: "금광석", price: 5, grade: Grade.Rare);
//                case "양털":
//                    return new Resource(id: 1005, name: "양털", price: 8, grade: Grade.Rare);
//                case "a":
//                    return new Resource(id: 1006, name: "a", price: 20, grade: Grade.Legendary);
//                default:
//                    throw new ArgumentException("잘못된 무기 ID 입니다!");
//            }
//        }
//    }
//}