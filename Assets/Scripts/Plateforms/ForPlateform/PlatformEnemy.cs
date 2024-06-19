using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    public List<Transform> players;
    private PlayerUI _ui;

    [SerializeField] private bool bossPlatform;
    [SerializeField] private Enemy enemy;
    [SerializeField] private string bossName;
    
    private void Start()
    {
        if (bossPlatform) Invoke(nameof(SetUi), 2);
    }

    private void SetUi()
    {
        _ui = PlayerManager.LocalPlayerInstance.gameObject.GetComponent<PlayerManager>().ui.GetComponent<PlayerUI>();
        _ui.bossName.text = bossName;
    }
    
    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            if (!_ui.bossBar.activeSelf && bossPlatform) _ui.bossBar.SetActive(true);
            if (!players.Contains(obj.gameObject.transform)) players.Add(obj.gameObject.transform);
        }
    }
    private void OnCollisionStay(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            if (!players.Contains(obj.gameObject.transform))
            {
                players.Add(obj.gameObject.transform);
            }
        }
    }
    private void OnCollisionExit (Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(JumpDelay),2.5f);
        }
    }
    
    void Update()
    {
        if (bossPlatform && _ui.bossBar.activeSelf)
        {
            _ui.bossBarFill.localScale = new Vector3(1f, enemy.Health / 100, 1f);
        }
        
        for(int i = 0; i < players.Count;i++)
        {
            if (players[i].GetComponent<Player>().IsRespawning) 
            {
                players.RemoveAt(i);
            }
        }
    }
    private void JumpDelay()
    {
        for(int i = 0; i < players.Count;i++)
        {
            if (!players[i].GetComponent<CapsuleCollider>().bounds.Intersects(this.GetComponent<MeshCollider>().bounds))
            {
                players.RemoveAt(i);
            }
        }
    }
}