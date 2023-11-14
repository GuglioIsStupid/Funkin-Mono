using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;

namespace FNFMono.States;

public class TitleState : State {
    private Sprite _girlfriendTitle;
    private Sprite _logoBumpin;
    public TitleState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        _girlfriendTitle = new Sprite((int)(Game1.ScreenWidth*0.4), (int)(Game1.ScreenHeight*0.07), "images/menu/gfDanceTitle");
        _logoBumpin = new Sprite(-150, -100, "images/menu/logoBumpin");
    }
    public override void LoadContent()
    {
        _girlfriendTitle.LoadContent(_game);
        _girlfriendTitle.LoadFrames("Content/images/menu/gfDanceTitle.xml");
        _girlfriendTitle.AddAnimationFromPrefix("gfDanceTitle", "gfDance", 24, true); 
        _girlfriendTitle.PlayAnimation("gfDanceTitle");

        _logoBumpin.LoadContent(_game);
        _logoBumpin.LoadFrames("Content/images/menu/logoBumpin.xml");
        _logoBumpin.AddAnimationFromPrefix("logoBumpin", "logo bumpin", 24, true);
        _logoBumpin.PlayAnimation("logoBumpin");
        
        Add(_girlfriendTitle);
        Add(_logoBumpin);
    }

    public override void UnloadContent()
    {
        _girlfriendTitle.UnloadContent();
        _logoBumpin.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
        SuperUpdate(gameTime);
       /*  _girlfriendTitle.Update(gameTime);
        _logoBumpin.Update(gameTime); */

        // if enter is pressed, go to the menu state
        if (Input.IsPressed(Keys.Enter))
        {
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content));
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        SuperDraw(gameTime, spriteBatch);
        /* _girlfriendTitle.Draw(spriteBatch);
        _logoBumpin.Draw(spriteBatch); */
    }
}