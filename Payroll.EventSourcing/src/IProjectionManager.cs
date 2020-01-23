namespace Payroll.EventSourcing
{
    public interface IProjectionManager
    {
        void UpdateProjections(object sender, object[] events);
        void Register(IProjection projection);
    }
}