using System;
using System.Resources;

namespace Smithy_Story
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _ = ResourceData.GetAll(); // ResourceData static 생성자 실행
            var __ = WeaponData.GetAll();  // WeaponData static 생성자 실행

            Console.Write("플레이어 이름을 입력해주세요: ");
            string playerName = Console.ReadLine();

            GameManager game = new GameManager(playerName);

            // 게임 시작
            game.Run();

            Console.WriteLine("게임을 종료합니다.");
        }
    }
}
