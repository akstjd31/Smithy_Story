using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class RequestData
    {

        // 변수
        private static DataManager<Request> manager = new DataManager<Request>();

        // 생성자
        static RequestData()
        {
            manager.AddItem(new Request(id: 101, name: "돌을(를) 갖다주세요!", 30, 3, RequestType.DeliverItem, neededCount: 5));
            manager.AddItem(new Request(id: 102, name: "나무을(를) 갖다주세요!", 30, 3, RequestType.DeliverItem, neededCount: 10));
            manager.AddItem(new Request(id: 103, name: "청동석을(를) 갖다주세요!", 30, 3, RequestType.DeliverItem, neededCount: 3));
            manager.AddItem(new Request(id: 104, name: "금을(를) 갖다주세요!", 30, 3, RequestType.DeliverItem));
        }

        // 메소드
        public static Request GetById(int id) => manager.GetById(id);
        public static Request GetByName(string name) => manager.GetByName(name);
        public static IEnumerable<Request> GetAll() => manager.GetAll();
    }
}
