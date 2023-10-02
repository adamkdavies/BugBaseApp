--
-- File generated with SQLiteStudio v3.4.4 on Mon Oct 2 01:45:02 2023
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Note
DROP TABLE IF EXISTS Note;

CREATE TABLE IF NOT EXISTS Note (
    NoteId      INTEGER PRIMARY KEY
                        UNIQUE,
    NoteText    TEXT,
    TicketID    INTEGER REFERENCES Ticket (TicketID),
    NoteOwnerId INTEGER REFERENCES User (UserId) 
);


-- Table: Role
DROP TABLE IF EXISTS Role;

CREATE TABLE IF NOT EXISTS Role (
    RoleId   INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleName TEXT    UNIQUE
);

INSERT INTO Role (
                     RoleId,
                     RoleName
                 )
                 VALUES (
                     0,
                     'Tester'
                 );

INSERT INTO Role (
                     RoleId,
                     RoleName
                 )
                 VALUES (
                     1,
                     'Developer'
                 );

INSERT INTO Role (
                     RoleId,
                     RoleName
                 )
                 VALUES (
                     2,
                     'Manager'
                 );

INSERT INTO Role (
                     RoleId,
                     RoleName
                 )
                 VALUES (
                     3,
                     'Product Owner'
                 );

INSERT INTO Role (
                     RoleId,
                     RoleName
                 )
                 VALUES (
                     4,
                     'Customer'
                 );


-- Table: State
DROP TABLE IF EXISTS State;

CREATE TABLE IF NOT EXISTS State (
    StateID   INTEGER PRIMARY KEY,
    StateName TEXT
);

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      0,
                      'New'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      1,
                      'Assigned'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      2,
                      'Working On It'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      3,
                      'Fixed'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      4,
                      'Not Fixed'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      5,
                      'Resolved'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      6,
                      'Can''t Reproduce'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      7,
                      'Invalid'
                  );

INSERT INTO State (
                      StateID,
                      StateName
                  )
                  VALUES (
                      8,
                      'Duplicate'
                  );


-- Table: Ticket
DROP TABLE IF EXISTS Ticket;

CREATE TABLE IF NOT EXISTS Ticket (
    TicketId     INTEGER PRIMARY KEY,
    Title        TEXT,
    Description  TEXT,
    Product      TEXT,
    Feature      TEXT,
    Iteration    TEXT,
    StateId      INTEGER REFERENCES State (StateID),
    QAOwnerId    INTEGER REFERENCES User (UserID),
    DevOwnerId   INTEGER REFERENCES User (UserID),
    AssignedToId INTEGER REFERENCES User (UserID) 
);


-- Table: TicketChangeHistory
DROP TABLE IF EXISTS TicketChangeHistory;

CREATE TABLE IF NOT EXISTS TicketChangeHistory (
    TicketChangeHistoryId INTEGER PRIMARY KEY AUTOINCREMENT
                                  UNIQUE,
    TicketChangeTypeId    INTEGER REFERENCES TicketChangeType (TicketChangeTypeId),
    TicketId              INTEGER REFERENCES Ticket (TicketID),
    TicketChangeDateTime  TEXT,
    Title                 TEXT    NOT NULL,
    Description           TEXT    NOT NULL,
    Product               TEXT,
    Feature               TEXT,
    Iteration             TEXT,
    StateId               INTEGER REFERENCES State (StateID),
    QAOwnerId             INTEGER REFERENCES User (UserId),
    DevOwnerId            INTEGER REFERENCES User (UserId),
    AssignedToId          INTEGER REFERENCES User (UserId),
    NoteText              TEXT
);


-- Table: TicketChangeType
DROP TABLE IF EXISTS TicketChangeType;

CREATE TABLE IF NOT EXISTS TicketChangeType (
    TicketChangeTypeId   INTEGER PRIMARY KEY AUTOINCREMENT
                                 UNIQUE,
    TicketChangeTypeName TEXT
);

INSERT INTO TicketChangeType (
                                 TicketChangeTypeId,
                                 TicketChangeTypeName
                             )
                             VALUES (
                                 0,
                                 'Created'
                             );

INSERT INTO TicketChangeType (
                                 TicketChangeTypeId,
                                 TicketChangeTypeName
                             )
                             VALUES (
                                 1,
                                 'Modified'
                             );

INSERT INTO TicketChangeType (
                                 TicketChangeTypeId,
                                 TicketChangeTypeName
                             )
                             VALUES (
                                 2,
                                 'Annotated'
                             );


-- Table: User
DROP TABLE IF EXISTS User;

CREATE TABLE IF NOT EXISTS User (
    UserId      INTEGER PRIMARY KEY
                        UNIQUE,
    UserName    TEXT    UNIQUE,
    DisplayName TEXT,
    Email       TEXT,
    Phone       TEXT,
    RoleId      INTEGER REFERENCES Role (RoleID) 
);


-- Trigger: AnnotationTrigger
DROP TRIGGER IF EXISTS AnnotationTrigger;
CREATE TRIGGER IF NOT EXISTS AnnotationTrigger
                       AFTER INSERT
                          ON Note
BEGIN
    INSERT INTO TicketChangeHistory (
                                        TicketChangeTypeId,
                                        TicketID,
                                        TicketChangeDateTime,
                                        NoteText
                                    )
                                    VALUES (
                                        2,
                                        new.TicketID,
                                        strftime('%Y-%m-%d %H-%M-%f', 'now'),
                                        new.NoteText
                                    );
END;


-- Trigger: TicketDelete
DROP TRIGGER IF EXISTS TicketDelete;
CREATE TRIGGER IF NOT EXISTS TicketDelete
                      BEFORE DELETE
                          ON Ticket
BEGIN
    DELETE FROM Note
          WHERE TicketId == old.TicketId;
    DELETE FROM TicketChangeHistory
          WHERE TicketId == old.TicketId;
END;


-- Trigger: TicketInsert
DROP TRIGGER IF EXISTS TicketInsert;
CREATE TRIGGER IF NOT EXISTS TicketInsert
                       AFTER INSERT
                          ON Ticket
BEGIN
    INSERT INTO TicketChangeHistory (
                                        TicketChangeTypeId,
                                        TicketID,
                                        TicketChangeDateTime,
                                        Title,
                                        Description,
                                        StateId,
                                        QAOwnerId,
                                        DevOwnerId,
                                        AssignedToId
                                    )
                                    VALUES (
                                        0,
                                        new.TicketID,
                                        strftime('%Y-%m-%d %H-%M-%f', 'now'),
                                        new.Title,
                                        new.Description,
                                        new.StateId,
                                        new.QAOwnerId,
                                        new.DevOwnerId,
                                        new.AssignedToId
                                    );
END;


-- Trigger: TicketModify
DROP TRIGGER IF EXISTS TicketModify;
CREATE TRIGGER IF NOT EXISTS TicketModify
                       AFTER UPDATE
                          ON Ticket
BEGIN
    INSERT INTO TicketChangeHistory (
                                        TicketChangeTypeId,
                                        TicketID,
                                        TicketChangeDateTime,
                                        Title,
                                        Description,
                                        Product,
                                        Feature,
                                        Iteration,
                                        StateId,
                                        QAOwnerId,
                                        DevOwnerId,
                                        AssignedToId
                                    )
                                    VALUES (
                                        1,
                                        new.TicketID,
                                        strftime('%Y-%m-%d %H-%M-%f', 'now'),
                                        CASE WHEN new.Title != old.Title THEN new.Title ELSE NULL END,
                                        CASE WHEN new.Description != old.Description THEN new.Description ELSE NULL END,
                                        CASE WHEN new.Product != old.Product THEN new.Product ELSE NULL END,
                                        CASE WHEN new.Feature != old.Feature THEN new.Feature ELSE NULL END,
                                        CASE WHEN new.Iteration != old.Iteration THEN new.Iteration ELSE NULL END,
                                        CASE WHEN new.StateId != old.StateId THEN new.StateId ELSE NULL END,
                                        CASE WHEN new.QAOwnerId != old.QAOwnerId THEN new.QAOwnerId ELSE NULL END,
                                        CASE WHEN new.DevOwnerId != old.DevOwnerId THEN new.DevOwnerId ELSE NULL END,
                                        CASE WHEN new.AssignedToId != old.AssignedToId THEN new.AssignedToId ELSE NULL END
                                    );
    UPDATE Ticket
       SET AssignedToId = CASE WHEN new.StateId == 0 OR 
                                    new.StateId == 3 OR 
                                    new.StateId == 5 OR 
                                    new.StateId == 6 OR 
                                    new.StateId == 7 OR 
                                    new.StateId == 8 THEN old.QAOwnerId ELSE new.DevOwnerId END
     WHERE old.TicketId == new.TicketId AND 
           old.StateId != new.StateId;
END;


COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
