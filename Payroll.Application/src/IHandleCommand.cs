using System;

namespace Payroll.Application
{
  public interface IHandleCommand<T>
  {
    void Handle(T cmd);
  }

  public interface IHandleCommand<T, R>
  {
    void Handle(T cmd, Action<R> cb);
  }
}