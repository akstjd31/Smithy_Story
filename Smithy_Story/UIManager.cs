using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class UIManager
    {
        // 상수
        // 변수
        private Player player;
        private Inventory inventory;
        private GameTime gameTime;
        private RequestManager requestManager;

        // UI 클래스
        private PlayerUI playerUI;
        private TimeUI timeUI;
        private RequestUI requestUI;
        private InventoryUI inventoryUI;

        // 생성자
        public UIManager(Player player, Inventory inventory, GameTime gameTime, RequestManager requestManager)
        {
            this.player = player;
            this.inventory = inventory;
            this.gameTime = gameTime;
            this.requestManager = requestManager;

            playerUI = new PlayerUI(player);
            timeUI = new TimeUI(gameTime);
            inventoryUI = new InventoryUI(inventory);
            requestUI = new RequestUI(requestManager);
        }

        // 전체 출력
        public void UpdateAll()
        {
            playerUI.Update();
            timeUI.Update();
            inventoryUI.Update();
            requestUI.Update();
        }
    }
}
