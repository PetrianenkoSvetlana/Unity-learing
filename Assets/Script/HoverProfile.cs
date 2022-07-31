using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverProfile : MonoBehaviour
{
    [SerializeField]
    private GameObject iconDelete;

    private void Start()
    {
        //iconDelete.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponentInParent<LoadProfile>().ActiveDeleteWindow(gameObject.name));
    }

    private void OnMouseEnter()
    {
        iconDelete.SetActive(true);
        //gameObject.GetComponent<Image>().color = Color.red;
    }

    private void OnMouseExit()
    {
        iconDelete.SetActive(false);
        //gameObject.GetComponent<Image>().color = Color.green;
    }


}
