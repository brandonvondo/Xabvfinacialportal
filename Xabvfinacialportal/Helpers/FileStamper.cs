using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Helpers
{
    public class FileStamper
    {
        public string MakeUnique(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            fileName = SlugMaker.URLFriendly(fileName);
            fileName = $"{fileName}-{DateTime.Now.Ticks}{extension}";
            return fileName;
        }
    }
}