using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.IO;
using SimpleFileBrowser;

public class MakeFile : MonoBehaviour
{
    public InputField robotNameField;
    public TMP_InputField robotScriptField;
    public string filename;
    string path;

    public void CreateText()
    {
        FileBrowser.SetDefaultFilter(".txt");
        //Path of the file
        SetFileName();
        //string path = Application.streamingAssetsPath + "/RecallChat/" + filename + ".txt";
        if(filename != "" && robotScriptField.text != "")
        {
            //path = EditorUtility.SaveFilePanel("Save your Script", "", "robot", "txt");
            //File.WriteAllText(path, robotScriptField.text);
            StartCoroutine(ShowSaveDialogCoroutine());

            //Create the file if it doesn't exists
           // string content = "Login date: " + System.DateTime.Now + "\n";
          

        }

        //Content of the file
    }

    public void SetFileName()
    {
        if (robotNameField.text != "")
        {
            filename = robotNameField.text;
        }
    }

    IEnumerator ShowSaveDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, filename + ".txt", "Save your Bot", "Save");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            // byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            // Or, copy the first file to persistentDataPath
            path = FileBrowser.Result[0];
            //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], path);
            File.WriteAllText(path, robotScriptField.text);
            //DisplayIt();
        }
    }


    /*
    public void SaveExplorer()
    {
        path = EditorUtility.SaveFilePanel("Save your Script", "", "robot", "txt");

    }
    */
}
