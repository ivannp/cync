using System;

namespace CloudSync.Core
{
    interface IDll
    {
        Delegate GetDelegate<T>(string name);
    }
}
