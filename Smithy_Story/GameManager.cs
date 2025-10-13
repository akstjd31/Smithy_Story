using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Smithy_Story
{
    
    // 화면 FSM
    public enum GameScreen
    {
        MainMenu,           // 메인 화면
        Explanation,        // 게임 설명
        ForgeMenu,          // 장비 제작/강화/수리 선택 화면
        Craft,              // 제작
        Enhance,            // 강화
        Repair,             // 수리
        DailyRequestMenu,   // 일일 의뢰 목록
        ArchiveRequestMenu, // 수락한 의뢰 목록
        WeaponMenu,         // 무기 만드는 법(정보)
        Inventory,          // 인벤토리(작업장)
        Shop,               // 재료 상점
        InGame,             // 인게임
        Exit                // 종료
    }

    public class GameManager
    {
        private Player player;
        private Inventory inventory;
        private GameTime gameTime;
        private RequestManager requestManager;
        private Shop shop;
        private UIManager uiManager;
        private GameScreen currentScreen;

        // 생성자(초기화 작업)
        public GameManager(string playerName)
        {
            player = new Player(playerName, money: 1000);
            inventory = new Inventory();
            gameTime = new GameTime();
            requestManager = new RequestManager();
            shop = new Shop();
            uiManager = new UIManager(player, inventory, gameTime, requestManager, shop);

            currentScreen = GameScreen.MainMenu;

            Settings(); // 초기 세팅
        }

        // 게임 구동시키기
        public void Run()
        {
            while (currentScreen != GameScreen.Exit)
            {

                // 각 화면(GameScreen)마다 해당 기능 수행
                switch (currentScreen)
                {
                    case GameScreen.MainMenu:
                        currentScreen = ShowMainMenu();
                        break;
                    case GameScreen.InGame:
                        currentScreen = ShowInGameMenu();
                        break;
                    case GameScreen.ForgeMenu:
                        currentScreen = ForgeMenu();
                        break;
                    case GameScreen.Craft:
                        currentScreen = CraftWeapon();
                        break;
                    case GameScreen.Enhance:
                        currentScreen = EnhanceWeapon();
                        break;
                    case GameScreen.Repair:
                        currentScreen = RepairWeapon();
                        break;
                    case GameScreen.Inventory:
                        currentScreen = ShowInventory();
                        break;
                    case GameScreen.Shop:
                        currentScreen = ShowShop();
                        break;
                    case GameScreen.DailyRequestMenu:
                        currentScreen = ShowDailyRequests();
                        break;
                    case GameScreen.ArchiveRequestMenu:
                        currentScreen = ShowAcceptedRequests();
                        break;
                    case GameScreen.WeaponMenu:
                        currentScreen = ShowWeaponManual();
                        break;
                    case GameScreen.Explanation:
                        currentScreen = ShowExplanation();
                        break;
                }
            }
        }

        // 메인 메뉴
        private GameScreen ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==========================");
                Console.WriteLine($"{player.Name}의 대장장이 이야기");
                Console.WriteLine("==========================");
                Console.WriteLine("1. 게임 시작");
                Console.WriteLine("2. 게임 설명");
                Console.WriteLine("3. 게임 종료");
                Console.Write("입력: ");

                // 메인 화면에서 선택
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return GameScreen.InGame;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return GameScreen.Explanation;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return GameScreen.Exit;
                    default:
                        Console.Clear();
                        Console.WriteLine("\n잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        // 인 게임
        private GameScreen ShowInGameMenu()
        {
            while (true)
            {
                Console.Clear();
                uiManager.UpdateTimeUI();
                uiManager.UpdatePlayerUI();
                Console.WriteLine("================================================================");

                Console.WriteLine("1. 장비 제작/강화/수리");
                Console.WriteLine("2. 오늘의 의뢰 확인");
                Console.WriteLine("3. 수락한 의뢰 목록 확인");
                Console.WriteLine("4. 인벤토리 보기");
                Console.WriteLine("5. 오늘의 상점 이용");
                Console.WriteLine("6. 무기 레시피 보기\n");
                Console.Write("입력: ");
                var key = Console.ReadKey(true).Key;

                // 인 게임 내 선택
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return GameScreen.ForgeMenu;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return GameScreen.DailyRequestMenu;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return GameScreen.ArchiveRequestMenu;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return GameScreen.Inventory;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        return GameScreen.Shop;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return GameScreen.WeaponMenu;
                    default:
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        // 특정 시간, 피로도 소요가 있는 작업 수행 시 끝 부분에 실행할 코드(풀 피로도인가? / 24시가 넘었는가?)
        private bool Sleep()
        {
            if (player.Fatigue >= Player.MaxFatigue || gameTime.Hour >= 24)
            {
                Console.Clear();
                Console.WriteLine("피로가 누적되었거나 하루가 끝났습니다. 강제로 휴식합니다...");
                Thread.Sleep(1000);

                Settings();

                Console.WriteLine("휴식을 취했습니다. 하루가 시작됩니다.");
                Thread.Sleep(1000);
                return true; // 하루가 끝났음을 알림
            }

            return false;
        }

        // 초기 설정
        private void Settings()
        {
            player.ResetFatigue();                      // 피로도 0
            gameTime.AddDays();                         // 하루 증가
            gameTime.SetTime(8, 0);                    // 시간 초기화
            shop.RefreshStock();                        // 상점 목록 리셋
            requestManager.GenerateDailyRequests();     // 일일 의뢰 추가
            player.DecreaseRequestDeadline();           // 보유 중인 의뢰 남은 일자 감소

            // 만료된 의뢰가 있는지 확인 후 정리
            requestManager.CheckExpiredRequests(player.ArchiveRequests, gameTime, inventory);
            currentScreen = GameScreen.InGame;
        }

        // 장비 제작/강화/수리 메뉴
        private GameScreen ForgeMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. 장비 제작");
                Console.WriteLine("2. 장비 강화");
                Console.WriteLine("3. 장비 수리");
                Console.WriteLine("Any. 뒤로가기");

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return GameScreen.Craft;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return GameScreen.Enhance;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return GameScreen.Repair;
                    default:
                        return GameScreen.InGame;
                }
            }
        }

        // 무기 제작
        private GameScreen CraftWeapon()
        {
            // 대장간 생성
            Craft craft = new Craft(inventory);     

            while (true)
            {
                Console.Clear();
                var craftable = craft.GetCraftableWeapon();

                // 제작 가능한 무기가 없음.
                if (!craftable.Any())
                {
                    Console.Clear();
                    Console.WriteLine("제작 가능한 무기가 없습니다.");
                    Thread.Sleep(1000);
                    return GameScreen.ForgeMenu;
                }

                Console.WriteLine("제작 가능한 무기 목록");
                for (int i = 0; i < craftable.Count; i++)
                    Console.WriteLine($"{i + 1}. {craftable[i].Name}");
                Console.Write("선택 (뒤로가기 0): ");

                var input = Console.ReadKey();
                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.ForgeMenu;

                char keyChar = input.KeyChar;
                if (char.IsDigit(keyChar))
                {
                    int num = int.Parse(keyChar.ToString());

                    if (num > 0 && num <= craftable.Count)
                    {

                        bool tryCraft = craft.CraftWeapon(craftable[num - 1]);

                        // 장비를 만들 수 있는 상태(주재료가 다 있음) - 피로도, 시간 소요
                        if (tryCraft)
                        {
                            gameTime.AddMinutes(120);
                            player.IncreaseFatigue(10);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(1000);
                }

                if (Sleep())
                    return GameScreen.InGame;

            }
        }

        // 무기 강화하기
        private GameScreen EnhanceWeapon()
        {
            EnhanceManager enhance = new EnhanceManager();
            while (true)
            {
                Console.Clear();
                var weapons = inventory.GetEnhancableWeapon();

                // 인벤토리에 강화할게 아예 없는 경우 (있어도 최대 15강인 무기거나, 무기가 없음)
                if (!weapons.Any())
                {
                    Console.Clear();
                    Console.WriteLine("강화 가능한 장비가 없습니다.");
                    Thread.Sleep(1000);
                    return GameScreen.ForgeMenu;
                }

                Console.WriteLine("강화 가능한 장비 목록");
                for (int i = 0; i < weapons.Count; i++)
                    Console.WriteLine($"{i + 1}. {weapons[i].Name} 현재 {weapons[i].EnhanceLevel}강");

                Console.Write("선택 (뒤로가기 0): ");
                var input = Console.ReadKey(true);
                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.InGame;

                char keyChar = input.KeyChar;
                if (char.IsDigit(keyChar))
                {
                    int num = int.Parse(keyChar.ToString());

                    if (num > 0 && num <= weapons.Count)
                    {
                        bool tryEnhance = enhance.Enhance(inventory, weapons[num - 1]);

                        // 강화 시도
                        if (tryEnhance)
                        {
                            gameTime.AddMinutes(60);
                            player.IncreaseFatigue(8);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("숫자를 입력해주세요.");
                    Thread.Sleep(1000);
                }

                if (Sleep())
                    return GameScreen.InGame; // 강제 종료 후 인게임으로 복귀
            }
        }

        // 인벤토리 보기
        public GameScreen ShowInventory()
        {
            bool open = true;

            while (open)
            {
                Console.Clear();
                uiManager.UpdateInventoryUI();
                Console.Write("입력(뒤로 가기 0): ");
                var input = Console.ReadKey(true);

                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.InGame;
            }

            return GameScreen.InGame;
        }

        // 무기 수리하기
        private GameScreen RepairWeapon()
        {
            Repair repair = new Repair(inventory);

            while (true)
            {
                var weaponsToRepair = repair.RepairWeapons(inventory);

                // 수리가 필요한 목록(내구도가 100 미만)
                if (!weaponsToRepair.Any())
                {
                    Console.Clear();
                    Console.WriteLine("수리 가능한 장비가 없습니다.");
                    Thread.Sleep(1000);
                    return GameScreen.ForgeMenu;
                }

                Console.Clear();
                Console.WriteLine("수리 가능한 장비 목록");
                for (int i = 0; i < weaponsToRepair.Count; i++)
                    Console.WriteLine($"{i + 1}. {weaponsToRepair[i].Name} 현 내구도 ({weaponsToRepair[i].Durability}/{Weapon.MaxDurability})");

                Console.Write("선택 (뒤로가기 0): ");
                var input = Console.ReadKey();

                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.ForgeMenu;

                char keyChar = input.KeyChar;
                if (char.IsDigit(keyChar))
                {
                    int num = int.Parse(keyChar.ToString());

                    if (num > 0 && num <= weaponsToRepair.Count)
                    {
                        // 수리 비용을 지불할 수 있는가?
                        int cost = repair.CalculateRepairCost(weaponsToRepair[num - 1]);
                        if (player.Money >= cost)
                        {
                            weaponsToRepair[num - 1].Repair();
                            player.Money -= cost;
                            gameTime.AddMinutes(30);
                            Console.Clear();
                            Console.WriteLine($"{weaponsToRepair[num - 1]} 수리 완료! (수리 비용: {cost})");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("돈이 부족합니다.");
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("숫자를 입력해주세요.");
                    Thread.Sleep(1000);
                }

                if (Sleep())
                    return GameScreen.InGame;
            }
        }

        // 상점
        private GameScreen ShowShop()
        {
            while (true)
            {
                // 상점 열기
                shop.ShowStock();

                Console.Write("구매할 아이템 번호 (뒤로가기 0): ");
                var input = Console.ReadKey(true);
                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.InGame;

                char keyChar = input.KeyChar;
                if (char.IsDigit(keyChar))
                {
                    int num = int.Parse(keyChar.ToString());

                    if (num > 0 && num <= shop.GetStockLength())
                    {
                        Console.Write("\n수량: ");
                        var qInput = Console.ReadKey(true);
                        char qKeyChar = qInput.KeyChar;

                        if (char.IsDigit(qKeyChar))
                        {
                            int quantity = int.Parse(qKeyChar.ToString());

                            if (quantity > 0)
                            {
                                bool tryBuy = shop.Buy(num, quantity, player, inventory);

                                // 구매 시도
                                if (tryBuy)
                                {
                                    gameTime.AddMinutes(15);
                                    player.IncreaseFatigue(2);
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\n수량을 잘못 입력하셨습니다.");
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\n숫자를 입력해주세요.");
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n해당 물품번호는 존재하지 않습니다.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n숫자를 입력해주세요.");
                    Thread.Sleep(1000);
                }

                if (Sleep())
                    return GameScreen.InGame; // 강제 종료 후 인게임으로 복귀
            }
        }

        // 일일 의뢰
        private GameScreen ShowDailyRequests()
        {
            var requests = requestManager.GetDailyRequests();

            while (true)
            {
                Console.Clear();
                uiManager.UpdateDailyRequestUI();

                Console.Write("수락할 의뢰 번호 (뒤로가기 0): ");

                var input = Console.ReadKey(true);

                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.InGame;

                char keyChar = input.KeyChar;
                if (char.IsDigit(keyChar))
                {
                    int num = int.Parse(keyChar.ToString());

                    if (num > 0 && num <= requests.Count)
                    {
                        bool isAccept = requestManager.AcceptRequest(num - 1, player, inventory);

                        if (isAccept)
                        {
                            gameTime.AddMinutes(120);
                            player.IncreaseFatigue(5);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n잘못된 입력입니다. 다시 시도해주세요.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("\n숫자를 입력해주세요.");
                    Thread.Sleep(1000);
                }

                if (Sleep())
                    return GameScreen.InGame; // 강제 종료 후 인게임으로 복귀
            }
        }


        // 플레이어 보유 의뢰(수락한 의뢰)
        private GameScreen ShowAcceptedRequests()
        {
            while (true)
            {
                Console.Clear();
                player.ShowActiveReqeusts();
                
                // 보유 의뢰가 없음
                if (!player.ArchiveRequests.Any())
                {
                    Console.WriteLine("현재 진행 중인 의뢰가 없습니다!");
                    Thread.Sleep(1000);
                    return GameScreen.InGame;
                }

                Console.WriteLine("1. 의뢰 포기");
                Console.WriteLine("2. 의뢰 완료");
                Console.Write("입력(뒤로 가기 0): ");

                // 입력 (의뢰 포기/완료 고르기)
                var input1 = Console.ReadKey(true);

                if (input1.Key.Equals(ConsoleKey.D0) || input1.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.InGame;

                char keyChar1 = input1.KeyChar;
                if (char.IsDigit(keyChar1))
                {
                    int selected = int.Parse(keyChar1.ToString());

                    // 의뢰 포기 선택
                    if (selected == 1)
                    {
                        Console.Write("\n몇 번째 의뢰를 포기하시겠습니까?: ");

                        // 입력 (포기할 의뢰 번호 정하기)
                        var input2 = Console.ReadKey(true);
                        char keyChar2 = input2.KeyChar;

                        if (char.IsDigit(keyChar2))
                        {
                            int discardNum = int.Parse(keyChar2.ToString());

                            if (discardNum > 0 && discardNum <= player.ArchiveRequests.Count)
                            {
                                Console.Write("\n정말로 포기하시겠습니까?(Y / N:Any): ");

                                // 입력 (해당 의뢰 포기가 맞는지 확인)
                                var input3 = Console.ReadKey(true);

                                // 의뢰 포기
                                if (input3.Key.Equals(ConsoleKey.Y))
                                {
                                    int id = player.ArchiveRequests[discardNum - 1].Item.ID;
                                    string reqName = player.ArchiveRequests[discardNum - 1].Name;
                                    RequestType type = player.ArchiveRequests[discardNum - 1].Type;

                                    // 맡겨진 물건이 존재하는 강화/수리 의뢰는 해당 의뢰 제거와 함께 인벤토리에 있는 아이템도 제거시켜야 한다.
                                    if (type.Equals(RequestType.RepairWeapon) || type.Equals(RequestType.EnhanceWeapon))
                                        inventory.RemoveDepositedItemById(id);

                                    player.ArchiveRequests.RemoveAt(discardNum - 1);
                                    Console.WriteLine($"\n[{reqName}] 의뢰를 포기하셨습니다!");
                                    Thread.Sleep(2000);
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("\n잘못된 입력입니다. 다시 시도해주세요.");
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\n숫자를 입력해주세요.");
                            Thread.Sleep(1000);
                        }
                    }

                    // 의뢰 완료 선택
                    else if (selected == 2)
                    {
                        Console.WriteLine("\n몇 번째 의뢰를 완료 처리 하시겠습니까?");

                        // 입력 (완료할 의뢰 번호)
                        var input4 = Console.ReadKey(true);
                        char keyChar4 = input4.KeyChar;
                        if (char.IsDigit(keyChar4))
                        {
                            int completeNum = int.Parse(keyChar4.ToString());
                            if (completeNum > 0 && completeNum <= player.ArchiveRequests.Count)
                            {

                                // 완료 처리하기
                                requestManager.CompleteRequest(player.ArchiveRequests[completeNum - 1], player, inventory);
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\n잘못된 입력입니다. 다시 시도해주세요.");
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n잘못된 입력입니다. 다시 시도해주세요.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("숫자를 입력해주세요.");
                    Thread.Sleep(1000);
                }
            }
        }

        // 무기 전체 리스트 보기 (제작에 필요한 재료 확인용)
        private GameScreen ShowWeaponManual()
        {
            while (true)
            {
                Console.Clear();

                // 모든 무기 데이터 정보 확인하기
                foreach (var weapon in WeaponData.GetAll())
                {
                    Console.WriteLine($"[{weapon.Name}]");

                    // 해당 무기에 필요한 재료들 출력
                    foreach (var item in weapon.RequiredResources)
                        Console.WriteLine($"- {item.Key.Name} : {item.Value}개");
                    Console.WriteLine();
                }
                Console.Write("입력(뒤로가기 0): ");
                var input = Console.ReadKey(true);
                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.InGame;
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 시도해주세요.");
                    Thread.Sleep(1000);
                }
            }
        }

        // 게임 설명
        private GameScreen ShowExplanation()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("게임 설명...");
                Console.Write("뒤로가기 0: ");

                var input = Console.ReadKey(true);
                if (input.Key.Equals(ConsoleKey.D0) || input.Key.Equals(ConsoleKey.NumPad0))
                    return GameScreen.MainMenu;
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 시도해주세요.");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
