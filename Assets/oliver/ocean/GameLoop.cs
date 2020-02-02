using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public Transform ocean, sky;
    public ShipController ship;
    public InputManager input;

    public enum GameState {
        AtTitle,
        PanningToShip,
        WaitingForFirstRepair,
        InGame
    }
    public GameState state { get; private set; }

    // assume no scaling/rotations on these!
    public float oceanLocalTop { get; private set; }
    public float skyLocalTop { get; private set; }
    public float oceanLocalBottom { get; private set; }
    public float skyLocalBottom { get; private set; }
    public float oceanHeight { get { return oceanLocalTop - oceanLocalBottom; } }
    public float skyHeight { get { return skyLocalTop - skyLocalBottom; } }
    public float skyWorldTop { get { return sky.position.y + skyLocalTop; } }
    public float oceanWorldBottom { get { return ocean.position.y + oceanLocalBottom; } }

    public AnimationCurve panToShipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float cameraTopBorder {
        get {
            return skyWorldTop - Camera.main.orthographicSize;
        }
    }

    public float cameraBottomBorder {
        get {
            return oceanWorldBottom + Camera.main.orthographicSize;
        }
    }

    public float cameraPanHeight {
        get {
            return cameraTopBorder - cameraBottomBorder;
        }
    }

    static GameLoop _instance;
    public static GameLoop instance { get {
        if (!_instance)
            _instance = GameObject.FindObjectOfType<GameLoop>();
        if (_instance == null)
            throw new System.Exception("Missing GameLoop instance");
        return _instance;
    } }

    void Start() {
        state = GameState.AtTitle;
        var p = Camera.main.transform.position;
        p.x = ocean.transform.position.x;
        p.y = cameraTopBorder;
        Camera.main.transform.position = p;

        var positions = new Vector3[10]; 
        var count = ocean.GetComponent<LineRenderer>().GetPositions(positions);
        oceanLocalTop = positions[0].y;
        oceanLocalBottom = positions[count - 1].y;
        count = sky.GetComponent<LineRenderer>().GetPositions(positions);
        skyLocalTop = positions[0].y;
        skyLocalBottom = positions[count - 1].y;
    }

    IEnumerator PanToShip() {
        float timer = 0;
        var start = Camera.main.transform.position.y;
        var dest = ship.transform.position.y;
        var distance = start - dest;
        do {
            timer += Time.deltaTime;
            var p = Camera.main.transform.position;
            p.y = cameraTopBorder - panToShipCurve.Evaluate(timer / GameConfig.instance.panToShipSeconds) * distance;
            Camera.main.transform.position = p;
            yield return new WaitForEndOfFrame();
        } while (timer < GameConfig.instance.panToShipSeconds);
        var b = Camera.main.transform.position;
        b.y = dest;
        Camera.main.transform.position = b;
        state = GameState.WaitingForFirstRepair;
    }    

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.AtTitle && input.players.Count > 0) {
            state = GameState.PanningToShip;
            StartCoroutine("PanToShip");
        }
    }
}
