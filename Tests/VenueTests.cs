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

    [Fact]
    public void Venue_Equals_VenueEqualsVenue()
    {
      Venue controlVenue = new Venue("ModaCenter");
      Venue testVenue = new Venue("ModaCenter");
      Assert.Equal(controlVenue, testVenue);
    }

    [Fact]
    public void Venue_Save_SaveToDatabase()
    {
      Venue newVenue = new Venue("ModaCenter");
      newVenue.Save();

      Venue testVenue = Venue.GetAll()[0];
      Assert.Equal(newVenue, testVenue);
    }

    [Fact]
    public void Venue_Find_FindsVenueInDB()
    {
       Venue controlVenue = new Venue("Modacenter");
       controlVenue.Save();
       Venue testVenue = Venue.Find(controlVenue.Id);

       Assert.Equal(controlVenue, testVenue);
    }

    public void Dispose()
    {
      Venue.DeleteAll();
    }
  }
}
