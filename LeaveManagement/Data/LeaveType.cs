using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Data;

public class LeaveType
{
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    public int NumberOfDays { get; set; }

    public List<LeaveAllocation> LeaveAllocations { get; set; }
}
