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

    // 단순히 의뢰 보상 계산, 데이터 생성만 함.
    public class RequestManager
    {
        // 상수
        const int MaxRequestCount = 3;  // 플레이어가 지닐 수 있는 의뢰 최대 개수

        // 변수
        private int nextId = 101;
        private List<Request> dailyRequests = new List<Request>();
        private Random rand = new Random();

        // 프로퍼티
        // 생성자
        // 메소드

        // 의뢰 수락(인덱스, 플레이어)
        public void AcceptRequest(int idx, Player player)
        {
            if (dailyRequests[idx] == null)
                return;

            // 보유 의뢰 개수를 초과하지 못함.
            if (player.ArchiveRequests.Count >= MaxRequestCount)
            {
                Console.WriteLine($"하루 받을 수 있는 최대 의뢰의 수는 {MaxRequestCount}개 입니다!");
                return;
            }

            var request = (Request)dailyRequests[idx].Clone();
            player.AddRequest(request);
            Console.WriteLine(request.Title + " 새로운 의뢰 수락!");
            dailyRequests.RemoveAt(idx);
        }

        // 의뢰 완료
        public void CompleteReqeust(Request request, Player player)
        {
            if (player.ArchiveRequests.Contains(request))
            {
                player.ArchiveRequests.Remove(request);
                Console.WriteLine(request.Title + " 의뢰 완료!");
            }
        }

        // 일일 의뢰 목록 생성하기
        public void GenerateDailyRequests(int count)
        {
            dailyRequests.Clear();  // 의뢰 리스트를 비우고 새로 만듦.

            for (int i = 0; i < count; i++)
            {
                Request newRequest = CreateRandomRequest();
                dailyRequests.Add(newRequest);
            }
        }

        // 의뢰 목록 전부 보기

        public List<Request> GetDailyRequests()
        {
            return dailyRequests;
        }

        // 만료된 의뢰가 있는지?
        public void CheckExpiredRequests(List<Request> requests, GameTime gameTime)
        {
            foreach (var req in requests)
            {
                if (req.IsExpired(gameTime.Day))
                {
                    Console.WriteLine($"[만료] 의뢰 '{req.Title}' 의 데드라인이 지났습니다! 해당 의뢰를 제거합니다.");
                    requests.Remove(req);
                }
            }
        }

        // 등급별 보상 계산
        private int CalculateReward(IItem item, RequestType type)
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
        private string CreateTitle(IItem item, RequestType type)
        {
            switch (type)
            {
                case RequestType.CraftWeapon:
                    return "[제작 의뢰]:" + item.Name;
                case RequestType.RepairWeapon:
                    return "[수리 의뢰]:" + item.Name;
                case RequestType.DeliverItem:
                    return "[배달 의뢰]:" + item.Name;
                default:
                    return item.Name;
            }
        }

        // 의뢰 종류 하나 뽑기
        public int RandomRequestType() => rand.Next((int) RequestType.Start + 1, (int) RequestType.End);
        
        // 만들어진 모든 데이터를 갖다가 의뢰 하나를 생성하기.
        private Request CreateRandomRequest()
        {
            // 무기, 재료 모두 합치기
            var items = new List<IItem>();
            items.AddRange(ResourceData.GetAll().Cast<IItem>());
            items.AddRange(WeaponData.GetAll().Cast<IItem>());

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

            return new Request(nextId++, title, item, reward, deadline, type);
        }
    }
}
