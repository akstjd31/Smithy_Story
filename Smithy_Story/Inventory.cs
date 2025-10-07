using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Inventory
    {
        // 변수
        private List<IItem> items = new List<IItem>();

        // 메소드

        // 해당 무기를 만들 수 있는가?
        public bool CanCraftWeapon(Weapon weapon)
        {
            if (items == null)
            {
                Console.WriteLine("[오류] 인벤토리의 items가 초기화되지 않았습니다.");
                return false;
            }

            // 무기 제작에 필요한 재료(Resource), 수량(int) 비교
            foreach (var reqResource in weapon.RequiredResources)
            {
                Resource requiredResource = reqResource.Key;
                int requiredAmount = reqResource.Value;

                // 제일 먼저 찾은거로 비교 (대소문자 무시)
                var haveItem = items.FirstOrDefault(i =>
                    i != null && i.Name.Equals(requiredResource.Name, StringComparison.OrdinalIgnoreCase));

                if (haveItem == null)
                {
                    Console.WriteLine($"[부족] {requiredResource.Name} 이(가) 인벤토리에 없습니다.");
                    return false;
                }

                if (haveItem.Quantity < requiredAmount)
                {
                    Console.WriteLine($"[부족] {requiredResource.Name} 부족 ({haveItem.Quantity}/{requiredAmount})");
                    return false;
                }

                Console.WriteLine($"[확인] {requiredResource.Name} OK ({haveItem.Quantity}/{requiredAmount})");
            }

            return true;
        }


        // 아이템 추가
        public void AddItem(IItem item, int quantity = 1)
        {
            if (quantity != 1)
                item.Quantity = quantity;

            // 개수를 늘릴 수 있는가?
            if (item.IsStackable)
            {
                // FirstOrDefault: 리스트에 존재하지 않아도 예외 발생하지 않음. default 반환
                // 즉, 같은 이름을 가진
                var existItem = items.FirstOrDefault(i => i.Name.Equals(item.Name) && i.IsStackable);

                // 아이템 존재하면 개수 늘리기
                if (existItem != null)
                {
                    existItem.Quantity += item.Quantity;
                    return;
                }
            }

            // 무기는 새롭게 추가하면 됨.
            items.Add(item);
        }

        // 아이템 삭제
        public void RemoveItemById(int id, int quantity = 1)
        {
            List<IItem> existItems = GetItemById(id);
            // 아이템이 존재하지 않음.
            if (existItems == null)
            {
                Console.WriteLine("해당 아이템은 대장간에 존재하지 않습니다!");
                return;
            }

            // 재료인 경우
            if (existItems[0].IsStackable)
            {
                var existItem = items.FirstOrDefault(i => i.Name.Equals(existItems[0].Name) && i.IsStackable);

                if (existItem != null)
                {
                    bool isExist = (existItem.Quantity - quantity) > 0;

                    // 재료 소모했는데 아직 1개 이상 있는 경우
                    if (isExist)
                    {
                        existItem.Quantity -= quantity;
                        return; // 애초에 재료는 공간 한 칸을 차지해서 existItems의 길이가 1일 수 밖에 없음.

                    }

                    items.Remove(existItem);
                    return;
                }

                // 강화 횟수를 비교하여 삭제
                // 코드 추가 필요!!!
            }
        }

        // 아이템 검색 (ID 기반) => 재료면 상관없지만, 중복되는 무기가 존재할 가능성이 있어서 리스트 형태 반환
        public List<IItem> GetItemById(int id)
        {
            return items.Where(i => i.ID == id).ToList();
        }

        // 아이템 검색 (문자열 기반) => 위와 똑같음.

        public List<IItem> GetItemsByName(string name)
        {
            return items.Where(i => i.Name.Contains(name)).ToList();
        }

        public void ShowInventory()
        {
            if (items.Count < 1)
            {
                Console.WriteLine("대장간에는 아무것도 없습니다!");
            }

            Console.WriteLine("- 무기/재료 목록");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine(items[i].ToString());
            }
            Console.WriteLine("=============================================");
        }
    }
}
