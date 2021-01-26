using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PhotonGameplayeManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CinemachineFreeLook freeLookCamera;

    private void Start()
    {
        Vector3 playerPos = new Vector3(Random.Range(-54, -46), -4, Random.Range(-10, 34));
        GameObject player= PhotonNetwork.Instantiate(playerPrefab.name, playerPos, Quaternion.identity);
        freeLookCamera.Follow = player.transform;
    }

}
