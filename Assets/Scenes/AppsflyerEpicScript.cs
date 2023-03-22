using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class AppsflyerEpicScript : MonoBehaviour
{
    void Start()
    {
        AppsflyerEpicModule afm = new AppsflyerEpicModule("DEV_KEY", "EPIC_APP_ID", this);
        afm.Start();

        // //set event name
        string event_name = "af_purchase";
        //set json string
        string event_values = "{\"af_currency\":\"USD\",\"af_price\":6.66,\"af_revenue\":24.12}";
        afm.LogEvent(event_name, event_values);
    }

    private void Update() { }
}
