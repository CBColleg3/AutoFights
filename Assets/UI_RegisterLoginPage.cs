using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RegisterLoginPage : MonoBehaviour
{
    public GameObject LoginPage;
    public GameObject RegisterPage;
    // Start is called before the first frame update
    void Start()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
    }

    public void GoToLoginPage()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
    }

    public void GoToRegisterPage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
    }
}
