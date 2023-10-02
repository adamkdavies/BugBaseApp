#BugBaseApp

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

```
{
    "noteId": long,
    "noteText": string,
    "ticketId": long,
    "noteOwnerId": long,
    *"ticket": [Ticket](#ticket)*,
    *"noteOwner": [User](#user)*
}
```

### Role


```
{
    "roleId": long,
    "roleName": string
}
```

### State

```
{
    "stateId": long,
    "stateName": string
}
```

### TicketChangeHistory

```
{
    "ticketChangeHistoryId": long,
    "ticketChangeTypeId": long,
    "ticketId": long,
    "ticketChangeDateTime": string,
    "title": string,
    "description": string
    "stateId": long,
    "qaOwnerId" : long,
    "devOwnerId" : long,
    "asssignedToId" : long,
    *"noteText" : [Note](#note)*,
    *"ticket" : [Ticket](#ticket)*,
    *"ticketChangeType" : [TicketChangeType](#ticketchangetype)*
}
```

### TicketChangeType

```
{
    "ticketChangeTypeId": long,
    "ticketChangeTypeName": string
}
```

### Ticket

```
{
    "ticketId": long,
    "title": string,
    "description" : string,
    "product" : string,
    "feature" : string,
    "iteration" : string,
    "stateId" : long,
    "qaOwnerId" : long,
    "devOwnerId" : long,
    "assignedToId" : long,
    *"assignedTo" : [User](#user),*
    *"devOwner" : [User](#user),*
    *"qaOwner" : [User](#user),*
    *"state" : [State](#state),*
    *"ticketChangeHistories" : [ [TicketChangeHistory](#ticketchangehistory) ]*
}
```

### User

```
{
    "userId : long,
    "userName" : string,
    "displayName" : string,
    "email" : string,
    "phone" : string,
    "roleId" : long,
    *"role" : [Role](#role)*
}
```

**Italicized items are output only.*

## Instructions

To run this application, you need sqlite3 to generate the database.

> choco install sqlite

If you don't have Chocolatey, download instructions are [here](https://chocolatey.org/install#individual).


## Notes

Some things considered but intentionally consided out of scope:

* Proper configurations for Product, Feature, Iteration/Release.
* Ability to configure alternate states and assignment triggers for tickets, safely.
* The handling of a *Removed* state that is typical in a tracking system.
* Securing of the endpoints.
* Support for likely third-party user environment (e.g. - AD/LDAP).
