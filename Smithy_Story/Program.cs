using System;
using System.Collections.Generic;
using System.Threading;

namespace Smithy_Story
{
    // 화면 FSM
    public enum GameScreen
    { 
        MainMenu, Explanation, RequestMenu, ArchiveRequestMenu, InventoryMenu, InGame, Exit
    }

    internal class Program
    {
        static ConsoleKeyInfo inputKeyInfo;                         // 키 정보
        const int MaxDailyRequestCount = 10;                        // 일일 최대 의뢰 목록 개수 제한

        static void Main(string[] args)
        {
            // 초기화 작업
            GameScreen currentScreen = GameScreen.MainMenu;         // 시작하면 메인화면
            Inventory inventory = new Inventory();                  // 인벤토리
            GameTime gameTime = new GameTime();                     // 커스텀으로 설정 가능, 기본 (day: 0, hour: 8, min: 0)
            RequestManager requestManager = new RequestManager();   // 의뢰 관리자
            UIManager uiManager = null;                             // 인 게임 UI
            Player player = null;                                   // 플레이어

            Console.WriteLine("플레이어 정보를 입력해주세요!");
            Console.Write("플레이어 이름: ");
            player = new Player(Console.ReadLine());
            Console.WriteLine(player.Name + "님 반갑습니다!");
            uiManager = new UIManager(player, inventory, gameTime, requestManager);

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

                    // 일일 의뢰 목록 보기
                    case GameScreen.RequestMenu:
                        currentScreen = ShowDailyRequestList(uiManager);
                        break;
                    // 인벤토리 보기
                    case GameScreen.InventoryMenu:
                        currentScreen = ShowInventory(uiManager);
                        break;
                    
                    // 게임 설명
                    case GameScreen.Explanation:
                        currentScreen = ShowExplanation();
                        break;

                    case GameScreen.Exit:
                        Console.WriteLine("\n게임을 종료합니다.");
                        break;

                }

                

                        // 본격적인 게임 시작
                        //while (true)
                        //{
                        //info = Console.ReadKey(true);
                        //if (info.Key == ConsoleKey.Z)
                        //    Console.WriteLine(RequestManager.CreateItemRequest().ToString());
                        //}

                        // 인벤토리 테스트
                        //foreach (var item in ResourceData.GetAll())
                        //    inventory.AddItem(item);

                        //inventory.AddItem(new Resource(1001, "돌", 1, Grade.Common));
                        //inventory.ShowInventory();


                        //inventory.RemoveItemById(1001);
                        //inventory.RemoveItemById(1001);
                        //inventory.ShowInventory();

            }
        }

        public static GameScreen ShowDailyRequestList(UIManager uiManager)
        {
            bool open = true;
            while (open)
            {
                Console.Clear();
                uiManager.UpdateDailyRequestUI();
                Console.Write("뒤로 가려면 0번 키 입력: ");
                var input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.D0 || input.Key == ConsoleKey.NumPad0)
                    return GameScreen.InGame;
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
                          "1. 오늘의 의뢰 확인하기\n" +
                          "2. 내가 수락한 의뢰 목록 확인하기\n" +
                          "3. 인벤토리 보기\n" +
                          "4. 오늘의 상점 이용하기\n" +
                          "5. 잠자기\n";
                            
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
                        return GameScreen.RequestMenu;
                    //uiManager.ShowInventory();
                    case ConsoleKey.D2:
                        return GameScreen.ArchiveRequestMenu;
                        //uiManager.ShowRequests();
                    case ConsoleKey.D3:
                        return GameScreen.InventoryMenu;
                    //case ConsoleKey.D4:
                    //    Console.WriteLine("4번키 입력!");
                    //    return GameScreen.MainMenu;
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
