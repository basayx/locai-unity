
# LOCAI 
#### One-Click Localization Solution for Games
https://youtu.be/6Mwvpk5kvWk

LOCAI, allows developers to add localization to their games with minimal workload. The developers can make their own games in their own native languages and let the LOCAI handle the whole localization process.

Unlike the other localization solutions, LOCAI works with text fields instead of text strings.
![Logo](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-logo.png)


All localization and translation operations are carried out in the background with the help of Gemini AI.
## Features

- One-click localization options
- Code-free using
- Supports for any kind of text scenario
- Allowing modification of the translation process easily
- Not only translates but aims to localize the texts
## Supported Game Engines

| Engine             | Availability  | Github Project Name |
| ----------------- | -----------------------------------------------|------------------- |
| Unity | Ready to use | locai-unity
| Godot | In development | -
| UE | Planned development | -

## Text Scenarios
LOCAI supports multiple text scenarios via various components.

![LOCAI Demo Dialogue Scene](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-dialogue.jpg)

To better understand which option is best for your game's texts, here is a scheme for this purpose:

![LOCAI Scehema](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-schema.jpg)

LOCAI supports 3 different localization and storing methods.

- #### Constant (Constant Meaning):
    If your text field always has the same meaning in every single language then use this.
    The translation process is done in the backend and stores all texts in all supported languages.

    *This solution fits well for menu buttons etc.*

- #### Runtime (Variable Meaning):
    If your text field stays the same for the same language but gets different meanings by active language then use this.
    The translation process is done in runtime and stored when a new language is activated that is not stored yet.

    *This solution fits well for language names etc.*

- #### Runtime Unstable (Undefined Meaning):
    If your text field gets various meanings regardless of language then use this.
    The translation process is done in runtime. It never stores any text.
    
    *This solution fits well for dialogues etc.*
    


  
## Showcase
### One-Click Menu
Via this menu option, you can add LOCAI components to all your texts with just one click.

This option has various options for various scenarios... Like, just adding to active objects or adding all object runtime components to less effort, etc.

Check the options to find the most suitable one for you:

![One-Click Menu GIF](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-oneclick.gif)


### Additional Prompts
Some text groups may need additional forwarding while applying the localization. For this purpose, you can add the LOCAI Additional Prompt component to the parent object of the texts. In this way, the texts check the additional prompt component from their parent to use it while sending a request to the Gemini AI.

In this example, as you can see, we are adding an additional prompt to our name label text:

![Additional Prompt GIF](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-additionalprompt.gif)


### Adding Multiple Language Support
LOCAI includes a lot of different language options that tested with Gemini. To support and add these languages to your game check the LOCAI Settings and add the target languages to the list.

![Language Support GIF](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-languagesupport.gif)


### Manual API Calls
LOCAI also supports manual calls for LOCAI functions. You can simply access Gemini API via LOCAIManager to get localization results for a target text.

```c#
    void ManualAPICallExample()
    {
        LOCAI.SupportedLanguages myTargetLanguage = SupportedLanguages.Korean;
        string myTextInOriginalLanguage = "Hey! This is a test!";
        
        LOCAIManager.SendGenerateContentRequest(myTargetLanguage, myTextInOriginalLanguage, Success, Fail);

        void Success(SupportedLanguages localizationLanguage, string localizedText)
        {
            //Do something here...
        }
        
        void Fail() { }
    }
```
## Getting Gemini API Key

You need to have a Gemini API Key to use the LOCAI. Check the below link to get one:
https://aistudio.google.com/app/u/1/apikey

> Keep it in mind! If you are using the *Runtime Unstable* type LOCAI components in your game then you need to keep the API key always active. But if you are using only the other two types LOCAI components then applying the localization for just one time will be enough and these components will not use the API key repeatedly.

After getting the API key you need to enter it into the API Key variable on the LOCAI Settings object.
  
## Deploying

LOCAI provides API Key variables for development purposes on LOCAI Settings. But do not recommend to deploy your Gemini API Key directly.

Instead of deploying your key directly, you can use various methods to keep your key safe and still access all LOCAI features.

![Deploy Options GIF](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-deploy0.jpg)

For this purpose, LOCAI provides 3 different predefined options to use in deploying phase:

- #### Using Google Cloud Function URL
    If you choose to use a Google Cloud Function to call Gemini API then you can simply past your function URL to a related field and LOCAI will work finely:

    ![Deploy Option 1](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-deploy1.png)

- #### Using Firebase Functions
    If you choose to use a Firebase Function to call Gemini API then you can simply past your Firebase function name to the related field after adding the Firebase SDK to your project:

    ![Deploy Option 2](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-deploy2.png)

> You can find an example function in the LOCAI Example folder to use in Firebase and Google Cloud.

- #### Using a Manual Key Vault Solution
    If you chose to use a Key Vault solution like *Google Cloud Key Management* or *Azure Key Vault* to get your API Key, then, you need to edit "LOCAIKeyVaultGateway" script and write a manual logic to forward the API key from the Key Vault solution to the releated variable inside of that script.
    
    LOCAI we will access to LOCAIKeyVaultGateway script to get the API key.

    ![Deploy Option 3](https://github.com/basayx/locai-unity/blob/main/repo-images/locai-deploy3.png)
