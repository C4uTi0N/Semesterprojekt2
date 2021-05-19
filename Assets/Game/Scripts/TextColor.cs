using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColor : MonoBehaviour
{
    private Text textComp;
    public Color bigTextColor = new Color(50, 180, 255, 255);
    public Color smolTextColor = new Color(255, 100, 255, 255);

    // Start is called before the first frame update
    private void Awake()
    {
        textComp = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (textComp.text.Contains("Big:"))
        {
            textComp.color = bigTextColor;
        }

        if (textComp.text.Contains("Smol:"))
        {
            textComp.color = smolTextColor;
        }

        if (!textComp.text.Contains("Big:") && !textComp.text.Contains("Smol:"))
        {
            textComp.color = Color.white;
        }
    }
}