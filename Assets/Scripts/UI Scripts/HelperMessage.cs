using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HelperMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string messageHeader = "";
    [Multiline]public string messageContex = "";
    public bool useOwnColor = false;
    private Color col = Color.white;
    private bool showHelper = false;

    private void Start()
    {
        showHelper = false;
        if (UIHandler.Instance.defaultHeaderColor != null && !useOwnColor) col = UIHandler.Instance.defaultHeaderColor;
        else if (useOwnColor && transform.GetComponent<Image>() != null) col = transform.GetComponent<Image>().color;
        else if (useOwnColor && transform.GetComponent<TextMeshProUGUI>() != null) col = transform.GetComponent<TextMeshProUGUI>().color;
        else if (useOwnColor && transform.GetComponent<Text>() != null) col = transform.GetComponent<Text>().color;
    }

    private void Update()
    {
        if (showHelper)
        {
            UIHandler.Instance.HelperObject.transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showHelper = true;
        UIHandler.Instance.HelperObject.SetActive(true);
        UIHandler.Instance.HelperObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = messageHeader;
        UIHandler.Instance.HelperObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = col;
        UIHandler.Instance.HelperObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = messageContex;
        Canvas.ForceUpdateCanvases();
        UIHandler.Instance.HelperObject.transform.GetComponent<VerticalLayoutGroup>().enabled = false;
        UIHandler.Instance.HelperObject.transform.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showHelper = false;
        UIHandler.Instance.HelperObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Header";
        UIHandler.Instance.HelperObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Text";
        UIHandler.Instance.HelperObject.SetActive(false);
    }
}
