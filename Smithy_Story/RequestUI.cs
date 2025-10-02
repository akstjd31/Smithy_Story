using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class RequestUI
    {
        // 변수
        RequestManager requestManager = new RequestManager();

        // 메소드
        public RequestUI(RequestManager manager)
        {
            this.requestManager = manager;
        }

        // 출력문 작성
        public void Update()
        {
            if (requestManager != null)
            {
            }
        }
    }
}
