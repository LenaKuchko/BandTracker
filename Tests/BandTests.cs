using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BandTracker.Objects;

namespace BandTracker
{
  [Collection("BandTracker")]

  public class BandTest : IDisposable
  {
    public BandTest()
    {
      DBConfiguration.ConnectionString = "Data Source=DESKTOP-6CVACGR\\SQLEXPRESS;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Band_GetAll_DatabaseEmptyOnload()
    {
      List<Band> testList = Band.GetAll();
      List<Band> controlList = new List<Band>{};
      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Band_Equals_BandEqualsBand()
    {
      Band controlBand = new Band("Maroon", new DateTime (2016, 05, 21));
      Band testBand = new Band("Maroon", new DateTime (2016, 05, 21));
      Assert.Equal(controlBand, testBand);
    }


    public void Dispose()
    {
      Band.DeleteAll();
    }
  }
}
