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
        // static 메모리 할당 순서 해결 = Lazy
        private static readonly Lazy<DataManager<Request>> lazyManager = new Lazy<DataManager<Request>>(() =>
        {
            var manager = new DataManager<Request>();

            // 자원 납품 의뢰
            manager.AddItem(new Request(
                id: 101,
                name: "돌 10개를 납품해주세요!",
                deadlineDay: 1,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("돌")
            ));

            manager.AddItem(new Request(
                id: 102,
                name: "나무 8개를 가져다주세요!",
                deadlineDay: 1,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("나무")
            ));

            manager.AddItem(new Request(
                id: 103,
                name: "철 5개를 납품해주세요!",
                deadlineDay: 1,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("철")
            ));

            manager.AddItem(new Request(
                id: 104,
                name: "가죽 4개가 필요합니다!",
                deadlineDay: 1,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("가죽")
            ));

            manager.AddItem(new Request(
                id: 105,
                name: "금광석 3개를 가져다주세요!",
                deadlineDay: 3,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("금광석")
            ));

            manager.AddItem(new Request(
                id: 106,
                name: "다이아몬드 1개를 납품해주세요!",
                deadlineDay: 5,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("다이아몬드")
            ));

            manager.AddItem(new Request(
                id: 107,
                name: "루비 2개를 구해오세요!",
                deadlineDay: 3,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("루비")
            ));

            manager.AddItem(new Request(
                id: 108,
                name: "은광석 5개를 납품해주세요!",
                deadlineDay: 2,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("은광석")
            ));

            manager.AddItem(new Request(
                id: 109,
                name: "나무 30개 대량 납품 요청!",
                deadlineDay: 4,
                requestType: RequestType.DeliverItem,
                item: ResourceData.GetByName("나무")
            ));

            // 무기 제작 의뢰
            manager.AddItem(new Request(
                id: 201,
                name: "곡괭이를 만들어주세요!",
                deadlineDay: 1,
                requestType: RequestType.CraftWeapon,
                item: WeaponData.GetByName("곡괭이")
            ));

            manager.AddItem(new Request(
                id: 202,
                name: "목검을 만들어주세요!",
                deadlineDay: 1,
                requestType: RequestType.CraftWeapon,
                item: WeaponData.GetByName("목검")
            ));

            manager.AddItem(new Request(
                id: 203,
                name: "가죽활을 제작해주세요!",
                deadlineDay: 2,
                requestType: RequestType.CraftWeapon,
                item: WeaponData.GetByName("가죽활")
            ));

            manager.AddItem(new Request(
                id: 204,
                name: "단검을 만들어주세요!",
                deadlineDay: 2,
                requestType: RequestType.CraftWeapon,
                item: WeaponData.GetByName("단검")
            ));

            manager.AddItem(new Request(
                id: 205,
                name: "철검을 제작해주세요!",
                deadlineDay: 3,
                requestType: RequestType.CraftWeapon,
                item: WeaponData.GetByName("철검")
            ));

            manager.AddItem(new Request(
                id: 206,
                name: "흑요석검을 제작해주세요!",
                deadlineDay: 5,
                requestType: RequestType.CraftWeapon,
                item: WeaponData.GetByName("흑요석검")
            ));

            // 무기 수리/강화 의뢰
            manager.AddItem(new Request(
                id: 301,
                name: "닳은 목검을 수리해주세요.",
                deadlineDay: 1,
                requestType: RequestType.RepairWeapon,
                item: WeaponData.GetByName("목검")
            ));

            manager.AddItem(new Request(
                id: 302,
                name: "부서진 철검을 수리해주세요.",
                deadlineDay: 1,
                requestType: RequestType.RepairWeapon,
                item: WeaponData.GetByName("철검")
            ));

            manager.AddItem(new Request(
                id: 303,
                name: "철제도끼를 +3까지 강화해주세요!",
                deadlineDay: 2,
                requestType: RequestType.EnhanceWeapon,
                item: WeaponData.GetByName("철제도끼")
            ));

            manager.AddItem(new Request(
                id: 304,
                name: "곡괭이를 +7로 강화해보고 싶어요.",
                deadlineDay: 5,
                requestType: RequestType.EnhanceWeapon,
                item: WeaponData.GetByName("곡괭이")
            ));

            manager.AddItem(new Request(
                id: 305,
                name: "용린검을 +5로 강화해주세요!",
                deadlineDay: 7,
                requestType: RequestType.EnhanceWeapon,
                item: WeaponData.GetByName("용린검")
            ));

            return manager;
        });


        private static DataManager<Request> Manager => lazyManager.Value;


        //static RequestData()
        //{
        //    // 재료 배달 의뢰
        //    manager.AddItem(new Request(101, "돌 10개를 납품해주세요!", 1, RequestType.DeliverItem, neededCount: 10, item: ResourceData.GetByName("돌")));
        //    manager.AddItem(new Request(102, "나무 8개를 가져다주세요!", 1, RequestType.DeliverItem, neededCount: 8, item: ResourceData.GetByName("나무")));
        //    manager.AddItem(new Request(103, "철 5개를 납품해주세요!", 1, RequestType.DeliverItem, neededCount: 5, item: ResourceData.GetByName("철")));
        //    manager.AddItem(new Request(104, "가죽 4개가 필요합니다!", 1, RequestType.DeliverItem, neededCount: 4, item: ResourceData.GetByName("가죽")));
        //    manager.AddItem(new Request(105, "금광석 3개를 가져다주세요!", 3, RequestType.DeliverItem, neededCount: 3, item: ResourceData.GetByName("금광석")));
        //    manager.AddItem(new Request(106, "다이아몬드 1개를 납품해주세요!", 5, RequestType.DeliverItem, neededCount: 1, item: ResourceData.GetByName("다이아몬드")));
        //    manager.AddItem(new Request(107, "루비 2개를 구해오세요!", 3, RequestType.DeliverItem, neededCount: 2, item: ResourceData.GetByName("루비")));
        //    manager.AddItem(new Request(108, "은광석 5개를 납품해주세요!", 2, RequestType.DeliverItem, neededCount: 5, item: ResourceData.GetByName("은광석")));
        //    manager.AddItem(new Request(109, "나무 30개 대량 납품 요청!", 4, RequestType.DeliverItem, neededCount: 30, item: ResourceData.GetByName("나무")));

        //    // 무기 제작 의뢰 
        //    manager.AddItem(new Request(201, "곡괭이를 만들어주세요!", 1, RequestType.CraftWeapon, item: WeaponData.GetByName("곡괭이")));
        //    manager.AddItem(new Request(202, "목검을 만들어주세요!", 1, RequestType.CraftWeapon, item: WeaponData.GetByName("목검")));
        //    manager.AddItem(new Request(203, "가죽활을 제작해주세요!", 2, RequestType.CraftWeapon, item: WeaponData.GetByName("가죽활")));
        //    manager.AddItem(new Request(204, "단검을 만들어주세요!", 2, RequestType.CraftWeapon, item: WeaponData.GetByName("단검")));
        //    manager.AddItem(new Request(205, "철검을 제작해주세요!", 3, RequestType.CraftWeapon, item: WeaponData.GetByName("철검")));
        //    manager.AddItem(new Request(206, "흑요석검을 제작해주세요!", 5, RequestType.CraftWeapon, item: WeaponData.GetByName("흑요석검")));

        //    // 무기 수리/강화 의뢰 == 물건을 맡김.
        //    manager.AddItem(new Request(301, "닳은 목검을 수리해주세요.", 1, RequestType.RepairWeapon, item: WeaponData.GetByName("목검")));
        //    manager.AddItem(new Request(302, "부서진 철검을 수리해주세요.", 1, RequestType.RepairWeapon, item: WeaponData.GetByName("철검")));
        //    manager.AddItem(new Request(303, "철제도끼를 +3까지 강화해주세요!", 2, RequestType.EnhanceWeapon, item: WeaponData.GetByName("철제도끼"), neededCount: 3));
        //    manager.AddItem(new Request(304, "곡괭이를 +7로 강화해보고 싶어요.", 5, RequestType.EnhanceWeapon, item: WeaponData.GetByName("곡괭이"), neededCount: 7));
        //    manager.AddItem(new Request(305, "용린검을 +5로 강화해주세요!", 7, RequestType.EnhanceWeapon, item: WeaponData.GetByName("용린검"), neededCount: 5));

        //    //manager.AddItem(new Request(401, "[급함] 돌 20개를 3분 안에 납품!", 200, 180, RequestType.DeliverItem, neededCount: 20));
        //    //manager.AddItem(new Request(402, "[긴급] 나무활을 5분 내 제작해주세요!", 300, 240, RequestType.CraftWeapon));
        //    //manager.AddItem(new Request(403, "[특수] 루비 3개를 빠르게 납품!", 400, 270, RequestType.DeliverItem, neededCount: 3));
        //    //manager.AddItem(new Request(404, "[대량 주문] 철 20개와 가죽 10개를 납품!", 500, 360, RequestType.DeliverItem, neededCount: 30));
        //}

        // 메소드
        public static Request GetById(int id) => Manager.GetById(id);
        public static Request GetByName(string name) => Manager.GetByName(name);
        public static IEnumerable<Request> GetAll() => Manager.GetAll();
    }
}
