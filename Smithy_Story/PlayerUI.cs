using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class PlayerUI
    {
        // 변수
        private Player player;

        // 생성자
        public PlayerUI(Player player)
        {
            this.player = player;
        }

        // 메소드
        public void Update()
        {
            if (player != null)
            {
                Console.WriteLine(player.ToString());
            }

        }
    }
}
