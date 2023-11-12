using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    float moveSpeed = 5.0f;
    float rotationSpeed = 90.0f;
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
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        rb = GetComponent<Rigidbody>();
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
            if(component.name.Contains("PW_") && component is Transform)
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

    

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        cookTime -= Time.deltaTime;
        if (cookTime <= 0f)
        {
            cookTime = 0f;
            transform.Find("Canvas/menu/waitTime").gameObject.GetComponent<TMP_Text>().SetText("Wait " + (int)cookTime+"s");
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = transform.forward * verticalInput * moveSpeed;
            // Применяем силу для перемещения объекта
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            // Рассчитываем угол поворота
            float rotation = horizontalInput * rotationSpeed * Time.deltaTime;

            // Поворачиваем объект
            rb.rotation *= Quaternion.Euler(0, rotation, 0);
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
    }

    async void CookToast()
    {
        foreach (var item in toast)
        {
            if (!item.activeSelf) {
                cookTime = cookTimeToast;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                item.SetActive(true);
                break;
            }
        } 
    }
    async void CookJuice()
    {
        foreach (var item in juice)
        {
            if (!item.activeSelf)
            {
                cookTime = cookTimeJuice;
                await Task.Delay(TimeSpan.FromSeconds(cookTime));
                item.SetActive(true);
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
}
