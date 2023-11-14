using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using FNFMono.Classes.AnimationNS;
using System.Xml;
using System;
using FNFmono.Classes;

namespace FNFMono.Classes;

public class Input {
    static KeyboardState currentKeyboardState;
    static KeyboardState previousKeyboardState;

    public static KeyboardState GetState() {
        previousKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        return currentKeyboardState;
    }

    public static bool IsPressed(Keys key) {
        return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
    }

    public static bool IsDown(Keys key) {
        return currentKeyboardState.IsKeyDown(key);
    }

    public static bool IsReleased(Keys key) {
        return !currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key);
    }

    public static bool IsUp(Keys key) {
        return !currentKeyboardState.IsKeyDown(key);
    }
}