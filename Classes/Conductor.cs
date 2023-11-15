using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using FNFMono.Classes.AnimationNS;
using System.Xml;
using System;
using Microsoft.VisualBasic;
using FNFMono.Classes;
using System.Linq;

namespace FNFmono.Classes;

class BPMChangeEvent {
    public int stepTime;
    public float songTime;
    public float bpm;

    public BPMChangeEvent(int stepTime, float songTime, float bpm) {
        this.stepTime = stepTime;
        this.songTime = songTime;
        this.bpm = bpm;
    }
}

class Conductor {
    public static float bpm = 100;
    public static float crochet = 60/bpm * 1000;
    public static float stepCrochet = crochet/4;
    public static float songPosition;
    public static float lastSongPos;
    public static float offset;

    public static int safeFrames = 10;
    public static float safeZoneOffset = safeFrames/60*1000;
    public static BPMChangeEvent[] bpmChangeMap = new BPMChangeEvent[1000000];

    public Conductor() {}

    public static void mapBPMChanges(SwagSong song) {
        bpmChangeMap = new BPMChangeEvent[1000000];

        float curBPM = song.bpm;
        int totalSteps = 0;
        float totalPos = 0;

        //for (i in 0...song.notes.length)
        for (int i = 0; i < song.notes.Length; i++) {
            if (song.notes[i].changeBPM && song.notes[i].bpm != curBPM) {
                curBPM = song.notes[i].bpm;
                BPMChangeEvent bpmEvent = new BPMChangeEvent(totalSteps, totalPos, curBPM);
                // push bpmEvent to bpmChangeMap
                bpmChangeMap.Append(bpmEvent);
            }

            int deltaSteps = song.notes[i].lengthInSteps;
            totalSteps += deltaSteps;
            totalPos += ((60/curBPM) * 1000 / 4) * deltaSteps;
        }
        Console.WriteLine("New BPM Map");
    }

    public static void changeBPM(float newBPM) {
        bpm = newBPM;
        crochet = 60/bpm * 1000;
        stepCrochet = crochet/4;
    }
}