# OpenAudio
We provide an asset to map all audio names to audioTypes.
so when you want to play an audio file, you only need to use audioType and forget about file names and strings.
you only need to define an audio type for each new audio file you add to your project.

## Usage 
- Create new enum for your audio types.
- put all your audio files in resource folder. (Resources root or inside any sub folder)
- for each one of your audio files, you need to add an item to your enum with THE EXACT SAME NAME! and use the API to play them.

you also can check out the sample folder in project.

## Commands
``` 
    // Play audio type
    AudioManager.Play(TEnum audioType, bool loop);

    // Stop audio type
    AudioManager.Stop(TEnum audioType);

    // Pause all audios
    AudioManager.StopAll();

    // Pause audio type
    AudioManager.Pause(TEnum audioType);

    // Set audio volume
    AudioManager.SetVolume(TEnum audioType, float volume);

    // Set all audio volumes
    AudioManager.SetVolume(float volume);

    // Mute audio manager
    AudioManager.Mute();

    // UnMute audio manager
    AudioManager.UnMute();


```
