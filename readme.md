---
title: Unity Epic
category: 6446526dddf659006c7ea807
order: 1
hidden: false
slug: unity-epic
---

> Link to repository  
> [GitHub](https://github.com/AppsFlyerSDK/appsflyer-unity-epic-sample-app)

# AppsFlyer Unity Epic SDK integration

AppsFlyer empowers gaming marketers to make better decisions by providing powerful tools to perform cross-platform attribution.

Game attribution requires the game to integrate the AppsFlyer SDK that records first opens, consecutive sessions, and in-app events. For example, purchase events.
We recommend you use this sample app as a reference for integrating the AppsFlyer SDK into your Unity Epic game.

<hr/>

## AppsflyerEpicModule - Interface

`AppsflyerEpicModule.cs`, included in the scenes folder, contains the required code and logic to connect to AppsFlyer servers and report events.

### `AppsflyerEpicModule(string appid, string devkey)`

This method receives your API key and app ID and initializes the AppsFlyer Module.

**Usage**:

```
AppsflyerEpicModule afm = new AppsflyerEpicModule("EPIC_APP_ID", "DEV_KEY");
```

**Arguments**:

- `EPIC_APP_ID`: Found in the Epic store link
- `DEV_KEY`: Get from the marketer or [AppsFlyer HQ](https://support.appsflyer.com/hc/en-us/articles/211719806-App-settings-#general-app-settings).

### `public void Start()`

This method sends first open and session requests to AppsFlyer.

**Usage**:

```
afm.Start();
```

### `public void LogEvent(string event_name, string event_values)`

This method receives an event name and JSON object and sends an in-app event to AppsFlyer.

**Usage**:

```
//set event name
string event_name = "af_purchase";
//set json string
string event_values = "{\"af_currency\":\"USD\",\"af_price\":6.66,\"af_revenue\":24.12}";
afm.LogEvent(event_name, event_values);
```

## Running the sample app

1. Open Unity hub and open the project.
2. Use the sample code in AppsflyerEpicScript.cs and update it with your DEV_KEY and APP_ID.
3. Add the AppsflyerEpicScript to an empty game object (or use the one in the scenes folder):  
   ![Request-OK](https://files.readme.io/b271553-small-EpicGameObject.PNG)
4. Launch the sample app via the Unity editor and check that your debug log shows the following message:  
   ![Request-OK](https://files.readme.io/7105a10-small-202OK.PNG)
5. After 24 hours, the AppsFlyer dashboard updates and shows organic and non-organic installs and in-app events.
