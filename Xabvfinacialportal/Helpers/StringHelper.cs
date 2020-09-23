using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Helpers
{
    public class StringHelper
    {
        public string noid(string myString)
        {
            myString = myString.Substring(0, myString.Length - 5);
            return myString;
        }

        public string idOnly(string myString)
        {
            myString = myString.Substring(myString.Length - 5);
            return myString;
        }
    }
}