using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Smithy_Story
{// 화면 FSM
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
                        Console.WriteLine("\n잘못된 입력입니다.");
                        Thread.Sleep(800);
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
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(800);
                        break;
                }
            }
        }

        // 자야됨.
        private bool Sleep()
        {
            if (player.Fatigue >= Player.MaxFatigue || gameTime.Hour >= 24)
            {
                Console.Clear();
                Console.WriteLine("\n피로가 누적되었거나 하루가 끝났습니다. 강제로 휴식합니다...");
                Thread.Sleep(1000);

                Settings(); // 하루 리셋

                Console.WriteLine("휴식을 취했습니다. 하루가 시작됩니다. (08:00)");
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
            gameTime.SetTime(23, 0);                    // 시간 초기화 (테스트)
            shop.RefreshStock();                        // 상점 목록 리셋
            requestManager.GenerateDailyRequests(7);    // 일일 의뢰 추가

            // 만료된 의뢰가 있는지 확인 후 정리
            requestManager.CheckExpiredRequests(player.ArchiveRequests, gameTime);
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

        // 무기 만들기
        private GameScreen CraftWeapon()
        {
            Craft craft = new Craft(inventory);

            while (true)
            {
                Console.Clear();
                var craftable = craft.GetCraftableWeapon();

                if (!craftable.Any())
                {
                    Console.WriteLine("제작 가능한 무기가 없습니다.");
                    Thread.Sleep(1000);
                    return GameScreen.ForgeMenu;
                }

                Console.WriteLine("제작 가능한 무기 목록:");
                for (int i = 0; i < craftable.Count; i++)
                    Console.WriteLine($"{i + 1}. {craftable[i].Name}");
                Console.Write("선택 (뒤로가기 0): ");

                string input = Console.ReadLine();
                if (input == "0") return GameScreen.ForgeMenu;

                if (int.TryParse(input, out int num) && num > 0 && num <= craftable.Count)
                {
                    craft.CraftWeapon(craftable[num - 1]);
                    gameTime.AddMinutes(120);
                    player.IncreaseFatigue(10);
                    Sleep();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(800);
                }
            }
        }
        
        // 무기 강화하기
        private GameScreen EnhanceWeapon()
        {
            EnhanceManager enhance = new EnhanceManager();
            var weapons = inventory.GetWeapon();
            if (!weapons.Any())
            {
                Console.WriteLine("강화 가능한 장비가 없습니다.");
                Thread.Sleep(1000);
                return GameScreen.ForgeMenu;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("강화 가능한 장비 목록:");
                for (int i = 0; i < weapons.Count; i++)
                    Console.WriteLine($"{i + 1}. {weapons[i].Name}");
                Console.Write("선택 (뒤로가기 0): ");

                string input = Console.ReadLine();
                if (input == "0") return GameScreen.ForgeMenu;

                if (int.TryParse(input, out int num) && num > 0 && num <= weapons.Count)
                {
                    enhance.Enhance(inventory, weapons[num - 1]);
                    gameTime.AddMinutes(60);
                    player.IncreaseFatigue(8);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(800);
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
                Console.Write("뒤로 가려면 0번 키 입력: ");
                string input = Console.ReadLine();

                if (input == "0")
                    return GameScreen.InGame;
            }

            return GameScreen.InGame;
        }

        // 무기 수리하기
        private GameScreen RepairWeapon()
        {
            Repair repair = new Repair(inventory);
            var weaponsToRepair = repair.RepairWeapons(inventory);
            if (!weaponsToRepair.Any())
            {
                Console.WriteLine("수리 가능한 장비가 없습니다.");
                Thread.Sleep(1000);
                return GameScreen.ForgeMenu;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("수리 가능한 장비 목록:");
                for (int i = 0; i < weaponsToRepair.Count; i++)
                    Console.WriteLine($"{i + 1}. {weaponsToRepair[i].Name} ({weaponsToRepair[i].Durability}/{Weapon.MaxDurability})");
                Console.Write("선택 (뒤로가기 0): ");

                string input = Console.ReadLine();

                if (input == "0")
                    return GameScreen.ForgeMenu;

                if (int.TryParse(input, out int num) && num > 0 && num <= weaponsToRepair.Count)
                {
                    int cost = repair.CalculateRepairCost(weaponsToRepair[num - 1]);
                    if (player.Money >= cost)
                    {
                        weaponsToRepair[num - 1].Repair();
                        player.Money -= cost;
                        gameTime.AddMinutes(30);
                    }
                    else
                    {
                        Console.WriteLine("돈이 부족합니다.");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Thread.Sleep(800);
                }

                if (Sleep())
                    return GameScreen.InGame; // 강제 종료 후 인게임으로 복귀
            }
        }

        // 상점
        private GameScreen ShowShop()
        {
            while (true)
            {
                shop.ShowStock();

                Console.Write("구매할 아이템 번호 (뒤로가기 0): ");
                string input = Console.ReadLine();

                if (input == "0")
                    return GameScreen.InGame;

                if (int.TryParse(input, out int num) && num > 0 && num <= shop.GetStockLength())
                {
                    Console.Write("수량: ");
                    string qInput = Console.ReadLine();
                    if (int.TryParse(qInput, out int quantity) && quantity > 0)
                    {
                        shop.Buy(num, quantity, player, inventory);
                        gameTime.AddMinutes(15);
                        player.IncreaseFatigue(2);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 시도해주세요.");
                        Thread.Sleep(800);
                    }
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
                string input = Console.ReadLine();

                if (input == "0")
                    return GameScreen.InGame;

                if (int.TryParse(input, out int num) && num > 0 && num <= requests.Count)
                {
                    requestManager.AcceptRequest(num - 1, player);
                    gameTime.AddMinutes(180);
                    player.IncreaseFatigue(5);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 시도해주세요.");
                    Thread.Sleep(800);
                }

                if (Sleep())
                    return GameScreen.InGame; // 강제 종료 후 인게임으로 복귀
            }
        }

        // 수락한 의뢰
        private GameScreen ShowAcceptedRequests()
        {
            player.ShowActiveReqeusts();
            Console.WriteLine("뒤로가기 0");
            Console.ReadKey(true);
            return GameScreen.InGame;
        }

        // 무기 전체 리스트 보기 (제작에 필요한 재료 확인용)
        private GameScreen ShowWeaponManual()
        {
            Console.Clear();
            foreach (var weapon in WeaponData.GetAll())
            {
                Console.WriteLine($"[{weapon.Name}]");
                foreach (var item in weapon.RequiredResources)
                    Console.WriteLine($"- {item.Key.Name} : {item.Value}개");
                Console.WriteLine();
            }
            Console.Write("뒤로가기 0: ");
            Console.ReadKey(true);
            return GameScreen.InGame;
        }

        // 설명
        private GameScreen ShowExplanation()
        {
            Console.WriteLine("게임 설명...");
            Console.Write("뒤로가기 0: ");
            Console.ReadKey(true);
            return GameScreen.MainMenu;
        }
    }
}
