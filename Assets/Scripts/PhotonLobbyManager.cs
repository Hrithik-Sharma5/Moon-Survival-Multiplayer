using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField playerNameInput;
    [SerializeField] InputField roomNameInputField;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject insideRoomPanel;
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] GameObject roomListPanel;
    [SerializeField] GameObject[] allPanels;

    void Start()
    {

    }

    #region Callbacks for buttons

    public void OnEnterNameButton()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnCreateRoomButton()
    {
        string roomname = roomNameInputField.text;
        RoomOptions roomOption = new RoomOptions();

        if (!string.IsNullOrEmpty(roomname))
        {
            PhotonNetwork.CreateRoom(roomname, roomOption); 
        }
    }

    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        ActivatePanel(startPanel);
    }

    public override void OnCreatedRoom()
    {
        ActivatePanel(insideRoomPanel);
    }


    #endregion

    #region Helper Functions

    private void ActivatePanel(GameObject panelToActivate)
    {
        foreach (GameObject item in allPanels)
        {
            if (item.name != panelToActivate.name)
            {
                item.SetActive(false);
            }
            else item.SetActive(true);
        }
    }


    #endregion 

}
