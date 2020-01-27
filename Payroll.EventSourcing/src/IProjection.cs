namespace Payroll.EventSourcing
{
    public interface IProjection
    {
        // 1 projection = 1 subscription
        /// <summary>
        /// where e is a domain event
        /// </summary>
        void Handle(object e, ISnapshotStore snapshots);
    }
}