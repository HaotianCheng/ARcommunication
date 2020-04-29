using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;
using System.Text;

public class RoomPanel : MonoBehaviourPunCallbacks
{
    public ObjectSpawner ObjectSpawner;
    public GameObject roomInfo;   
    public GameObject chatWidget;
    public GameObject startButton;
    public Login login;
    public Text roomMembers;
    public Text chatBox;
    public InputField chatField;

    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        UpdateRoomText();
        chatBox.text = "";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName);
        UpdateRoomText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ObjectSpawner.isChatting = false;
        UpdateRoomText();
        roomInfo.SetActive(true);
        chatWidget.SetActive(false);
    }

    private void UpdateRoomText()
    {
        roomMembers.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            roomMembers.text += player.NickName + "\n";
        }
    }

    public void OnStartChatButton()
    {
        ObjectSpawner.isChatting = true;
        roomInfo.SetActive(false);
        chatWidget.SetActive(true);
        photonView.RPC("StartChat", RpcTarget.Others);
    }

    [PunRPC]
    public void StartChat()
    {
        ObjectSpawner.isChatting = true;
        roomInfo.SetActive(false);
        chatWidget.SetActive(true);
    }

    public void OnLeaveButton()
    {
        ObjectSpawner.isChatting = false;
        roomInfo.SetActive(true);
        chatWidget.SetActive(false);
        PhotonNetwork.LeaveRoom();
        login.SetScreen(login.lobbyPanel);
    }

    public async void SendMessage(bool isSelf)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                             
                string xmlString = await login.storage.GetXmlFromUser(player.NickName); // get player's public key from cloud  
                string plainText = chatField.text;

                UnicodeEncoding ByteConverter = new UnicodeEncoding();
                byte[] dataToEncrypt = ByteConverter.GetBytes(plainText);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlString);
                byte[] cypherText = Security.RSAEncrypt(dataToEncrypt, rsa.ExportParameters(false), false);
                byte[] signedData;
                if (!isSelf) { signedData = Security.FakeHashAndSignBytes(dataToEncrypt); }
                else { signedData = Security.HashAndSignBytes(dataToEncrypt); }


                // send to player
                photonView.RPC("ReceiveText", player, cypherText, signedData, PhotonNetwork.LocalPlayer);

            }
        }
    }

    [PunRPC]
    public async void ReceiveText(byte[] cypherText, byte[] signedData, Player sender)
    {
        string xmlString = await login.storage.GetXmlFromUser(sender.NickName); // get sender's public key from cloud  
        Debug.Log(sender.NickName + "\n" + xmlString);
        byte[] decryptedData = Security.RSADecrypt(cypherText, false);

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xmlString);

        if (decryptedData != null && Security.VerifySignedHash(decryptedData, signedData, rsa.ExportParameters(false)))
        {
            Debug.Log("The data was verified.");

            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            DisplayMessage(ByteConverter.GetString(decryptedData), sender);
        }
        else
        {
            Debug.Log("The data does not match the signature.");
            chatBox.text = "Cipher Attack!!!";
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            Debug.Log(ByteConverter.GetString(decryptedData));
        }

    }

    public void DisplayMessage(string plainText,  Player sender)
    {
        chatBox.text = sender.NickName +": " + plainText;
    }

    public void OnSentMessageButton(bool isSelf)
    {
        DisplayMessage(chatField.text, PhotonNetwork.LocalPlayer);
        SendMessage(isSelf);
    }

}
