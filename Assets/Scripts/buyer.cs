using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Buyer : NetworkBehaviour
{
    MeshRenderer mesh;
    //public Player player;
    PlayerController player;
    float wait = 2f;
    float invis = 0f;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
            return;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        else
        {
            invis -= Time.deltaTime;
            if (invis <= 0f)
            {
                mesh.enabled = true;
                wait -= Time.deltaTime;
                if (wait <= 0f)
                {
                    wait = 2f;
                    var rand = new System.Random();
                    int choose = rand.Next(0, 4);
                    switch (choose)
                    {
                        case 0:
                            {
                                var toast = player.toast;
                                takeFood(toast);
                                if (invis > 0f) addMoneyServerRpc(1);
                                break;
                            }
                        case 1:
                            {
                                var juice = player.juice;
                                takeFood(juice);
                                if (invis > 0f) addMoneyServerRpc(1);
                                break;
                            }
                        case 2:
                            {
                                var coockie = player.coockie;
                                takeFood(coockie);
                                if (invis > 0f) addMoneyServerRpc(2);
                                break;
                            }
                        case 3:
                            {
                                var donut = player.donut;
                                takeFood(donut);
                                if (invis > 0f) addMoneyServerRpc(3);
                                break;
                            }
                        case 4:
                            {
                                var cake = player.cake;
                                takeFood(cake);
                                if (invis > 0f) addMoneyServerRpc(3);
                                break;
                            }
                    }
                }
            }
        }
    }

    [ServerRpc]
    void addMoneyServerRpc(short money)
    {
        addMoneyClientRpc(money);
    }

    [ClientRpc]
    void addMoneyClientRpc(short money)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players)
        {
            item.GetComponent<PlayerController>().money += money;
        }
    }

    private void takeFood(List<GameObject> foods)
    {
        foreach (var item in foods)
        {
            if (item.activeSelf)
            {
                item.SetActive(false);
                mesh.enabled = false;
                invis = 5f;
                wait = 5f;
                break;
            }
        }
    }
}
