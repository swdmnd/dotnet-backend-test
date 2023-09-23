# CapitalPlacementProgram

This is a Visual Studio project to provide backend for creating Programs in Capital Placement.

.NET 7 and local CosmosDB emulator are used.

Change CosmosDB configuration in `CapitalPlacementProgram/appsettings.json`:

```json
...
  "CosmosDbHost": "https://localhost:8081",
  "CosmosDbKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
  "CosmosDbName": "CPJobs",
  "CosmosDbContainer": "Jobs"
...
```

Requests must be raw string in body with content type `application/json`, except for file upload (`multipart/form-data`).

Text response will always be `application/json`.

The flow is as follows:

1. Create new program by POST-ing the program's details to /api/jobs/create. An ID will be returned.
2. Upload cover image using PATCH request to /api/jobs/cover/{id}, where {id} is the ID returned from step 1.
2. Add application form details by sending PATCH request with the details in request body to /api/jobs/forms/{id}. 
3. Add workflow stages by sending PATCH request with the details in request body to /api/jobs/workflow/{id}.
4. Get the program details by sending Get request to /api/jobs/{id}.
5. Publish program by sending empty PATCH request to /api/jobs/publish/{id}.

## API Endpoints

## GET /api/jobs
Get all submitted programs.
<table>
<tr>
<th>Request Parameters</th>
<td>None</td>
</tr>
<tr>
<th>Response</th>
<td>[200, application/json] List of all submitted programs.</td>
</tr>
</table>

## GET /api/jobs/{id}
Get program with id {id}.
<table>
<tr>
<th>Request Parameters</th>
<td>None</td>
</tr>
<tr>
<th>Response</th>
<td>[200, application/json] Program with id {id} if exists, otherwise returns code 404.</td>
</tr>
</table>

## DELETE /api/jobs/{id}
Delete program with id {id}.
<table>
<tr>
<th>Request Parameters</th>
<td>None</td>
</tr>
<tr>
<th>Response</th>
<td>Returns 204 if success, 404 if program with {id} is not found</td>
</tr>
</table>

## POST /api/jobs/create
Create program.

|    | param | type | description |
| -- | ----- | ---- | ----------- |
| **Request** | title | string | |
| | summary | string? | |
| | description | string | |
| | keySkills | Array of string? | |
| | benefits | string? | |
| | criteria | string? | |
| | type | string | |
| | dateStart | string? | fill with formatted DateTime string |
| | dateOpenApplication | string | fill with formatted DateTime string |
| |  dateCloseApplication | string | fill with formatted DateTime string |
| | duration | long? | unix timestamp |
| | location | string | |
| | minimalQualifications | string? | |
| | maxApplicants | int? | |
| **Response** | id | string | program unique id |
| | details | - | the contents are the same as Request |

## PATCH /api/jobs/forms/{id}
Add application form to program with id {id}.

Example request:

```json
{
    "personalInformation": {
        "phoneIsInternal":false,
        "phoneIsHidden":true,
        "nationalityIsInternal":false,
        "nationalityIsHidden":true,
        "currentResidenceIsInternal":false,
        "currentResidenceIsHidden":true,
        "idNumberIsInternal":false,
        "idNumberIsHidden":true,
        "dobIsInternal":false,
        "dobIsHidden":true,
        "genderIsInternal":false,
        "genderIsHidden":true,
        "additionalQuestions": [
            {
                "type": "paragraph",
                "questionText": "how are you?"
            },
            {
                "type": "yesNo",
                "questionText": "are you ok?"
            },
            {
                "type": "multipleChoice",
                "questionText": "Choose one."
                "choices": ["4","3","2","1"],
                "maxChoiceAllowed": 1
            }
        ]
    },
    "profile": {
        "educationIsMandatory": false,
        "educationIsHidden": false,
        "experienceIsMandatory": false,
        "experienceIsHidden": false,
        "resumeIsMandatory": false,
        "resumeIsHidden": false
    },
    "additionalQuestions": [
        {
            "type": "shortAnswer",
            "questionText": "hello"
        }
    ]
}
```
**Response**: **204** if success, **404** if {id} not found.

## PATCH /api/jobs/cover/{id}
Upload cover image for the program.

**Request**: image file (png, jpg, jpeg, bmp) with size less than 1 MB using multipart/form-data.

**Response**: **204** if success, **422** if uploaded file is not an image or the size is more than 1 MB, **404** if not found.

## GET /api/jobs/cover/{id}
Get cover image file for the program.

## PATCH /api/jobs/workflow/{id}
Add workflow stages to the program.

Example request:

```json
{
    "hideFromApplicants": true,
    "stages": [
        {
            "name": "applied",
            "type": "shortlisting"
        },
        {
            "name": "video",
            "type": "videoInterview",
            "questionText": "describe yourself",
            "additionalInformation": "shoot outdoors at your favorite place",
            "duration": 60,
            "durationUnit": "sec",
            "submissionDeadlineInDays": 30
        },
        {
            "name": "placement",
            "type": "placement"
        }
    ]
}
```

## PATCH /api/jobs/publish/{id}
Set isPublished flag; Publish the program.

**Request Body**: None
**Response**: **200** if success or **404** if not found

## GET /api/jobs/published
Get all published programs.
