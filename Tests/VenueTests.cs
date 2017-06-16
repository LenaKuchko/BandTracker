using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BandTracker.Objects;

namespace BandTracker
{
  [Collection("BandTracker")]

  public class VenueTest : IDisposable
  {
    public VenueTest()
    {
      DBConfiguration.ConnectionString = "Data Source=DESKTOP-6CVACGR\\SQLEXPRESS;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
    }

    [Fact]
     public void Venue_GetAll_DatabaseEmptyOnload()
     {
       List<Venue> testList = Venue.GetAll();
       List<Venue> controlList = new List<Venue>{};
       Assert.Equal(controlList, testList);
     }

    public void Dispose()
    {

    }
  }
}
