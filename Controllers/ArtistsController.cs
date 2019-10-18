using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using mtg_api.Data;
using mtg_api.Models;

namespace mtg_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ArtistsController : ControllerBase
  {
    private readonly CardContext _context;
    public ArtistsController(CardContext context)
    {
      _context = context;
    }

    [HttpGet]
    [EnableQuery()]
    public ActionResult<IEnumerable<Artist>> Get()
    {
      return Ok(_context.Artists);
    }

    [HttpGet("{id}")]
    public ActionResult<Artist> Get(string id)
    {
      var artist = _context.Artists.Find(id);

      if (artist == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(artist);
      }
    }

    [HttpPost]
    public ActionResult Post([FromBody] Artist artist)
    {
      var found = _context.Artists.Find(artist.Id);

      if (found != null)
      {
        return BadRequest("Card already exists!");
      }
      else
      {
        _context.Artists.Add(artist);
        _context.SaveChanges();
      }
      return Ok("Card added!");
    }

    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody] Artist artist)
    {
      return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
      return Ok();
    }
  }
}