using API.Entities;

namespace API.DTOs;

public class MemberDTO
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public string PhotoUrl { get; set; }
    
    public DateTime CreatedTime { get; set; }

    public DateTime LastActive { get; set; }
    
    public int Age { get; set; }
    
    public string Gender { get; set; }
    
    public string Adress { get; set; }
    
    public string Description { get; set; }

    public List<PhotoDTO> Photos { get; set; }
    
    public List<string> Likes { get; set; }
    
}