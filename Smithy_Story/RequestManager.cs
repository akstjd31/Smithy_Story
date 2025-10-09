using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 의뢰 종류
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

        // 일일 의뢰 생성
        public void GenerateDailyRequests()
        {
            // 의뢰 리스트 초기화
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

                // 미리 계산해 둔 보상 적용
                clone.SetReward(CalculateReward(baseReq));

                // 강화 수치 적용
                clone.SetNeededCount(GetEnhanceLevel(clone.Name));


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

        // 타이틀에서 강화 수치 가져오기
        private int GetEnhanceLevel(string text)
        {
            // 숫자(\d+)만 추출
            var matches = Regex.Matches(text, @"\d+");

            if (matches.Count > 0)
            {
                // 여러 숫자가 있으면 첫 번째만 반환
                return int.Parse(matches[0].Value);
            }

            return 1; // 숫자 없을 경우 기본값
        }

        // 의뢰 수락
        public bool AcceptRequest(int index, Player player, Inventory inventory)
        {
            if (index < 0 || index >= dailyRequests.Count)
            {
                Console.WriteLine("잘못된 의뢰 인덱스입니다.");
                return false;
            }

            if (player.ArchiveRequests.Count >= MaxRequestCount)
            {
                Console.Clear();
                Console.WriteLine($"보유 의뢰 수는 최대 {MaxRequestCount}개까지 가능합니다.");
                Thread.Sleep(2000);
                return false;
            }

            var request = dailyRequests[index];

            player.AddRequest(request);
            dailyRequests.RemoveAt(index);

            RequestType type = request.Type;

            // 강화/수리 의뢰일 경우만 맡기는 아이템이 존재하므로
            if (request.Item != null && (type.Equals(RequestType.EnhanceWeapon) || type.Equals(RequestType.RepairWeapon)))
            {
                (request.Item as Weapon).IsItemDeposited = true;
                inventory.AddItem(request.Item);
            }

            Console.Clear();
            Console.WriteLine($"[{request.Name}] 의뢰를 수락했습니다!");
            Thread.Sleep(2000);
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

            var items = inventory.GetItemById(request.Item.ID);

            // 의뢰 종류에 따른 완료 시점
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
                        {
                            request.IsCompleted = (item as Weapon)?.Durability == Weapon.MaxDurability;

                            // 완료 처리 후 인벤토리에서 제거
                            if (request.IsCompleted)
                                inventory.RemoveItem(item);
                        }
                            
                    }

                    break;
                case RequestType.EnhanceWeapon:
                    // 인벤토리에 존재하는 같은 아이템 중
                    var weapons = items.OfType<Weapon>().ToList();
                    foreach (var weapon in weapons)
                    {
                        // 의뢰자가 원하는 강화 수치 == 인벤에 존재하는 강화 수치가 똑같은 아이템 => 성공
                        request.IsCompleted = (weapon.EnhanceLevel == request.NeededCount);

                        if (request.IsCompleted)
                            inventory.RemoveItem(weapon);
                    }

                    break;
            }

            // 완료 처리 (완료된 보유 의뢰 제거)
            if (request.IsCompleted)
            {
                Console.Clear();
                player.Money += request.Reward;
                Console.WriteLine($"[{request.Name}] 의뢰 완료! (보상: {request.Reward})");
                player.ArchiveRequests.Remove(request);
                Thread.Sleep(1000);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("해당 의뢰는 완료할 수 없습니다!");
                Thread.Sleep(1000);
            }
        }


        // 만료된 의뢰 제거
        public void CheckExpiredRequests(List<Request> requests, GameTime gameTime, Inventory inventory)
        {
            // 만료된 의뢰 리스트 인덱스
            var iStack = new Stack<int>();

             for (int i = 0; i < requests.Count; i++)
             {
                if (requests[i].IsExpired())
                {
                    iStack.Push(i);
                }
             }

            while (iStack.Count > 0)
            {
                int i = iStack.Pop();
                Console.WriteLine($"[{requests[i].Name}] 의뢰가 만료되었습니다.");

                // 강화나 수리 의뢰였다면 인벤토리에서 해당 아이템 제거
                if (requests[i].Type.Equals(RequestType.EnhanceWeapon) || requests[i].Type.Equals(RequestType.RepairWeapon))
                {
                    foreach (Weapon weapon in inventory.GetWeapon())
                    {
                        // ID와 빌려준 아이템(IsItemDeposited)인지 확인 후 제거
                        if (requests[i].Item.ID == weapon.ID)
                        {
                            if (weapon.IsItemDeposited)
                                inventory.RemoveItem(weapon);
                        }
                    }
                }

                // 뒤에서부터 제거 (리스트 구조 생각해서)
                requests.RemoveAt(i);
                Thread.Sleep(2000);
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
