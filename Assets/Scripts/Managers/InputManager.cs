using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Color[] playerColors;
    [System.NonSerialized]
    public List<Player> players = new List<Player>();

    PlayerInput wasdPlayer = null;
    PlayerInput arrowPlayer = null;

    private void Update()
    {
        if (!arrowPlayer && Input.GetKeyDown(KeyCode.DownArrow) && wasdPlayer != null)
        {
            wasdPlayer.SwitchCurrentControlScheme("KeyboardLeft", Mouse.current);
        }
    }

    void OnPlayerJoined(PlayerInput playerInput) {
        Debug.Log($"Player joined: {playerInput.name}, {playerInput.devices.Count}");
        Debug.Log(playerInput.currentControlScheme);
        var player = playerInput.GetComponent<Player>();
        players.Add(player);

        if (playerInput.currentControlScheme == "Keyboard&Mouse" && wasdPlayer == null)
        {
            wasdPlayer = playerInput;
            playerInput.SwitchCurrentControlScheme("KeyboardLeft", Keyboard.current);
        }
        else if((playerInput.currentControlScheme == "Keyboard&Mouse" && wasdPlayer != null)||playerInput.currentControlScheme == null)
        {
            arrowPlayer = playerInput;
            playerInput.SwitchCurrentControlScheme("KeyboardRight", Keyboard.current);
            wasdPlayer.SwitchCurrentControlScheme("KeyboardLeft", Keyboard.current);
            Debug.Log(wasdPlayer.currentControlScheme);
        }

        player.SetColor(playerColors[players.Count - 1]);
    }

    void OnPlayerLeft(PlayerInput playerInput) {
        Debug.Log($"Player left: {playerInput.name}");
        playerInput.GetComponent<Player>().Die();
    }
}
