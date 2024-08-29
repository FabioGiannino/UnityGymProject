using System;
using System.Collections.Generic;

public enum StateEffect
{
    Cold,
    Fire,
    Poison
}

public interface IAffectable
{

    void Init();
    void OnStateEntered(StateEffect effect);
    void OnStateExited(StateEffect effect);
}
