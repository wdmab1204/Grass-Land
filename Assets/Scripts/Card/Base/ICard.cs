using System;
using CardNameSpace.Base;

public interface ICard
{
    public CardInfo CardInfo { get; set; }
    public object User { get; set; }

    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void Upgrade() { }
}

