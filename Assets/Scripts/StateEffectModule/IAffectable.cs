using System;
using System.Collections.Generic;


public interface IAffectable
{

    void Init();
    void OnStateEntered(StateEffect effect);
    void OnStateExited(StateEffect effect);
}
