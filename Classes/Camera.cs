using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using FNFMono.Classes.AnimationNS;
using System.Xml;
using System;

namespace FNFmono.Classes;

public class Camera {
    public Vector2 Position;

    public Vector2 target;
    
    public Camera() {
        Position = new Vector2(0, 0);
        target = new Vector2(0, 0);
    }

    public void Update(GameTime gameTime) {
        //
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        //
    }
}