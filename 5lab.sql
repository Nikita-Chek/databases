/* 1////////////////////////////////////// */

create database lab5;
use lab5;

drop table person;
create table person
(
person_id int primary key auto_increment,
person_name varchar(20)
);
insert into person (person_name) values
('nick'), ('mike'), ('anne');

drop table account;
create table account
(
account_id int primary key auto_increment,
account_balance double,
account_owner int,
constraint cn1 foreign key (account_owner) references person (person_id)
);


/* 2////////////////////////////////////// */
/* 3////////////////////////////////////// */
/* 4////////////////////////////////////// */
/* 5////////////////////////////////////// */