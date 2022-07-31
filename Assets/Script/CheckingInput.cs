using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckingInput : MonoBehaviour
{
    public GameObject warningText;
    //public Sprite[] sprites;
    public int minHeight = 40, maxHeight = 55;


    //Check - нет ошибок
    /// <summary>
    /// Проверка input на ошибки
    /// </summary>
    /// <param name="check">Условие для проверки input</param>
    /// <returns>Есть ли ошибка</returns>
    public bool Checking(bool check)
    {
        if (check)
        {
            var rectTransf = gameObject.GetComponent<RectTransform>();
            var sizeDelta = rectTransf.sizeDelta;
            rectTransf.sizeDelta = new Vector2(sizeDelta.x, minHeight);
            gameObject.GetComponentInChildren<Image>().color = Color.black;
            warningText.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
        else
        {
            var rectTransf = gameObject.GetComponent<RectTransform>();
            var sizeDelta = rectTransf.sizeDelta;
            rectTransf.sizeDelta = new Vector2(sizeDelta.x, maxHeight);
            gameObject.GetComponentInChildren<Image>().color = new Color32(244, 66, 54, 255);
            warningText.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
        return !check;
    }

}
