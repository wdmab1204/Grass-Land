using System;
using CardNameSpace.Base;

public interface ICard
{
    public CardInfo CardInfo { get; set; }
    protected object User { get; set; }
    public void Start();
    public void Update();
    public void Exit();
    public void Upgrade();
}

