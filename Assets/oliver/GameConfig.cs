using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenuAttribute()]
public class GameConfig : ScriptableObject
{
    // feel free to put global game settings in here
    public float walkSpeed = 2;
    public float climbSpeed = 1;
    public float directionInputMinThreshold = 0.1f;
    // doing our own gravity so we can use Rigidbody2D.MovePosition()
    public float gravity = 5;

    static GameConfig _instance;
    public static GameConfig instance { get {
        if (!_instance)
            _instance = Resources.Load<GameConfig>("Game Config");
        if (_instance == null)
            throw new System.Exception("Missing GameConfig file");
        return _instance;
    } }
}
