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

    [Fact]
    public void Venue_AddBand_AssignsBandToVenue()
     {
      Venue newVenue = new Venue("Modacenter");
      newVenue.Save();
      Band newBand = new Band("Maroon5");
      newBand.Save();
      Console.WriteLine(newBand.Id);

      newVenue.AddBand(newBand);
      List<Band> testList = newVenue.GetBands();
      List<Band> controlList = new List<Band>{newBand};

      Assert.Equal(controlList, testList);
     }

    [Fact]
    public void Venue_Update_UpdatesVenueInfo()
    {
      Venue newVenue = new Venue("Modacenter");
      newVenue.Save();

      newVenue.Update("Providence Park");

      Venue controlVenue = new Venue("Providence Park", newVenue.Id);

      Assert.Equal(controlVenue, newVenue);
    }

    [Fact]
    public void Venue_Delete_DeletesSingleVenue()
    {
      Venue venue1 = new Venue("ModaCenter");
      venue1.Save();
      Venue venue2 = new Venue("Providence Park");
      venue2.Save();

      venue1.DeleteSingleVenue();

      List<Venue> testList = Venue.GetAll();
      List<Venue> controlList = new List<Venue>{venue2};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Venue_DeleteBands_DeletesAllOfVenuesBands()
    {
      Venue venue = new Venue("Providence Park");
      venue.Save();
      Band band1 = new Band("Maroon5");
      band1.Save();
      Band band2 = new Band("Rammstein");
      band2.Save();

      venue.AddBand(band1);
      venue.AddBand(band2);
      venue.DeleteBands();

      List<Band> testList = venue.GetBands();
      List<Band> controlList = new List<Band>{};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Venue_DeleteBandRelationship_DeletesRelationship()
    {
      Venue venue = new Venue("ModaCenter");
      venue.Save();
      Band band1 = new Band("Maroon5");
      band1.Save();
      Band band2 = new Band("Rammstein");
      band2.Save();

      venue.AddBand(band1);
      venue.AddBand(band2);
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
