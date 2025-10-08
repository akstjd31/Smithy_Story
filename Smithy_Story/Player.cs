using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class Player
    {
        // 상수
        public static readonly int MaxFatigue = 100; // 최대 피로도

        // 변수
        private string name;
        private int fatigue;                    // 피로도
        private int money;                      // 자산

        private List<Request> archiveRequests;  // 수락한 의뢰 목록

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

            archiveRequests = new List<Request>();
        }

        // 메소드
        // 피로도 증가
        public void IncreaseFatigue(int amount)
        {
            Fatigue += amount;
            if (Fatigue > MaxFatigue) Fatigue = MaxFatigue;
        }

        // 피로도 초기화 (하루가 지났을 때)
        public void ResetFatigue() => Fatigue = 0;

        // 보유 중인 의뢰 출력
        public void ShowActiveReqeusts()
        {
            if (!this.archiveRequests.Any())
            {
                Console.WriteLine("현재 진행 중인 의뢰가 없습니다.");
                return;
            }

            Console.WriteLine(this.name + "의 의뢰 목록");
            foreach (var request in this.archiveRequests)
                Console.WriteLine("- " + request.Title);
        }

        public override string ToString() => $"이름: {Name}\t피로도: {Fatigue}/{MaxFatigue}\t돈: {Money:N0}";
    }
}
