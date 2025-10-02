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

        // 재료 추가 필요!!!!!!!!!!!!! (1001번부터 시작)
        private static Dictionary<int, Resource> resourcesById = new Dictionary<int, Resource>()
        {
            { 1001, new Resource(id:1001, name:"돌",         price:1,  grade:Grade.Common) },
            { 1002, new Resource(id:1002, name:"나무",       price:2,  grade:Grade.Common) },
            { 1003, new Resource(id:1003, name:"금광석",     price:5,  grade:Grade.Rare) },
            { 1004, new Resource(id:1004, name:"다이아몬드", price:10, grade:Grade.Epic) }    
        };

        private static Dictionary<string, int> nameToId = new Dictionary<string, int>()
        {
            { "돌",              1001 },
            { "나무",            1002 },
            { "금광석",          1003 },
            { "다이아몬드",      1004 }
        };

        // 프로퍼티
        // 생성자
        // 메소드

        // id로 재료 찾기
        public static Resource GetResourceById(int id)
        {
            if (resourcesById.ContainsKey(id))
            {
                var res = resourcesById[id];
                return res;
            }
            throw new ArgumentException($"{id}에 해당되는 ID가 ResourceData에 존재하지 않습니다.");
        }

        // name으로 재료 찾기
        public static Resource GetResourceByName(string name)
        {
            if (nameToId.ContainsKey(name))
                return GetResourceById(nameToId[name]);

            throw new ArgumentException($"재료 {name}은(는) ResourceData에 존재하지 않습니다.");
        }
    }
}
