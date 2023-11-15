using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;
using System.Runtime.InteropServices;
using System.Reflection.Metadata.Ecma335;
using FNFMono.Classes.AnimationNS;

namespace FNFMono.Objects;

class StrumNote : Sprite {
    public float resetAnim = 0;
    public int noteData = 0;
    public float direction = 90;
    public bool downscroll = false;
    public bool sustainReduce = true;
    public int player;
    public int SwagWidth = (int)(160 * 0.7);
    public int ID;
    public StrumNote(int x, int y, int data, int player, Game1 _game) : base(x, y, "images/NOTE_assets") {
        LoadFrames("Content/images/NOTE_assets.xml");
        LoadContent(_game);

        noteData = data;
        this.player = player;
        ScrollFactor = Vector2.Zero;

        SetGraphicSize(Width*0.7);

        switch (data) {
            case 0:
                AddAnimationFromPrefix("static", "arrow static instance 1");
                AddAnimationFromPrefix("pressed", "left press", 24, false);
                AddAnimationFromPrefix("confirm", "left confirm", 24, false);
                break;
            case 1:
                AddAnimationFromPrefix("static", "arrow static instance 2");
                AddAnimationFromPrefix("pressed", "down press", 24, false);
                AddAnimationFromPrefix("confirm", "down confirm", 24, false);
                break;
            case 2:
                AddAnimationFromPrefix("static", "arrow static instance 4");
                AddAnimationFromPrefix("pressed", "up press", 24, false);
                AddAnimationFromPrefix("confirm", "up confirm", 24, false);
                break;
            case 3:
                AddAnimationFromPrefix("static", "arrow static instance 3");
                AddAnimationFromPrefix("pressed", "right press", 24, false);
                AddAnimationFromPrefix("confirm", "right confirm", 24, false);
                break;
        }

        UpdateHitbox();

        ID = data;
    }

    public void PostAddedToGroup() {
        PlayAnim("static");
        Position.X += SwagWidth * noteData;
        Position.X += 50;
        Position.X += 1280/2 * player;
        ID = noteData;
    }

    // update function (allows us to subclass)
    public override void Update(GameTime gameTime) {
        if (resetAnim > 0) {
            if (gameTime != null) resetAnim -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (resetAnim <= 0) {
                PlayAnim("static");
                resetAnim = 0;
            }
        }

        base.Update(gameTime);
    }

    public void PlayAnim(string anim, bool force=false) {
        PlayAnimation(anim, force);
        if (CurAnimation != null) {
            CenterOffsets();
            CenterOrigin();
        }
        resetAnim = 0.1f;
    }
}