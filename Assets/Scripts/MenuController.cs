using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class MenuController : Photon.PunBehaviour
{
    [SerializeField] private string versionName = "0.1";
    [SerializeField] private GameObject usernameMenu;
    [SerializeField] private GameObject connectPanel;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField createGameInput;
    [SerializeField] private InputField joinGameInput;
    [SerializeField] private GameObject startButton;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
    }

    private void Start()
    {
        usernameMenu.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void ChangeUsernameInput()
    {
        if (usernameInput.text.Length >= 3)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void SetUserame()
    {
        usernameMenu.SetActive(false);
        PhotonNetwork.playerName = usernameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { MaxPlayers = 5 }, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
