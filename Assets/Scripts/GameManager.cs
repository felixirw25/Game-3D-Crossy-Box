using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;
    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    private int playerLastMaxTravel;

    public void SetPause(bool isPause){
        isPause=true;
        if(isPause){
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            gamePanel.SetActive(false);
        }
        else{
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
        }
    }
    private void Start()
    {
        // setup gameOver Panel
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);

        // belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        // depan
        for (int z = 1; z <= frontDistance; z++){
            var prefab = GetNextRandomTerrainPrefab(z);
            
            CreateTerrain(prefab, z);
        }

        player.SetUp(backDistance, extent);
    }
    private void Update(){
        // cek player apakah msi hidup?
        if(player.IsDie && gameOverPanel.activeInHierarchy==false){
            StartCoroutine(ShowGameOverPanel());
        }

        // Infinite Terrain System
        if(player.MaxTravel==playerLastMaxTravel)
            return;
        playerLastMaxTravel = player.MaxTravel;

        // Bikin ke depan
        var randTBPrefab = GetNextRandomTerrainPrefab(player.MaxTravel+frontDistance);
        CreateTerrain(randTBPrefab, player.MaxTravel+frontDistance);

        // Hapus belakang
        // Cara mudah tapi bisa error
        var lastTB = map[(player.MaxTravel-1)+backDistance];

        // Cara susah tapi tidak bisa error
        // TerrainBlock lastTB = map[player.MaxTravel+frontDistance];
        // int lastPos=player.MaxTravel;
        // foreach(var (pos, tb) in map){
        //     if(pos<lastPos){
        //         lastPos = pos;
        //         lastTB = tb;
        //     }
        // }

        map.Remove((player.MaxTravel-1)+backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel+backDistance, extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2);

        Debug.Log("GameOver");
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        scoreText.enabled = false;
    }

    private void CreateTerrain(GameObject prefab, int zPos){
        var go = Instantiate(prefab, new Vector3(0,0,zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos].GetType());
    }
    private GameObject GetNextRandomTerrainPrefab(int nextPos){
        bool isUniform=true;
        var tbRef = map[nextPos-1];
        for (int distance = 1; distance <= maxSameTerrainRepeat; distance++){
            if(map[nextPos - distance].GetType() != tbRef.GetType()){
                isUniform = false;
                break;
            }
        }
        if(isUniform){
            if(tbRef is Grass)
                return road;
            else
                return grass;
        }
        return Random.value>0.5f ? road : grass;
    }
    
}