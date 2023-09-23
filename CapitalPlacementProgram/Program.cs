using Microsoft.EntityFrameworkCore;
using CapitalPlacementProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JobContext>();

var app = builder.Build();

RouteGroupBuilder api = app.MapGroup("/api");
RouteGroupBuilder jobs = api.MapGroup("/jobs");

jobs.MapGet("/", GetAllJobs);
jobs.MapGet("/{id}", GetJob);
jobs.MapDelete("/{id}", DeleteJob);
jobs.MapPost("/create", CreateJob);
jobs.MapPatch("/forms/{id}", AddApplicationForm);
jobs.MapPatch("/cover/{id}", UploadCoverImage);
jobs.MapGet("/cover/{id}", GetCoverImage);
jobs.MapPatch("/workflow/{id}", AddWorkflow);
jobs.MapGet("/published", GetPublishedJobs);
jobs.MapPatch("/publish/{id}", PublishJob);

app.MapGet("/", () => "Hello World!");

app.Run();

using (var context = new JobContext())
{
    // Ensure Database exists
    await context.Database.EnsureCreatedAsync();
}


static async Task<IResult> GetAllJobs(JobContext db)
{
    return TypedResults.Ok(await db.JobItems.Select(x => new JobItemDto(x)).ToArrayAsync());
}
static async Task<IResult> GetPublishedJobs(JobContext db)
{
    return TypedResults.Ok(await db.JobItems.Where(x => x.IsPublished).Select(x => new JobItemDto(x)).ToArrayAsync());
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

static async Task<IResult> PublishJob(Guid id, JobContext db)
{
    var jobItem = await db.JobItems.FindAsync(id);

    if (jobItem is null) return TypedResults.NotFound();

    jobItem.IsPublished = true;

    await db.SaveChangesAsync();

    return TypedResults.Ok();
}

static async Task<IResult> AddApplicationForm(Guid id, ApplicationForm form, JobContext db)
{
    var jobItem = await db.JobItems.FindAsync(id);

    if (jobItem is null) return TypedResults.NotFound();

    jobItem.ApplicationForm = form;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> UploadCoverImage(Guid id, HttpContext httpContext, JobContext db)
{
    var jobItem = await db.JobItems.FindAsync(id);

    if (jobItem is null) return TypedResults.NotFound();

    if (httpContext.Request.Form.Files.Count > 0)
    {
        var file = httpContext.Request.Form.Files[0];

        // TODO: checks againts mime type or file signature instead of filename extension
        string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".bmp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        {
            // The extension is invalid ... discontinue processing the file
            return TypedResults.UnprocessableEntity();
        }

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);

            // Less than 1 MB
            if(stream.Length < 1048576)
            {
                jobItem.ApplicationForm.CoverImage = new CoverImage { 
                    Content = stream.ToArray(),
                    Extension = ext.Substring(1)
                };

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
        }
        
    }

    return TypedResults.UnprocessableEntity();
}

static async Task<IResult> GetCoverImage(Guid id, JobContext db, HttpContext httpContext)
{
    var jobItem = await db.JobItems.FindAsync(id);

    if (jobItem is null || jobItem.ApplicationForm.CoverImage is null) return TypedResults.NotFound();

    httpContext.Response.Headers.Add("Cache-Control", "no-cache, must-revalidate, max-age=0, post-check=0, pre-check=0");

    httpContext.Response.Headers.Add("Content-Disposition", "inline; filename=cover");
    return TypedResults.Bytes(jobItem.ApplicationForm.CoverImage.Content, "image/"+jobItem.ApplicationForm.CoverImage.Extension);
}

static async Task<IResult> AddWorkflow(Guid id, Workflow workflow, JobContext db)
{
    var jobItem = await db.JobItems.FindAsync(id);

    if (jobItem is null) return TypedResults.NotFound();

    jobItem.Workflow = workflow;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

// Expose class for testing purposes.
public partial class Program { }