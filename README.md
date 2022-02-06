# YtbToMp3

Script for downloading and converting YouTube videos to mp3 files.

### Arguments

1. Text file with YouTube URL addresses, e.g.:

   song-urls.txt
    ```
    https://www.youtube.com/watch?v=Il0S8BoucSA
    https://www.youtube.com/watch?v=2Vv-BfVoq4g
    https://www.youtube.com/watch?v=Qz9gmiLBVFA
    https://www.youtube.com/watch?v=KrgJp7Z1Hv8
    https://www.youtube.com/watch?v=09R8_2nJtjg
    ```
3. Output directory where to save downloaded MP3s. (Default: ".")

### Run the Script

```
>YtbToMp3.exe song-urls.txt ./my_songs/
https://www.youtube.com/watch?v=Il0S8BoucSA
Ed Sheeran - Shivers [Official Video] 100.00%
https://www.youtube.com/watch?v=2Vv-BfVoq4g
Ed Sheeran - Perfect (Official Music Video) 100.00%
https://www.youtube.com/watch?v=Qz9gmiLBVFA
Sebastián Yatra - Tacones Rojos (Official Video) 100.00%
https://www.youtube.com/watch?v=KrgJp7Z1Hv8
Shawn Mendes - It'll Be Okay 100.00%
https://www.youtube.com/watch?v=09R8_2nJtjg
Maroon 5 - Sugar (Official Music Video) 100.00%

Finished in 46.72 s
```

### Used NuGet packages:
 * YoutubeExplode 6.0.7
 * YoutubeExplode.Converter 6.0.7