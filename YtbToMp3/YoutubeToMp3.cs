using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;

namespace YtbToMp3
{
    internal class YoutubeToMp3
    {
        private const string Mp3Extension = ".mp3";

        private readonly YoutubeClient Youtube = new();

        public async Task DownloadAsync(IEnumerable<string> youtubeUrls, string saveToDirectory = null,
            IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            var allTasks = new List<Task>();

            foreach (var url in youtubeUrls)
            {
                allTasks.Add(DownloadAsync(url, saveToDirectory, progress, cancellationToken));
            }

            await Task.WhenAll(allTasks);
        }

        private async Task DownloadAsync(string youtubeUrl, string saveToDirectory = null,
            IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            Video video = await GetVideoAsync(youtubeUrl);

            string mp3FileName = CreateMp3FileName(video.Title);

            string outputFilePath = CombineFilePath(saveToDirectory, mp3FileName);

            await Youtube.Videos.DownloadAsync(youtubeUrl, outputFilePath, progress, cancellationToken);
        }

        private async Task<Video> GetVideoAsync(string youtubeUrl)
        {
            return await Youtube.Videos.GetAsync(youtubeUrl);
        }

        private string CreateMp3FileName(string videoTitle)
        {
            string fileName = videoTitle.Trim() + Mp3Extension;

            return ReplaceAllInvalidFileNameChars(fileName, '_');
        }

        private string CombineFilePath(string saveToDirectory, string mp3FileName)
        {
            if (saveToDirectory is not null)
            {
                Directory.CreateDirectory(saveToDirectory);

                return Path.Combine(saveToDirectory, mp3FileName);
            }

            return mp3FileName;
        }

        private string ReplaceAllInvalidFileNameChars(string fileName, char replaceWithChar)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName.Replace(invalidChar, replaceWithChar);
            }

            return fileName;
        }

    }
}