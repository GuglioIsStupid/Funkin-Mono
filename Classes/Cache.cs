using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using FNFMono;

namespace FNFmono.Classes;

public class Cache {
    private Game1 game;
    public Dictionary<string, Texture2D> Textures;
    
    public Cache(Game1 game) {
        this.game = game;
        Textures = new Dictionary<string, Texture2D>();
    }
    // hashmap

    public Texture2D NewTexture(string name) {
        if (Textures.ContainsKey(name))
            return Textures[name];
        else
            return Textures[name] = game.Content.Load<Texture2D>(name);
        
    }
}