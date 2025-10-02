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

    public class RequestManager
    {
        // 상수
        const int MaxRequestCount = 3;      // 최대 받을 수 있는 의뢰는 3개

        // 변수
        private int nextId = 101;
        private Random rand = new Random();

        private List<Request> requests = new List<Request>();
        //private int todayRequestCount = 0;  // 하루 받은 의뢰의 총 개수

        // 프로퍼티
        // 생성자
        // 메소드

        // 만료된 의뢰가 있는지?
        public void CheckExpiredRequests(GameTime gameTime)
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

        // 현재 수락한 의뢰 목록 확인하기
        public List<Request> GetAllRequests() => requests;

        // 의뢰를 더 받을 수 있는지? + 사용자 답변
        public void TryAddRequest(Request request, char answer)
        {
            if (requests.Count >= MaxRequestCount)
            {
                Console.WriteLine("오늘은 더 이상 의뢰를 받을 수 없습니다!");
                return;
            }

            // 사용자의 답변에 따른 의뢰 수락/거절
            if (answer.Equals('Y'))
            {
                request.Status = RequestStatus.Accepted;
                requests.Add(request);
            }
            else
            {
                request.Status = RequestStatus.Failed;
            }
        }

        // 의뢰 완료 처리 (보상을 받을 플레이어, 완료된 의뢰)
        public void CompleteRequest(Player player, Request request)
        {
            if (!requests.Contains(request))
            {
                Console.WriteLine("해당 의뢰는 목록에 존재하지 않습니다!");
                return;
            }

            // 보상 지급
            player.Money += CalculateReward(request.Item, request.Type);
            requests.Remove(request);
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
        public int RandomRequestType() => rand.Next((int) RequestType.Start + 1, (int) RequestType.End);
        
        // 만들어진 모든 데이터를 갖다가 의뢰 하나를 생성하기.
        public Request CreateItemRequest()
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

            return new Request(nextId++, title, item, reward, deadline, type);
        }
    }
}
