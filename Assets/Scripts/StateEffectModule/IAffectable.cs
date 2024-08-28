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
    Dictionary<StateEffect,float> GetCurrentStatesEffects();

    Action OnIceEffectStarted {  get; set; }
    Action OnIceEffectEnded { get; set;}    
    Action OnFireEffectStarted { get; set; }
    Action OnFireEffectEnded { get; set;}
    Action OnPoisonEffectStarted { get;set; }
    Action OnPoisonEffectEnded { get;set; }
}
