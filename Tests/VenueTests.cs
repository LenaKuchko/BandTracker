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
      Venue controlVenue = new Venue("ModaCenter", 5000);
      Venue testVenue = new Venue("ModaCenter", 5000);
      Assert.Equal(controlVenue, testVenue);
    }

    [Fact]
    public void Venue_Save_SaveToDatabase()
    {
      Venue newVenue = new Venue("ModaCenter", 5000);
      newVenue.Save();

      Venue testVenue = Venue.GetAll()[0];
      Assert.Equal(newVenue, testVenue);
    }

    [Fact]
    public void Venue_Find_FindsVenueInDB()
    {
       Venue controlVenue = new Venue("Modacenter", 6000);
       controlVenue.Save();
       Venue testVenue = Venue.Find(controlVenue.Id);

       Assert.Equal(controlVenue, testVenue);
    }

    [Fact]
    public void Venue_AddBand_AssignsBandToVenue()
     {
      Venue newVenue = new Venue("Modacenter", 5000);
      newVenue.Save();
      Band newBand = new Band("Maroon5");
      newBand.Save();
      Console.WriteLine(newBand.Id);

      newVenue.AddBand(newBand, new DateTime(2016, 05, 21));
      List<Band> testList = newVenue.GetBands();
      List<Band> controlList = new List<Band>{newBand};

      Assert.Equal(controlList, testList);
     }

    [Fact]
    public void Venue_Update_UpdatesVenueInfo()
    {
      Venue newVenue = new Venue("Modacenter", 5000);
      newVenue.Save();

      newVenue.Update("Providence Park");

      Venue controlVenue = new Venue("Providence Park", 5000, newVenue.Id);

      Assert.Equal(controlVenue, newVenue);
    }

    // [Fact]
    // public void Venue_GetAllOtherBands_ReturnsOtherBands()
    // {
    //   Venue venue = new Venue("Providence Park");
    //   venue.Save();
    //   Band band1 = new Band("Maroon5");
    //   band1.Save();
    //   Band band2 = new Band("Rammstein");
    //   band2.Save();
    //
    //   venue.AddBand(band1, new DateTime(2017, 05, 13));
    //
    //   List<Band> testList = venue.GetBandsNotBelong();
    //   List<Band> controlList = new List<Band>{band2};
    //
    //   Assert.Equal(controlList, testList);
    // }

    [Fact]
    public void Venue_Delete_DeletesSingleVenue()
    {
      Venue venue1 = new Venue("ModaCenter", 7000);
      venue1.Save();
      Venue venue2 = new Venue("Providence Park", 10000);
      venue2.Save();

      venue1.DeleteSingleVenue();

      List<Venue> testList = Venue.GetAll();
      List<Venue> controlList = new List<Venue>{venue2};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Venue_DeleteBands_DeletesAllOfVenuesBands()
    {
      Venue venue = new Venue("Providence Park", 10000);
      venue.Save();
      Band band1 = new Band("Maroon5");
      band1.Save();
      Band band2 = new Band("Rammstein");
      band2.Save();

      venue.AddBand(band1, new DateTime(2013, 04, 19));
      venue.AddBand(band2, new DateTime(2013, 04, 19));
      venue.DeleteBands();

      List<Band> testList = venue.GetBands();
      List<Band> controlList = new List<Band>{};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Venue_DeleteBandRelationship_DeletesRelationship()
    {
      Venue venue = new Venue("ModaCenter", 5000);
      venue.Save();
      Band band1 = new Band("Maroon5");
      band1.Save();
      Band band2 = new Band("Rammstein");
      band2.Save();

      venue.AddBand(band1, new DateTime(2013, 04, 19));
      venue.AddBand(band2, new DateTime(2013, 04, 19));
      venue.DeleteBandRelationship(band1);

      List<Band> testList = venue.GetBands();
      List<Band> controlList = new List<Band>{band2};

      Assert.Equal(controlList, testList);
    }

    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }
  }
}
