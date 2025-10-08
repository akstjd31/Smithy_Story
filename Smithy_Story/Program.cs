using System;

namespace Smithy_Story
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("플레이어 이름을 입력해주세요: ");
            string playerName = Console.ReadLine();

            GameManager game = new GameManager(playerName);

            // 게임 시작
            game.Run();

            Console.WriteLine("게임을 종료합니다.");
        }
    }
}
