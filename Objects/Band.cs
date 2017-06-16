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
      Date = default(DateTime);
      Id = 0;
    }

    public Band(string name, DateTime date, int id = 0)
    {
      Name = name;
      Date = date;
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
        DateTime date = rdr.GetDateTime(2);

        Band newBand = new Band(name, date, id);
        allBands.Add(newBand);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      DB.CloseConnection();

      return allBands;
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
