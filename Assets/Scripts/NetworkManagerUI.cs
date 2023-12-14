using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField]
    private Button btn_Server;
    [SerializeField]
    private Button btn_Client;
    [SerializeField]
    private Button btn_Disconnect;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        btn_Server.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            OnNetChanged(true);
        });
        btn_Client.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            OnNetChanged(true);
        });
        btn_Disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            OnNetChanged(false);
        });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (OwnerClientId == clientId)
            {
                OnNetChanged(true);
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (clientId) =>
        {
            if (OwnerClientId == clientId)
            {
                OnNetChanged(false);
            }
        };
    }

    private void OnNetChanged(bool connected)
    {
        if (connected)
        {
            btn_Server.gameObject.SetActive(false);
            btn_Client.gameObject.SetActive(false);
            btn_Disconnect.gameObject.SetActive(true);
        }
        else
        {
            btn_Server.gameObject.SetActive(true);
            btn_Client.gameObject.SetActive(true);
            btn_Disconnect.gameObject.SetActive(false);
        }
    }
}