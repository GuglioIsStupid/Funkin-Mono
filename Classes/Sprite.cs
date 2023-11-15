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

public class Sprite {
    public float Alpha;
    public string Text, FontName, Path;
    public Vector2 Position, Scale, Offset, Origin;
    public Rectangle SourceRect;
    public bool IsActive;
    public Texture2D Texture;

    Vector2 origin;
    RenderTarget2D renderTarget;
    //SpriteFont font;
    
    public int angle;

    public int Width, Height;

    public List<Animation> Animations;
    public List<NewAnim> AnimsToAdd;
    public string DefaultAnimation;
    public List<Frame> Frames;

    public bool isAnimating = false;
    public float frameIndex;
    public Animation CurAnimation;
    public int FramesPerSecond;

    public XmlDocument spriteSheet;

    public Color Color = Color.White;

    public Camera camera;
    public Vector2 ScrollFactor = new Vector2(1, 1);

    public Sprite(int x, int y, string path="") {
        Path = path;
        Position = new Vector2(x, y);
        Scale = new Vector2(1, 1);
        Alpha = 1.0f;
        angle = 0;
        Text = string.Empty;
        FontName = "Fonts/Font";
        SourceRect = Rectangle.Empty;
        IsActive = true;
        Width = 0;
        Height = 0;
        Offset = new Vector2(0, 0);
        Origin = new Vector2(0, 0);

        AnimsToAdd = new List<NewAnim>();
        Frames = new List<Frame>();
        Animations = new List<Animation>();
    }

    public void LoadFrames(string xmlPath) {
        if (spriteSheet == null)
            spriteSheet = new XmlDocument();

        spriteSheet.Load(xmlPath);
        XmlElement root = spriteSheet.DocumentElement;
        XmlNodeList subTextures = root.SelectNodes("SubTexture");
            
        foreach (XmlNode subTexture in subTextures) {
            Frame frame = new Frame();
            frame.Name = subTexture.Attributes["name"].Value;
            frame.X = Convert.ToInt32(subTexture.Attributes["x"].Value);
            frame.Y = Convert.ToInt32(subTexture.Attributes["y"].Value);
            frame.Width = Convert.ToInt32(subTexture.Attributes["width"].Value);
            frame.Height = Convert.ToInt32(subTexture.Attributes["height"].Value);
            frame.RealWidth = Convert.ToInt32(subTexture.Attributes["width"].Value);
            frame.RealHeight = Convert.ToInt32(subTexture.Attributes["height"].Value);

            if (subTexture.Attributes["frameWidth"] != null)
                frame.RealWidth = Convert.ToInt32(subTexture.Attributes["frameWidth"].Value);
            if (subTexture.Attributes["frameHeight"] != null)


            frame.OffsetX = 0;
            frame.OffsetY = 0;
            if (subTexture.Attributes["frameX"] != null)
                frame.OffsetX = Convert.ToInt32(subTexture.Attributes["frameX"].Value);
            if (subTexture.Attributes["frameY"] != null)
                frame.OffsetY = Convert.ToInt32(subTexture.Attributes["frameY"].Value);
            Frames.Add(frame);
        }
    }

    public void AddAnimationFromPrefix(string name, string prefix, int frameRate=30, bool loop=true) {
        List<Frame> frames = new List<Frame>();

        if (this.Frames != null) {
            foreach (Frame frame in this.Frames) {
                if (frame.Name.StartsWith(prefix)) {
                    frames.Add(frame);
                }
            }

            // add to animations
            Animation anim = new Animation(frames, name, loop, frameRate);
            this.Animations.Add(anim);
        } else
            this.Frames = new List<Frame>();
    }

    public void PlayAnimation(string name) {
        if (this.Animations != null) {
            foreach (Animation anim in this.Animations) {
                if (anim.Name == name) {
                    this.frameIndex = 0;
                    this.CurAnimation = anim;
                    this.isAnimating = true;
                }
            }
        }
    }

    //can take int or float
    public void SetGraphicSize(dynamic width=null, dynamic height=null) {
        if (width == null) width = 0;
        if (height == null) height = 0;
        
        Scale = new Vector2(
            (float)width / GetFrameWidth(),
            (float)height / GetFrameHeight()
        );

        if (width <= 0)
            Scale.X = Scale.Y;
        if (height <= 0)
            Scale.Y = Scale.X;
    }

    public int GetFrameWidth() {
        if (this.CurAnimation != null)
            return this.CurAnimation.Frames[(int)this.frameIndex].Width;
        else
            return this.Width;
    }

    public int GetFrameHeight() {
        if (this.CurAnimation != null)
            return this.CurAnimation.Frames[(int)this.frameIndex].Height;
        else
            return this.Height;
    }

    public void UpdateHitbox() {
        int w = GetFrameWidth();
        int h = GetFrameHeight();

        Width = (int)Math.Abs(this.Scale.X) * w;
        Height = (int)Math.Abs(this.Scale.Y) * h;

        this.Offset = new Vector2(
            (int)(-0.5 * (Width-w)),
            (int)(-0.5 * (Height-h))
        );

        CenterOrigin();
    }

    public void CenterOrigin() {
        this.Origin = new Vector2(
            (int)(0.5 * Width),
            (int)(0.5 * Height)
        );
    }

    public void CenterOffsets() {
        this.Offset = new Vector2(
            (int)(-0.5 * Width),
            (int)(-0.5 * Height)
        );
    }

    public void LoadContent(Game1 game, string path = "") {
        if (path != string.Empty)
            Path = path;
        if (Path != string.Empty)
            Texture = game.Cache.NewTexture(Path);
        
        if (Texture != null){
            Width = Texture.Width;
            Height = Texture.Height;
        } else{
            Width = 0;
            Height = 0;
        }

        if (SourceRect == Rectangle.Empty)
            SourceRect = new Rectangle(0, 0, (int)Width, (int)Height);
    }

    public virtual void UnloadContent() {
        Texture.Dispose();
        Animations.Clear();
        Frames.Clear();
    }

    public virtual void Update(GameTime gameTime) {
        if (this.CurAnimation != null && this.isAnimating) {
            if (this.frameIndex >= this.CurAnimation.Frames.Count)
                this.frameIndex = 0;
            
            this.frameIndex += (float)gameTime.ElapsedGameTime.TotalSeconds * this.CurAnimation.Framerate;

            if (this.frameIndex >= this.CurAnimation.Frames.Count)
                if (this.CurAnimation.Loop)
                    this.frameIndex = 0;
                else
                    this.frameIndex = this.CurAnimation.Frames.Count - 1;
            
            this.SourceRect = new Rectangle(
                this.CurAnimation.Frames[(int)this.frameIndex].X,
                this.CurAnimation.Frames[(int)this.frameIndex].Y,
                this.CurAnimation.Frames[(int)this.frameIndex].Width,
                this.CurAnimation.Frames[(int)this.frameIndex].Height
            );

            // Weird and gross workaround for if the frame is larger than the texture
            if (this.SourceRect.X + this.SourceRect.Width > this.Texture.Width)
                this.SourceRect.Width = this.Texture.Width - this.SourceRect.X;
            if (this.SourceRect.Y + this.SourceRect.Height > this.Texture.Height)
                this.SourceRect.Height = this.Texture.Height - this.SourceRect.Y;
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        // set to clamp wrap mode
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

        // only spriteeffect is so if the SourceRect is larger than the texture, it will still draw correctly
        int _x = (int)Position.X;
        int _y = (int)Position.Y;
        int _ox = (int)Origin.X;
        int _oy = (int)Origin.Y;
        int _sx = (int)Scale.X;
        int _sy = (int)Scale.Y;

        _x += _ox - ((int)Offset.X);
        _y += _oy - ((int)Offset.Y);

        // if sprite is not animating, draw the whole texture
        if (isAnimating) {
            _ox += (int)this.CurAnimation.Frames[(int)this.frameIndex].OffsetX;
            _oy += (int)this.CurAnimation.Frames[(int)this.frameIndex].OffsetY;
        }
            
        if (camera != null) {
            _x -= (int)camera.Position.X * (int)ScrollFactor.X;
            _y -= (int)camera.Position.Y * (int)ScrollFactor.Y;
        }

        spriteBatch.Draw(Texture,
                        new Vector2(_x, _y),
                        SourceRect,
                        Color * Alpha,
                        MathHelper.ToRadians(angle),
                        new Vector2(_ox, _oy),
                        Scale,
                        SpriteEffects.None,
                        0.0f);
        spriteBatch.End();
    }
}