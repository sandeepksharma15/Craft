using Hangfire.Client;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace Craft.Jobs.Jobs;

public class LogJobFilter : IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
{
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    public void OnCreated(CreatedContext filterContext) =>
        Logger.InfoFormat(
            "Job that is based on method {0} has been created with id {1}",
            filterContext.Job.Method.Name,
            filterContext.BackgroundJob?.Id);

    public void OnCreating(CreatingContext filterContext) =>
            Logger.InfoFormat("Creating a job based on method {0}...", filterContext.Job.Method.Name);

    public void OnPerformed(PerformedContext filterContext) =>
        Logger.InfoFormat("Job {0} has been performed", filterContext.BackgroundJob.Id);

    public void OnPerforming(PerformingContext filterContext) =>
            Logger.InfoFormat("Starting to perform job {0}", filterContext.BackgroundJob.Id);

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction) =>
        Logger.InfoFormat(
            "Job {0} state was changed from {1} to {2}",
            context.BackgroundJob.Id,
            context.OldStateName,
            context.NewState.Name);

    public void OnStateElection(ElectStateContext context)
    {
        if (context.CandidateState is FailedState failedState)
            Logger.WarnFormat(
                "Job '{0}' has been failed due to an exception {1}",
                context.BackgroundJob.Id,
                failedState.Exception);
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction) =>
        Logger.InfoFormat(
            "Job {0} state {1} was unapplied.",
            context.BackgroundJob.Id,
            context.OldStateName);
}
