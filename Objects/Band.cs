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

    public Band()
    {
      Name = null;
      Id = 0;
    }

    public Band(string name, int id = 0)
    {
      Name = name;
      Id = id;
    }

    public static List<Band>GetAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", DB.GetConnection());
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Band> allBands = new List<Band>{};
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Band newBand = new Band(name, id);
        allBands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return allBands;
    }
    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        return (this.Id == newBand.Id &&
                this.Name == newBand.Name);
      }
    }

    public void Save()
     {
       DB.CreateConnection();
       DB.OpenConnection();

       SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@BandName)", DB.GetConnection());

       cmd.Parameters.Add(new SqlParameter("@BandName", this.Name));

       SqlDataReader rdr = cmd.ExecuteReader();
       while(rdr.Read())
       {
         this.Id = rdr.GetInt32(0);
       }
       if (rdr != null)
       {
         rdr.Close();
       }
       DB.CloseConnection();
     }

    public static Band Find(int searchId)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@BandId", searchId));

      SqlDataReader rdr = cmd.ExecuteReader();
      Band foundBand = new Band();
      while (rdr.Read())
      {
        foundBand.Id = rdr.GetInt32(0);
        foundBand.Name = rdr.GetString(1);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();
      return foundBand;
    }

    public void AddVenue(Venue venueToAdd, DateTime date)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (band_id, venue_id, date) VALUES (@BandId, @VenueId, @Date);", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@BandId", this.Id));
      cmd.Parameters.Add(new SqlParameter("@VenueId", venueToAdd.Id));
      cmd.Parameters.Add(new SqlParameter("@Date", date));

      cmd.ExecuteNonQuery();

      DB.CloseConnection();
    }

    public List<Venue> GetVenues()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (venues.id = bands_venues.venue_id) WHERE band_id = @BandId;", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@BandId", this.Id));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Venue> venues = new List<Venue>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int capacity = rdr.GetInt32(2);

        Venue newVenue = new Venue(name, capacity, id);
        venues.Add(newVenue);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return venues;
    }

    public static void DeleteAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("DELETE FROM bands;", DB.GetConnection());
      cmd.ExecuteNonQuery();
      DB.CloseConnection();
    }
  }
}
