using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleCommands : MonoBehaviour
{
    public TMP_InputField titleInput;
    public bool focusedOnce = false;

    // Update is called once per frame
    void Update()
    {

        if (titleInput.isFocused) focusedOnce = true;
        if(Input.GetKeyDown(KeyCode.Return) && focusedOnce && titleInput.text == "build")
        {
            SceneManager.LoadScene("RoboMaker");
        }
        else if (Input.GetKeyDown(KeyCode.Return) && focusedOnce && titleInput.text == "fight")
        {
            SceneManager.LoadScene("CharacterSelect");
        }
    }
}
