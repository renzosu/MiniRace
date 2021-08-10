using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject sceneCamera;
    public GameObject minimap;
    public GameObject Ping;
    public GameObject Speedometer;
    public Text PingText;
    public Text SpeedometerText;
    public GameObject disconnectUI;
    private bool isOff = false;

    public GameObject joinLeaveFeed;
    public GameObject joinLeaveText;
    public GameObject gameMusic;
    private GameObject myPlayerGO;

    private void Awake()
    {
        gameCanvas.SetActive(true);
    }
    private void Update()
    {
        CheckInput();
        PingText.text = "PING: " + PhotonNetwork.GetPing();

        UpdateSpeedometer();
    }
    private void CheckInput()
    {
        if (isOff && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(false);
            isOff = false;
        }
        else if (!isOff && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(true);
            isOff = true;
        }
    }
    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-0.5f, 0.5f);
        myPlayerGO = PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(this.transform.position.x * 1, this.transform.position.y), Quaternion.identity, 0);
        myPlayerGO.GetComponent<CarController>().enabled = true;
        myPlayerGO.GetComponent<CarInputHandler>().enabled = true;
        myPlayerGO.GetComponentInChildren<Camera>().enabled = true;
        myPlayerGO.transform.Find("Camera").GetComponentInChildren<Camera>().enabled = true;
        myPlayerGO.transform.Find("MiniMapCamera").GetComponentInChildren<Camera>().enabled = true;
        myPlayerGO.GetComponent<CarSfxHandler>().enabled = true;

        // myPlayerGO.transform.Find("Trails").transform.Find("TrailRendererLeftWheel").GetComponentInChildren<TrailRenderer>().enabled = true;
        // myPlayerGO.transform.Find("Trails").transform.Find("TrailRendererLeftWheel").GetComponentInChildren<TrailRendererHandler>().enabled = true;
        // myPlayerGO.transform.Find("Trails").transform.Find("TrailRendererRightWheel").GetComponentInChildren<TrailRenderer>().enabled = true;
        // myPlayerGO.transform.Find("Trails").transform.Find("TrailRendererRightWheel").GetComponentInChildren<TrailRendererHandler>().enabled = true;

        gameCanvas.SetActive(false);
        sceneCamera.SetActive(false);
        minimap.SetActive(true);
        Ping.SetActive(true);
        Speedometer.SetActive(true);
        gameMusic.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(joinLeaveText, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(joinLeaveFeed.transform, false);
        obj.GetComponent<Text>().text = player.NickName + " joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(joinLeaveText, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(joinLeaveFeed.transform, false);
        obj.GetComponent<Text>().text = player.NickName + " left the game";
        obj.GetComponent<Text>().color = Color.red;
    }

    private void UpdateSpeedometer()
    {
        if (myPlayerGO != null)
        {
            float xVelo = Mathf.Abs(myPlayerGO.GetComponent<Rigidbody2D>().GetPointVelocity(transform.position).x / 3);
            float yVelo = Mathf.Abs(myPlayerGO.GetComponent<Rigidbody2D>().GetPointVelocity(transform.position).y / 3);

            if (xVelo >= yVelo)
                SpeedometerText.text = Mathf.RoundToInt(xVelo) + " KM/H";
            else SpeedometerText.text = Mathf.RoundToInt(yVelo) + " KM/H";
        }
    }
}
