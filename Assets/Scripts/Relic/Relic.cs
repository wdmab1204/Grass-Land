using System;
public class Relic
{
    public string name;
    public string description;
    public UnityEngine.Sprite sprite;
    public Relic(string name, string description, UnityEngine.Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
    }
}
