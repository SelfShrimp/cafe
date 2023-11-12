using HelloWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    private CharacterController controller;
    private float speed = 2.0F;
    private float gravity = 20.0F;
    float sensitivity = 2.0f;
    private Vector3 moveDirection = Vector3.zero;
    float cookTime = 0f;
    float cookTimeToast = 5f;
    float cookTimeJuice = 3f;
    float cookTimeCoockie = 7f;
    float cookTimeDonut = 10f;
    float cookTimeCake = 10f;

    Rigidbody rb;
    public Button[] buttons;
    public List<GameObject> toast;
    public List<GameObject> juice;
    public List<GameObject> coockie;
    public List<GameObject> donut;
    public List<GameObject> cake;
    public short money = 0;

    bool isRotating = false;

    PlayerController playerServer;

    public bool iAmServer = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (!IsOwner)
            return;
        Camera.main.GetComponent<CamScript>().target = transform;
        buttons[0].onClick.AddListener(CookToast);
        buttons[1].onClick.AddListener(CookJuice);
        buttons[2].onClick.AddListener(CookCoockie);
        buttons[3].onClick.AddListener(CookDonut);
        buttons[4].onClick.AddListener(CookCake);
        buttons[5].onClick.AddListener(AddCoockie);
        buttons[6].onClick.AddListener(AddDonut);
        buttons[7].onClick.AddListener(AddCake);

        var toastTag = GameObject.FindGameObjectWithTag("toast");
        var toast = toastTag.GetComponentsInChildren<Component>(true);
        foreach (var component in toast)
        {
            if (component.name.Contains("PW_") && component is Transform)
            {
                this.toast.Add(component.gameObject);
            }
        }
        var juiceTag = GameObject.FindGameObjectWithTag("juice");
        var juice = juiceTag.GetComponentsInChildren<Component>(true);
        foreach (var component in juice)
        {
            if (component.name.Contains("PW_") && component is Transform)
            {
                this.juice.Add(component.gameObject);
            }
        }
        var coockieTag = GameObject.FindGameObjectWithTag("coockie");
        var coockie = coockieTag.GetComponentsInChildren<Component>(true);
        foreach (var component in coockie)
        {
            if (component.name.Contains("PW_") && component is Transform)
            {
                this.coockie.Add(component.gameObject);
            }
        }
        var donutTag = GameObject.FindGameObjectWithTag("donut");
        var donut = donutTag.GetComponentsInChildren<Component>(true);
        foreach (var component in donut)
        {
            if (component.name.Contains("PW_") && component is Transform)
            {
                this.donut.Add(component.gameObject);
            }
        }
        var cakeTag = GameObject.FindGameObjectWithTag("cake");
        var cake = cakeTag.GetComponentsInChildren<Component>(true);
        foreach (var component in cake)
        {
            if (component.name.Contains("PW_") && component is Transform)
            {
                this.cake.Add(component.gameObject);
            }
        }
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        if (cookTime >= 0f)
        {
            cookTime -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<ColorSync>().ChangeColor();
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            transform.Rotate(0, mouseX, 0);
        }

        if (cookTime <= 0f)
        {
            cookTime = 0f;
            transform.Find("Canvas/menu/waitTime").gameObject.GetComponent<TMP_Text>().SetText("Wait " + (int)cookTime + "s");
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (transform.Find("Canvas/interaction").gameObject.activeSelf)
                {
                    transform.Find("Canvas/interaction").gameObject.SetActive(false);
                    transform.Find("Canvas/menu").gameObject.SetActive(true);
                }
                else if (transform.Find("Canvas/menu").gameObject.activeSelf)
                {
                    transform.Find("Canvas/interaction").gameObject.SetActive(true);
                    transform.Find("Canvas/menu").gameObject.SetActive(false);
                }
            }
        }
        else
        {
            transform.Find("Canvas/menu/waitTime").gameObject.GetComponent<TMP_Text>().SetText("Wait " + (int)cookTime + "s");
        }
        transform.Find("Canvas/money/moneyText").gameObject.GetComponent<TMP_Text>().SetText("Money: " + money);

        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players)
        {
            item.gameObject.GetComponent<PlayerController>().toast = toast;
            item.gameObject.GetComponent<PlayerController>().juice = juice;
            item.gameObject.GetComponent<PlayerController>().coockie = coockie;
            item.gameObject.GetComponent<PlayerController>().donut = donut;
            item.gameObject.GetComponent<PlayerController>().cake = cake;

        }
    }
    /*public class ObjectsData : INetworkSerializable
    {
        public GameObject[] toast;
        public List<GameObject> juice;
        public List<GameObject> cookie;
        public List<GameObject> donut;
        public List<GameObject> cake;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref toast);

        }
    }*/


    [ServerRpc]
    void CmdCookToastServerRpc(int index)
    {
        Debug.Log($"������ �������: ");
        RpcSetToastActiveClientRpc(index);
        /*foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
            NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerController>().toast[index].SetActive(true);*/
    }

    [ClientRpc]
    void RpcSetToastActiveClientRpc(int index)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach( var item in players) 
        {
            item.gameObject.GetComponent<PlayerController>().toast[index].SetActive(true);
        }
        Debug.Log($"������ �������: ");
        //this.toast[index].SetActive(true);
    }

    async void CookToast()
    {
        for (int i = 0; i < toast.Count; i++)
        {
            if (!toast[i].activeSelf)
            {
                cookTime = cookTimeToast;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                //toast[i].SetActive(true);
                CmdCookToastServerRpc(i);
                break;
            }
        }
    }

    [ServerRpc]
    void CmdCookJuiceServerRpc(int index)
    {
        RpcSetJuiceActiveClientRpc(index);
    }

    [ClientRpc]
    void RpcSetJuiceActiveClientRpc(int index)
    {
        juice[index].SetActive(true);
    }
    async void CookJuice()
    {
        for (int i = 0; i < juice.Count; i++)
        {
            if (!juice[i].activeSelf)
            {
                cookTime = cookTimeJuice;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                juice[i].SetActive(true);
                CmdCookJuiceServerRpc(i);
                break;
            }
        }
    }
    async void CookCoockie()
    {
        foreach (var item in coockie)
        {
            if (!item.activeSelf)
            {
                cookTime = cookTimeCoockie;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                item.SetActive(true);
                break;
            }
        }
    }

    async void CookDonut()
    {
        foreach (var item in donut)
        {
            if (!item.activeSelf)
            {
                cookTime = cookTimeDonut;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                item.SetActive(true);
                break;
            }
        }
    }
    async void CookCake()
    {
        foreach (var item in cake)
        {
            if (!item.activeSelf)
            {
                cookTime = cookTimeCake;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                item.SetActive(true);
                break;
            }
        }
    }

    void AddCoockie()
    {
        if (money >= 5)
        {
            money -= 5;
            buttons[2].interactable = true;
            buttons[5].interactable = false;
        }
    }
    void AddDonut()
    {
        if (money >= 10)
        {
            money -= 10;
            buttons[3].interactable = true;
            buttons[6].interactable = false;
        }
    }
    void AddCake()
    {
        if (money >= 10)
        {
            money -= 10;
            buttons[4].interactable = true;
            buttons[7].interactable = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "interactionZone")
        {
            Transform interactionPanel = transform.Find("Canvas/interaction");
            interactionPanel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "interactionZone")
        {
            transform.Find("Canvas/interaction").gameObject.SetActive(false);
            transform.Find("Canvas/menu").gameObject.SetActive(false);
        }
    }
}