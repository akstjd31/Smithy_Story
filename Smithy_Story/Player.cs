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
        const int MaxFatigue = 100; // 최대 피로도
        const int MaxRequestCount = 3;

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
            private set
            {
                if (value <= 0) fatigue = 0;
                else if (value > MaxFatigue) fatigue = MaxFatigue;
                else fatigue = value;
            }
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
        // 의뢰 수락
        public void AcceptRequest(Request request)
        {
            if (request == null)
                return;

            // 보유 의뢰 개수를 초과하지 못함.
            if (this.archiveRequests.Count >= MaxRequestCount)
            {
                Console.WriteLine($"하루 받을 수 있는 최대 의뢰의 수는 {MaxRequestCount}개 입니다!");
                return;
            }

            this.archiveRequests.Add(request);
            Console.WriteLine(request.Title + ": 새로운 의뢰 수락!");
        }

        // 의뢰 완료
        public void CompleteReqeust(Request request)
        {
            if (this.archiveRequests.Contains(request))
            {
                archiveRequests.Remove(request);
                Console.WriteLine(request.Title + ": 의뢰 완료!");
            }
        }

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

        public override string ToString() => $"이름: {Name}\t피로도: {Fatigue}/{MaxFatigue}\t돈: {Money}";
    }
}
