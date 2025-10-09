using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 플레이어 클래스
    public class Player
    {
        // 상수
        public static readonly int MaxFatigue = 100; // 최대 피로도

        // 변수
        private string name;                        // 이름
        private int fatigue;                        // 피로도
        private int money;                          // 자산

        // 프로퍼티
        public string Name
        {
            get => name; private set { name = value; }   
        }

        public int Fatigue
        {
            get => fatigue;
            private set {  fatigue = value; }
        }

        public int Money
        {
            get => money;
            set
            {
                if (value <= 0) money = 0;
                else money = value;
            }
        }

        public List<Request> ArchiveRequests
        { get; private set;} = new List<Request>();

        // 생성자
        public Player(string name, int fatigue = 0, int money = 0)
        {
            this.name = name;
            this.fatigue = fatigue;
            this.money = money;
        }

        // 메소드
        // 피로도 증가
        public void IncreaseFatigue(int amount)
        {
            Fatigue += amount;
            if (Fatigue > MaxFatigue) Fatigue = MaxFatigue;
        }

        // 의뢰 남은 일자 줄이기
        public void DecreaseRequestDeadline()
        {
            foreach (var request in ArchiveRequests)
            {
                request.DecreaseDeadlineDay();
            }
        }

        // 피로도 초기화 (하루가 지났을 때)
        public void ResetFatigue() => Fatigue = 0;

        // 의뢰 추가
        public void AddRequest(Request req) => ArchiveRequests.Add(req);

        // 보유 중인 의뢰 출력
        public void ShowActiveReqeusts()
        {
            Console.Clear();
            Console.WriteLine(this.name + "의 의뢰 목록");
            Console.WriteLine("=================================================");

            for (int i = 0; i < ArchiveRequests.Count; i++)
            {
                //string output = "";
                
                //output = $"{i + 1}. [{ArchiveRequests[i].Name}]\t남은 기간: {ArchiveRequests[i].DeadlineDay}";
                //switch (ArchiveRequests[i].Type)
                //{
                //    case RequestType.RepairWeapon:
                //    case RequestType.CraftWeapon:
                //        output = $"{i + 1}. {ArchiveRequests[i].Name}";
                //        break;
                //    case RequestType.DeliverItem:
                //        output = $"{i + 1}. {ArchiveRequests[i].Name }"

                //}

                Console.WriteLine($"{i + 1}. [{ArchiveRequests[i].Name}]\t남은 기간: {ArchiveRequests[i].DeadlineDay}");
            }

            Console.WriteLine();
        }

        // ToString 재정의
        public override string ToString() => $"이름: {Name}\t피로도: {Fatigue}/{MaxFatigue}\t돈: {Money:N0}";
    }
}
