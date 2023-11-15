using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using FNFMono.Classes.AnimationNS;
using System.Xml;
using System;
using FNFmono.Classes;
using System.Net;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
// json
namespace FNFMono.Classes;

// typedef for swagsong

class SwagSong {
    public string song { get; set; }
    // array called "notes" holding SwagSection
    public SwagSection[] notes { get; set; }
    public float bpm { get; set; }
    public bool needsVoices { get; set; }
    public float speed { get; set; }

    public string player1 { get; set; }
    public string player2 { get; set; }
    public bool validScore { get; set; }
}
class JsonLayout {
    public SwagSong song { get; set; }
}

class NSong {
    public string song;
    public SwagSection[] notes;
    public float bpm;
    public bool needsVoices = true;
    public float speed = 1.0f;

    public string player1 = "bf";
    public string player2 = "dad";

    public NSong(string song, SwagSection[] notes, float bpm) {
        this.song = song;
        this.notes = notes;
        this.bpm = bpm;
    }

    public static SwagSong LoadFromJson(string jsonInput, string folder="") {
        if (folder == "") 
            folder = jsonInput;
        //jsonInput is a file path
        // rawJson is a string of the json file
        //string rawJson = File.OpenRead("Content/data/" + folder.ToLower() + "/" + jsonInput.ToLower().Trim() + ".json");
        /* using FileStream openStream = File.OpenRead();*/
        //Debug.WriteLine("Content/data/" + folder.ToLower() + "/" + jsonInput.ToLower().Trim() + ".json");
        return ParseJSONShit("Content/data/" + folder.ToLower() + "/" + jsonInput.ToLower().Trim() + ".json");
    }

    public static SwagSong ParseJSONShit(string rawJson) {
        /* using FileStream openStream = File.OpenRead(rawJson); */
        rawJson = File.ReadAllText(rawJson);
        JsonLayout swagShit = JsonSerializer.Deserialize<JsonLayout>(rawJson);
        // make it a swagsong
        SwagSong swagSong = new SwagSong();
        swagSong.song = swagShit.song.song;
        swagSong.notes = swagShit.song.notes;
        swagSong.bpm = swagShit.song.bpm;
        swagSong.needsVoices = swagShit.song.needsVoices;
        swagSong.speed = swagShit.song.speed;
        swagSong.player1 = swagShit.song.player1;
        swagSong.player2 = swagShit.song.player2;
        swagSong.validScore = swagShit.song.validScore;
        return swagSong;
    }
}