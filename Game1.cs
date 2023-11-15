using FNFmono.Classes;
using FNFMono.Classes;
using FNFMono.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace FNFMono;

public class Game1 : Game
{
    public GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;

    public static int ScreenWidth = 1280;
    public static int ScreenHeight = 720;

    private State _currentState;
    private State _nextState;

    public Cache Cache;

    public void ChangeState(State state)
    {
        _nextState = state;
    }
    public Color UintToRGB(uint hex)
    {
        int r = (int)((hex >> 16) & 0xFF);
        int g = (int)((hex >> 8) & 0xFF);
        int b = (int)((hex) & 0xFF);

        return new Color(r, g, b);
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Cache = new Cache(this);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        this.Window.Title = "Friday Night Funkin' : Monogame";
        this.Window.AllowUserResizing = false;
        _graphics.PreferredBackBufferWidth = ScreenWidth;
        _graphics.PreferredBackBufferHeight = ScreenHeight;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _currentState = new TitleState(this, _graphics.GraphicsDevice, Content);
        _currentState.LoadContent();
        _nextState = null;

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        Input.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        if (_nextState != null)
        {
            _currentState = _nextState;
            _currentState.LoadContent();
            _nextState = null;
        }

        _currentState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _currentState.Draw(gameTime, _spriteBatch);
        base.Draw(gameTime);
    }
}
