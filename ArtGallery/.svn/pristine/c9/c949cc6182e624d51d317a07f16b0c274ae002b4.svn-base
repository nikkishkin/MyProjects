/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     16.07.15 18:13:45                            */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserRole') and o.name = 'FK_USERROLE_REFERENCE_USER')
alter table UserRole
   drop constraint FK_USERROLE_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserRole') and o.name = 'FK_USERROLE_REFERENCE_ROLE')
alter table UserRole
   drop constraint FK_USERROLE_REFERENCE_ROLE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Role')
            and   type = 'U')
   drop table Role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"User"')
            and   type = 'U')
   drop table "User"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserRole')
            and   type = 'U')
   drop table UserRole
go

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role (
   Id                   int                  identity,
   Name                 nvarchar(50)         not null,
   Description          nvarchar(1000)       null,
   constraint PK_ROLE primary key (Id)
)
go

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table "User" (
   Id                   int                  identity,
   Username             nvarchar(100)        not null,
   Password             nvarchar(100)        not null,
   Email                nvarchar(50)         not null,
   "First Name"         nvarchar(100)        not null,
   "Last Name"          nvarchar(100)        not null,
   CreationDate         datetime             not null,
   constraint PK_USER primary key nonclustered (Id)
)
go

/*==============================================================*/
/* Table: UserRole                                              */
/*==============================================================*/
create table UserRole (
   UserId               int                  not null,
   RoleId               int                  not null,
   constraint PK_USERROLE primary key (UserId, RoleId)
)
go

alter table UserRole
   add constraint FK_USERROLE_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table UserRole
   add constraint FK_USERROLE_REFERENCE_ROLE foreign key (RoleId)
      references Role (Id)
go

