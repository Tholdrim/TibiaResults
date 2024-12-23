﻿using Azure.Storage.Blobs;
using System.Text.Json;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Providers
{
    internal class AzureBlobProvider : IHighscoreProvider
    {
        private readonly BlobContainerClient _blobContainerClient;

        public AzureBlobProvider(Uri blobContainerUri)
        {
            _blobContainerClient = new BlobContainerClient(blobContainerUri);
        }

        public Task<Highscore?> GetHighscoreAsync(string identifier, DateOnly date) => DownloadHighscoreAsync(identifier, date);

        private async Task<Highscore?> DownloadHighscoreAsync(string identifier, DateOnly date)
        {
            var blob = _blobContainerClient.GetBlobClient($"{identifier}/{date:yyyy-MM-dd}.json");
            var blobExists = await blob.ExistsAsync();

            if (!blobExists)
            {
                return null;
            }

            using var blobStream = await blob.OpenReadAsync();

            var highscoresRoot = await JsonSerializer.DeserializeAsync<HighscoreRoot>(blobStream);

            return highscoresRoot?.Highscores;
        }
    }
}
