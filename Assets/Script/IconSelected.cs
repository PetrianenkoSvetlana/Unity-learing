using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IconSelected : MonoBehaviour
{

    public Sprite[] icons;
    private int index = 0;

    public string passwordToEdit = "My Password";

    // Start is called before the first frame update
    void Start()
    {
        //transform.GetChild(0).GetComponent<Image>().sprite = icons[^1];
        transform.GetChild(1).GetComponent<Image>().sprite = icons[index];
        //transform.GetChild(2).GetComponent<Image>().sprite = icons[1];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LeftMove()
    {
        transform.localPosition = Vector3.zero;
        transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == icons.Length - 1 ? 0 : index + 1;
        transform.GetChild(2).GetComponent<Image>().sprite = icons[index];

        transform.DOLocalMoveX(-160, 0.5f).Restart();
    }

    public void RightMove()
    {
        transform.localPosition = Vector3.zero;
        transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == 0 ? icons.Length - 1 : index - 1;
        transform.GetChild(0).GetComponent<Image>().sprite = icons[index];

        transform.DOLocalMoveX(160, 0.5f).Restart();
    }
}
