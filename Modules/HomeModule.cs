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
        model.Add("venue", venue);
        model.Add("venues-bands", venue.GetBands());
        model.Add("allBands", Band.GetAll());
        return View["index.cshtml", model];
      };
      Get["/venues/{venId}/bands/{bandId}/events"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Venue selectedVenue = Venue.Find(parameters.venId);
        Band selectedBand = Band.Find(parameters.bandId);
        model.Add("venue", selectedVenue);
        model.Add("allVenues", Venue.GetAll());
        model.Add("show-venue", selectedVenue);
        model.Add("venues-bands", selectedVenue.GetBands());
        model.Add("events", selectedVenue.GetEvents(selectedBand));
        model.Add("allBands", Band.GetAll());
        return View["index.cshtml", model];
      };
      Get["/venues/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("bands", Band.GetAll());
        model.Add("add-venue", null);
        return View["form.cshtml", model];
      };
      Post["/venues/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("bands", Band.GetAll());
        Venue newVenue = new Venue(Request.Form["name"]);
        newVenue.Save();
        if (Request.Form["band"] != "")
        {
          Band selectedBand = Band.Find(Request.Form["band"]);
          newVenue.AddBand(selectedBand, Request.Form["date"]);
          model.Add("band", Band.Find(Request.Form["band"]));
        }
        model.Add("new-venue", newVenue);
        return View["form.cshtml", model];
      };
      Get["/venues/{id}/update"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Venue selectedVenue = Venue.Find(parameters.id);
        model.Add("venue", selectedVenue);
        model.Add("bands", selectedVenue.GetBands());
        model.Add("show", "update-form");
        // model.Add("otherBands", selectedVenue.GetBandsNotBelong());

        return View["update_form.cshtml", model];
      };
      Patch["/venues/{id}/update"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Venue selectedVenue = Venue.Find(parameters.id);
        string bands = Request.Form["band"];
        if(bands != null)
        {
          string[] values = bands.Split(',');
          foreach(string bandId in values)
          {
            selectedVenue.DeleteBandRelationship(Band.Find(int.Parse(bandId)));
          }
        }
        selectedVenue.Update(Request.Form["name"]);
        model.Add("venue", selectedVenue);
        model.Add("bands", selectedVenue.GetBands());
        model.Add("show", "update-info");
        return View ["update_form.cshtml", model];
      };
      Get["/bands/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("new-band", null);
        model.Add("venues", Venue.GetAll());
        return View["form.cshtml", model];
      };
      Post["/bands/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Band newBand = new Band(Request.Form["name"]);
        newBand.Save();
        if (Request.Form["venue-selected"] != "")
        {
          Venue selectedVenue = Venue.Find(Request.Form["venue-choose"]);
          newBand.AddVenue(selectedVenue, Request.Form["date"]);
          model.Add("venue-selected", selectedVenue);
        }
        model.Add("allVenues", Venue.GetAll());
        model.Add("allBands", Band.GetAll());
        return View["index.cshtml", model];
      };
      Delete["/venues/{id}/delete"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Venue selectedVenue = Venue.Find(parameters.id);
        selectedVenue.DeleteSingleVenue();
        string name = selectedVenue.Name;
        model.Add("allVenues", Venue.GetAll());
        model.Add("allBands", Band.GetAll());
        model.Add("delete-confirm", name);
        return View["index.cshtml", model];

      };
    }
  }
}
