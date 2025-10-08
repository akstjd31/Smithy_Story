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
            // Common 1001 ~
            manager.AddItem(new Resource(1001, "돌",   2, Grade.Common));
            manager.AddItem(new Resource(1002, "나무", 4, Grade.Common));
            manager.AddItem(new Resource(1003, "가죽", 6, Grade.Common));
            manager.AddItem(new Resource(1004, "천",   8, Grade.Common));
            manager.AddItem(new Resource(1005, "구리", 10, Grade.Common));

            // Rare 2001 ~
            manager.AddItem(new Resource(2001, "실",          15, Grade.Rare));
            manager.AddItem(new Resource(2002, "철",          20, Grade.Rare));
            manager.AddItem(new Resource(2003, "은광석",      25, Grade.Rare));
            manager.AddItem(new Resource(2004, "금광석",      30, Grade.Rare));
            manager.AddItem(new Resource(2005, "불의 정수",   35, Grade.Rare));
            manager.AddItem(new Resource(2006, "얼음의 정수", 40, Grade.Rare));

            // Epic 3001 ~
            manager.AddItem(new Resource(3001, "수정",     50,  Grade.Epic));
            manager.AddItem(new Resource(3002, "흑요석",   60,  Grade.Epic));
            manager.AddItem(new Resource(3003, "루비",     75,  Grade.Epic));
            manager.AddItem(new Resource(3004, "사파이어", 80,  Grade.Epic));
            manager.AddItem(new Resource(3005, "에메랄드", 100, Grade.Epic));

            // Legendary 4001 ~
            manager.AddItem(new Resource(4001, "다이아몬드", 150, Grade.Legendary));
            manager.AddItem(new Resource(4002, "용의 비늘",  200, Grade.Legendary));
            manager.AddItem(new Resource(4003, "마력의 핵",  220, Grade.Legendary));
            manager.AddItem(new Resource(4004, "고대 금속",  250, Grade.Legendary));
        }


        // 메소드
        public static Resource GetById(int id) => manager.GetById(id);
        public static Resource GetByName(string name) => manager.GetByName(name);
        public static IEnumerable<Resource> GetAll() => manager.GetAll();
    }
}
