using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Shop
    {
        // 상수
        const int MaxDisplayItemCount = 5;  // 상점 진열 최대 갯수
        // 변수
        private IItem[] stock;              // 구매 시 해당 위치는 빈칸으로 남아있는게 나아서 리스트말고 배열로 선언함.
        private Random rand;

        // 생성자
        public Shop()
        {
            stock = new IItem[MaxDisplayItemCount];
            rand = new Random();
        }

        // 길이 반환
        public int GetStockLength() => stock.Length;

        public void RefreshStock()
        {
            // 배열 초기화
            Array.Clear(stock, 0, stock.Length);

            // 전체 재료 목록 가져오기
            var resources = ResourceData.GetAll().ToList();

            // 랜덤으로 5개(MaxDisplayItemCount) 선택
            var randomSelection = resources
                .OrderBy(x => rand.Next())
                .Take(MaxDisplayItemCount)
                .ToList();

            // 배열에 하나씩 채워 넣기
            for (int i = 0; i < randomSelection.Count; i++)
            {
                stock[i] = randomSelection[i];
            }
        }


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

        // 구매 할 번호 - 1 == 인덱스, 수량, 플레이어
        public void Buy(int num, int quantity, Player player, Inventory inventory)
        {
            int idx = num - 1;

            var item = stock[idx];

            // 수량 입력이 잘못 들어왔을 경우
            if (quantity < 1 || quantity > item.Quantity)
            {
                Console.WriteLine($"구매하려는 [{stock[idx].Name}]의 수량이 부족하거나 1 이상의 숫자를 입력해주세요.");
                return;
            }

            // 구매하려는 플레이어의 보유 금액이 부족할 때
            int totalPrice = item.Price * quantity;
            if (player.Money >= totalPrice)
            {
                // 인벤에 추가
                inventory.AddItem(item, quantity);

                // 플레이어 소비, 상점 개수 변동
                player.Money -= totalPrice;

                Console.WriteLine($"{item.Name}을(를) {quantity}만큼 구매했습니다!");

                // 해당 물품의 수량이 없다면 null 처리
                if (stock[idx].Quantity - quantity <= 0)
                    stock[idx] = null;
            }
            else
            {
                Console.WriteLine(player.Name + "님의 골드가 부족합니다!");
            }
        }
    }
}
