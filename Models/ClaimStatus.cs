namespace CMCS.Models
{
    /// <summary>
    /// Defines the possible workflow states for a ClaimModel instance.
    /// </summary>
    public enum ClaimStatus
    {
        // Enum Member: Initial state before the lecturer fully submits the claim.
        PendingSubmission,

        // Approval Workflow
        // Enum Member: Awaiting review and verification by the Programme Co-ordinator.
        AwaitingPCVerification,
        // Enum Member: Awaiting final review and approval by the Academic Manager.
        AwaitingAMApproval,

        // Final States
        // Enum Member: The claim has been fully approved for payment.
        Approved,
        // Enum Member: The claim was rejected by the Programme Co-ordinator.
        RejectedByPC,
        // Enum Member: The claim was rejected by the Academic Manager.
        RejectedByAM
    }
}