using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Inventory
    {
        // 변수
        private List<IItem> items = new List<IItem>();

        // 메소드

        // 의뢰자한테 맡겨진 물건을 지우기 (사실 이 코드가 유효하지 않을 때가 있음.. 확률은 매우 낮지만)
        public void RemoveDepositedItemById(int id)
        {
            for (int i = 0; i < items.Count; i++)
            { 
                if (items[i].ID == id && items[i] is Weapon && (items[i] as Weapon).IsItemDeposited)
                {
                    items.RemoveAt(i);
                    break;
                }
            }   
        }

        // 인벤토리에 있는 무기 반환
        public List<Weapon> GetWeapon() => items.OfType<Weapon>().ToList();

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

                // 필요한 재료가 없거나 수량이 부족한 경우
                if (haveItem == null || haveItem.Quantity < requiredAmount)
                    return false;
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
        
        // 아이템 삭제(item)
        public void RemoveItem(IItem item)
        {
            // 무기타입인 경우
            if (item is Weapon)
            {
                foreach (Weapon i in items.OfType<Weapon>())
                {
                    // 강화수치 비교 후 삭제
                    if (i.EnhanceLevel == (item as Weapon).EnhanceLevel)
                    {
                        items.Remove(i);
                        return;
                    }
                }
            }

            items.Remove(item);
        }

        // 아이템 삭제(ID)
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
        public List<IItem> GetItemById(int id) => items.Where(i => i.ID == id).ToList();

        // 아이템 검색 (문자열 기반) => 위와 똑같음.

        public List<IItem> GetItemsByName(string name) => items.Where(i => i.Name.Contains(name)).ToList();

        // 아이템 수량이 충분한지?
        public bool HasEnoughItem(IItem item, int neededCount)
        {
            var exist = items.FirstOrDefault(i => i.ID == item.ID && i.IsStackable);
            return exist != null && exist.Quantity >= neededCount;
        }

        // 아이템 차감
        public bool ConsumeItem(IItem item, int neededCount)
        {
            if (!HasEnoughItem(item, neededCount))
                return false;

            RemoveItemById(item.ID, neededCount);
            return true;
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
