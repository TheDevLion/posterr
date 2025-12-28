namespace PosterrBackend.Domain.Enums
{
    public enum RepostAvailabilityStatus
    {
        PostNonExistent,
        UserIsOwner,
        AlreadyRepostedByUser,
        UserExceededDailyActions,
        AvailableToRepost
    }
}

