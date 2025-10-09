using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Smithy_Story
{
    // 상점 클래스
    public class Shop
    {
        // 상수
        const int MaxDisplayItemCount = 5;  // 상점 진열 최대 갯수
        const int MaxQuantity = 8;          // 재료 최대 갯수
        const int MinQuantity = 2;          // 재료 최소 갯수
        // 변수
        private IItem[] stock;              // 구매 시 해당 위치는 빈칸으로 남아있는게 나아서 리스트말고 배열로 선언함.
        private Random rand;

        // 등급별 확률 가중치
        Dictionary<Grade, int> gradeWeights = new Dictionary<Grade, int>
    {
        { Grade.Common, 60 },
        { Grade.Rare, 27 },
        { Grade.Epic, 10 },
        { Grade.Legendary, 3 } 
    };

        // 생성자
        public Shop()
        {
            stock = new IItem[MaxDisplayItemCount];
            rand = new Random();
        }

        // 길이 반환
        public int GetStockLength() => stock.Length;

        // 상점 새로고침
        public void RefreshStock()
        {
            // 배열 초기화
            Array.Clear(stock, 0, stock.Length);

            var resources = ResourceData.GetAll().ToList();

            // 등급별로 나누기
            var groupedResources = resources.GroupBy(r => r.Grade)
                                            .ToDictionary(g => g.Key, g => g.ToList());

            // 상점에 표시할 아이템 목록
            List<Resource> selected = new List<Resource>();

            for (int i = 0; i < MaxDisplayItemCount; i++)
            {
                Grade selectedGrade = GetRandomGrade(gradeWeights);

                // 해당 등급에 포함된 재료들
                if (groupedResources.ContainsKey(selectedGrade))
                {
                    var list = groupedResources[selectedGrade];
                    var item = list[rand.Next(list.Count)];

                    // 복사본 생성 및 수량 뽑기
                    Resource newItem = new Resource(item.ID, item.Name, item.Price, item.Grade);
                    newItem.Quantity = rand.Next(MinQuantity, MaxQuantity+1);   // 2 ~ 8

                    selected.Add(newItem);
                }
            }
            
            // 실제 진열대에 옮기기
            for (int i = 0; i < selected.Count; i++)
                stock[i] = selected[i];
        }


        // 등급 뽑기
        private Grade GetRandomGrade(Dictionary<Grade, int> weights)
        {
            // 확률 (1 ~ 100)
            int roll = rand.Next(1, 101);

            int cumulative = 0;
            foreach (var kvp in weights)
            {
                cumulative += kvp.Value;
                if (roll <= cumulative)
                    return kvp.Key;
            }

            return Grade.Common;
        }
        
        // 출력
        public void ShowStock()
        {
            Console.Clear();
            Console.WriteLine("===================== 상점 =====================");
            for (int i = 0; i < stock.Length; i++)
            {
                var item = stock[i];

                if (item != null)
                    Console.WriteLine($"{i + 1}. {item.ToString()}");
                else
                    Console.WriteLine($"{i + 1}. 물품 준비 중");
            }
            Console.WriteLine("================================================");
        }

        // 인덱스 체크
        public bool IsExistItemIdx(int idx) => stock[idx] != null;

        // 수량 체크(이미 인덱스 체크는 한 상태)
        public bool IsExistItemQuantity(int idx, int quantity) => quantity > 0 && (quantity <= stock[idx].Quantity) ? true : false;

        // 구매
        public bool Buy(int num, int quantity, Player player, Inventory inventory)
        {
            int idx = num - 1;

            if (stock[idx] == null)
            {
                Console.Clear();
                Console.WriteLine($"해당 칸에 물품이 없습니다.");
                Thread.Sleep(1000);
                return false;
            }

            // 예외
            if (quantity < 1 || quantity > stock[idx].Quantity)
            {
                Console.Clear();
                Console.WriteLine($"구매하려는 [{stock[idx].Name}]의 수량이 부족하거나 1 이상의 숫자를 입력하세요.");
                Thread.Sleep(1000);
                return false;
            }

            // 플레이어의 보유 금액이 부족할 때
            int totalPrice = stock[idx].Price * quantity;
            if (player.Money >= totalPrice)
            {
                var item = (Resource)stock[idx].Clone();
                item.Quantity = quantity;
                inventory.AddItem(item);

                // 플레이어 소비, 상점 개수 변동
                player.Money -= totalPrice;
                stock[idx].Quantity -= quantity;

                Console.WriteLine($"{stock[idx].Name}을(를) {quantity}만큼 구매했습니다!");

                // 해당 물품의 수량이 없다면 null 처리
                if (stock[idx].Quantity <= 0)
                    stock[idx] = null;

                return true;
            }
            else
            {
                Console.WriteLine(player.Name + "님의 골드가 부족합니다!");
                return false;
            }
        }
    }
}
