USE OEMS
Go

CREATE TABLE Students
(
StudentId int IDENTITY (1, 1) NOT NULL,
Name varchar(100),
AccountId int,
CourseId int,
CONSTRAINT PK_Students PRIMARY KEY NONCLUSTERED (StudentId),
);

Go

CREATE TABLE Lecturers
(
LecturerId int IDENTITY (1, 1) NOT NULL,
Name varchar(100),
Area varchar(30),
AccountId int,
CourseId int,
CONSTRAINT PK_Lecturers PRIMARY KEY NONCLUSTERED (LecturerId),
);

Go


CREATE TABLE Accounts
(
AccountId int IDENTITY (1, 1) NOT NULL,
Username varchar(20) NULL,
Password varchar(10) NULL,
FName varchar(60) NULL,
LName varchar(60) NULL,
Type varchar(10) NULL,
CONSTRAINT PK_Accounts PRIMARY KEY NONCLUSTERED (AccountId),
);

Go

CREATE TABLE Courses
(
CourseId int IDENTITY (1, 1) NOT NULL,
Name varchar(40),
CONSTRAINT PK_Courses PRIMARY KEY NONCLUSTERED (CourseId),
);

Go



ALTER TABLE Students    
ADD CONSTRAINT FK_Students_Accounts FOREIGN KEY (AccountId)     
    REFERENCES Accounts (AccountId)     
    ON DELETE CASCADE    
    ON UPDATE CASCADE    
;    
GO

ALTER TABLE Students    
ADD CONSTRAINT FK_Students_Courses FOREIGN KEY (CourseId)     
    REFERENCES Courses (CourseId)     
    ON DELETE CASCADE    
    ON UPDATE CASCADE    
;    
GO 

ALTER TABLE Lecturers   
ADD CONSTRAINT FK_Lecturers_Accounts FOREIGN KEY (AccountId)     
    REFERENCES Accounts (AccountId)     
    ON DELETE CASCADE    
    ON UPDATE CASCADE    
;    
GO 

ALTER TABLE Lecturers   
ADD CONSTRAINT FK_Lecturers_Courses FOREIGN KEY (CourseId)     
    REFERENCES Courses (CourseId)     
    ON DELETE CASCADE    
    ON UPDATE CASCADE    
;    
GO 