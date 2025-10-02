using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public enum RequestType
    {
        Start,
        CraftWeapon,    // 무기 제작
        RepairWeapon,   // 무기 수리
        DeliverItem,    // 재료 전달
        End
    }

    public static class RequestManager
    {
        // 상수
        // 변수
        private static int nextId = 101;
        private static Random rand = new Random();

        // 프로퍼티
        // 생성자
        // 메소드

        // 등급별 보상 계산
        private static int CalculateReward(IItem item, RequestType type)
        {
            int baseReward = 50;     // 의뢰 종류에 따른 기본 보상
            switch (type)
            {
                case RequestType.CraftWeapon: baseReward = 100; break;
                //case RequestType.RepairWeapon: baseReward = 50; break;
                case RequestType.DeliverItem: baseReward = 30; break;
            }

            double multiplier;  // 등급 가중치
            switch (item.Grade)
            {
                case Grade.Common: multiplier = 1.0; break;
                case Grade.Rare: multiplier = 1.5; break;
                case Grade.Epic: multiplier = 2.5; break;
                case Grade.Legendary: multiplier = 5.0; break;
                default: multiplier = 1.0; break;
            }

            // 기본 보상 * 등급 * 아이템 개수
            return (int)(baseReward * multiplier * item.Quantity);
        }

        // 타이틀 생성
        private static string CreateTitle(IItem item, RequestType type)
        {
            switch (type)
            {
                case RequestType.CraftWeapon:
                    return "[제작 의뢰]\t" + item.Name;
                case RequestType.RepairWeapon:
                    return "[수리 의뢰]\t" + item.Name;
                case RequestType.DeliverItem:
                    return "[배달 의뢰]\t" + item.Name;
                default:
                    return item.Name;
            }
        }

        // 의뢰 종류 하나 뽑기
        public static int RandomRequestType() => rand.Next((int) RequestType.Start + 1, (int) RequestType.End);
        
        // 만들어진 모든 데이터를 갖다가 의뢰 하나를 생성하기.
        public static Request CreateItemRequest()
        {
            // 무기, 재료 모두 합치기
            var items = new List<IItem>();
            items.AddRange(WeaponData.GetAll().Cast<IItem>());
            items.AddRange(ResourceData.GetAll().Cast<IItem>());

            if (items.Count == 0)
            {
                Console.WriteLine("저장된 데이터가 없습니다!");
                return null;
            }

            // 랜덤 아이템 선택
            IItem item = items[rand.Next(items.Count)];

            // 의뢰 종류 랜덤으로 선택
            RequestType type = (RequestType)RandomRequestType();


            int reward = CalculateReward(item, type);
            string title = CreateTitle(item, type);
            int deadline = 3;   // 수정 필요

            return new Request(nextId++, title, reward, deadline, type);
        }
    }
}
