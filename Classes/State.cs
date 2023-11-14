using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace FNFMono.Classes;

public abstract class State : Group{
    #region Fields
    protected ContentManager _content;
    protected GraphicsDevice _graphicsDevice;
    protected Game1 _game;
    #endregion

    #region Methods
    public abstract void LoadContent();
    public abstract void UnloadContent();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) {
        _game = game;
        _graphicsDevice = graphicsDevice;
        _content = content;
    }
    #endregion
}