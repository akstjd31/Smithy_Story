using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 장비 제작 클래스
    public class Craft
    {
        // 상수
        // 변수
        private Inventory inventory = new Inventory();

        // 프로퍼티
        // 생성자
        // 사용자의 인벤 받아오기
        public Craft(Inventory inventory)
        {
            this.inventory = inventory;
        }

        // 메소드
        // 만들 수 있는 무기 리스트 반환
        public List<Weapon> GetCraftableWeapon()
        {
            if (inventory == null)
                return null;

            List<Weapon> craftableWeapons = new List<Weapon>();
            foreach (var weapon in WeaponData.GetAll())
            {
                // 만들 수 있는 무기인가?
                if (inventory.CanCraftWeapon(weapon))
                    craftableWeapons.Add(weapon);
            }
            return craftableWeapons;
        }

        public bool CraftWeapon(Weapon weapon)
        {
            if (weapon == null)
                return false;

            foreach (var reqResource in weapon.RequiredResources)
            {
                Resource resource = reqResource.Key;
                int requiredAmount = reqResource.Value;

                var haveItem = inventory.GetItemsByName(resource.Name).FirstOrDefault();

                // 재료 개수 줄이기 or 삭제
                if (haveItem != null)
                {
                    haveItem.Quantity -= requiredAmount;
                    if (haveItem.Quantity < 1)
                        inventory.RemoveItem(haveItem);
                }
            }

            Console.WriteLine($"{weapon.Name} 제작 중...");
            Thread.Sleep(1000);

            inventory.AddItem(weapon);
            Console.WriteLine($"{weapon.Name} 제작 완료!");
            Thread.Sleep(2000);

            return true;
        }
    }
}
