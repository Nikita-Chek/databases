use lab6;

# tables

drop table alcoholic;
create table alcoholic
(
    id int primary key auto_increment,
    name varchar(20),
    experience tinyint,
    alcohol_id int,
    company_id int,
    constraint alc_1 foreign key (company_id) references company(id),
    constraint alc_2 foreign key (alcohol_id) references alcohol(id)
);

drop table company;
create table company
(
    id int primary key auto_increment,
    city varchar(20),
    members int
);

drop table alcohol;
create table alcohol
(
    id int primary key auto_increment,
    name varchar(20)
);

drop table meeting;
create table meeting
(
    id int primary key auto_increment,
    company_id int,
    date timestamp default current_timestamp(),
    constraint mee_1 foreign key (company_id) references company(id)
);

drop table attendence;
create table attendence
(
    meeting_id int,
    alcoholic_id int,
    attendence bool default false,
    constraint att_1 foreign key (meeting_id) references meeting(id),
    constraint att_2 foreign key (alcoholic_id) references alcoholic(id)
);

drop table alcoholic_attendence;
create  table alcoholic_attendence
(
    alcoholic_id int primary key,
    miss int default 0,
    constraint acl_att_1 foreign key (alcoholic_id) references alcoholic(id)
);

drop table alcoholic_diary;
create table alcoholic_diary
(
    alcoholic_id int primary key,
    alcoholic_miss int default 0,
    alcoholic_drink int default 0,
    constraint alc_dia_1 foreign key (alcoholic_id) references alcoholic (id)
);



# procedure trigger

create trigger attendence_monitoring after insert on attendence
    for each row
    begin
        if !new.attendence then
            update alcoholic_attendence
                set miss = miss + 1
            where alcoholic_attendence.alcoholic_id = new.alcoholic_id;
        end if;
    end;

create trigger attendence_chek before insert on attendence
    for each row
    begin
        if !(new.alcoholic_id in (select alcoholic.id from alcoholic
            inner join company c on alcoholic.company_id = c.id
            inner join meeting m on c.id = m.company_id
        where m.id = new.meeting_id)) then
        signal sqlstate '45000'
            set message_text = 'No alcoholic in the company';
        end if;
    end;
insert into attendence values
(2,1,1);


create trigger alcoholic_attendence_update after update on alcoholic_attendence
    for each row
    begin
        update alcoholic_diary
            set alcoholic_miss = new.miss
        where alcoholic_id = new.alcoholic_id;
    end;

drop procedure alcoholic_transfer;
create procedure alcoholic_transfer(in alc int, in new_comp int)
begin
    declare old_comp int;
    set old_comp = (select company_id from alcoholic where id = alc);

    start transaction;
    update alcoholic
        set company_id = new_comp
        where id = alc;
    if row_count() > 0 then
        update company
            set members = members - 1
        where id = old_comp;
        update company
            set members = members + 1
        where id = new_comp;
        commit;
    else rollback;
    end if;
end;

select * from company;
select * from alcoholic where company_id = 1;
call alcoholic_transfer(15,4);

create trigger alcoholic_insert after insert on alcoholic
    for each row
    begin
        update company
            set members = members + 1
        where NEW.company_id = company.id;
    end;

drop procedure company_members_count;
create procedure company_members_count()
begin
    declare end int;
    declare i int;
    set end = (select count(id) from company), i = 1;
    label1: loop
        update company
        set members = (select count(id) from alcoholic
                        where company_id = i)
        where company.id = i;
        set i = i+1;
    if i <= end then
       iterate label1;
    end if;
    leave label1;
    end loop label1;
end;
call company_members_count();

create procedure drink(in alc int, in litrs int)
begin
    update alcoholic_diary
        set alcoholic_drink = alcoholic_drink + litrs
    where alcoholic_id = alc;
end;
call drink(1, 3);


# view

create or replace view top5_alcoholic as
    (select name, experience from alcoholic
    order by experience desc limit 5);

select * from top5_alcoholic;


create or replace view unsuccessful_alcoholic as
    (select alcoholic.name, alcoholic.id, (ad.alcoholic_miss + ad.alcoholic_drink)/2 from alcoholic
    inner join alcoholic_diary ad on alcoholic.id = ad.alcoholic_id
    order by (ad.alcoholic_miss + ad.alcoholic_drink)/2 desc);
select * from unsuccessful_alcoholic;

select * from alcoholic_diary;

# index

drop index alc_index on alcoholic;
create index alc_index on alcoholic(name(3));

explain select * from alcoholic
where name like 'D%';

# user
use lab6;

create table test
(
    id int
);

drop user 'user1'@'localhost';
create user 'user1'@'localhost' identified by '12345';
grant GRANT OPTION, select, create on lab6.* to 'user1';

drop user user2;
create user user2 identified by 'qwerty';
grant select on lab6.top5_alcoholic to user2;

revoke create on lab6.* from user1;
drop table test;