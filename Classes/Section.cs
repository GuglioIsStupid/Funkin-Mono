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

class SwagSection {
    public dynamic[] sectionNotes { get; set;}
    public int lengthInSteps { get; set;}
    public int typeOfSection { get; set;}
    public bool mustHitSection { get; set;}
    public float bpm { get; set;}
    public bool changeBPM { get; set;}
    public bool altAnim { get; set;}
}

class Section {
    // no limit list
    public dynamic[] sectionNotes = new dynamic[1000000];
    public int lengthInSteps = 16;
    public int typeOfSection = 0;
    public bool mustHitSection = true;

    public static int COPYCAT = 0;

    public Section(int lengthInSteps = 16) {
        this.lengthInSteps = lengthInSteps;
    }
}