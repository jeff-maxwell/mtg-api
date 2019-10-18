using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mtg_api.Models
{
  public class Artist
  {
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public ICollection<Card> Cards { get; set; }
  }
}
