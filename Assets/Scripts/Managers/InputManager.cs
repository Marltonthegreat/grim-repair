using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Color[] playerColors;
    [System.NonSerialized]
    public List<Player> players = new List<Player>();
    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    PlayerInput wasdPlayer = null;
    PlayerInput arrowPlayer = null;

    bool arrowPlayerJoined;
    bool wasdPlayerJoined;

    

    private void Update()
    {
        if (!arrowPlayerJoined && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) )
        {
            wasdPlayer?.SwitchCurrentControlScheme("KeyboardLeft", Mouse.current);
            arrowPlayerJoined = true;
            /*if (arrowPlayer)
            {
                arrowPlayer.SwitchCurrentControlScheme("KeyboardRight", Keyboard.current);  
            }*/
        }

        if (!wasdPlayerJoined && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            arrowPlayer?.SwitchCurrentControlScheme("KeyboardRight", Mouse.current);
            wasdPlayerJoined = true;
            /*if (wasdPlayer)
            {
                wasdPlayer.SwitchCurrentControlScheme("KeyboardLeft", Keyboard.current);
            }*/
        }
    }

    void OnPlayerJoined(PlayerInput playerInput) {
        Debug.Log($"Player joined: {playerInput.name}, {playerInput.devices.Count}, {playerInput.currentControlScheme}");
        var player = playerInput.GetComponent<Player>();
        players.Add(player);

        if (playerInput.currentControlScheme == "Keyboard&Mouse" || playerInput.currentControlScheme == null)
        {
            if (wasdPlayerJoined && !wasdPlayer)
            {
                wasdPlayer = playerInput;
                playerInput.SwitchCurrentControlScheme("KeyboardLeft", Keyboard.current);

                arrowPlayer?.SwitchCurrentControlScheme("KeyboardRight", Keyboard.current);

            }
            else if (arrowPlayerJoined && !arrowPlayer)
            {
                arrowPlayer = playerInput;
                playerInput.SwitchCurrentControlScheme("KeyboardRight", Keyboard.current);

                wasdPlayer?.SwitchCurrentControlScheme("KeyboardLeft", Keyboard.current);

            }
            else
            {
                wasdPlayerJoined = true;
                wasdPlayer = playerInput;
                playerInput.SwitchCurrentControlScheme("KeyboardLeft", Keyboard.current);

                arrowPlayer?.SwitchCurrentControlScheme("KeyboardRight", Keyboard.current);
            }
        }

        //Debug.Log(playerInput.currentControlScheme);

        player.SetColor(playerColors[players.Count - 1]);
        cameraManager.AddCameraTarget(player.gameObject.transform);
    }

    void OnPlayerLeft(PlayerInput playerInput) {
        Debug.Log($"Player left: {playerInput.name}");
        cameraManager.RemoveCameraTarget(playerInput.GetComponent<Player>().transform);
        playerInput.GetComponent<Player>().Die();
    }
}
