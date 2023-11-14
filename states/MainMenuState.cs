using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;

namespace FNFMono.States;

public class MainMenuState : State {
    private Sprite _bgMagenta;
    private Sprite _bg;
    private Sprite _storyButton;

    public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        _bgMagenta = new Sprite(0, 0, "images/menu/menuDesat");
        _bg = new Sprite(0, 0, "images/menu/menuBG");
        _storyButton = new Sprite(0, 0, "images/menu/mainmenu/menu_story_mode");
    }

    public override void LoadContent()
    {
        _bgMagenta.LoadContent(_game);
        _bgMagenta.Color = _game.UintToRGB(0xFFfd719b);
    
        _bg.LoadContent(_game);
        
        _storyButton.LoadContent(_game);
        _storyButton.LoadFrames("Content/images/menu/mainmenu/menu_story_mode.xml");
        _storyButton.AddAnimationFromPrefix("storyButton", "story_mode basic", 24, true);
        _storyButton.PlayAnimation("storyButton");
    }

    public override void UnloadContent()
    {
        _bgMagenta.UnloadContent();
        _bg.UnloadContent();
        _storyButton.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
        SuperUpdate(gameTime);
        _bgMagenta.Update(gameTime);
        _bg.Update(gameTime);
        _storyButton.Update(gameTime);

        // if enter is pressed, go to the menu state
        if (Input.IsPressed(Keys.Enter))
        {
            _game.ChangeState(new PlayState(_game, _graphicsDevice, _content));
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        SuperDraw(gameTime, spriteBatch);
        _bgMagenta.Draw(spriteBatch);
        _bg.Draw(spriteBatch);
        _storyButton.Draw(spriteBatch);
    }
}