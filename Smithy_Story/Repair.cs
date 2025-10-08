using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{

    // 장비 수리 클래스
    public class Repair
    {
        // 변수
        private Inventory inventory = new Inventory();
        // 생성자
        public Repair(Inventory inventory)
        {
            this.inventory = inventory;
        }

        // 수리 해야되는 무기 리스트 반환
        public List<Weapon> RepairWeapons(Inventory inventory)
        {
            if (inventory == null)
                return null;

            List<Weapon> weaponsToRepair = new List<Weapon>();

            // 내구도가 100 이하인 것들만 담음.
            foreach (Weapon weapon in inventory.GetWeapon())
            {
                if (weapon.Durability < Weapon.MaxDurability)
                    weaponsToRepair.Add(weapon);
            }

            return weaponsToRepair;
        }

        // GPT가 짜준 수리 비용 계산
        public int CalculateRepairCost(Weapon weapon)
        {
            double gradeMultiplier = 1.0;
            if (weapon.Grade == Grade.Rare) gradeMultiplier = 1.3;
            else if (weapon.Grade == Grade.Epic) gradeMultiplier = 1.7;
            else if (weapon.Grade == Grade.Legendary) gradeMultiplier = 2.5;

            // 2. 강화 수치에 따른 추가 가치 (기하급수 대신 선형 증가)
            // 강화 수치 1당 15% 증가
            double enhanceMultiplier = 1 + (0.15 * weapon.EnhanceLevel);

            // 3. 전체 무기 가치 계산
            double totalValue = weapon.BaseValue() * enhanceMultiplier * gradeMultiplier;

            // 4. 내구도 손상 비율 계산 (0~1 사이)
            double durabilityRatio = (double)(100 - weapon.Durability) / 100;

            // 5. 최종 수리비 = 전체 가치 × 손상 비율
            int repairCost = (int)(weapon.BaseValue() * durabilityRatio);

            //// (선택) 수리비 상한
            //if (repairCost > 50000)
            //    repairCost = 50000;

            return repairCost;
        }
    }
}
