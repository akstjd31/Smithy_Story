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
    // 의뢰 상태 enum 클래스 <= 이거 굳이 필요한가...??? 그냥 플레이어가 받은 의뢰만 리스트에 넣고 완료 or 실패하면 리스트에서 제거하면 될듯
    public enum RequestStatus { Pending, Accepted, Completed, Failed }

    public class Request : ICloneable
    {
        // 상수
        // 변수
        // 프로퍼티
        public int ID {  get; private set; }
        public string Title { get; private set; }
        public IItem Item { get; private set; }
        public int Reward { get; private set; }
        public int DeadlineDay { get; private set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;  // 의뢰 상태 (의뢰를 받았는지? 아님 거절했는지? 그런거)
        public RequestType Type { get; private set; }                       // 의뢰 종류 (제작, 재료 운반 등..)

        // 생성자
        public Request(int id, string title, IItem item, int reward, int deadlineDay, RequestType requestType)
        {
            ID = id;
            Title = title;
            Item = item;
            Reward = reward;
            DeadlineDay = deadlineDay;
            Type = requestType;
        }

        // 메소드
        
        // 의뢰 실패 유무 확인
        public bool IsExpired(int day)
        {
            return day > DeadlineDay;
        }

        public object Clone()
        {
            return new Request(ID, Title, Item, Reward, DeadlineDay, Type)
            {
            };
        }

        public override string ToString() =>  $"{Title}\n(보상: {Reward} 골드, 주어진 시간: {DeadlineDay} 일, 상태: {Status})";
    }
}
