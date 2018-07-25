/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     07.07.15 18:43:27                            */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Task') and o.name = 'FK_TASK_REFERENCE_USER')
alter table Task
   drop constraint FK_TASK_REFERENCE_USER
go

alter table Task
   drop constraint PK_TASK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tmp_Task')
            and   type = 'U')
   drop table tmp_Task
go

execute sp_rename Task, tmp_Task
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

set identity_insert Task on
go

insert into Task (Id, Percentage, Content, Date, Name, State, WorkerId)
select Id, Percentage, Content, Date, Name, State, WorkerId
from tmp_Task
go

set identity_insert Task off
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tmp_Task')
            and   type = 'U')
   drop table tmp_Task
go

alter table Attachment
   add constraint FK_ATTACHME_REFERENCE_TASK foreign key (TaskId)
      references Task (Id)
go

alter table Task
   add constraint FK_TASK_REFERENCE_USER foreign key (WorkerId)
      references "User" (Id)
go

