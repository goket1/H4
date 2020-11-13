DROP SCHEMA public CASCADE;
CREATE SCHEMA public;

GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;
GRANT ALL ON SCHEMA public TO superuser;

-- Owner
create table OwnerName(
    Id serial,
    First text,
    Last text,
    PRIMARY KEY (Id)
);

create table Address(
    Id serial,
    No int,
    StreetName text,
    Postcode int,
    -- TODO Postal codes? country specific?
    City text,
    primary key (Id)
);

create table Owner(
    CVR int,
    Email text,
    Name int,
    Address int,

    foreign key (Address) references Address(Id),
    foreign key (Name) references OwnerName(Id),

    primary key (CVR)
);

create table PhoneNumber(
    OwnerCVR int,
    PhoneNumber text,
    primary key (OwnerCVR, PhoneNumber),
    foreign key (OwnerCVR) references Owner(CVR)
    -- TODO County codes?
);

-- Farm
create table Farm(
    -- produktionsenhedsnummer / virk lokalitets nummer
    PNumber int,
    -- Centrale HusdyrbrugsRegister
    Chr int,
    Name text,
    Address Address,
    primary key (Chr)
);

create table BoxType(
    Id serial,
    Type Text,
    primary key (Id)
);

create table Box(
    No int,
    Outdoor bool,
    Type int,
    primary key (No),
    foreign key (Type) references BoxType(Id)
);

-- Stall
create table Stall(
    No serial,
    FarmChr int,
    foreign key (FarmChr) references Farm(Chr)
);

create table SmartUnitType(
    Id serial,
    Type Text,
    primary key (Id)
);

create table SmartUnit(
    MacAddress macaddr,
    IpAddress inet,
    Serialnumber int,
    Type int,
    primary key(Serialnumber),
    foreign key(Type) references SmartUnitType(Id)
);

create table BoxMonitor(
    Value text,
    Time timestamp,
    BoxNo int,
    foreign key (BoxNo) references Box(No)
);

CREATE TYPE StateSeverity AS ENUM ('info', 'verbose', 'error', 'critical');

create table State(
    Id serial,
    Severity StateSeverity,
    primary key (Id)
);

create table SmartUnitState(
    Time timestamp,
    SmartUnitSerialNumber int,
    StateId int,
    primary key (Time, SmartUnitSerialNumber, StateId),
    foreign key (SmartUnitSerialNumber) references SmartUnit(Serialnumber),
    foreign key (StateId) references State(Id)
);

CREATE TYPE EarmarkColor AS ENUM ('red', 'green', 'blue');

create table Earmark(
    Id int,
    Color EarmarkColor,
    -- Centrale HusdyrbrugsRegister
    ChrNo int,
    primary key (Id)
);

create table AnimalType(
    Id serial,
    Name text
);

create table Animal(
    Earmark Earmark,
    Sex bool, -- Triggered?
    

    primary key (Earmark)
)