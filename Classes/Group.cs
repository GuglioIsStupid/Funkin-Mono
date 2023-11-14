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

public class Group {
    public List<dynamic> Members;

    public Group() {
        Members = new List<dynamic>();
    }

    public void Add(dynamic member, int index = -1) {
        if (index == -1)
            Members.Add(member);
        else
            Members.Insert(index, member);
    }

    public void Remove(dynamic member) {
        Members.Remove(member);
    }

    public void RemoveAt(int index) {
        Members.RemoveAt(index);
    }

    public void SuperUpdate(GameTime gameTime) {
        foreach (dynamic member in Members) {
            // if Update function exists
            if (member.GetType().GetMethod("Update") != null)
                member.Update(gameTime);
        }
    }

    public void SuperDraw(GameTime gameTime, SpriteBatch spriteBatch) {
        foreach (dynamic member in Members) {
            // if Draw function exists
            if (member.GetType().GetMethod("Draw") != null)
                member.Draw(spriteBatch);
        }
    }
}