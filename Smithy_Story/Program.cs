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

    internal class Program
    {

        static ConsoleKeyInfo inputKeyInfo;                         // 키 정보

        const int MaxDailyRequestCount = 7;                        // 일일 최대 의뢰 목록 개수 제한
        const int StartMoney = 1000;                                // 시작 금액

        static void Main(string[] args)
        {

            // 초기화 작업
            ResourceData.GetAll().ToList();                         // 데이터 강제 로드 시키기
            WeaponData.GetAll().ToList();                           // 위와 같음.

            GameScreen currentScreen = GameScreen.MainMenu;         // 시작하면 메인화면
            Inventory inventory = new Inventory();                  // 인벤토리
            GameTime gameTime = new GameTime();                     // 커스텀으로 설정 가능, 기본 (day: 0, hour: 8, min: 0)
            RequestManager requestManager = new RequestManager();   // 의뢰 관리자
            Shop shop = new Shop();                                 // 재료 상점
            UIManager uiManager = null;                             // 인 게임 UI
            Player player = null;                                   // 플레이어



            Console.WriteLine("플레이어 정보를 입력해주세요!");
            Console.Write("플레이어 이름: ");
            player = new Player(Console.ReadLine(), money: StartMoney);
            Console.WriteLine(player.Name + "님 반갑습니다!");
            uiManager = new UIManager(player, inventory, gameTime, requestManager, shop);

            // 미리 의뢰 만들어놓기
            requestManager.GenerateDailyRequests(MaxDailyRequestCount);

            while (currentScreen != GameScreen.Exit)
            {
                Console.Clear();

                // FSM에 따른 기능 구현
                switch (currentScreen)
                {
                    // 메인 화면
                    case GameScreen.MainMenu:
                        currentScreen = ShowMainMenu(player.Name);
                        break;

                    // 인 게임
                    case GameScreen.InGame:
                        currentScreen = ShowInGame(currentScreen, player, inventory, requestManager, gameTime, uiManager);
                        break;

                    // 장비 제작/강화/수리 메뉴
                    case GameScreen.ForgeMenu:
                        currentScreen = ForgeMenual();
                        break;

                    // 제작
                    case GameScreen.Craft:
                        currentScreen = CraftWeapon(inventory);
                        break;

                    // 강화
                    case GameScreen.Enhance:
                        currentScreen = EnhanceWeapon(inventory);
                        break;

                    case GameScreen.Repair:
                        currentScreen = RepairWeapon(inventory, player);
                        break;

                    // 일일 의뢰 목록 보기
                    case GameScreen.DailyRequestMenu:
                        currentScreen = ShowDailyRequestList(uiManager, requestManager, player);
                        break;

                    // 수락한 의뢰 목록 보기
                    case GameScreen.ArchiveRequestMenu:
                        currentScreen = ShowArchiveReqeustList(player);
                        break;

                    // 모든 무기 정보 만드는 법(레시피)
                    case GameScreen.WeaponMenu:
                        currentScreen = ShowWeaponMenual();
                        break;

                    // 인벤토리 보기
                    case GameScreen.Inventory:
                        currentScreen = ShowInventory(uiManager);
                        break;

                    // 상점
                    case GameScreen.Shop:
                        currentScreen = ShowResourceShop(shop, player, inventory);
                        break;
                    // 게임 설명
                    case GameScreen.Explanation:
                        currentScreen = ShowExplanation();
                        break;

                    case GameScreen.Exit:
                        Console.WriteLine("\n게임을 종료합니다.");
                        break;

                }
            }
        }

        // 장비 수리하기
        public static GameScreen RepairWeapon(Inventory inventory, Player player)
        {
            Repair repair = new Repair(inventory);
            bool open = true;
            int num = 0;

            while (open)
            {
                var weaponsToRepair = repair.RepairWeapons(inventory); // 인벤토리에 있는 장비 리스트
                Console.Clear();
                Console.WriteLine("========================== 수리 가능한 장비 ==========================");

                if (weaponsToRepair.Count < 0)
                {
                    Console.WriteLine("강화 가능한 장비가 없습니다!");
                }
                else
                {
                    for (int i = 0; i < weaponsToRepair.Count; i++)
                        Console.WriteLine($"{i + 1}. {weaponsToRepair[i].Name}\t현 내구도 [{weaponsToRepair[i].Durability}/{Weapon.MaxDurability}]");

                    Console.WriteLine("======================================================================");
                }

                Console.WriteLine("어떤 장비를 수리하실 건가요?(뒤로 가기 0)");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.ForgeMenu;

                if (char.IsDigit(input.KeyChar))
                    num = input.KeyChar - '0';

                int repairCost = repair.CalculateRepairCost(weaponsToRepair[num - 1]);
                // 장비 수리 시도
                if (num > 0 && num <= weaponsToRepair.Count)
                {
                    Console.WriteLine($"해당 장비를 수리하시겠습니까? (Y / Any)\n수리비용: {repairCost}");
                    var answer = Console.ReadKey(true);

                    // 수락
                    if (answer.Key.Equals(ConsoleKey.Y))
                    {
                        // 수리할 비용이 있는가?
                        if (player.Money >= repairCost)
                        {
                            weaponsToRepair[num - 1].Repair();
                            player.Money -= repairCost;
                        }
                        else
                        {
                            Console.WriteLine($"수리 비용이 부족합니다! (부족한 금액: {Math.Abs(player.Money - repairCost)}");
                            return GameScreen.ForgeMenu;
                        }
                    }

                    // 거절
                    else
                        return GameScreen.ForgeMenu;
                }
            }

            return GameScreen.ForgeMenu;
        }

        // 장비 강화하기
        public static GameScreen EnhanceWeapon(Inventory inventory)
        {
            EnhanceManager enhanceManager = new EnhanceManager();
            bool open = true;
            int num = 0;

            while (open)
            {
                var weapons = inventory.GetWeapon();      // 인벤토리에 있는 장비 리스트
                Console.Clear();
                Console.WriteLine("========================== 강화 가능한 장비 ==========================");

                if (weapons.Count < 0)
                {
                    Console.WriteLine("강화 가능한 장비가 없습니다!");
                }
                else
                {
                    for (int i = 0; i < weapons.Count; i++)
                        Console.WriteLine($"{i + 1}. {weapons[i].Name}");

                    Console.WriteLine("======================================================================");
                }

                Console.WriteLine("어떤 장비를 강화하실 건가요?(뒤로 가기 0)");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.ForgeMenu;

                if (char.IsDigit(input.KeyChar))
                    num = input.KeyChar - '0';

                // 장비 강화 시도
                if (num > 0 && num <= weapons.Count)
                    enhanceManager.Enhance(inventory, weapons[num - 1]);
            }

            return GameScreen.ForgeMenu;
        }

        // 장비 제작하기
        public static GameScreen CraftWeapon(Inventory inventory)
        {
            Craft craft = new Craft(inventory);                     // 작업대 생성
            bool open = true;
            int num = 0;

            while (open)
            {
                var craftableWeapons = craft.GetCraftableWeapon();  // 제작 가능한 장비 리스트
                Console.Clear();
                Console.WriteLine("========================== 제작 가능한 장비 ==========================");

                if (craftableWeapons.Count < 0)
                {
                    Console.WriteLine("제작 가능한 장비가 없습니다!");
                }
                else
                {
                    for (int i = 0; i < craftableWeapons.Count; i++)
                        Console.WriteLine($"{i + 1}. {craftableWeapons[i].Name}");

                    Console.WriteLine("======================================================================");
                }

                Console.WriteLine("어떤 장비를 제작하실 건가요?(뒤로 가기 0)");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.ForgeMenu;

                if (char.IsDigit(input.KeyChar))
                    num = input.KeyChar - '0';

                // 장비 제작 시도
                if (num > 0 && num <= craftableWeapons.Count)
                    craft.CraftWeapon(craftableWeapons[num - 1]);
            }

            return GameScreen.ForgeMenu;
        }

        // 장비 제작/강화/수리 선택
        public static GameScreen ForgeMenual()
        {
            bool open = true;
            string menu = "1. 장비 제작하기\n" +
                          "2. 장비 강화하기\n" +
                          "3. 장비 수리하기\n" +
                          "Any. 뒤로가기";

            while (open)
            {
                Console.Clear();
                Console.WriteLine(menu);
                Console.Write("입력: ");
                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    // 장비 제작
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return GameScreen.Craft;

                    // 장비 강화
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return GameScreen.Enhance;

                    // 장비 수리
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return GameScreen.Repair;
                    default:
                        return GameScreen.InGame;
                }

            }
            return GameScreen.InGame;
        }

        // 수락한 의뢰 확인하기
        public static GameScreen ShowArchiveReqeustList(Player player)
        {
            bool open = true;
            while (open)
            {
                Console.Clear();
                Console.WriteLine("==================== 수락한 의뢰 목록 ====================");
                if (player.ArchiveRequests.Count < 1)
                {
                    Console.WriteLine("보유한 의뢰가 없습니다!");
                }
                else
                {
                    foreach (var request in player.ArchiveRequests)
                    {
                        Console.WriteLine(request.ToString());
                    }
                }
                Console.WriteLine("==========================================================\n");

                Console.WriteLine("뒤로 가려면 0번 키 입력");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.InGame;
            }

            return GameScreen.InGame;
        }

        // 무기 레시피 보기
        public static GameScreen ShowWeaponMenual()
        {
            bool open = true;

            Console.Clear();
            while (open)
            {
                foreach (var weapon in WeaponData.GetAll())
                {
                    Console.WriteLine($"[{weapon.Name}]");
                    foreach (var item in weapon.RequiredResources)
                        Console.WriteLine($"- {item.Key.Name} : {item.Value}개");
                    Console.WriteLine();
                }
                Console.WriteLine("===========================================");
                Console.Write("뒤로 가려면 0번 키 입력: ");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.InGame;
            }

            return GameScreen.InGame;
        }

        // 재료 상점
        public static GameScreen ShowResourceShop(Shop shop, Player player, Inventory inventory)
        {
            bool open = true;
            int num, quantity;
            
            shop.RefreshStock();    // 샵 새로고침은 따로 일(Day)이 지나면 갱신되어야함. (일단 테스트용이므로 여기에 있음)
            while (open)
            {
                Console.Clear();
                shop.ShowStock();

                Console.Write("구매하실 재료의 번호를 입력해주세요(뒤로 가기 0): ");
                num = int.Parse(Console.ReadLine());

                if (num == 0)
                    return GameScreen.InGame;

                if (num > shop.GetStockLength())
                {
                    Console.WriteLine("번호를 잘못 입력하셨습니다.");
                    Thread.Sleep(200);
                    continue;
                }

                // 해당 위치에 아이템이 없는 경우 (다 팔림)
                if (!shop.IsExistItemIdx(num - 1))
                {
                    Console.WriteLine("현재 번호에 아이템이 없습니다.");
                    Thread.Sleep(200);
                    continue;
                }

                Console.Write("\n구매하실 재료의 수량을 입력해주세요: ");
                quantity = int.Parse(Console.ReadLine());

                if (!shop.IsExistItemQuantity(num - 1, quantity))
                {
                    Console.WriteLine("잘못된 수량을 입력하셨습니다.");
                    continue;
                }

                shop.Buy(num, quantity, player, inventory);
            }

            return GameScreen.InGame;
        }

        // 일일 의뢰 확인
        public static GameScreen ShowDailyRequestList(UIManager uiManager, RequestManager reqManager, Player player)
        {
            int num = 0;
            bool open = true;
            var requests = reqManager.GetDailyRequests();

            while (open)
            {
                Console.Clear();
                uiManager.UpdateDailyRequestUI();

                Console.Write("수락할 의뢰를 선택하세요(뒤로가기 0): ");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.InGame;

                if (char.IsDigit(input.KeyChar))
                    num = input.KeyChar - '0';

                if (num < 0 || num > requests.Count)
                    continue;

                reqManager.AcceptRequest(num - 1, player);
            }

            return GameScreen.InGame;
        }

        // 게임 설명 관련
        public static GameScreen ShowExplanation()
        {
            bool open = true;
            string text = "여기에 설명하는 글 작성"; // 수정 필요!!!!!!!!!!!!!!!!!!!

            while (open)
            {
                // 설명 글 출력
                Console.Write("뒤로 가려면 0번 키 입력: ");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.MainMenu;
            }

            return GameScreen.MainMenu;
        }


        // 인벤토리 목록 보기
        public static GameScreen ShowInventory(UIManager uiManager)
        {
            bool open = true;

            while (open)
            {
                Console.Clear();
                uiManager.UpdateInventoryUI();
                Console.Write("뒤로 가려면 0번 키 입력: ");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.InGame;
            }

            return GameScreen.InGame;
        }

        // 인 게임 출력
        public static GameScreen ShowInGame(GameScreen currentScreen, Player player, Inventory inventory, RequestManager reqManager, GameTime gameTime, UIManager uiManager)
        {
            string menu = "=============================================\n" +
                          "1. 장비 제작/강화/수리하기\n" +
                          "2. 오늘의 의뢰 확인하기\n" +
                          "3. 내가 수락한 의뢰 목록 확인하기\n" +
                          "4. 인벤토리 보기\n" +
                          "5. 오늘의 상점 이용하기\n" +
                          "6. 무기 레시피 보기\n";

            while (currentScreen == GameScreen.InGame)
            {
                Console.Clear();
                uiManager.UpdatePlayerUI();
                uiManager.UpdateTimeUI();

                Console.WriteLine(menu);

                Console.Write("입력: ");
                ConsoleKeyInfo input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        return GameScreen.ForgeMenu;
                    case ConsoleKey.D2:
                        return GameScreen.DailyRequestMenu;
                    //uiManager.ShowInventory();
                    case ConsoleKey.D3:
                        return GameScreen.ArchiveRequestMenu;
                    //uiManager.ShowRequests();
                    case ConsoleKey.D4:
                        return GameScreen.Inventory;
                    case ConsoleKey.D5:
                        return GameScreen.Shop;
                    case ConsoleKey.D6:
                        return GameScreen.WeaponMenu;

                }
            }

            return GameScreen.MainMenu;
        }


        // 메인 화면 메소드
        static GameScreen ShowMainMenu(string name)
        {
            string[] menu = new string[]
            {
                "==============================================",
                name + "의 대장장이 이야기",
                "==============================================\n",
                "1. 게임 시작",
                "2. 게임 설명",
                "3. 게임 종료",
            };

            int windowHeight = Console.WindowHeight;
            int topPadding = (windowHeight - menu.Length) / 4;

            for (int i = 0; i < topPadding; i++)
                Console.WriteLine();

            foreach (string line in menu)
                WriteCentered(line);

            Console.Write("해당되는 번호를 입력하세요: ");
            inputKeyInfo = Console.ReadKey(true);

            switch (inputKeyInfo.Key)
            {
                // 게임 시작
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return GameScreen.InGame;
                // 게임 설명 버튼 눌렀을 시
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return GameScreen.Explanation;

                // 게임 종료
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return GameScreen.Exit;

                // 예외
                default:
                    Console.WriteLine("\n키를 잘못 입력함!\n");
                    return GameScreen.MainMenu;
            }
        }

        // 가로 기준 센터 계산
        static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int leftPadding = (windowWidth - text.Length) / 2;
            if (leftPadding < 0) leftPadding = 0; // 안전 처리
            Console.WriteLine(new string(' ', leftPadding) + text);
        }
    }
}
