# UGFEE: Unity Growth &amp; Funnel Engineer Exam

This exam is designed to test your abilities in the following areas:
- UI Implementation
- C# Proficiency
- Analytics Tracking
- A/B/C Test Implementation

It should take you approximately 2-4 hours to finish this exam. You may submit your completed exam to us however you see fit (i.e.: Email, project fork).

This project was created with Unity 6000.0.38f1.

## Instructions
In `MainScene.unity`, a skeleton UI has been created for you, consisting of a Welcome screen and a "Lead Source" screen (how the user heard about us) which appears after the Welcome screen. 

Your tasks are as follows:
- Create two new UI Screens
- Set up an A/B/C test which determines:
  - What screen shows up after the Lead Source screen
  - What price the user will pay, if they are presented with a Paywall
- Gather anayltics on how the user interacts with the UI

## UI Implementation
### UI Implementation, Part One
- In `Assets/UI REFERENCE`, a UI Designer has created a mockup of a Free-Trial UI, with a reference resolution of 480 x 800. Implement this UI in Unity. It should look acceptable in a variety of aspect ratios in a Portrait orientation.
- SDF Fonts you can use are found in `Assets/Fonts`. Please use TextMeshPro for all Text-based UI
- Create a Prefab of your completed UI in the `Prefabs` folder.

### UI Implementation, Part Two
- In `Assets/UI REFERENCE`, a UI Designer has created a mockup of a Paywall UI, with a reference resoultion of 480 x 800. There are price points for two subscription products. Implement this UI in Unity, using the same rules in UI Implementation, Part One.
- A user should be able to click on either subscription product, observe that it is selected, and then then click "Start Free Trial" to make their selection.
- You are not required to implement any payment or storefront features!
- Create a prefab of your completed UI in the `Prefabs` folder.

## C# Proficiency
### A Note on Architecture / Design Patterns Used
We rely heavily on a Service Locator design pattern. Our `ServiceLocator` contains `IServices` which implement pieces of functionality, and are `Bind()` / `Unbind()` to/from the Service Locator during the application lifecycle. An IService is usually abstracted by an interface and then implemented via a concrete class. In the coding part of this exam, you will write several IServices to implement A/B Testing and Analytics functionality.

###A/B/C testing
In this section of the exam, we will assign our user to an A, B, or C cohort at runtime, and load appropriate prefabs / values based on the user's cohort. To get you started, we have suggested a JSON format in `Assets/Resources/configuration.json`. You may use any format you like that satisfies the requirements.

### Configuration Service
-  Review `Assets/Scripts/Util/IConfigService.cs` Create an `IConfigService` called `LocalConfigService.cs` which does the following:
   - Implements IConfigService 
   - Reads JSON stored in configuration.json which resides in the `Resources` folder
   - Deserializes the JSON into a data structure within `FetchConfig()`
   - Allows for the retrieval of values through `GetValue()`
 
### Bootstrapping our test
- In `Assets/Scripts/Bootstrapper.cs`, write code that randomly assigns the user to one of the following cohorts: `VariantA`, `VariantB`, or `VariantC`
- Bind the `IConfigService` that you wrote to the `ServiceLocator`, and deserialize the contents of the configuration JSON.
- In the LeadSource UI, once the user clicks the "Next" button:
  - If the user is a member of Variant A, ensure that they are presented with the "Free-Trial" UI. 
  - If the user is a member of Variant B or C, ensure that they are presented with the "Paywall" UI
    - Ensure the prices for the Annual and Monthly Subscription products are displayed as defined in your configuration JSON.
   
### Analytics Collection
Now that we have a working A/B/C Test set up, we need to be able to know how it performs. In a real world-application, we would send this data to a tool like MixPanel using their Unity API: https://docs.mixpanel.com/docs/tracking-methods/sdks/unity

A good analytics implementation allows us to:
- Register a user, with metadata (i.e.: a Unique ID to identify them, what cohort they are a member of, etc.)
- Know what UI screens the user sees
- Track interactions with the application (button clicks, etc.)

Such an implementation is described in `Assets/Scripts/Util/IAnalyticsService.cs`. Review it, and implement an LocalAnalyticsService which does the following:
- Implements IAnalyticsService
- Stores `ALL ANALYTICS` collected somewhere locally (PlayerPrefs, a file - whatever you choose!)
  - `ALL ANALYTICS` is purposely defined loosely - it is up to you to track what the user has seen, and how they have interacted with the application!
- `Flush()`es the data to a "source of truth" for analysis. In a real-world application, analytics would be aggregated to a tool such as MixPanel.
  - `LocalAnalyticsService` can simply print all the analytics you've collected to the console via Debug.Log, once the user clicks "Start Free Trial" on the Free Trial / Paywall UI Screens.
