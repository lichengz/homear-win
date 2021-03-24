using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New User Pool", menuName = "User Pool")]
public class UserPool : ScriptableObject
{
    public List<User> userList = new List<User>();
}
