using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPanel<T> : Panel where T : Panel
{
    public static TabPanel<T> current;
    public override void React()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            current = null;
        }
        else
        {
            if (current)
            {
               current.gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
            current = this;
        }
    }
}
