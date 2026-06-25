namespace TheCat.Data.Definitions
{
    public enum P0AssetProductionQueueState
    {
        ReadyForCodexCandidateProduction,
        CandidatePackCompletePendingUnityReview,
        BlockedByUnityValidation,
        ReadyForFormalInstall,
        DeferredUntilP0Acceptance
    }
}
