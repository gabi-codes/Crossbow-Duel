using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject sceneCamera;
    public Text pingText;

    [HideInInspector] public GameObject LocalPlayer;
    public Text RespawnTimerText;
    public GameObject RespawnMenu;
    private float TimerAmount = 5f;
    private bool RunSpawnTimer = false;

    private void Awake()
    {
        Instance = this;
        gameCanvas.SetActive(true);
    }

    private void Update()
    {
        pingText.text = "Ping: " + PhotonNetwork.GetPing();
        if(RunSpawnTimer)
        {
            StartRespawn();
        }
    }

    public void RespawnLocation()
    {
        float randomValue = Random.Range(-1f, 1f);
        LocalPlayer.transform.localPosition = new Vector2(randomValue, 3f);
    }

    public void EnableRespawn()
    {
        TimerAmount = 5f;
        RunSpawnTimer = true;
        RespawnMenu.SetActive(true);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f, 1f);

        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
        gameCanvas.SetActive(false);
    }

    private void StartRespawn()
    {
        TimerAmount -= Time.deltaTime;
        RespawnTimerText.text = "Respawn in " + TimerAmount.ToString("F0");

        if(TimerAmount <= 0)
        {
            LocalPlayer.GetComponent<PhotonView>().RPC("Respawn", PhotonTargets.AllBuffered);
            LocalPlayer.GetComponent<Health>().EnableInput();
            RespawnLocation();
            RespawnMenu.SetActive(false);
            RunSpawnTimer = false;
        }
    }
}
