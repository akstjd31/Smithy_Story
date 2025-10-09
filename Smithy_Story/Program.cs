using System;
using System.Resources;
using System.Threading;

namespace Smithy_Story
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // static 객체 실행 -> 메모리 확보 및 타 클래스 실행 순서 오류 방지
            var _ = ResourceData.GetAll(); // ResourceData static 생성자 실행
            var __ = WeaponData.GetAll();  // WeaponData static 생성자 실행

            // 이름 입력받기
            Console.Write("플레이어 이름을 입력해주세요: ");
            string playerName = Console.ReadLine();

            Console.WriteLine(playerName + " 님 환영합니다!!");
            Thread.Sleep(1000);

            // 게임 초기 작업(생성자)
            GameManager game = new GameManager(playerName);

            // 게임 시작
            game.Run();
        }
    }
}
