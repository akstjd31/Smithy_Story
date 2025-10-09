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
        public bool Enhance(Inventory inventory, Weapon weapon)
        {
            // 최대 수치(15강) 도달
            if (weapon.EnhanceLevel == MaxEnhanceLevel)
            {
                Console.Clear();
                Console.WriteLine($"{weapon.Name} 은(는) 이미 최대 레벨에 도달했습니다!");
                Thread.Sleep(2000);
                return false;
            }
                

            Random rand = new Random();
            double randDouble = rand.NextDouble(); // 0.0 ~ 1.0

            Enhance enhance = EnhanceData.GetData(weapon.EnhanceLevel + 1);

            Console.WriteLine("\n강화 중...");
            Thread.Sleep(1000);

            // 확률 데이터 빼놓기
            double success = enhance.SuccessRate;
            double fail = enhance.FailRate;
            double destroy = enhance.DestroyRate;

            // 누적 확률 계산
            if (randDouble <= success)
            {
                weapon.LevelUp();
                Console.WriteLine($"{weapon.Name} 강화에 성공했습니다! (현재 {weapon.EnhanceLevel}강)");
            }
            else if (success < randDouble && randDouble <= success + fail)
            {
                Console.WriteLine($"{weapon.Name} 강화에 실패했습니다. (현재 {weapon.EnhanceLevel}강)");
            }
            else
            {
                Console.WriteLine($"{weapon.Name}은(는) 파괴되었습니다.");
                inventory.RemoveItem(weapon);
            }
            Thread.Sleep(1000);

            return true;
        }

    }
}
