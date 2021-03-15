# OpenFreeAPI.Google.Translate

Free Google Translate API.

## Step-by-step guide

Before all, we need to using namespace;

```csharp
using OpenFreeAPI.Google.Translate;
using OpenFreeAPI.Google.Translate.Models;
```

Then, we create a translator instance. We use translating French to English as an example:

```csharp
var translator = new GoogleTranslator(SupportedLanguageEnum.fr, SupportedLanguageEnum.en);
```

After that, you can translate something:

```csharp
string text = "Bonjour!";
string eng = translator.Trasnlate(text);
```

Or maybe you need async, you can use:

```csharp
string text = "Bonjour!";
string eng = await translator.TrasnlateAsync(text);
```

## Credit

Thanks to all the projects (in no particular order) listed below.

| Owner | Link | License |
| ----- | ---- | ------- |
| [GitHub@franck-gaspoz](https://github.com/franck-gaspoz) | <https://github.com/franck-gaspoz/GTranslatorAPI> | MIT |
| [GitHub@Guila767](https://github.com/Guila767) | <https://github.com/Guila767/GoogleTranslateApi> | MIT |