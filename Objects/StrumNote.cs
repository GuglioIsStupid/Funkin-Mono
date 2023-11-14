using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;
using System.Runtime.InteropServices;

namespace FNFMono.Objects;

class StrumNote : Sprite {
    public float resetAnim = 0;
    public int noteData = 0;
    public float direction = 90;
    public bool downscroll = false;
    public bool sustainReduce = true;
    public int player;
    public int SwagWidth = (int)(160 * 0.7);
    public StrumNote(int x, int y, int data, int player) : base(x, y, "images/NOTE_assets") {
        LoadFrames("Content/images/NOTE_assets.xml");

        noteData = data;
        this.player = player;
        ScrollFactor = Vector2.Zero;

        AddAnimationFromPrefix("green", "arrowUP");
        AddAnimationFromPrefix("blue", "arrowDOWN");
        AddAnimationFromPrefix("purple", "arrowLEFT");
        AddAnimationFromPrefix("red", "arrowRIGHT");

        SetGraphicSize(Width*0.7);

        Position.X += SwagWidth * data;
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
                AddAnimationFromPrefix("static", "arrow static instance 3");
                AddAnimationFromPrefix("pressed", "up press", 24, false);
                AddAnimationFromPrefix("confirm", "up confirm", 24, false);
                break;
            case 3:
                AddAnimationFromPrefix("static", "arrow static instance 4");
                AddAnimationFromPrefix("pressed", "right press", 24, false);
                AddAnimationFromPrefix("confirm", "right confirm", 24, false);
                break;
        }

        UpdateHitbox();
    }
}