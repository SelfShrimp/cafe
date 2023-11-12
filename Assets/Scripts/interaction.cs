using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
        /*if (other.tag == "Player")
        {
            Transform playerTransform = other.transform;
            Transform interactionPanel = playerTransform.Find("Canvas/interaction");
            interactionPanel.gameObject.SetActive(true);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.tag == "Player")
        {
            Transform playerTransform = other.transform;
            playerTransform.Find("Canvas/interaction").gameObject.SetActive(false);
            playerTransform.Find("Canvas/menu").gameObject.SetActive(false);
        }*/
    }
}
