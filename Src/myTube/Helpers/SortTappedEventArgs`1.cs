// myTube.Helpers.SortTappedEventArgs`1


using System;

namespace myTube.Helpers
{
  public class SortTappedEventArgs<T> : EventArgs
  {
    public ControlDirection Direction { get; set; }

    public T Object { get; set; }
  }
}
