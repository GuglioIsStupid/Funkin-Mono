using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;
using FNFMono.Objects;

namespace FNFMono.States;

public class PlayState : State {
    private Group _playerStrums;
    private Group _enemyStrums;
    private int[] pressArray = new int[4];

    private Keys[] inputList = { Keys.D, Keys.F, Keys.J, Keys.K };
    public PlayState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        _playerStrums = new Group();
        _enemyStrums = new Group();
    }
    public override void LoadContent()
    {
        for (int i = 0; i < 4; i++)
        {
            StrumNote strum = new StrumNote(-100, -100, i, 1, _game);
            _playerStrums.Add(strum);
            strum.PostAddedToGroup();

            StrumNote strum2 = new StrumNote(-100, -100, i, 0, _game);
            _enemyStrums.Add(strum2);
            strum2.PostAddedToGroup();
        }

        Add(_playerStrums);
        Add(_enemyStrums);
    }

    public override void UnloadContent()
    {

    }

    public override void Update(GameTime gameTime)
    {
        SuperUpdate(gameTime);

        // if any key from inputList is pressed, add it to pressArray
        for (int i = 0; i < 4; i++)
        {
            if (Input.IsPressed(inputList[i]))
                pressArray[i] = 1;
            else if (Input.IsReleased(inputList[i]))
                pressArray[i] = 0;
        }

        // animate corresponding _playerStrums member
        for (int i = 0; i < 4; i++)
        {
            if (pressArray[i] == 1)
            {
                StrumNote strum = _playerStrums.Get(i);
                strum.PlayAnim("pressed");
            } else if (pressArray[i] == 0)
            {
                StrumNote strum = _playerStrums.Get(i);
                strum.PlayAnim("static");
            }
        }

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        SuperDraw(gameTime, spriteBatch);
    }
}