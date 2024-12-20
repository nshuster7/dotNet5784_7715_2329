﻿namespace BO;

public class Employee
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; set; }
    public WorkStatus? Status { get; set; }
    public Type? Type { get; set; }
    public int HourlyRate { get; set; }
    public BO.TaskInEmployee? CurrentTaskId { get; set; }
    public string? ImageRelativeName { get; set; }
    public override string ToString() => this.ToStringProperty();
}
