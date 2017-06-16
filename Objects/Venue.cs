using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTracker.Objects
{
  public class Venue
  {
    public int Id {get; set;}
    public string Name {get; set;}

    public Venue(string name, int id = 0)
    {
      Name = name;
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

        Venue newVenue = new Venue(name, id);
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
        return (this.Id == newVenue.Id && this.Name == newVenue.Name);
      }
    }

    public void Save()
     {
       DB.CreateConnection();
       DB.OpenConnection();

       SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@VenueName)", DB.GetConnection());

       cmd.Parameters.Add(new SqlParameter("@VenueName", this.Name));

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

    public static void DeleteAll()
    {
      DB.CreateConnection();
      DB.OpenConnection();

      SqlCommand cmd = new SqlCommand("DELETE FROM venues;", DB.GetConnection());
      cmd.ExecuteNonQuery();
      DB.CloseConnection();
    }
  }
}
