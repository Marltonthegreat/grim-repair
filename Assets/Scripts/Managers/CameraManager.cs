using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject cameraLookAtPoint;
    List<Player> playerComponents = new List<Player>();

    private void Update()
    {
        if (playerComponents.Count <= GetComponents<Player>().Length + 1)
        {
            playerComponents.Clear();
            playerComponents.AddRange(FindObjectsOfType<Player>());
        }

        cameraLookAtPoint.transform.position = AveragePositions(playerComponents);
    }

    private Vector3 AveragePositions(List<Player> players)
    {
        Vector3 position = Vector3.zero;

        foreach (Player player in players)
        {
            position.x += player.gameObject.transform.position.x;
            position.y += player.gameObject.transform.position.y;
            position.z += player.gameObject.transform.position.z;
        }

        return position / players.Count;
    }
}
