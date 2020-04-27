using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Security.Cryptography;

public class Lobby : MonoBehaviourPunCallbacks
{
    public Button joinButton;
    public Button createButton;
    public InputField roomName;
    public Text log;
    public Storage storage;

    public override void OnEnable()
    {
        base.OnEnable(); // this is necesary because photon callback will get registered here otherwise you will not get photon callback
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            joinButton.interactable = false;
            createButton.interactable = false;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            joinButton.interactable = true;
            createButton.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log(PhotonNetwork.IsConnectedAndReady);
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected2");      
        joinButton.interactable = true;
        createButton.interactable = true;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("roomCreated");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joinedRoom");
        storage.login.SetScreen(storage.login.roomPanel);
        // update public key to cloud       
        storage.CreateAndUploadPublicKey();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        log.text = message;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        log.text = message;
    }

    public void OnCreateRoomButton()
    {
        PhotonNetwork.NickName = storage.userName;
        if (roomName.text != string.Empty)
        {
            RoomOptions roomOp = new RoomOptions();
            roomOp.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomName.text, roomOp);
        }
        else
        {
            log.text = "Room Name cannot be Empty";
        }
    }

    public void OnJoinRoomButton()
    {
        PhotonNetwork.NickName = storage.userName;
        if (roomName.text != string.Empty)
        {
            PhotonNetwork.JoinRoom(roomName.text);
        }
        else
        {
            log.text = "Room Name cannot be Empty";
        }
    }

    public void DisconnectServer()
    {
        PhotonNetwork.Disconnect();
    }

}
