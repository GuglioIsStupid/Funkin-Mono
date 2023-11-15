using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace FNFMono.Classes;

public class AudioManager {
    private Game1 game;
    public Dictionary<string, SoundEffect> Sounds;
    public Dictionary<string, Song> Songs;
    
    public AudioManager(Game1 game) {
        this.game = game;
        Sounds = new Dictionary<string, SoundEffect>();
    }
    
    public SoundEffect NewSound(string name, string path) {
        if (Sounds.ContainsKey(name))
            return Sounds[name];
        else
            return Sounds[name] = game.Content.Load<SoundEffect>(path);
    }

    public Song NewMusic(string name, string path) {
        path = "songs/" + path;
        if (Songs.ContainsKey(name))
            return Songs[name];
        else
            return Songs[name] = game.Content.Load<Song>(path);
    }

    // PlayMusic
    public void PlayMusic(string file, float volume = 1f, bool loop = false) {
        Song song = NewMusic(file, file);
        MediaPlayer.Volume = volume;
        MediaPlayer.IsRepeating = loop;
        MediaPlayer.Play(song);
    }
}