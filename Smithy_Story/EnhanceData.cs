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
        new Enhance(1.00, 0.00, 0.00),  // 1강
        new Enhance(0.90, 0.10, 0.00),  // 2강
        new Enhance(0.80, 0.20, 0.00),  // 3강
        new Enhance(0.70, 0.30, 0.00),  // 4강
        new Enhance(0.50, 0.50, 0.00),  // 5강
        new Enhance(0.45, 0.50, 0.05),  // 6강
        new Enhance(0.40, 0.53, 0.07),  // 7강
        new Enhance(0.35, 0.55, 0.10),  // 8강
        new Enhance(0.30, 0.57, 0.13),  // 9강
        new Enhance(0.25, 0.60, 0.15),  // 10강
        new Enhance(0.18, 0.60, 0.22),  // 11강
        new Enhance(0.15, 0.60, 0.25),  // 12강
        new Enhance(0.10, 0.60, 0.30),  // 13강
        new Enhance(0.05, 0.60, 0.35),  // 14강
        new Enhance(0.03, 0.57, 0.40)   // 15강
        };

        public static Enhance GetData(int level)
        {
            if (level >= enhanceList.Count)
                level = enhanceList.Count - 1;

            return enhanceList[level];
        }
    }
}
