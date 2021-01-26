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
    [SerializeField] GameObject playerInfoPrefab;
    [SerializeField] GameObject playerInfoPrefabParent;
    [SerializeField] GameObject roomPlayerPrefab;
    [SerializeField] GameObject roomPlayerPrefabParent;
    [SerializeField] GameObject startGameButton;
    [SerializeField] Text currentStatus;

    private Dictionary<string, RoomInfo> roomListInfo;
    private Dictionary<string, GameObject> roomListInfoGameobject;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        roomListInfo = new Dictionary<string, RoomInfo>();
        roomListInfoGameobject = new Dictionary<string, GameObject>();
    }

    //private void Update()
    //{
    //    currentStatus.text = "Current state: "+PhotonNetwork.NetworkClientState;
    //}


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

    public void OnShowRoomListButton() 
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(roomListPanel);
    }

    public void OnStartGameButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Gameplay");
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();

        foreach (RoomInfo room in roomList)
        {
            if (roomListInfo.ContainsKey(room.Name))
            {
                roomListInfo.Remove(room.Name);
            }
            roomListInfo.Add(room.Name, room);
            GameObject roomListPlayerinfo = Instantiate(roomPlayerPrefab, roomPlayerPrefabParent.transform);
            roomListPlayerinfo.transform.Find("Room Name").GetComponent<Text>().text = room.Name;
            roomListPlayerinfo.transform.Find("Join Room").GetComponent<Button>().onClick.AddListener(()=>AddToRoom(room.Name));
            roomListInfoGameobject.Add(room.Name, roomListPlayerinfo);
        }


    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) startGameButton.SetActive(true);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerListGameobject = Instantiate(playerInfoPrefab, playerInfoPrefabParent.transform);
            playerListGameobject.transform.Find("Player Name").GetComponent<Text>().text = player.NickName;
        }
        ActivatePanel(insideRoomPanel);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerListGameobject = Instantiate(playerInfoPrefab, playerInfoPrefabParent.transform);
        playerListGameobject.transform.Find("Player Name").GetComponent<Text>().text = newPlayer.NickName;
    }

    public override void OnLeftLobby()
    {
        ClearRoomList();
        roomListInfoGameobject.Clear();
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

    private void AddToRoom(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);
    }

    private void ClearRoomList()
    {
        foreach (var item in roomListInfoGameobject.Values)
        {
            Destroy(item);
        }

        roomListInfoGameobject.Clear();
    }

    #endregion 

}
