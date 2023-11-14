using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;

namespace FNFMono.States;

public class PlayState : State {
    public PlayState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        
    }
    public override void LoadContent()
    {
        
        
    }

    public override void UnloadContent()
    {
        
    }

    public override void Update(GameTime gameTime)
    {
        SuperUpdate(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        SuperDraw(gameTime, spriteBatch);
    }
}