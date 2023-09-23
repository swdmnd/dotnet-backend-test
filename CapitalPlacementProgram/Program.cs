using Microsoft.EntityFrameworkCore;
using CapitalPlacementProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Azure.Core;

using (var context = new JobContext())
{
    // Ensure Database exists
    await context.Database.EnsureCreatedAsync();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JobContext>();

var app = builder.Build();

RouteGroupBuilder api = app.MapGroup("/api");
RouteGroupBuilder jobs = api.MapGroup("/jobs");

jobs.MapGet("/", GetAllJobs);
jobs.MapGet("/{id}", GetJob);
jobs.MapDelete("/{id}", DeleteJob);
jobs.MapPost("/create", CreateJob);

app.MapGet("/", () => "Hello World!");

app.Run();

static async Task<IResult> GetAllJobs(JobContext db)
{
    return TypedResults.Ok(await db.JobItems.Select(x => new JobItemDto(x)).ToArrayAsync());
}

static async Task<IResult> GetJob(Guid id, JobContext db)
{
    return await db.JobItems.FindAsync(id)
        is JobItem item
            ? TypedResults.Ok(new JobItemDto(item))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateJob(JobDetails jobDetails, JobContext db)
{
    var jobItem = new JobItem
    {
        Details = jobDetails,
        CreatedOn = DateTime.UtcNow.ToString(),
        CreatedBy = "admin"
    };

    db.JobItems.Add(jobItem);
    await db.SaveChangesAsync();

    var jobItemDto = new JobItemDto(jobItem);

    return TypedResults.Created($"/{jobItem.Id}", jobItemDto);
}

static async Task<IResult> DeleteJob(Guid id, JobContext db)
{
    if (await db.JobItems.FindAsync(id) is JobItem job)
    {
        db.JobItems.Remove(job);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}

// Expose class for testing purposes.
public partial class Program { }