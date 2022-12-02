using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer2.Game
{
    public static class FieldSizes
    {
        public static FieldSize Small = new FieldSize(20, 10, "small");
        public static FieldSize Medium = new FieldSize(20, 15, "medium");
        public static FieldSize Large = new FieldSize(20, 20, "large");
    }
}
