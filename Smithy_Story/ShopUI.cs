using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 상점 UI 클래스
    public class ShopUI
    {
        // 변수
        private Shop shop;

        // 생성자
        public ShopUI(Shop shop)
        {
            this.shop = shop;
        }

        // 메소드
        // 출력
        public void Update()
        {
            if (shop != null)
                shop.ShowStock();
        }
    }
}
