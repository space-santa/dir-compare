using System.Collections.Generic;

namespace DirCompare
{
    public interface IOutput
    {
        void Write(List<string> md5sums);
    }
}
