using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCouture.Utility
{
    class ASCIIArt
    {
        public string Shirt()
        {
            /*
               __   __
             /|  `-´  |\
            /_|  o.o  |_\
              | o`o´o |
              |  o^o  |
              |_______|
             */
            return "   __   __\n /|  `-´  |\\\n/_|  o.o  |_\\\n  | o`o´o |\n  |  o^o  |\n  |_______|";
        }

        public string Dress()
        {
//            |   | 
//           ( \ / ) 
//            \\*//
//            ))*((
//           ///|\\\ 
//          ////|\\\\ 

            return "  |   |\n ( \\ / )\n  \\\\*//\n  ))*((\n ///|\\\\\\\n////|\\\\\\\\\n";
        }

        public string Pants()
        {
//            ,==c==.
//            |_/|\_|
//            | ´|` |
//            |  |  |
//            |  |  |
//            |__|__|

            return ",==c==.\n|_/|\\_|\n| ´|` |\n|  |  |\n|  |  |\n|__|__|";
        }

        public string Hat()
        {
//                 ,~"""~.
//              ,-/       \-.
//            .' '`._____.'` `. 
//            `-._         _,-'
//                `--...--'

            return "     ,~\"\"\"~.\n  ,-/       \\-.\n.' '`._____.'` `.\n `-._         _,-'\n     `--...--'";
        }

        public string Glasses()
        {
//            /\     /\
//              \ _____\
//               (_)-(_)

            return "/\\     /\\\n  \\ _____\\\n   (_)-(_)\n";
        }

        public string Other()
        {
//              _   _
//             /)`:'(\
//            //| : |\\
//              |-'-|
//              | | |
//              |_|_|
 
            return "  _   _\n /)`:'(\\\n//| : |\\\\\n  |-'-|\n  | | |\n  |_|_|";
        }
    }
}
