using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HomeAR.Managers;

[System.Serializable]
public class Schedule
{
    public User[] slots;
    public Schedule()
    {
        slots = new User[UserManager.numOfScheduleSlots];
    }

    public void InsertSlot(int index)
    {
        if (UserManager.curUser == null)
        {
            UserManager.curUser = new User(0, "Licheng");
        }
        slots[index] = UserManager.curUser;
    }
}
