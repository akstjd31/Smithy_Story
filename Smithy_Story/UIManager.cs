using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // UI 관리자 클래스 (모든 UI 관리) => 근데 잘 사용하고 있는거 같지 않음
    public class UIManager
    {
        // 상수
        // 변수
        private Player player;
        private Inventory inventory;
        private GameTime gameTime;
        private RequestManager requestManager;
        private Shop shop;

        // UI 클래스
        private PlayerUI playerUI;
        private TimeUI timeUI;
        private RequestUI dailyRequestUI;
        private InventoryUI inventoryUI;
        private ShopUI shopUI;

        // 생성자
        public UIManager(Player player, Inventory inventory, GameTime gameTime, RequestManager requestManager, Shop shop)
        {
            this.player = player;
            this.inventory = inventory;
            this.gameTime = gameTime;
            this.requestManager = requestManager;
            this.shop = shop;

            playerUI = new PlayerUI(player);
            timeUI = new TimeUI(gameTime);
            inventoryUI = new InventoryUI(inventory);
            dailyRequestUI = new RequestUI(requestManager);
            shopUI = new ShopUI(shop);
        }

        // 의뢰 목록 출력
        public void UpdateDailyRequestUI()
        {
            dailyRequestUI.Update();
        }

        // 인벤토리 출력

        public void UpdateInventoryUI()
        {
            inventoryUI.Update();
        }

        // 플레이어 정보(이름, 피로도) 출력

        public void UpdatePlayerUI()
        {
            playerUI.Update();
        }

        // 게임 시간 출력

        public void UpdateTimeUI()
        {
            timeUI.Update();
        }

        //// 상점 UI 출력
        //public void UpdateShopUI()
        //{
        //    shopUI.Update();
        //}
    }
}
