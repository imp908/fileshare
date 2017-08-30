
--create 
-------------------
drop table employees;
drop table project;
drop table emp_pro;

create table employees (
emp_no int primary key,Name varchar(15), Address varchar(15), Profession varchar(15), dept_no int,
manager_no int, Salary number );

create table project (pro_name varchar(10), pro_no int , pro_mng int );

create table emp_pro ( p_no int, emp_no int , precent_time number  );
-------------------


--insert
-------------------
insert into employees values (1001,'NAME 1001','Address_4','Proffession_7',1,1015,20000);
insert into employees values (1002,'NAME 1002','Address_3','Proffession_4',4,1015,45000);
insert into employees values (1003,'NAME 1003','Address_4','Proffession_7',1,1010,15000);
insert into employees values (1004,'NAME 1004','Address_3','Proffession_2',3,1015,50000);
insert into employees values (1005,'NAME 1005','Address_1','Proffession_3',4,1015,10000);
insert into employees values (1006,'NAME 1006','Address_2','Proffession_3',2,1009,40000);
insert into employees values (1007,'NAME 1007','Address_4','Proffession_5',2,1012,20000);
insert into employees values (1008,'NAME 1008','Address_2','Proffession_7',1,1014,20000);
insert into employees values (1009,'NAME 1009','Address_1','Proffession_3',4,1015,35000);
insert into employees values (1010,'NAME 1010','Address_4','Proffession_7',4,1013,15000);
insert into employees values (1011,'NAME 1011','Address_2','Proffession_5',2,1012,20000);
insert into employees values (1012,'NAME 1012','Address_1','Proffession_3',2,1013,20000);
insert into employees values (1013,'NAME 1013','Address_3','Proffession_3',4,1014,45000);
insert into employees values (1014,'NAME 1014','Address_5','Proffession_2',4,1016,50000);
insert into employees values (1015,'NAME 1015','Address_1','Proffession_3',4,1016,55000);
insert into employees values (1016,'NAME 1016','Address_1','Proffession_7',3,1016,45000);



insert into project values ('Prog 1',1,1001);
insert into project values ('Prog 2',2,1003);
insert into project values ('Prog 3',3,1001);
insert into project values ('Prog 4',4,1001);
insert into project values ('Prog 5',5,1003);
insert into project values ('Prog 6',6,1008);
insert into project values ('Prog 7',7,1001);
insert into project values ('Prog 8',8,1001);
insert into project values ('Prog 9',9,1003);
insert into project values ('Prog 10',10,1008);
insert into project values ('Prog 11',11,1002);
insert into project values ('Prog 12',12,1004);
insert into project values ('Prog 13',13,1004);
insert into project values ('Prog 14',14,1003);
insert into project values ('Prog 15',15,1004);
insert into project values ('Prog 16',16,1005);



insert into emp_pro values (1,1001,50);
insert into emp_pro values (2,1003,21);
insert into emp_pro values (3,1001,41);
insert into emp_pro values (4,1001,76);
insert into emp_pro values (5,1003,63);
insert into emp_pro values (6,1008,3);
insert into emp_pro values (7,1001,60);
insert into emp_pro values (8,1001,98);
insert into emp_pro values (9,1003,97);
insert into emp_pro values (10,1008,68);
insert into emp_pro values (11,1002,91);
insert into emp_pro values (12,1004,55);
insert into emp_pro values (13,1004,97);
insert into emp_pro values (14,1003,5);
insert into emp_pro values (15,1004,62);
insert into emp_pro values (16,1005,74);
-------------------




--Q/A
-------------------

--1) All employes with the same Profession,dept_no as employee with id 1006
with t as (
select Profession,dept_no from employees
where emp_no = '1006')
, t2 as (
select * from employees
)
select * from t2 
join t on t2.Profession = t.Profession and t2.dept_no=t.dept_no;

--2) progect number and progect manager name
/*
assuming, that manager of project is employee and we need not his name but
to populate name of employee who is manager of an employee who is, in his turn,
manager of a progect 
*/
select ep.pro_name,em2.Name from project ep
left join employees em on ep.pro_mng = em.emp_no
left join employees em2 on em2.emp_no=em.manager_no
;

--3) select amployees with salary > then their manager
select em.emp_no,em.Name from employees em
join employees em2 on em.manager_no=em2.emp_no
where (em.salary > em2.salary)
;

--4) all amployees working on > 3 projects
select * from employees em where em.emp_no in (
select emp_no from emp_pro group by emp_no having (count (emp_no) >3)
);

--5) professions and salary of employees from same address as employee with max salary
with t as(
select em.profession,em.salary,em.address from employees em
where em.salary =( select max(salary) from employees)
)
select em2.profession,em2.salary 
from employees em2 join t on em2.address = t.address
;
-------------------
