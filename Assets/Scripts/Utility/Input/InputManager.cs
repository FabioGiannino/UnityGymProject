using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    private static Inputs input;

    static InputManager ()
    {
        input = new Inputs ();
        input.Enable();
    }
    public static Inputs.PlayerActions Player { get { return input.Player; } }

}
