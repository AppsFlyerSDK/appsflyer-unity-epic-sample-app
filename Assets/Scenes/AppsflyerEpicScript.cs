using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

public class AppsflyerEpicScript : MonoBehaviour
{
    public string DEV_KEY;
    public string EPIC_APP_ID;

    void Start()
    {
        AppsflyerEpicModule afm = new AppsflyerEpicModule(DEV_KEY, EPIC_APP_ID, this);
        afm.Start();
        string af_uid = afm.GetAppsFlyerUID();

        bool newerDate = afm.IsInstallOlderThanDate("2023-06-13T10:00:00+02:00");
        bool olderDate = afm.IsInstallOlderThanDate("2023-02-11T10:00:00+02:00");
        Debug.Log("newerDate:" + (newerDate ? "true" : "false"));
        Debug.Log("olderDate:" + (olderDate ? "true" : "false"));
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
