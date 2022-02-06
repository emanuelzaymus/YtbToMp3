using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;

namespace YtbToMp3
{
    public class YoutubeToMp3
    {
        private const string Mp3Extension = ".mp3";

        private readonly YoutubeClient _youtube = new();

        public async Task DownloadAsync(IEnumerable<string> youtubeUrls, string saveToDirectory = ".",
            CancellationToken cancellationToken = default)
        {
            var allTasks = youtubeUrls
                .Select(url => DownloadAsync(url, saveToDirectory, cancellationToken: cancellationToken))
                .ToList();

            await Task.WhenAll(allTasks);
        }

        public async Task DownloadAsync(string youtubeUrl, string saveToDirectory = ".",
            IProgress<double>? progress = null, CancellationToken cancellationToken = default)
        {
            var videoTitle = await GetVideoTitleAsync(youtubeUrl);

            string mp3FileName = CreateMp3FileName(videoTitle);

            string outputFilePath = CombineFilePath(saveToDirectory, mp3FileName);

            await _youtube.Videos.DownloadAsync(youtubeUrl, outputFilePath, progress, cancellationToken);
        }

        private async Task<string> GetVideoTitleAsync(string youtubeUrl)
        {
            Video video = await _youtube.Videos.GetAsync(youtubeUrl);

            return video.Title;
        }

        public string GetVideoTitle(string youtubeUrl)
        {
            return GetVideoTitleAsync(youtubeUrl).GetAwaiter().GetResult();
        }

        private string CreateMp3FileName(string videoTitle)
        {
            string fileName = videoTitle.Trim() + Mp3Extension;

            return ReplaceAllInvalidFileNameChars(fileName, '_');
        }

        private string CombineFilePath(string saveToDirectory, string mp3FileName)
        {
            Directory.CreateDirectory(saveToDirectory);

            return Path.Combine(saveToDirectory, mp3FileName);
        }

        private string ReplaceAllInvalidFileNameChars(string fileName, char replaceWithChar)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar, replaceWithChar);
            }

            return fileName;
        }
    }
}