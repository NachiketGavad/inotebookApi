# inotebookApi

## Description:
This is a Web API built with ASP.NET Core MVC 6 to support the `inotebook` React.js application. It handles various operations such as user authentication and note management.

## Supported Operations:
1. **Authentication:**
   - `POST api/auth/CreateUser`: Endpoint for creating a new user.
   - `POST api/auth/LoginUser`: Endpoint for user login.

2. **Note Management:**
   - `GET api/notes/GetNote`: Endpoint for fetching a note.
   - `POST api/notes/CreateNote`: Endpoint for creating a new note.
   - `PUT api/notes/UpdateNote`: Endpoint for updating an existing note.
   - `DELETE api/notes/DeleteNote`: Endpoint for deleting a note.

## GitHub Repository Link:
[inotebook](https://github.com/NachiketGavad/inotebook)

## Purpose:
The primary purpose of this API is to serve as the backend for the `inotebook` application, providing necessary functionalities like user management and note handling.

## Technologies Used:
- ASP.NET Core MVC 6
