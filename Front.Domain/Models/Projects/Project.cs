using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front.Domain.Models.Projects;

public class Project
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string Route { get; set; }
}
