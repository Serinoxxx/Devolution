using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperText : MonoBehaviour {

    public Animation helperTextAnim;
    public Text UIHelperText;
    public AudioSource source;

    [TextArea(3, 10)]
    public string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThirdPersonUserControl>())
        {
            DisplayMessage(message);
            Destroy(this);
        }
    }

    public void DisplayMessage(string msg)
    {
        UIHelperText.text = msg;
        helperTextAnim.Stop();
        helperTextAnim.Play();
        if (source != null)
            source.Play();
    }



}
