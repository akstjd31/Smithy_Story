using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Smithy_Story
{
    // 의뢰 클래스
    public class Request : IData, ICloneable
    {
        // 상수
        // 변수
        // 프로퍼티
        public int ID {  get; private set; }
        public string Name { get; private set; }                            // 의뢰 제목
        public IItem Item { get; private set; }                             // 의뢰 아이템
        public int NeededCount { get; private set; }                        // 필요로 하는 갯수 (강화를 부탁하는 사람이면 해당 수치는 원하는 강화 수치)
        public int Reward { get; private set; }                             // 보상
        public int DeadlineDay { get; set; }
        public RequestType Type { get; private set; }                       // 의뢰 종류 (제작, 재료 운반 등..)

        public bool IsCompleted { get; set; } = false;                      // 의뢰 완료 여부

        // 생성자
        public Request(int id, string name, int deadlineDay, RequestType requestType, 
            IItem item, int reward = 0, int neededCount = 1)
        {
            ID = id;
            Name = name;
            NeededCount = neededCount;
            Item = (IItem)item.Clone();
            Reward = reward;
            DeadlineDay = deadlineDay;
            Type = requestType;
        }

        // 메소드
        // 필요 갯수 또는 강화 수치 세팅
        public void SetNeededCount(int nc) => NeededCount = nc;

        // 보상 세팅
        public void SetReward(int reward) => Reward = reward;

        // 아이템 세터
        public void SetItem(IItem item) => Item = item;

        // 만기일 감소
        public void DecreaseDeadlineDay() => DeadlineDay--;

        // 의뢰 실패 유무 확인
        public bool IsExpired() => DeadlineDay <= 0;

        public object Clone()
        {
            return new Request(
                id: ID,
                name: Name,
                deadlineDay: DeadlineDay,
                requestType: Type,
                item: (IItem)Item.Clone(),
                reward: Reward,
                neededCount: NeededCount
            );
        }

        // ToString 재정의
        public override string ToString() =>  $"{Name}\n(보상: {Reward} 골드, 만기 일자: {DeadlineDay} 일)";
    }
}
