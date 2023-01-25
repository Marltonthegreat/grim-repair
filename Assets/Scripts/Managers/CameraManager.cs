using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startCam;
    [SerializeField] private CinemachineVirtualCamera characterCams;
    private static CinemachineTargetGroup cameraTargets;

    private void Start()
    {
        cameraTargets = GetComponent<CinemachineTargetGroup>();
    }

    private void Update()
    {
        if (!cameraTargets.IsEmpty) SwapCameraPriority();
    }

    public void SwapCameraPriority()
    {
        startCam.Priority = 9;
        characterCams.Priority = 10;
    }

    public void AddCameraTarget(Transform transform)
    {
        cameraTargets.AddMember(transform, 1, 0);
    }

    public void RemoveCameraTarget(Transform transform)
    {
        cameraTargets.RemoveMember(transform);
    }
}
