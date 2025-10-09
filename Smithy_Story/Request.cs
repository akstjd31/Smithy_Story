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
        public string Name { get; private set; }                            // 의뢰 제목(?)
        public IItem Item { get; private set; }
        public int NeededCount { get; private set; }                        // 필요로 하는 갯수
        public int Reward { get; set; }
        public int DeadlineDay { get; private set; }
        public RequestType Type { get; private set; }                       // 의뢰 종류 (제작, 재료 운반 등..)

        public bool IsCompleted { get; set; } = false;                      // 의뢰의 완료 여부
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

        public Request()
        {
            
        }

        // 메소드
        // 아이템 세터
        public void SetItem(IItem item)
        {
            Item = item;
        }

        // 만기 이전 완료 확인
        public bool IsCompletedBeforeDeadline(int currentDay)
        {
            return IsCompleted && currentDay <= DeadlineDay;
        }

        // 의뢰 실패 유무 확인
        public bool IsExpired(int day)
        {
            return day > DeadlineDay;
        }
        public object Clone()
        {
            return new Request(ID, Name, NeededCount, Type, (IItem)Item.Clone());
        }

        public override string ToString() =>  $"{Name}\n(보상: {Reward} 골드, 주어진 시간: {DeadlineDay} 일";
    }
}
