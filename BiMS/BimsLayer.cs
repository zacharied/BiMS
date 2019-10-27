using System;
using System.Collections.Generic;
using System.Text;
using theori;
using BiMS.IO;

namespace BiMS
{
    public abstract class BimsLayer : Layer
    {
        public abstract void OnControllerButtonPressed(ControllerInput input);
        public abstract void OnControllerButtonReleased(ControllerInput input);
    }
}
