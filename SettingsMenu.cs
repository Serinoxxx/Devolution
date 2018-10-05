using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class SettingsMenu : MonoBehaviour {

    public GameObject SettingsPanel;
    public Slider sliderSens;

    private Animator _anim;

    private TimeScaler _scaler;
    private void Start()
    {
        _anim = gameObject.GetComponent<Animator>();

        _scaler = new TimeScaler(0f);
    }
    // Update is called once per frame
    void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            if (SettingsPanel.activeInHierarchy)
            {
                //Resume and Close Menu
                Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = false;
                Camera.main.GetComponent<CameraOrbit2>().SetMouseSensitivity(sliderSens.value);
                TimeScaleManager.instance.RemoveTimeScale(_scaler);
                TimeScaleManager.instance.UnscaleAnimators();
                _anim.SetTrigger("Deactivate");
                SettingsPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                //Pause Menu
                Camera.main.GetComponent<CameraOrbit2>().CameraDisabled = true;
                sliderSens.value = Camera.main.GetComponent<CameraOrbit2>().MouseSensitivity / 10f;
                TimeScaleManager.instance.AddTimeScale(_scaler);
                TimeScaleManager.instance.NormaliseAnimators();
                _anim.SetTrigger("Activate");
                SettingsPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
	}

    public void OnReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnMainMenu()
    {

    }
}
