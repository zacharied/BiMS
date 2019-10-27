using System;
using System.Collections.Generic;
using System.Text;
using theori.IO;

namespace BiMS.IO
{
    // TODO Disposable stuff.
    public enum ControllerInput
    {
        BT1, BT2, BT3, BT4, BT5, BT6, BT7,
        TurntableUp, TurntableDown,
        Start
    }

    public abstract class Controller
    {
        private static Controller instance;
        public Action<ControllerInput>? ButtonPressed, ButtonReleased;

        public static Controller Instance()
        {
            // TODO Make this not bad.
            if (instance == null)
                instance = new KeyboardController();

            return instance;
        }
    }

    class KeyboardController : Controller
    {
        private readonly Dictionary<KeyCode, ControllerInput> keyMappings;

        public KeyboardController()
        {
            Keyboard.KeyPress += OnKeyPress;
            Keyboard.KeyRelease += OnKeyRelease;

            keyMappings = new Dictionary<KeyCode, ControllerInput>
            {
                // TODO Load from config.
                { KeyCode.Q, ControllerInput.BT1 },
                { KeyCode.W, ControllerInput.BT2 },
                { KeyCode.E, ControllerInput.BT3 },
                { KeyCode.R, ControllerInput.BT4 },
                { KeyCode.U, ControllerInput.BT5 },
                { KeyCode.I, ControllerInput.BT6 },
                { KeyCode.O, ControllerInput.BT7 },
                { KeyCode.LSHIFT, ControllerInput.TurntableDown },
                { KeyCode.LCTRL, ControllerInput.TurntableUp }
            };
        }

        private void OnKeyPress(KeyInfo info)
        {
            if (keyMappings.ContainsKey(info.KeyCode) && ButtonPressed != null)
            {
                ButtonPressed.Invoke(keyMappings[info.KeyCode]);
            }
        }

        private void OnKeyRelease(KeyInfo info)
        {
            if (keyMappings.ContainsKey(info.KeyCode) && ButtonReleased != null)
            {
                ButtonReleased.Invoke(keyMappings[info.KeyCode]);
            }
        }
    }
}
