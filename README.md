# HiveMind

This is a personal project with the dual goal of being a playground to experiment, learn, and keep my development skills sharp as well as to help with my day job by storing and organizing important information in a logical, searchable and discoverable manner.


## HiveMind Clients

#### HiveMind-Angular

This Angular project will serve as the web client for this application

## HiveMind Server

#### HiveMind.Aggregate

This Node.js project serves and the API gateway for the backend services.  It will house a GraphQL endpoint that will connect with the various back-end services/APIs.

#### HiveMind.Common

This is a C# library project that will be used accross multiple C# services.  It contains common classes and services.

### HiveMind.Services (APIs)

The `HiveMind.Services...` are a set of Docker containers that make up the backend services (APIs)

#### HiveMind.Servies.Graph

This is a C# project that controls and moderates access to the Neo4j Graph Database.  This project will allow us to connect various entities within the system.  

For example connecting people with projects, ideas, and/or problems and solutions.

#### HiveMind.Services.Ideas

This C# project is used to record random ideas.  These ideas may later turn into actionable objectives.

#### HiveMind.Services.Problems

This C# project is used to record problems and possibly solutions that we encounter on a daily basis.  Recording this problems and solutions in a logical manner will help us later to find soutions to problems we may encounter in the future.

#### HiveMind.Services.Projects

This C# project is used to record information about specific NASA projects.  Within the NASA environment there are numerous projects each with their own personnel, mission, and environment. 

#### HiveMind.Servies.Search

This C# project is used to handle search across the entire HiveMind application.  It will leverage ElasticSearch to power the search capabilities.

#### HiveMind.Services.Work

This Node.js project is used to track and surface work items tracked in various ticketing systems such as Azure DevOPS, and Gemini.  Surfacing these work items here will enable us to gather metrics and associate them with other areas such as problems, projects, ideas, etc.