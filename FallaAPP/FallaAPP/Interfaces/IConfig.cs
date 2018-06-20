using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FallaAPP.Interfaces
{
    public interface IConfig
    {
        string DirectoryDB { get; }

        ISQLitePlatform Platform { get; }
    }
}
