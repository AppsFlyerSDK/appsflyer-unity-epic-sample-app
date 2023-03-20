using System;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

public class AppsflyerEpicModule
{
    public string devkey { get; }
    public string appid { get; }
    public int af_counter { get; set; }
    public string af_device_id { get; }
    public MonoBehaviour mono { get; }

    public AppsflyerEpicModule(string devkey, string appid, MonoBehaviour mono)
    {
        this.devkey = devkey;
        this.appid = appid;
        this.mono = mono;

        this.af_counter = PlayerPrefs.GetInt("af_counter");
        // Debug.Log("af_counter: " + af_counter);

        this.af_device_id = PlayerPrefs.GetString("af_device_id");

        //in case there's no AF-ID yet
        if (String.IsNullOrEmpty(af_device_id))
        {
            af_device_id = GenerateGuid();
            PlayerPrefs.SetString("af_device_id", af_device_id);
        }

        // Debug.Log("af_device_id: " + af_device_id);
    }

    // report first open event to AppsFlyer (or session if counter > 2)
    public void Start()
    {
        // Debug.Log(SystemInfo.deviceType + " | " + SystemInfo.deviceModel + " | " + SystemInfo.operatingSystem);

        // setting the device ids and request body
        DeviceIDs deviceid = new DeviceIDs { type = "custom", value = af_device_id };
        DeviceIDs[] deviceids = { deviceid };

        string deviceModel = SystemInfo.operatingSystem
            .Replace(" ", "-")
            .Replace("(", "")
            .Replace(")", "");
        if (deviceModel.Length > 24)
        {
            deviceModel = deviceModel.Substring(0, 24);
        }

        RequestData req = new RequestData
        {
            timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
            device_os_version = "1.0.0",
            device_model = deviceModel,
            app_version = "1.0.0",
            device_ids = deviceids,
            request_id = GenerateGuid(),
            limit_ad_tracking = false
        };

        // set request type
        AppsflyerRequestType REQ_TYPE =
            af_counter < 2
                ? AppsflyerRequestType.FIRST_OPEN_REQUEST
                : AppsflyerRequestType.SESSION_REQUEST;

        // post the request via steam http client
        mono.StartCoroutine(SendEpicPostReq(req, REQ_TYPE));
    }

    // report inapp event to AppsFlyer
    public void LogEvent(string event_name, string event_values)
    {
        // setting the device ids and request body
        DeviceIDs deviceid = new DeviceIDs { type = "custom", value = af_device_id };
        DeviceIDs[] deviceids = { deviceid };

        RequestData req = new RequestData
        {
            timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
            device_os_version = "1.0.0",
            device_model = SystemInfo.operatingSystem
                .Replace(" ", "-")
                .Replace("(", "")
                .Replace(")", ""),
            app_version = "1.0.0",
            device_ids = deviceids,
            request_id = GenerateGuid(),
            limit_ad_tracking = false,
            event_name = event_name,
            event_values = event_values
        };

        // set request type
        AppsflyerRequestType REQ_TYPE = AppsflyerRequestType.INAPP_EVENT_REQUEST;

        // post the request via steam http client
        mono.StartCoroutine(SendEpicPostReq(req, REQ_TYPE));
    }

    // send post request with Steam HTTP Client
    private IEnumerator SendEpicPostReq(RequestData req, AppsflyerRequestType REQ_TYPE)
    {
        // serialize the json and remove empty fields
        string json = JsonConvert.SerializeObject(
            req,
            Newtonsoft.Json.Formatting.None,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
        );
        // Debug.Log(json);

        // create auth token
        string auth = HmacSha256Digest(json, devkey);

        // define the url based on the request type
        string url;
        switch (REQ_TYPE)
        {
            case AppsflyerRequestType.FIRST_OPEN_REQUEST:
                url = "https://events.appsflyer.com/v1.0/c2s/first_open/app/epic/" + appid;
                break;
            case AppsflyerRequestType.SESSION_REQUEST:
                url = "https://events.appsflyer.com/v1.0/c2s/session/app/epic/" + appid;
                break;
            case AppsflyerRequestType.INAPP_EVENT_REQUEST:
                url = "https://events.appsflyer.com/v1.0/c2s/inapp/app/epic/" + appid;
                break;
            default:
                url = null;
                break;
        }

        // set the request body
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // set the request content type
        uwr.SetRequestHeader("Content-Type", "application/json");
        // set the authorization
        uwr.SetRequestHeader("Authorization", auth);

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        switch (REQ_TYPE)
        {
            case AppsflyerRequestType.FIRST_OPEN_REQUEST:
                Debug.Log("Request type: FIRST_OPEN_REQUEST");
                break;
            case AppsflyerRequestType.SESSION_REQUEST:
                Debug.Log("Request type: SESSION_REQUEST");
                PlayerPrefs.SetInt("af_counter", af_counter);
                break;
            case AppsflyerRequestType.INAPP_EVENT_REQUEST:
                Debug.Log("Request type: INAPP_EVENT_REQUEST");
                break;
        }
        Debug.Log("Is success: " + uwr.result);
        Debug.Log("Response Code:: " + uwr.responseCode);

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            // TODO: handle/log error
        }
        else
        {
            switch (REQ_TYPE)
            {
                // increase the appsflyer counter on a first-open/session request
                case AppsflyerRequestType.FIRST_OPEN_REQUEST:
                case AppsflyerRequestType.SESSION_REQUEST:
                    af_counter++;
                    PlayerPrefs.SetInt("af_counter", af_counter);
                    break;
                case AppsflyerRequestType.INAPP_EVENT_REQUEST:
                    break;
            }
        }

        // // attach the request to the handler based on the request type
        // switch (REQ_TYPE)
        // {
        //     case AppsflyerRequestType.FIRST_OPEN_REQUEST:
        //         OnHTTPCallBack(HTTPRequestCompleted_t pCallback, bool bIOFailure)
        //         break;
        //     case AppsflyerRequestType.SESSION_REQUEST:
        //         OnHTTPCallBack(HTTPRequestCompleted_t pCallback, bool bIOFailure)
        //         break;
        //     case AppsflyerRequestType.INAPP_EVENT_REQUEST:
        //         OnHTTPCallBack(HTTPRequestCompleted_t pCallback, bool bIOFailure)
        //         break;
        // }
    }

    // generate GUID for post request and AF id
    private string GenerateGuid()
    {
        Guid myuuid = Guid.NewGuid();
        return myuuid.ToString();
    }

    // handle HTTP callback from steam
    // private void OnHTTPCallBack(HTTPRequestCompleted_t pCallback, bool bIOFailure)
    // {
    //     // handle error
    //     if (!pCallback.m_bRequestSuccessful || bIOFailure)
    //     {
    //         Debug.LogError("ERROR sending req of type: " + pCallback.m_ulContextValue);
    //         Debug.LogError("status code: " + pCallback.m_eStatusCode);
    //     } //handle success
    //     else
    //     {
    //         Debug.Log("Success sending req of type: " + pCallback.m_ulContextValue);
    //         Debug.Log("status code: " + pCallback.m_eStatusCode);
    //         switch ((AppsflyerRequestType)pCallback.m_ulContextValue)
    //         {
    //             // increase the appsflyer counter on a first-open/session request
    //             case AppsflyerRequestType.FIRST_OPEN_REQUEST:
    //             case AppsflyerRequestType.SESSION_REQUEST:
    //                 af_counter++;
    //                 PlayerPrefs.SetInt("af_counter", af_counter);
    //                 break;
    //             case AppsflyerRequestType.INAPP_EVENT_REQUEST:
    //                 break;
    //         }
    //     }
    // }

    // generate hmac auth for post requests
    private string HmacSha256Digest(string message, string secret)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] keyBytes = encoding.GetBytes(secret);
        byte[] messageBytes = encoding.GetBytes(message);
        System.Security.Cryptography.HMACSHA256 cryptographer =
            new System.Security.Cryptography.HMACSHA256(keyBytes);

        byte[] bytes = cryptographer.ComputeHash(messageBytes);

        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}

[Flags]
enum AppsflyerRequestType : ulong
{
    FIRST_OPEN_REQUEST = 100,
    SESSION_REQUEST = 101,
    INAPP_EVENT_REQUEST = 102
}

[Serializable]
class RequestData
{
    public string timestamp;
    public string device_os_version;
    public string device_model;
    public string app_version;
    public DeviceIDs[] device_ids;
    public string request_id;
    public bool limit_ad_tracking;
    public string event_name;
    public string event_values;
}

[Serializable]
class DeviceIDs
{
    public string type;
    public string value;
}
