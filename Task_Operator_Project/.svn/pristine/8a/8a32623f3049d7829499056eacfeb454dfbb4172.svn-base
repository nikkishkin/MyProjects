/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     07.07.15 18:52:23                            */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Attachment') and o.name = 'FK_ATTACHME_REFERENCE_TASK')
alter table Attachment
   drop constraint FK_ATTACHME_REFERENCE_TASK
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Task') and o.name = 'FK_TASK_REFERENCE_USER')
alter table Task
   drop constraint FK_TASK_REFERENCE_USER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Attachment')
            and   type = 'U')
   drop table Attachment
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Task')
            and   type = 'U')
   drop table Task
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"User"')
            and   type = 'U')
   drop table "User"
go

/*==============================================================*/
/* Table: Attachment                                            */
/*==============================================================*/
create table Attachment (
   Id                   int                  identity,
   Path                 nvarchar(150)        not null,
   TaskId               int                  not null,
   constraint PK_ATTACHMENT primary key (Id)
)
go

/*==============================================================*/
/* Table: Task                                                  */
/*==============================================================*/
create table Task (
   Id                   int                  identity,
   Percentage           int                  not null,
   Content              nvarchar(1000)       null,
   Date                 datetime             not null,
   Name                 nvarchar(100)        not null,
   State                tinyint              not null default 0,
   WorkerId             int                  null,
   constraint PK_TASK primary key (Id)
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
   IsManager            bit                  not null,
   AvatarPath           nvarchar(150)        null,
   constraint PK_USER primary key nonclustered (Id)
)
go

alter table Attachment
   add constraint FK_ATTACHME_REFERENCE_TASK foreign key (TaskId)
      references Task (Id)
go

alter table Task
   add constraint FK_TASK_REFERENCE_USER foreign key (WorkerId)
      references "User" (Id)
go

