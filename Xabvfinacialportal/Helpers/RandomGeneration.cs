using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Helpers
{
    public class RandomGeneration
    {
        public int GenerateRandomNo4dig()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}