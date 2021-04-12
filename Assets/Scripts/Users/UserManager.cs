using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HomeAR.Events;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }
    [SerializeField] UserPool userPool;
    [SerializeField] GameObject userPrefab;
    [SerializeField] Transform userContainer;
    [SerializeField] UserEvent OnUserAdded;
    [SerializeField] Text inputName;
    [SerializeField] Text welcomMsg;
    public static int numOfScheduleSlots = 6;

    public static User curUser = new User(0, "Licheng");
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        RefreshUserList();
    }

    void RefreshUserList()
    {
        foreach (Transform child in userContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (User user in userPool.userList)
        {
            generateUserPrefab(user).transform.SetParent(userContainer);
        }
    }

    public void AddUser()
    {
        User newUser = new User(0, inputName.text);
        userPool.userList.Add(newUser);
        curUser = newUser;
        OnUserAdded.Raise(newUser);
        RefreshUserList();
    }

    GameObject generateUserPrefab(User user)
    {
        GameObject go = Instantiate(userPrefab, Vector3.zero, Quaternion.identity);
        go.transform.GetChild(0).GetComponent<Text>().text = user.name;
        return go;
    }

    public void ShowWelcomeMsg(User user)
    {
        welcomMsg.text = "Welcome, " + user.name;
    }

}
