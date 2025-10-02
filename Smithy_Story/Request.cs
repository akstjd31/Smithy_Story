using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    // 의뢰 상태 enum 클래스
    public enum RequestStatus { Pending, Accepted, Completed, Failed }

    public class Request
    {
        // 상수
        // 변수
        // 프로퍼티
        public int Id {  get; private set; }
        public string Name { get; private set; }
        public int Reward { get; private set; }
        public int DeadlineDay { get; private set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;  // 의뢰 상태 (의뢰를 받았는지? 아님 거절했는지? 그런거)
        public RequestType Type { get; private set; }                       // 의뢰 종류 (제작, 재료 운반 등..)

        // 생성자
        public Request(int id, string name, int reward, int deadlineDay, RequestType requestType)
        {
            Id = id;
            Name = name;
            Reward = reward;
            DeadlineDay = deadlineDay;
            Type = requestType;
        }

        // 메소드
        
        // 의뢰 실패 유무 확인
        public bool IsExpired(GameTime time)
        {
            return time.Day > DeadlineDay;
        }

        public override string ToString()
        {
            return $"{Name}의 의뢰:\t(보상: {Reward} 골드, 마감: Day {DeadlineDay}, 상태: {Status})\n";
        }
    }
}
