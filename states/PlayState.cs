using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using FNFMono.Classes;
using FNFMono.Objects;
// audio
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using FNFmono.Classes;
using System.Reflection.Metadata;

namespace FNFMono.States;

public class PlayState : State {
    private Group _playerStrums;
    private Group _enemyStrums;
    private Group _strumGroup;
    private int[] pressArray = new int[4];
    private Group notes;
    private Group sustainNotes;
    private List<Note> unspawnNotes = new List<Note>();

    private Song Inst;
    private SoundEffect Vocals;

    private Keys[] inputList = { Keys.D, Keys.F, Keys.J, Keys.K };
    private float spawnTime = 2000;
    private bool generatedMusic = false;

    private SwagSong SONG;

    public PlayState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        _playerStrums = new Group();
        _enemyStrums = new Group();
        _strumGroup = new Group();
    }
    public override void LoadContent()
    {
        for (int i = 0; i < 4; i++) {
            StrumNote strum = new StrumNote(75, 84, i, 0, _game);
            _enemyStrums.Add(strum);
            strum.PostAddedToGroup();

            _strumGroup.Add(strum);
        }
        for (int i = 0; i < 4; i++)
        {
            StrumNote strum = new StrumNote(75, 84, i, 1, _game);
            _playerStrums.Add(strum);
            strum.PostAddedToGroup();
            
            _strumGroup.Add(strum);
        }

        Add(_strumGroup);

        SONG = NSong.LoadFromJson("test");

        Inst = _game.Content.Load<Song>("songs/" + SONG.song.ToLower() + "/Inst");
        MediaPlayer.Volume = 1.0f;
        MediaPlayer.IsRepeating = false;
        Vocals = _game.Content.Load<SoundEffect>("songs/" + SONG.song.ToLower() + "/Voices");

        GenerateSong();

        MediaPlayer.Play(Inst);
        Vocals.Play();

        Debug.WriteLine("SONG.song: " + SONG.song);
    }

    public void GenerateSong() {
        SwagSong songData = SONG;
        string curSong = songData.song;

        notes = new Group();
        sustainNotes = new Group();
        Add(notes);

        SwagSection[] noteData = songData.notes;

        foreach (SwagSection section in noteData)
        {
            foreach (dynamic songNotes in section.sectionNotes)
            {
                JsonElement daStrumTimeJ = songNotes[0];
                JsonElement daNoteDataJ = songNotes[1];

                // convert to float, int, bool
                float daStrumTime = daStrumTimeJ.GetSingle();
                int daNoteData = daNoteDataJ.GetInt32();
                bool gottaHitNote = section.mustHitSection;

                if (daNoteData > 3)
                    gottaHitNote = !gottaHitNote;

                Note oldNote = null;
                if (unspawnNotes.Count > 0)
                    oldNote = unspawnNotes[0];

                Note swagNote = new Note(daStrumTime, daNoteData%4, oldNote, false, false, null, _game, SONG.speed);
                swagNote.sustainLength = songNotes[2].GetSingle();
                swagNote.ScrollFactor = Vector2.Zero;
                swagNote.mustPress = gottaHitNote;
                unspawnNotes.Add(swagNote);

                int susLength = (int)(swagNote.sustainLength / Conductor.stepCrochet);
                for (int susNote = -1; susNote < (int)susLength-1; susNote++)
                {
                    oldNote = unspawnNotes[unspawnNotes.Count - 1];

                    Note sustainNote = new Note(daStrumTime + (Conductor.stepCrochet * susNote) + Conductor.stepCrochet, daNoteData%4, oldNote, true, false, null, _game, SONG.speed);
                    sustainNote.ScrollFactor = Vector2.Zero;
                    unspawnNotes.Add(sustainNote);
                    sustainNote.mustPress = gottaHitNote;

                    if (swagNote.mustPress) sustainNote.Position.X += 1280 / 2;
                }

                if (swagNote.mustPress) swagNote.Position.X += 1280 / 2;
            }
        }

        // sort unspawnNotes by strumTime
        unspawnNotes = unspawnNotes.OrderBy(o => o.strumTime).ToList();

        generatedMusic = true;
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
        if (generatedMusic) Conductor.songPosition += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        float fakeCrochet = (60 / SONG.bpm) * 1000;

        for (int i = 0; i < notes.Members.Count; i++) {
            int strumLineMid = 84 + (int)(Note.swagWidth/2);
            Note note = notes.Get(i);
            note.Position.Y = 84 - ((Conductor.songPosition-450) - note.strumTime) * (0.45f * SONG.speed);
            if (note.isSustainNote 
                && (!note.mustPress || (note.wasGoodHit || (note.prevNote.wasGoodHit && !note.canBeHit)))
                && note.Position.Y + note.Offset.Y <= strumLineMid) {
                    // change note.Scale.y based on distance from strumLineMid
                    int noteHeight = note.GetFrameHeight();
                    
                }
            //Debug.WriteLine("note.Position.Y: " + note.Position.Y);
            // if past safe zone, remove note
            if (note.strumTime - (Conductor.songPosition-450) < -Conductor.safeZoneOffset * 2) {
                notes.Remove(note);
                if (!note.mustPress) _enemyStrums.Get(note.noteData).PlayAnim("confirm");
                else _playerStrums.Get(note.noteData).PlayAnim("confirm");
                note.Destroy();
            }
        }

        //Debug.WriteLine("Conductor.songPosition: " + Conductor.songPosition);

        if (unspawnNotes.Count > 0) {
            float time = spawnTime;
            if (SONG.speed < 1) time /= SONG.speed;
            if (unspawnNotes[0].multSpeed < 1) time /= unspawnNotes[0].multSpeed;

            while (unspawnNotes.Count > 0 && unspawnNotes[0].strumTime - (Conductor.songPosition-450) < time) {
                Note note = unspawnNotes[0];
                unspawnNotes.RemoveAt(0);
                if (note.isSustainNote) notes.Add(note, 0);
                else notes.Add(note);
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        SuperDraw(gameTime, spriteBatch);
    }
}