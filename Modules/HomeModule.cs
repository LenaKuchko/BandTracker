using Nancy;
using System.Collections.Generic;
using System;
using BandTracker.Objects;

namespace BandTracker
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("allVenues", Venue.GetAll());
        model.Add("allBands", Band.GetAll());
        return View["index.cshtml", model];
      };
      Get["/venues/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Venue venue = Venue.Find(parameters.id);
        model.Add("allVenues", Venue.GetAll());
        model.Add("show-venue", venue);
        model.Add("venues-bands", venue.GetBands());
        return View["index.cshtml", model];
      };
      Get["venues/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("bands", Band.GetAll());
        return View["form.cshtml"];
      };
      Post["venues/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Venue newVenue = new Venue(Request.Form["name"]);
        newVenue.Save();
        if (Request.Form("band") != "")
        {
          newVenue.AddBand(Band.Find(Request.Form["band"]));
          model.Add("band", Band.Find(Request.Form["band"]));
        }
        model.Add("new-venue", newVenue);
        return View["form.cshtml", model];
      };
    }
  }
}
