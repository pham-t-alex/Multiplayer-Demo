using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        Instance = this;
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void StartSession()
    {
        SessionOptions options = new SessionOptions
        {
            MaxPlayers = 2,
            IsLocked = false,
            IsPrivate = false
        }.WithRelayNetwork();

        ISession session = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Code: {session.Code}");
    }

    public async void JoinSession(string code)
    {
        await MultiplayerService.Instance.JoinSessionByCodeAsync(code);
        Debug.Log("Joined session");
    }
}