using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public TMP_InputField CodeInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinSession()
    {
        SessionManager.Instance.JoinSession(CodeInput.text);
    }
}
