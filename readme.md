---
title: Unity Epic
category: 6446526dddf659006c7ea807
order: 1
hidden: false
slug: unity-epic
---

> Link to repository  
> [GitHub](https://github.com/AppsFlyerSDK/appsflyer-unity-epic-sample-app)

## AppsFlyer Unity Epic SDK integration

AppsFlyer empowers gaming marketers to make better decisions by providing powerful tools to perform cross-platform attribution.

Game attribution requires the game to integrate the AppsFlyer SDK that records first opens, consecutive sessions, and in-app events. For example, purchase events.
We recommend you use this sample app as a reference for integrating the AppsFlyer SDK into your Unity Epic game.

<hr/>

## AppsflyerEpicModule - Interface

`AppsflyerEpicModule.cs`, included in the scenes folder, contains the required code and logic to connect to AppsFlyer servers and report events.

### AppsflyerEpicModule

This method receives your API key, app ID and parent MonoBehaviour and initializes the AppsFlyer Module.

**Method signature**

```
AppsflyerEpicModule(string appid, string devkey, MonoBehaviour mono)
```

**Usage**:

```
AppsflyerEpicModule afm = new AppsflyerEpicModule("EPIC_APP_ID", "DEV_KEY", this);
```

**Arguments**:

- `EPIC_APP_ID`: Found in the Epic store link
- `DEV_KEY`: Get from the marketer or [AppsFlyer HQ](https://support.appsflyer.com/hc/en-us/articles/211719806-App-settings-#general-app-settings).

### Start

This method sends first open/session requests to AppsFlyer.

**Method signature**

```
void Start(bool skipFirst = false)
```

**Usage**:

```
// without the flag
afm.Start();

// with the flag
bool skipFirst = [SOME_CONDITION];
afm.Start(skipFirst);
```

### LogEvent

This method receives an event name and JSON object and sends an in-app event to AppsFlyer.

**Method signature**

```
void LogEvent(string event_name, Dictionary<string, object> event_parameters)
```

**Usage**:

```
// set event name
string event_name = "af_purchase";
// set event values
Dictionary<string, object> event_parameters = new Dictionary<string, object>();
event_parameters.Add("af_currency", "USD");
event_parameters.Add("af_price", 6.66);
event_parameters.Add("af_revenue", 12.12);
// send logEvent request
afm.LogEvent(event_name, event_parameters);
```

## Running the sample app

1. Open Unity hub and open the project.
2. Use the sample code in AppsflyerEpicScript.cs and update it with your DEV_KEY and APP_ID.
3. Add the AppsflyerEpicScript to an empty game object (or use the one in the scenes folder):  
   ![Request-OK](https://files.readme.io/b271553-small-EpicGameObject.PNG)
4. Launch the sample app via the Unity editor and check that your debug log shows the following message:  
   ![Request-OK](https://files.readme.io/7105a10-small-202OK.PNG)
5. After 24 hours, the AppsFlyer dashboard updates and shows organic and non-organic installs and in-app events.

## Implementing AppsFlyer in your Epic game

### Setup

1. Add EOS to your Unity project. Follow the [Epic Online Services Unity plugin instructions](https://github.com/PlayEveryWare/eos_plugin_for_unity) and add it through your package manager.
2. Add `EOSManager.cs` to a game object.
3. Add the script from `Assets/Scenes/AppsflyerEpicModule.cs` to your app.
4. Use the sample code in `Assets/Scenes/AppsflyerEpicScript.cs` and update it with your `DEV_KEY` and `APP_ID`.
5. Initialize the SDK.

```
AppsflyerEpicModule afm = new AppsflyerEpicModule("DEV_KEY", "EPIC_APP_ID", this);
```

6. [Start](#public-void-startbool-skipfirst--false) the AppsFlyer integration.
7. Report [in-app events](#public-void-logeventstring-event_name-dictionarystring-object-event_parameters).
