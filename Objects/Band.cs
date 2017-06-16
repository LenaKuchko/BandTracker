using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Band
  {
    public int Id {get; set;}
    public string Name {get; set;}
    public DateTime Date {get; set;}

    public Band(string name, DateTime date, int id = 0)
    {
      Name = name;
      Date = date;
      Id = id;
    }
  }
}
