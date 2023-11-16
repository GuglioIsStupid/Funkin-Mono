using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using FNFMono.Classes.AnimationNS;
using System.Xml;
using System;
using FNFmono.Classes;
using FNFMono.Classes;
using FNFMono.States;
using System.Runtime.InteropServices;
using FNFMono.Objects;
using FNFMono;
using System.Diagnostics;

class EventNote {
    public float strumTime;
    public string eventName;
    public string value1;
    public string value2;
}

class Note : Sprite {
    public Dictionary<string, dynamic> extraData = new Dictionary<string, dynamic>();

    public float strumTime = 0;
    public bool mustPress = false;
    public int noteData = 0;
    public bool canBeHit = false;
    public bool tooLate = false;
    public bool wasGoodHit = false;
    public bool ignoreNote = false;
    public bool hitByOpponent = false;
    public bool noteWasHit = false;
    public Note prevNote;
    public Note nextNote;

    public Note[] tail = new Note[100];
    public Note parent;
    public bool blockHit = false;

    public float sustainLength = 0;
    public bool isSustainNote = false;
    // get set noteType
    public string noteType {get; set;}

    public string eventName = "";
    public int eventLength = 0;
    public string eventValue1 = "";
    public string eventValue2 = "";

    public string animSuffix = "";
    public bool gfNote = false;
    public float earlyHitMult = 1;
    public float lateHitMult = 1;
    public bool lowPriority = false;

    public static int SUSTAIN_SIZE = 44;
    public static float swagWidth = 160 * 0.7f;
    public static string defaultNoteSkin = "NOTE_assets";

    public float offsetX = 0;
    public float offsetY = 0;
    public float offsetAngle = 0;
    public float multAlpha = 1;
    public float multSpeed = 1;

    public bool copyX = true;
    public bool copyY = true;
    public bool copyAngle = true;
    public bool copyAlpha = true;

    public float hitHealth = 0.023f;
    public float missHealth = 0.0475f;
    public string rating = "unknown";
    public float ratingMod = 0;
    public bool ratingDisabled = false;

    public float distance = 2000; 
    public float correctionOffset = 0;
    public float angleDir = 0;

    public Note(float strumTime, int noteData, Note prevNote=null, bool sustainNote=false, bool inEditor=false, dynamic createdFrom=null, Game1 _game=null, float speed=1.0f) : base(0, 0, "images/NOTE_assets") {
        LoadFrames("Content/images/NOTE_assets.xml");
        LoadContent(_game);
        if (prevNote == null) prevNote = this;

        this.prevNote = prevNote;
        isSustainNote = sustainNote;
        
        Position.X += 75;
        Position.X += noteData * swagWidth;
        Position.Y -= 2000;
        this.strumTime = strumTime;
        this.noteData = noteData;

        AddAnimationFromPrefix("greenScroll", "green instance");
        AddAnimationFromPrefix("redScroll", "red instance");
        AddAnimationFromPrefix("blueScroll", "blue instance");
        AddAnimationFromPrefix("purpleScroll", "purple instance");

        AddAnimationFromPrefix("greenholdend", "green hold end");
        AddAnimationFromPrefix("redholdend", "red hold end");
        AddAnimationFromPrefix("blueholdend", "blue hold end");
        AddAnimationFromPrefix("purpleholdend", "pruple end hold");

        AddAnimationFromPrefix("greenhold", "green hold piece");
        AddAnimationFromPrefix("redhold", "red hold piece");
        AddAnimationFromPrefix("bluehold", "blue hold piece");
        AddAnimationFromPrefix("purplehold", "purple hold piece");

        SetGraphicSize(Width*0.7f);
        UpdateHitbox();

        switch (noteData) {
            case 0:
                PlayAnimation("purpleScroll");
                break;
            case 1:
                PlayAnimation("blueScroll");
                break;
            case 2:
                PlayAnimation("greenScroll");
                break;
            case 3:
                PlayAnimation("redScroll");
                break;
        }
        Position.X += 50;

        CenterOffsets();
        CenterOrigin();

        if (isSustainNote && prevNote != null) {
            Alpha = 0.6f;

            Position.X += Width/2;

            switch (noteData) {
                case 2:
                    PlayAnimation("greenholdend");
                    break;
                case 3:
                    PlayAnimation("redholdend");
                    break;
                case 1:
                    PlayAnimation("blueholdend");
                    break;
                case 0:
                    PlayAnimation("purpleholdend");
                    break;
            }

            UpdateHitbox();

            Position.X -= Width/2;

            if (prevNote.isSustainNote) {
                switch (prevNote.noteData) {
                    case 0:
                        prevNote.PlayAnimation("purplehold");
                        break;
                    case 1:
                        prevNote.PlayAnimation("bluehold");
                        break;
                    case 2:
                        prevNote.PlayAnimation("greenhold");
                        break;
                    case 3:
                        prevNote.PlayAnimation("redhold");
                        break;
                }
                prevNote.Scale.Y *= (float)(Conductor.stepCrochet / 100 * 1.5 * speed);
                prevNote.UpdateHitbox();
            }
            
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (mustPress)
        {
            canBeHit = (strumTime > (Conductor.songPosition-450) - (Conductor.safeZoneOffset * lateHitMult) &&
                        strumTime < (Conductor.songPosition-450) + (Conductor.safeZoneOffset * earlyHitMult));

            if (strumTime < (Conductor.songPosition-450) + (Conductor.safeZoneOffset * earlyHitMult))
                tooLate = true;
        } else {
            canBeHit = false;

            if (strumTime <= (Conductor.songPosition-450))
                wasGoodHit = true;
        }
            

        if (tooLate) {
            Alpha = 0.3f;
        }        
    }
}