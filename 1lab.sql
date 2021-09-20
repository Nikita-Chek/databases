use first;

CREATE TABLE `staff` (
  `employee_id` int(11) NOT NULL,
  `employee_name` varchar(50) DEFAULT NULL,
  `employee_surname` varchar(50) DEFAULT NULL,
  `employee_position` enum('barman','cook') DEFAULT NULL,
  `nick_name` char(20) DEFAULT (concat(`employee_name`,`employee_id`)),
  PRIMARY KEY (`employee_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci

CREATE TABLE `sells` (
  `sell_id` int(11),
  `sell_date` datetime,
  `barman_id` int(11),
  `sell_amount` float,
  PRIMARY KEY (`sell_id`),
  KEY `barman_id` (`barman_id`),
  CONSTRAINT `sells_1` FOREIGN KEY (`barman_id`) REFERENCES `staff` (`employee_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci

CREATE TABLE `products` (
  `product_id` int(11) NOT NULL,
  `product_name` varchar(50) DEFAULT NULL,
  `product_type` varchar(30) DEFAULT NULL,
  `product_price` float DEFAULT NULL,
  PRIMARY KEY (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci

CREATE TABLE `product_sells` (
  `sell_id` int(11) NOT NULL,
  `product_id` int(11) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  PRIMARY KEY (`sell_id`),
  KEY `product_id` (`product_id`),
  CONSTRAINT `product_sells` FOREIGN KEY (`sell_id`) REFERENCES `sells` (`sell_id`),
  CONSTRAINT `product_sells` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci

CREATE TABLE `contracts` (
  `contract_id` int(11) unique,
  `salary` int(11) DEFAULT NULL,
  `contract_start_data` datetime DEFAULT CURRENT_TIMESTAMP,
  `contract_end_data` date,
  KEY `contract_id` (`contract_id`),
  CONSTRAINT `contracts` FOREIGN KEY (`contract_id`) REFERENCES `staff` (`employee_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci





create database BRSM;
use BRSM;

create table Authors
(
author_id integer primary key,
author_name varchar(30)
);

create table Rubrik
(
rubrik_id int primary key,
rubril_name varchar(30),
popularity int(100)
);

create table News
(
news_id integer primary key,
news_name varchar(30),
author_id int,
rubrik_id int,
constraint cn1 foreign key (author_id) references Authors (author_id),
constraint cn2 foreign key (rubrik_id) references Rubrik (rubrik_id)
);

create table Arcticles
(
arcticle_id integer primary key,
arcticle_name varchar(30),
author_id int,
rubrik_id int,
constraint cn4 foreign key (arcticle_id) references Authors (author_id),
foreign key (rubrik_id) references Rubrik (rubrik_id)
);

create table Income
(
author_id int primary key,
salary float,
constraint cn3 foreign key (author_id) references Authors (author_id)
);

create table Ad
(
ad_id integer primary key,
ad_name varchar(30),
cost float
);

create table Arcticles_Ad
(
link_id integer primary key,
ad_id integer,
arcticle_id integer,
foreign key (ad_id) references Ad (ad_id),
foreign key (arcticle_id) references Arcticles (arcticle_id)
);





alter table Income
drop foreign key cn3;

alter table News
drop foreign key cn1;

alter table Arcticles
drop foreign key cn4;

alter table Income
add constraint cn3 foreign key (author_id) references Authors (author_id);

alter table Authors
add column author_birth date;

alter table Authors
drop primary key;

alter table Authors
modify author_id int primary key auto_increment;

insert into Authors
(author_name, author_birth)
values ('Too', '2020-10-9'), ('Roo', '2000-03-9'), ('Doo', '2020-11-4');

update Authors
set author_birth = date_add(curdate(), interval(
if( dayofweek(curdate())>5,
if(dayofweek(curdate())=6, 6, 5),
5-dayofweek(curdate())))
day)
where author_name = 'Too';

alter table authors
add column sex set('m', 'w');

set sql_safe_updates = 0;
UPDATE authors SET sex = CONCAT(sex, 'it');


select * from Authors;