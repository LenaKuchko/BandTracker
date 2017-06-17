using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Venue
  {
    public int Id {get; set;}
    public string Name {get; set;}
    public int Capacity {get; set;}

    public Venue()
    {
      Name = null;
      Capacity = 0;
      Id = 0;
    }

    public Venue(string name, int capacity, int id = 0)
    {
      Name = name;
      Capacity = capacity;
      Id = id;
    }

    public static List<Venue>GetAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM venues;", DB.GetConnection());
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Venue> allVenues = new List<Venue>{};
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int capacity = rdr.GetInt32(2);

        Venue newVenue = new Venue(name, capacity, id);
        allVenues.Add(newVenue);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return allVenues;
    }

    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        return (this.Id == newVenue.Id &&
                this.Name == newVenue.Name &&
                this.Capacity == newVenue.Capacity);
      }
    }

    public void Save()
     {
       DB.CreateConnection();
       DB.OpenConnection();

       SqlCommand cmd = new SqlCommand("INSERT INTO venues (name, capacity) OUTPUT INSERTED.id VALUES (@VenueName, @VenueCapacity)", DB.GetConnection());

       cmd.Parameters.Add(new SqlParameter("@VenueName", this.Name));
       cmd.Parameters.Add(new SqlParameter("@VenueCapacity", this.Capacity));

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

    public static Venue Find(int searchId)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @VenueId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@VenueId", searchId));

      SqlDataReader rdr = cmd.ExecuteReader();

      Venue foundVenue = new Venue();
      while (rdr.Read())
      {
        foundVenue.Id = rdr.GetInt32(0);
        foundVenue.Name = rdr.GetString(1);
        foundVenue.Capacity = rdr.GetInt32(2);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return foundVenue;
    }

    public void AddBand(Band bandToAdd, DateTime date)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (band_id, venue_id, date) VALUES (@BandId, @VenueId, @Date);", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@BandId", bandToAdd.Id));
      cmd.Parameters.Add(new SqlParameter("@VenueId", this.Id));
      cmd.Parameters.Add(new SqlParameter("@Date", date));

      cmd.ExecuteNonQuery();

      DB.CloseConnection();
    }

    public List<Band> GetBands()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT bands.* FROM venues JOIN bands_venues ON (venues.id = bands_venues.venue_id) JOIN bands ON (bands.id = bands_venues.band_id) WHERE venue_id = @VanueId;", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@VanueId", this.Id));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Band> bands = new List<Band>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Band newBand = new Band(name, id);
        bands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return bands;
    }

    public List<Band> GetOtherBands()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT bands.id, bands.name FROM bands LEFT JOIN bands_venues on bands.id=bands_venues.band_id LEFT JOIN venues on venues.id=bands_venues.venue_id WHERE venue_id <> @VenueId OR bands_venues.venue_id is null GROUP BY bands.id, bands.name;", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@VenueId", this.Id));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Band> bands = new List<Band>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Band newBand = new Band(name, id);
        bands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return bands;
    }

    public List<DateTime> GetEvents(Band band)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("SELECT * FROM bands_venues WHERE band_id = @BandId AND venue_id = @VenueId", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@BandId", band.Id));
      cmd.Parameters.Add(new SqlParameter("@VenueId", this.Id));

      SqlDataReader rdr = cmd.ExecuteReader();

      List<DateTime> events = new List<DateTime>{};
      while(rdr.Read())
      {
        DateTime date = rdr.GetDateTime(3);
        events.Add(date);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return events;
    }

    public void Update(string name)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("UPDATE venues SET name = @VenueName OUTPUT INSERTED.name WHERE id = @VenueId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@VenueName", name));
      cmd.Parameters.Add(new SqlParameter("@VenueId", this.Id));

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Name = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();
    }

    public void DeleteSingleVenue()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("DELETE FROM venues WHERE id = @VenueId; DELETE FROM bands_venues WHERE venue_id = @VenueId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@VenueId", this.Id));
      cmd.ExecuteNonQuery();
      DB.CloseConnection();
    }

    public void DeleteBands()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE venue_id = @VenueId", DB.GetConnection());
      cmd.Parameters.Add(new SqlParameter("@VenueId", this.Id));

      cmd.ExecuteNonQuery();
      DB.CloseConnection();
    }

    public void DeleteBandRelationship(Band toDelete)
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE band_id = @BandId;", DB.GetConnection());

      cmd.Parameters.Add(new SqlParameter("@BandId", toDelete.Id));
      cmd.ExecuteNonQuery();
      DB.CloseConnection();
    }

    public static void DeleteAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("DELETE FROM venues; DELETE FROM bands_venues;", DB.GetConnection());
      cmd.ExecuteNonQuery();
      DB.CloseConnection();
    }
  }
}
