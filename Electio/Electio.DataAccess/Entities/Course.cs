﻿namespace Electio.DataAccess.Entities;
public class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Quota { get; set; }
}
