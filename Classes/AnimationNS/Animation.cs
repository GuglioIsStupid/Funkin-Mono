using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNFMono.Classes.AnimationNS;

public class Animation {
    public List<Frame> Frames = new List<Frame>();
    public string Name;
    public bool Loop;
    public int Framerate;

    public Animation(List<Frame> frames, string name, bool loop, int framerate) {
        Frames = frames;
        Name = name;
        Loop = loop;
        Framerate = framerate;
    }
}