using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Enums
{
    public enum ResponseDtoEnum
    {
        Success=200,
        Unknow=300,
        NoData = 400,
        Duplicated=401,
        Error = 500,
        RegisterWithDependency = 406
    }
}
