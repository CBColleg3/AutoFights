using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;

public class GetText : MonoBehaviour
{
    public Transform contentWindow;

    public GameObject recallTextObject;
    public string filename;
    int yOffset = 0;

    private void Start()
    {
        string readFromFilePath = Application.streamingAssetsPath + "/RecallChat/" + filename + ".txt";

        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        foreach (string line in fileLines)
        {
            yOffset += 10;
            Instantiate(recallTextObject, new Vector2(contentWindow.position.x, contentWindow.position.y + yOffset), Quaternion.identity);
            recallTextObject.GetComponent<TextMeshProUGUI>().SetText(line);
        }
    }
}
