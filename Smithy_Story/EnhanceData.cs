using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{

    public static class EnhanceData
    {
        // <강화 수치, 확률>
        private static readonly List<Enhance> enhanceList = new List<Enhance>()
        {
        new Enhance(1.00, 0.00, 0.00),
        new Enhance(0.90, 0.10, 0.00),
        new Enhance(0.75, 0.20, 0.05),
        new Enhance(0.50, 0.40, 0.10),
        new Enhance(0.30, 0.50, 0.20)
        };

        public static Enhance GetData(int level)
        {
            if (level >= enhanceList.Count)
                level = enhanceList.Count - 1;

            return enhanceList[level];
        }
    }
}
