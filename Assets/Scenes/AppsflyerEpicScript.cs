using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

public class AppsflyerEpicScript : MonoBehaviour
{
    void Start()
    {
        AppsflyerEpicModule afm = new AppsflyerEpicModule("DEV_KEY", "EPIC_APP_ID", this);
        afm.Start();
        string af_uid = afm.GetAppsFlyerUID();

        // set event name
        string event_name = "af_purchase";
        // set event values
        Dictionary<string, object> event_parameters = new Dictionary<string, object>();
        event_parameters.Add("af_currency", "USD");
        event_parameters.Add("af_price", 6.66);
        event_parameters.Add("af_revenue", 12.12);
        // send logEvent request
        afm.LogEvent(event_name, event_parameters);
    }

    private void Update() { }
}
