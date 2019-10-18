using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using mtg_api.Data;
using mtg_api.Models;

namespace mtg_api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddDbContext<CardContext>(options =>
        options.UseInMemoryDatabase("CardDb")
      );
      // Add the OData Service
      services.AddOData();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      Initialize(app.ApplicationServices);
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      //app.UseMvc();
      app.UseMvc(routeBuilder => 
      { 
        routeBuilder.EnableDependencyInjection();
        routeBuilder.Select()
                    .Count()
                    .Expand()
                    .OrderBy()
                    .Filter();
      });
    }

    private void Initialize(IServiceProvider service)
    {
      using (var scope = service.CreateScope())
      {
        var scopeServiceProvider = scope.ServiceProvider;
        var db = scopeServiceProvider.GetService<CardContext>();

        db.Artists.Add(new Artist { Id = "111", Name = "Mark Poole", Email = "mark@poole.com", Phone = "405-555-1212" });
        db.Artists.Add(new Artist { Id = "222", Name = "Sandra Everingham", Email = "Sandra@Everingham.com", Phone = "405-555-1212" });
        db.Artists.Add(new Artist { Id = "333", Name = "Douglas Schuler", Email = "Douglas@Schuler.com", Phone = "405-555-1212" });
        db.Artists.Add(new Artist { Id = "444", Name = "Christopher Rush", Email = "Christopher@Rush.com", Phone = "405-555-1212" });
        db.Artists.Add(new Artist { Id = "555", Name = "Anson Maddocks", Email = "Anson@Maddocks.com", Phone = "405-555-1212" });

        db.Cards.Add(new Card
        {
          Name = "Counterspell",
          SetName = "Limited Edition Alpha",
          SetCode = "LEA",
          CollectionNumber = 54,
          Text = "Counters target spell as it is being cast.",
          FlavorText = "Nope.",
          Rarity = CardRarity.Uncommon.ToString(),
          Type = CardType.Instant.ToString(),
          Artist = "Mark Poole",
          ArtistInfo = db.Artists.Find("111")
        }
        );

        db.Cards.Add(new Card
        {
          Name = "Dark Ritual",
          SetName = "Limited Edition Alpha",
          SetCode = "LEA",
          CollectionNumber = 98,
          Text = "Add 3 black mana to your mana pool.",
          Rarity = CardRarity.Common.ToString(),
          Type = CardType.Instant.ToString(),
          Artist = "Sandra Everingham",
          ArtistInfo = db.Artists.Find("111")
        }
        );

        db.Cards.Add(new Card
        {
          Name = "Demonic Tutor",
          SetName = "Limited Edition Alpha",
          SetCode = "LEA",
          CollectionNumber = 104,
          Text = "You may search your library for one card and take it into your hand. Reshuffle your library afterwards.",
          Rarity = CardRarity.Uncommon.ToString(),
          Type = CardType.Sorcery.ToString(),
          Artist = "Douglas Schuler",
          ArtistInfo = db.Artists.Find("333")
        }
        );

        db.Cards.Add(new Card
        {
          Name = "Lightning Bolt",
          SetName = "Limited Edition Alpha",
          SetCode = "LEA",
          CollectionNumber = 161,
          Text = "Lightning Bolt does 3 damage to one target.",
          Rarity = CardRarity.Common.ToString(),
          Type = CardType.Instant.ToString(),
          Artist = "Christopher Rush",
          ArtistInfo = db.Artists.Find("444")
        }
        );

        db.Cards.Add(new Card
        {
          Name = "Llanowar Elves",
          SetName = "Limited Edition Alpha",
          SetCode = "LEA",
          CollectionNumber = 210,
          Text = "Tap to add 1 green mana to your mana pool. This tap can be played as an interrupt.",
          FlavorText = "Whenever the Llanowar Elves gather the fruites of their forest, they leave one plant of each type untouched, considering that nature's portion.",
          Rarity = CardRarity.Common.ToString(),
          Type = CardType.Creature.ToString(),
          Power = 1,
          Toughness = 1,
          Artist = "Anson Maddocks",
          ArtistInfo = db.Artists.Find("555")
        }
        );

        db.SaveChanges();
      }
    }
  }
}
