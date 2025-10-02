using System;
using System.Threading;

namespace Smithy_Story
{
    internal class Program
    {
        static GameTime gameTime = new GameTime(); // 커스텀으로 설정 가능, 기본 (day: 0, hour: 8, min: 0)

        static void Main(string[] args)
        {
            // 초기화 작업
            Inventory inventory = new Inventory();
            Player player;
            bool isGameOn = true;
            bool gameOver = false;
            ConsoleKeyInfo inputKeyInfo;

            // 메인 화면 부분
            string[] menu = new string[]
            {
                "==============================================",
                "대장장이 이야기",
                "==============================================\n",
                "1. 게임 시작",
                "2. 게임 설명",
                "3. 게임 종료",
            };

            PrintMain(menu);

            while (isGameOn)
            {
                Console.Write("해당되는 번호를 입력하세요: ");
                inputKeyInfo = Console.ReadKey(true);

                switch (inputKeyInfo.Key)
                {
                    // 게임 시작 버튼 눌렀을 시
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Console.Clear();
                        Console.WriteLine("플레이어 정보를 입력해주세요!");
                        Console.Write("플레이어 이름: ");
                        player = new Player(Console.ReadLine());

                        Console.WriteLine(player.Name + "님 반갑습니다!");

                        ConsoleKeyInfo info;
                        while (true)
                        {
                            info = Console.ReadKey(true);
                            if (info.Key == ConsoleKey.Z)
                                Console.WriteLine(RequestManager.CreateItemRequest().ToString());
                        }
                        
                        // 인벤토리 테스트
                        //foreach (var item in ResourceData.GetAll())
                        //    inventory.AddItem(item);

                        //inventory.AddItem(new Resource(1001, "돌", 1, Grade.Common));
                        //inventory.ShowInventory();


                        //inventory.RemoveItemById(1001);
                        //inventory.RemoveItemById(1001);
                        //inventory.ShowInventory();

                        // 본격적인 게임 시작
                        //while (!gameOver)
                        //{
                        //    GameStart();
                        //}

                        break;

                    // 게임 설명 버튼 눌렀을 시
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        break;

                    // 게임 종료
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        Console.WriteLine("\n게임을 종료합니다.");
                        isGameOn = false;
                        break;

                    // 예외
                    default:
                        Console.WriteLine("\n키를 잘못 입력함!\n");
                        break;

                }

            }
        }

        static void GameStart()
        {
            Console.Clear();
            Console.WriteLine("게임 시간: " + gameTime.GetFormattedTime());
        }

        // 메인 화면 출력문
        static void PrintMain(string[] menu)
        {
            int windowHeight = Console.WindowHeight;
            int topPadding = (windowHeight - menu.Length) / 4;

            for (int i = 0; i < topPadding; i++)
                Console.WriteLine();

            foreach (string line in menu)
                WriteCentered(line);
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
