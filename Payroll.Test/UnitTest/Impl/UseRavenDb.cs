using System;
using System.Collections.Generic;
using System.Linq;
using Payroll.EventSourcing;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;

namespace Payroll.Test.UnitTest.Impl
{
  public class UseRavenDb : ICacheStore
  {
    private string _dbName;
    private string _dbUrl;
    private DocumentStore _store;

    public void Start(string[] dbUrls, string dbName)
    {
      _store = new DocumentStore {
        Urls = dbUrls,
        Database = dbName
      };

      _store.Initialize();
    }

    public IReadOnlyCollection<T> All<T>() {
      using(var session = _store.OpenSession())
      {
        return session.Query<T>().ToList();
      }
    }

    public void Delete<T>(Guid id) {
      using(var session = _store.OpenSession())
      {
        session.Delete(_typedId<T>(id));
        session.SaveChanges();
      }
    }

    // TODO: fix delete all record
    public void DeleteAll<T>() {
      throw new NotImplementedException();
      // using(var session = _store.OpenSession())
      // {
      //   _store.Operations.Send(new DeleteByQueryOperation(new IndexQuery {
      //     Query = "Id:*",
      //   }));
      // }
    }

    public T Get<T>(Guid id) {
      using(var session = _store.OpenSession())
      {
        return session.Load<T>(_typedId<T>(id));
      }
    }

    public bool GetRecent<T>(out T record) {
      using(var session = _store.OpenSession())
      {
        var list = session.Query<T>().OrderByDescending($"{typeof(T)}/ById").Take(1).ToList();
        record = list.FirstOrDefault();
        return list.Count() > 0;
      }
    }

    public bool GetRecent<T>(int n, out IEnumerable<T> records) {
      using(var session = _store.OpenSession())
      {
        records = session.Query<T>().OrderByDescending(x => true).Take(n).ToList();
        return records.Count() > 0;
      }
    }

    public void Store<T>(Guid id, T document) {
      using(var session = _store.OpenSession())
      {
        session.Store(document, _typedId<T>(id));
        session.SaveChanges();
      }
    }

    public void UpdateIfFound<T>(Guid id, Action<T> apply) {
      using(var session = _store.OpenSession())
      {
        var record = session.Load<T>(_typedId<T>(id));
        if(record is null)
          return;
        apply(record);
        session.SaveChanges();
      }
    }

    private string _typedId<T>(Guid id)
    {
      return $"{typeof(T).Name}/{id.ToString()}";
    }

    private string _plainId<T>(string id)
    {
      return id.Split('/')[1];
    }
  }
}