﻿using System;
using System.Collections.Generic;

namespace Knight.Framework.Net
{
    public static partial class NetworkMessageOpcode
    {
        public const ushort C2R_Login       = 10001;
        public const ushort R2C_Login       = 10002;
        public const ushort C2G_LoginGate   = 10003;
        public const ushort G2C_LoginGate   = 10004;
    }
}
