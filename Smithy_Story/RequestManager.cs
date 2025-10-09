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
        EnhanceWeapon,  // 무기 강화
        RepairWeapon,   // 무기 수리
        DeliverItem,    // 재료 전달
        End
    }

    public class RequestManager
    {
        // 상수
        private const int MaxRequestCount = 3;    // 하루 최대 수락 가능 수
        private const int DailyRequestCount = 7;  // 하루 표시되는 의뢰 개수

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

                var clone = (Request)baseReq.Clone();
                
                clone.Reward = CalculateReward(baseReq);  // 미리 계산해 둔 보상 적용

                // 수리 의뢰면 내구도 조정
                if (clone.Type == RequestType.RepairWeapon)
                {
                    var durability = rand.Next(0, 100);
                    (clone.Item as Weapon).SetDurability(durability);
                }

                dailyRequests.Add(clone);
            }

            Console.WriteLine($"오늘의 의뢰 {dailyRequests.Count}개가 생성되었습니다!");
        }

        // 의뢰 수락
        public bool AcceptRequest(int index, Player player, Inventory inventory)
        {
            if (index < 0 || index >= dailyRequests.Count)
            {
                Console.WriteLine("잘못된 의뢰 인덱스입니다.");
                return false;
            }

            if (player.GetRequestCount() >= MaxRequestCount)
            {
                Console.Clear();
                Console.WriteLine($"의뢰 수락 불가! 하루 최대 {MaxRequestCount}개까지 가능합니다.");
                Thread.Sleep(2000);
                return false;
            }

            var request = (Request)dailyRequests[index].Clone();          // 아래 코드가 실행되도록 아이템을 집어넣기

            player.AddRequest(request);
            dailyRequests.RemoveAt(index);

            RequestType type = request.Type;
            // 강화/수리 의뢰일 경우만 플레이어한테 맡기므로
            if (request.Item != null && (type.Equals(RequestType.EnhanceWeapon) || type.Equals(RequestType.RepairWeapon)))
            {
                (request.Item as Weapon).IsItemDeposited = true;    // 맡긴 물건 표시
                inventory.AddItem(request.Item);
            }


            return true;
        }

        // 의뢰 완료 처리
        public void CompleteRequest(Request request, Player player, Inventory inventory)
        {
            if (!player.ArchiveRequests.Contains(request))
            {
                Console.WriteLine("해당 아이템이 존재하지 않습니다!");
                Thread.Sleep(1000);
                return;
            }

            var items = inventory.GetItemById(request.Item.ID);  // 같은 ID를 가진 인벤토리 아이템

            switch (request.Type)
            {
                case RequestType.DeliverItem:
                    if (request.Item != null && inventory.HasEnoughItem(request.Item, request.NeededCount))
                    {
                        inventory.ConsumeItem(request.Item, request.NeededCount);
                        request.IsCompleted = true;
                    }
                    break;

                // 제작: 해당 아이템 존재 여부 판단
                case RequestType.CraftWeapon:
                    request.IsCompleted = items.Count > 0;
                    break;
                // 수리: 걍 해당 아이템 내구도 100 판단
                case RequestType.RepairWeapon:
                    foreach (var item in items)
                    {
                        // 무기 중에 내구도 100 찾으면 완료 처리
                        if (item is Weapon)
                            request.IsCompleted = (item as Weapon)?.Durability == Weapon.MaxDurability;
                    }

                    break;
                case RequestType.EnhanceWeapon:
                    // 인벤토리에 존재하는 같은 아이템 중
                    foreach (var item in items)
                    {
                        // 무기 중에 같은 강화 수치 무기 존재 == 완료
                        if (item is Weapon)
                            request.IsCompleted = (item as Weapon)?.EnhanceLevel == (request.Item as Weapon)?.EnhanceLevel;
                    }

                    break;
            }

            if (request.IsCompleted)
            {
                player.ArchiveRequests.Remove(request);
                Console.WriteLine($"[{request.Name}] 의뢰 완료!");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("해당 의뢰는 완료할 수 없습니다!");
                Thread.Sleep(1000);
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
        private int CalculateReward(Request request)
        {
            RequestType type = request.Type;
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

            if (request.Item != null)
            {
                switch (request.Item.Grade)
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
            }

            return (int)(baseReward * gradeMultiplier * request.NeededCount);
        }
    }
}
