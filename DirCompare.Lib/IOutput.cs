using System.Collections.Generic;

namespace DirCompare.Lib
{
    public interface IOutput
    {
        void Write(List<string> md5sums);
    }
}
