﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;
//using SmartDLL;
using SimpleFileBrowser;


public class FileManager : MonoBehaviour
{
    string path;
    string savePath;
    string fileName;

    public string[] importText;
    List<string> formattedText = new List<string>();

    private void Start()
    {
        FileBrowser.SetDefaultFilter(".txt");
    }

    public void OpenExplorer()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Load your Bot", "Load");

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
            path = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], path);
            DisplayIt();
        }
    }



    void DisplayIt()
    {
        if(path != null)
        {
            //start coroutine
           GetText();
        } else
        {
            print("path is null!");
        }
    }

    void GetText()
    {
        //importText = "";
  
        formattedText.Clear();
        importText = File.ReadAllLines(path);
        foreach(string text in importText)
        {
            formattedText.AddRange(text.Split(';'));
        }
        importText = formattedText.Where(x => !string.IsNullOrEmpty(x)).ToArray();
    }

}

