using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class EnhanceManager
    {
        // 상수
        const int MaxEnhanceLevel = 15;

        // 메소드
        // 무기 강화
        public void Enhance(Inventory inventory, Weapon weapon)
        {
            // 레벨 최고치에 이미 달성한 경우
            if (weapon.EnhanceLevel == MaxEnhanceLevel)
                return;

            Random rand = new Random();
            double randDouble = rand.NextDouble();  // 0.0 ~ 1.0

            Enhance enhance = EnhanceData.GetData(weapon.EnhanceLevel + 1);

            Console.WriteLine("강화 중...");
            Thread.Sleep(500);

            // 성공
            if (enhance.SuccessRate >= randDouble)
            {
                Console.WriteLine($"{weapon.Name} 강화에 성공했습니다! (현재 {weapon.EnhanceLevel}강)");
                weapon.LevelUp();
                Thread.Sleep(200);
            }

            // 실패
            else if (enhance.DestroyRate >= randDouble)
            {
                Console.WriteLine($"{weapon.Name} 강화에 실패했습니다. (현재 {weapon.EnhanceLevel}강)");
                Thread.Sleep(200);
            }

            else
            {
                Console.WriteLine($"{weapon.Name}은(는) 파괴되었습니다.");
                inventory.RemoveItem(weapon);
                Thread.Sleep(200);
            }
        }
    }
}
