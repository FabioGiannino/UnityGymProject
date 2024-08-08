using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton 
    private static Player instance;
    public static Player GetPlayer {  
        get 
        {
            if (instance != null) return instance; 
            instance = GameObject.FindObjectOfType<Player>();
            return instance;
        } 
    }
    #endregion

    #region Mono
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    #endregion
}
