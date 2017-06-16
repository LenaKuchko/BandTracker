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
      Band controlBand = new Band("Maroon5");
      Band testBand = new Band("Maroon5");
      Assert.Equal(controlBand, testBand);
    }

    [Fact]
    public void Band_Save_SaveToDatabase()
    {
      Band newBand = new Band("Maroon5");
      newBand.Save();

      Band testBand = Band.GetAll()[0];
      Assert.Equal(newBand, testBand);
    }

    [Fact]
    public void Band_Find_FindsBandInDB()
    {
       Band controlBand = new Band("Maroon5");
       controlBand.Save();
       Band testBand = Band.Find(controlBand.Id);

       Assert.Equal(controlBand, testBand);
    }

    [Fact]
    public void Band_AddVenue_AssignsVenueToBand()
     {
      Venue newVenue = new Venue("Modacenter");
      newVenue.Save();
      Band newBand = new Band("Maroon5");
      newBand.Save();

      newBand.AddVenue(newVenue, new DateTime(2017, 03, 12));
      List<Venue> testList = newBand.GetVenues();
      List<Venue> controlList = new List<Venue>{newVenue};

      Assert.Equal(controlList, testList);
     }


    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }
  }
}
