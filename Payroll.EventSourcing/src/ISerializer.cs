using System;

namespace Payroll.EventSourcing
{
  /// <summary>
  /// from Alexey Zimarev sample
  /// https://github.com/UbiquitousAS/WorkshopEventSourcing/blob/master/src/Marketplace.Framework/ISerializer.cs
  /// </summary>
  public interface ISerializer
  {
    bool isJSON { get; }
    byte[] Serialize(object graph);
    object Deserialize(byte[] data, Type type);
  }
}