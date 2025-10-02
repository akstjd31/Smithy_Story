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
        private Dictionary<int, IItem> items = new Dictionary<int, IItem>();

        // 메소드
        // 아이템 추가
        public void AddItem(IItem item)
        {
            if (items.ContainsKey(item.ID))
            {
                
            }
        }
    }
}
