using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Color[] playerColors;
    [System.NonSerialized]
    public List<Player> players = new List<Player>();

    void OnPlayerJoined(PlayerInput playerInput) {
        Debug.Log($"Player joined: {playerInput.name}, {playerInput.devices.Count}");
        var player = playerInput.GetComponent<Player>();
        players.Add(player);
        player.SetColor(playerColors[players.Count - 1]);
        CameraManager.AddCameraTarget(player.transform);
    }

    void OnPlayerLeft(PlayerInput playerInput) {
        Debug.Log($"Player left: {playerInput.name}");
        CameraManager.RemoveCameraTarget(playerInput.GetComponent<Player>().transform);
        playerInput.GetComponent<Player>().Die();
    }
}
