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
        private List<IItem> stock;
        private Random rand;

        // 생성자
        public Shop()
        {
            stock = new List<IItem>();
            rand = new Random();
        }

        public void RefreshStock()
        {
            stock.Clear();

            var resources = ResourceData.GetAll().ToList();

            var randomSelection = resources.OrderBy(x => rand.Next()).Take(MaxDisplayItemCount).ToList();

            stock.AddRange(randomSelection);
        }

        public void ShowStock()
        {
            Console.WriteLine("===================== 상점 =====================");
            for (int i = 0; i < stock.Count; i++)
            {
                var item = stock[i];
                Console.WriteLine($"{i + 1}. {item.ToString()}");
            }
            Console.WriteLine("================================================");
        }

        // 인덱스 체크
        public bool IsExistItemIdx(int idx) => idx >= 0 && idx < stock.Count ? true : false;

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
                inventory.AddItem(item, quantity);
                player.Money -= totalPrice;

                Console.WriteLine($"{item.Name}을(를) {quantity}만큼 구매했습니다!");
                stock.RemoveAt(idx);
            }
            else
            {
                Console.WriteLine(player.Name + "님의 골드가 부족합니다!");
            }
        }
    }
}
