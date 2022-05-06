namespace TibiaResults.Core
{
    internal class ResultComputingService : IResultComputingService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IHighscoreRetrievalService _highscoreRetrievalService;
        private readonly ILevelTrackingService _levelTrackingService;

        public ResultComputingService(
            IConfigurationService configurationService,
            IHighscoreRetrievalService highscoreRetrievalService,
            ILevelTrackingService levelTrackingService)
        {
            _configurationService = configurationService;
            _highscoreRetrievalService = highscoreRetrievalService;
            _levelTrackingService = levelTrackingService;
        }

        public async Task<IResult> GetResultAsync()
        {
            var result = Result.CreateNew();
            var levelTracker = LevelTracker.CreateEmpty();

            foreach (var category in CategoryHelper.GetCategories().OrderBy(c => c == Categories.Experience))
            {
                var categoryResult = await GetCategoryResultAsync(category, levelTracker);

                result.Add(category, categoryResult);
            }

            return result;
        }

        private CategoryResult ComputeCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore)
        {
            return ComputeCategoryResult(oldHighscore, newHighscore, () => GetResultEntries(oldHighscore!, newHighscore!));
        }

        private CategoryResult ComputeExperienceCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore, ILevelTracker levelTracker)
        {
            return ComputeCategoryResult(oldHighscore, newHighscore, () => GetExperienceResultEntries(oldHighscore!, newHighscore!, levelTracker));
        }

        private async Task<CategoryResult> GetCategoryResultAsync(Category category, ILevelTracker levelTracker)
        {
            var oldHighscore = await _highscoreRetrievalService.GetOldHighscoreAsync(category.Identifier);
            var newHighscore = await _highscoreRetrievalService.GetNewHighscoreAsync(category.Identifier);

            if (category == Categories.Experience)
            {
                return ComputeExperienceCategoryResult(oldHighscore, newHighscore, levelTracker);
            }

            _levelTrackingService.UpdateLevelTracker(levelTracker, oldHighscore, newHighscore);

            return ComputeCategoryResult(oldHighscore, newHighscore);
        }

        private IEnumerable<CategoryResultEntry> GetExperienceResultEntries(IEnumerable<HighscoreEntry> oldHighscore, IEnumerable<HighscoreEntry> newHighscore, ILevelTracker levelTracker)
        {
            foreach (var character in _configurationService.Characters)
            {
                var characterLevels = levelTracker.GetCharacterLevels(character);
                var newEntry = newHighscore?.Where(e => e.Name == character).SingleOrDefault();

                if (newEntry == null && !characterLevels.New.HasValue)
                {
                    continue;
                }

                var isApproximate = false;
                var newExperience = newEntry?.Value;

                if (!newExperience.HasValue)
                {
                    isApproximate = true;
                    newExperience = ExperienceHelper.ForLevel(characterLevels.New!.Value);
                }

                var oldExperience = oldHighscore?.Where(e => e.Name == character).Select(e => (long?)e.Value).SingleOrDefault();

                if (oldExperience == null && characterLevels.Old.HasValue)
                {
                    isApproximate = true;
                    oldExperience = ExperienceHelper.ForLevel(characterLevels.Old.Value);
                }

                var progress = oldExperience.HasValue ? newExperience - oldExperience.Value : null;

                yield return new CategoryResultEntry(newEntry?.Rank, character, newExperience.Value, progress, isApproximate);
            }
        }

        private IEnumerable<CategoryResultEntry> GetResultEntries(IEnumerable<HighscoreEntry> oldHighscore, IEnumerable<HighscoreEntry> newHighscore)
        {
            foreach (var character in _configurationService.Characters)
            {
                var newEntry = newHighscore?.Where(e => e.Name == character).SingleOrDefault();

                if (newEntry == null)
                {
                    continue;
                }

                var oldValue = oldHighscore?.Where(e => e.Name == character).Select(e => (long?)e.Value).SingleOrDefault();
                var progress = oldValue.HasValue ? newEntry.Value - oldValue.Value : (long?)null;

                yield return new CategoryResultEntry(newEntry.Rank, newEntry.Name, newEntry.Value, progress);
            }
        }

        private static CategoryResult ComputeCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore, Func<IEnumerable<CategoryResultEntry>> resultEntriesGettingDelegate)
        {
            if (oldHighscore == null || newHighscore == null)
            {
                return CategoryResult.CreateNotAvailable();
            }

            var resultEntries = resultEntriesGettingDelegate().ToList();

            return CategoryResult.Create(resultEntries);
        }
    }
}
