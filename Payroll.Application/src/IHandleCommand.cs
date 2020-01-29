namespace Payroll.Application
{
  public interface IHandleCommand<T>
  {
    void Handle(T cmd);
  }

  public interface IHandleCommand<T, R>
  {
    R Handle(T cmd);
  }
}