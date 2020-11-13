-- Stall

-- Farm
create table Farm(
    -- produktionsenhedsnummer / virk lokalitets nummer
    PNumber integer,
    -- Centrale HusdyrbrugsRegister
    Chr integer,
    Name text,
    Address Address
);

-- Owner
create table OwnerName(
    Id serial,
    First text,
    Last text,
    PRIMARY KEY (Id)
);

create table Address(
    Id serial,
    No integer,
    StreetName text,
    Postcode integer,
    -- TODO Postal codes? country specific?
    City text,
    primary key (Id)
);

create table PhoneNumber(
    OwnerCVR INTEGER,
    PhoneNumber text,
    primary key (OwnerCVR, PhoneNumber)
    -- TODO County codes?
);

create table owner(
    CVR integer,
    Email text,
    Name integer,
    Address integer,

    foreign key (Address) references Address(Id),
    foreign key (Name) references OwnerName(Id),

    primary key (CVR)
);