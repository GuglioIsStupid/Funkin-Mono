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
    private SwagSong SONG;
    private Group notes;
    private List<Note> unspawnNotes = new List<Note>();

    private Song Inst;
    private SoundEffect Vocals;

    private Keys[] inputList = { Keys.D, Keys.F, Keys.J, Keys.K };
    private float spawnTime = 2000;
    private bool generatedMusic = false;
    public PlayState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
    {
        _playerStrums = new Group();
        _enemyStrums = new Group();
        _strumGroup = new Group();
    }
    public override void LoadContent()
    {
        for (int i = 0; i < 4; i++) {
            StrumNote strum = new StrumNote(50, 42, i, 0, _game);
            _enemyStrums.Add(strum);
            strum.PostAddedToGroup();

            _strumGroup.Add(strum);
        }
        for (int i = 0; i < 4; i++)
        {
            StrumNote strum = new StrumNote(50, 42, i, 1, _game);
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

    }

    public void GenerateSong() {
        SwagSong songData = SONG;
        string curSong = songData.song;

        notes = new Group();
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

                Note swagNote = new Note(daStrumTime, daNoteData%4, oldNote, false, false, null, _game);
                swagNote.sustainLength = songNotes[2].GetSingle();
                //swagNote.altNote = songNotes[3];
                swagNote.ScrollFactor = Vector2.Zero;
                swagNote.mustPress = gottaHitNote;

                unspawnNotes.Add(swagNote);

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
        foreach(Note note in notes.Members) {
            
            note.Position.Y = 42 - ((Conductor.songPosition-450) - note.strumTime) * (0.45f * SONG.speed);
            //Debug.WriteLine("note.Position.Y: " + note.Position.Y);
            // if past safe zone, remove note
            if (note.strumTime - (Conductor.songPosition-450) < -Conductor.safeZoneOffset * 2) {
                notes.Remove(note);
                StrumNote strum = null;
                if (note.mustPress) strum = _playerStrums.Get(note.noteData);
                else strum = _enemyStrums.Get(note.noteData);
                break;
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
                notes.Add(note);
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        SuperDraw(gameTime, spriteBatch);
    }
}