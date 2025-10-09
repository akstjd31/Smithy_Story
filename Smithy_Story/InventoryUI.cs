using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 인벤토리 UI
    public class InventoryUI
    {
        // 변수
        private Inventory inventory;

        // 생성자
        public InventoryUI(Inventory inventory)
        {
            this.inventory = inventory;
        }

        // 메소드
        // 출력문 작성
        public void Update()
        {
            if (inventory != null)
                inventory.ShowInventory();
        }
    }
}
