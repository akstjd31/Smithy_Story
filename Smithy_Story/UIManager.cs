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

        // 의뢰 목록 출력
        public void UpdateRequestUI()
        {
            requestUI.Update();
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

    }
}
