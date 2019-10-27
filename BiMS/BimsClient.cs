using System;
using System.Collections.Generic;
using System.Text;
using theori;
using theori.Platform;
using BiMS.Gameplay;
using BiMS.IO;

namespace BiMS
{
    public sealed class BimsClient : Client
    {
        protected override Layer CreateInitialLayer() => new GameplayLayer();
    }
}
