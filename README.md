# BugBaseApp

A simple application to track bugs.

## Endpoints

### Notes

* /api/Notes GET,POST
* /api/Notes/{id} GET
  
### Roles

* /api/Roles GET
* /api/Roles{id} GET

### States

* /api/States GET
* /api/States/{id} GET

### TicketChangeHistories

* /api/TicketChangeHistories GET
* /api/TicketChangeHistories/{id} GET

### TicketChangeTypes

* /api/TicketChangeTypes GET
* /api/TicketChangeTypes/{id} GET

### Tickets

* /api/Tickets GET,POST
* /api/Tickets/{id} GET,PATCH,DELETE

### Users

* /api/Users GET,POST
* /api/Users/{id} GET,PATCH,DELETE

## Payloads

### Note

>{
>    "noteId": long,
>   "noteText": string,
>   "ticketId": long,
>   "noteOwnerId": long,
>   *"ticket": [Ticket](#ticket)*,
>    *"noteOwner": [User](#user)*
>}

### Role



>{
>    "roleId": long,
>    "roleName": string
>}

### State

>{
>    "stateId": long,
>    "stateName": string
>}

### TicketChangeHistory

>{
>    "ticketChangeHistoryId": long,
>    "ticketChangeTypeId": long,
>    "ticketId": long,
>    "ticketChangeDateTime": string,
>    "title": string,
>    "description": string
>    "stateId": long,
>    "qaOwnerId" : long,
>    "devOwnerId" : long,
>    "asssignedToId" : long,
>    *"noteText" : [Note](#note)*,
>    *"ticket" : [Ticket](#ticket)*,
>    *"ticketChangeType" : [TicketChangeType](#ticketchangetype)*
>}

### TicketChangeType

>{
>    "ticketChangeTypeId": long,
>    "ticketChangeTypeName": string
>}

### Ticket


>{
>    "ticketId": long,
>    "title": string,
>    "description" : string,
>    "product" : string,
>    "feature" : string,
>    "iteration" : string,
>    "stateId" : long,
>    "qaOwnerId" : long,
>    "devOwnerId" : long,
>    "assignedToId" : long,
>    *"assignedTo" : [User](#user),*
>    *"devOwner" : [User](#user),*
>    *"qaOwner" : [User](#user),*
>    *"state" : [State](#state),*
>    *"ticketChangeHistories" : [ [TicketChangeHistory](#ticketchangehistory) ]*
>}

### User

>{
>    "userId : long,
>    "userName" : string,
>    "displayName" : string,
>    "email" : string,
>    "phone" : string,
>    "roleId" : long,
>    *"role" : [Role](#role)*
>}

**Italicized items are output only.*

## Instructions

To run this application, you need sqlite3 to generate the database.

> choco install sqlite

If you don't have Chocolatey, download instructions are [here](https://chocolatey.org/install#individual).

Lastly, assignment should be handled via state transitions, by setting stateId in the [Ticket](#ticket).

State transitions are as follows:

[Ticket](#ticket).assignedToId = state in {New, Fixed, Resolved, Can't Reproduce, Invalid, Duplicate} then [Ticket](#ticket).qaOwnerId else [Ticket](#ticket).devOwnerId


## Notes

Some things considered but intentionally consided out of scope:

* Proper configurations for Product, Feature, Iteration/Release.
* Ability to configure alternate states and assignment triggers for tickets, safely.
* The handling of a *Removed* state that is typical in a tracking system.
* Securing of the endpoints.
* Support for likely third-party user environment (e.g. - AD/LDAP).
* Unit testing.

Some things stubled upon, and to revisit:

* Pure automatic generation of models/context - don't know if this exists for core for Entity Framework, most answers involved scaffolding tools.  If they do, they would need some configurations for typical sqlite id's, or I'd have to transition to text and guid generators for id's.
* Proper handling of unique items, not null items, etc.
* Getting swashbuckle to replace the example value properly.