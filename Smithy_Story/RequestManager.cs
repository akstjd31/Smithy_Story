using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

    public class RequestManager
    {
        // 상수
        private const int MaxRequestCount = 3;    // 하루 최대 수락 가능 수
        private const int DailyRequestCount = 7; // 하루 표시되는 의뢰 개수

        // 변수
        private List<Request> dailyRequests = new List<Request>();
        private Random rand = new Random();

        // 플레이어가 받을 수 있는 의뢰 목록 반환
        public List<Request> GetDailyRequests()
        {
            return dailyRequests;
        }

        // 하루 의뢰 생성
        public void GenerateDailyRequests()
        {
            dailyRequests.Clear();

            var allRequests = RequestData.GetAll().ToList();
            if (allRequests.Count == 0)
            {
                Console.WriteLine("의뢰 데이터가 존재하지 않습니다.");
                return;
            }

            // 무작위로 N개 선택
            for (int i = 0; i < DailyRequestCount; i++)
            {
                var index = rand.Next(allRequests.Count);
                var baseReq = allRequests[index];

                var clone = (Request)baseReq.Clone();                                // ID 붙여주기
                dailyRequests.Add(clone);
            }

            Console.WriteLine($"오늘의 의뢰 {dailyRequests.Count}개가 생성되었습니다!");
        }

        // 의뢰 수락
        public void AcceptRequest(int index, Player player)
        {
            if (index < 0 || index >= dailyRequests.Count)
            {
                Console.WriteLine("잘못된 의뢰 인덱스입니다.");
                return;
            }

            if (player.ArchiveRequests.Count >= MaxRequestCount)
            {
                Console.WriteLine($"의뢰 수락 불가! 하루 최대 {MaxRequestCount}개까지 가능합니다.");
                return;
            }

            var request = (Request)dailyRequests[index].Clone();
            player.AddRequest(request);
            dailyRequests.RemoveAt(index);

            Console.WriteLine($"[의뢰 수락] {request.Name}");
        }

        // 의뢰 완료 처리
        public void CompleteRequest(Request request, Player player)
        {
            if (player.ArchiveRequests.Remove(request))
            {
                Console.WriteLine($"[의뢰 완료] {request.Name} — 보상: {request.Reward}G");
            }
            else
            {
                Console.WriteLine("해당 의뢰는 플레이어의 목록에 없습니다.");
            }
        }

        // 만료된 의뢰 제거
        public void CheckExpiredRequests(List<Request> requests, GameTime gameTime)
        {
            var expired = new List<Request>();

            foreach (var req in requests)
            {
                if (req.IsExpired(gameTime.Day))
                {
                    expired.Add(req);
                }
            }

            foreach (var req in expired)
            {
                requests.Remove(req);
                Console.WriteLine($"[만료] {req.Name} — 데드라인이 지났습니다.");
            }
        }

        // 보상 계산 (아이템 등급과 수량에 따라 가중치 부여)
        private int CalculateReward(IItem item, RequestType type, int neededCount)
        {
            int baseReward = 50;

            switch (type)
            {
                case RequestType.CraftWeapon:
                    baseReward = 100;
                    break;
                case RequestType.DeliverItem:
                    baseReward = 30;
                    break;
                default:
                    baseReward = 50;
                    break;
            }

            double gradeMultiplier = 1.0;

            switch (item.Grade)
            {
                case Grade.Common:
                    gradeMultiplier = 1.0;
                    break;
                case Grade.Rare:
                    gradeMultiplier = 1.5;
                    break;
                case Grade.Epic:
                    gradeMultiplier = 2.5;
                    break;
                case Grade.Legendary:
                    gradeMultiplier = 5.0;
                    break;
                default:
                    gradeMultiplier = 1.0;
                    break;
            }

            return (int)(baseReward * gradeMultiplier * neededCount);
        }
    }
}
