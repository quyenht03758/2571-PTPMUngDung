/* ===========================================================
   CompanyDB - Full schema + rich seed data
   Compatible with SQL Server (SSMS)
   =========================================================== */

IF DB_ID('CompanyDB') IS NOT NULL
BEGIN
    ALTER DATABASE CompanyDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CompanyDB;
END
GO
CREATE DATABASE CompanyDB;
GO
USE CompanyDB;
GO

/* =======================
   1) SCHEMA
   ======================= */

-- Department
CREATE TABLE Department
(
    Dnumber      INT          NOT NULL PRIMARY KEY,
    Dname        VARCHAR(30)  NOT NULL UNIQUE,
    MgrSSN       CHAR(9)      NULL,
    MgrStartDate DATE         NULL
);

-- Employee
CREATE TABLE Employee
(
    Fname       VARCHAR(15)  NOT NULL,
    Minit       CHAR(1)      NULL,
    Lname       VARCHAR(15)  NOT NULL,
    Ssn         CHAR(9)      NOT NULL PRIMARY KEY,
    Bdate       DATE         NULL,
    Address     VARCHAR(100) NULL,
    Sex         CHAR(1)      NULL,           -- 'M'/'F'
    Salary      DECIMAL(10,2) NULL,
    Super_ssn   CHAR(9)      NULL,           -- self-FK
    Dno         INT          NOT NULL        -- Department
);

ALTER TABLE Employee
  ADD CONSTRAINT FK_Employee_Department
      FOREIGN KEY (Dno) REFERENCES Department(Dnumber);

ALTER TABLE Employee
  ADD CONSTRAINT FK_Employee_Supervisor
      FOREIGN KEY (Super_ssn) REFERENCES Employee(Ssn);

-- Project
CREATE TABLE Project
(
    Pnumber   INT          NOT NULL PRIMARY KEY,
    Pname     VARCHAR(50)  NOT NULL,
    Plocation VARCHAR(50)  NULL,
    Dnum      INT          NOT NULL
);
ALTER TABLE Project
  ADD CONSTRAINT FK_Project_Department
      FOREIGN KEY (Dnum) REFERENCES Department(Dnumber);

-- WorksOn (Employee ↔ Project)
CREATE TABLE WorksOn
(
    Essn   CHAR(9)       NOT NULL,
    Pno    INT           NOT NULL,
    Hours  DECIMAL(6,2)  NOT NULL DEFAULT 0,
    CONSTRAINT PK_WorksOn PRIMARY KEY (Essn, Pno),
    CONSTRAINT FK_WorksOn_Emp FOREIGN KEY (Essn) REFERENCES Employee(Ssn),
    CONSTRAINT FK_WorksOn_Proj FOREIGN KEY (Pno)  REFERENCES Project(Pnumber)
);
GO

/* =======================
   2) SEED DATA
   ======================= */

-- Departments (8)
INSERT Department (Dnumber, Dname, MgrSSN, MgrStartDate) VALUES
(1,'HR',            NULL, NULL),
(2,'IT',            NULL, NULL),
(3,'Finance',       NULL, NULL),
(4,'R&D',           NULL, NULL),
(5,'Marketing',     NULL, NULL),
(6,'Sales',         NULL, NULL),
(7,'Operations',    NULL, NULL),
(8,'Support',       NULL, NULL);

-- Employees (28)
-- Tip: có nhiều tháng sinh khác nhau để test "birthday in current month"
INSERT Employee (Fname,Minit,Lname,Ssn,Bdate,Address,Sex,Salary,Super_ssn,Dno) VALUES
('An',   'Q','Nguyen', '100000001','1995-01-10','HCM', 'M', 1200,NULL,2),
('Binh', 'A','Tran',   '100000002','1990-02-20','Hanoi','F', 1300,'100000001',2),
('Chi',  'B','Le',     '100000003','1992-03-15','Da Nang','F',1250,'100000001',2),
('Dung', 'C','Pham',   '100000004','1991-04-25','HCM', 'M', 1500,'100000001',4),
('Em',   'D','Ho',     '100000005','1993-05-30','HCM', 'F', 1400,'100000004',4),
('Giang','E','Vo',     '100000006','1994-06-12','Hanoi','F',1350,'100000004',4),
('Huy',  'F','Phan',   '100000007','1995-07-07','HCM', 'M', 1600,'100000002',3),
('Khanh','G','Do',     '100000008','1996-08-18','Hanoi','M', 1700,'100000007',3),
('Lam',  'H','Tran',   '100000009','1997-09-05','HCM', 'M', 1450,'100000007',3),
('Mai',  'I','Nguyen', '100000010','1998-10-23','HCM', 'F', 1500,'100000002',1),
('Nam',  'J','Bui',    '100000011','1991-11-11','Hanoi','M', 1550,'100000010',1),
('Oanh', 'K','Truong', '100000012','1992-12-29','Da Nang','F',1500,'100000010',1),

('Phuc','L','Nguyen',  '100000013','1990-01-02','HCM','M',1800,NULL,5),
('Quynh','M','Vo',     '100000014','1992-02-14','Hanoi','F',1400,'100000013',5),
('Rin',  'N','Le',     '100000015','1993-03-09','HCM','F',1450,'100000013',5),
('Son',  'O','Pham',   '100000016','1991-04-17','HCM','M',1750,'100000013',6),
('Trang','P','Do',     '100000017','1994-05-21','Hanoi','F',1550,'100000016',6),
('Uyen', 'Q','Bui',    '100000018','1996-06-28','HCM','F',1500,'100000016',6),

('Viet','R','Ngo',     '100000019','1995-07-03','Hanoi','M',1650,NULL,7),
('Wy',  'S','Dang',    '100000020','1990-08-08','HCM','F',1450,'100000019',7),
('Xuan','T','Phan',    '100000021','1992-09-12','Da Nang','F',1480,'100000019',7),
('Yen', 'U','Nguyen',  '100000022','1997-10-26','HCM','F',1500,'100000019',8),
('Zed', 'V','Tran',    '100000023','1998-11-04','Hanoi','M',1350,'100000022',8),
('Anh', 'W','Le',      '100000024','1993-12-16','HCM','F',1380,'100000022',8),

('Bao', 'X','Pham',    '100000025','1995-03-25','HCM','M',1600,'100000001',2),
('Cuong','Y','Hoang',  '100000026','1991-06-06','Hanoi','M',1720,'100000004',4),
('Dieu','Z','Vu',      '100000027','1994-09-19','Da Nang','F',1520,'100000007',3),
('Gia', 'A','Ly',      '100000028','1990-12-01','HCM','M',1820,'100000013',5);

-- Set some managers (MgrSSN) after employees exist
UPDATE Department SET MgrSSN='100000001', MgrStartDate='2020-01-01' WHERE Dnumber=2; -- IT
UPDATE Department SET MgrSSN='100000007', MgrStartDate='2020-02-01' WHERE Dnumber=3; -- Finance
UPDATE Department SET MgrSSN='100000004', MgrStartDate='2020-03-01' WHERE Dnumber=4; -- R&D
UPDATE Department SET MgrSSN='100000013', MgrStartDate='2020-04-01' WHERE Dnumber=5; -- Marketing
UPDATE Department SET MgrSSN='100000016', MgrStartDate='2020-05-01' WHERE Dnumber=6; -- Sales
UPDATE Department SET MgrSSN='100000019', MgrStartDate='2020-06-01' WHERE Dnumber=7; -- Operations
UPDATE Department SET MgrSSN='100000022', MgrStartDate='2020-07-01' WHERE Dnumber=8; -- Support
UPDATE Department SET MgrSSN='100000010', MgrStartDate='2020-08-01' WHERE Dnumber=1; -- HR

-- Projects (12)
INSERT Project (Pnumber,Pname,Plocation,Dnum) VALUES
(101,'ERP Upgrade','HCM',2),
(102,'Mobile App','Hanoi',2),
(103,'Data Warehouse','HCM',3),
(104,'AI Research','Hanoi',4),
(105,'Website Revamp','HCM',5),
(106,'CRM Rollout','Hanoi',6),
(107,'Logistics Optimization','Da Nang',7),
(108,'Helpdesk Automation','HCM',8),
(109,'Security Hardening','Hanoi',2),
(110,'E-invoice System','HCM',3),
(111,'Chatbot CS','HCM',8),
(112,'Sales Analytics','Hanoi',6);

-- WorksOn (≈85 rows) — mỗi nhân viên tham gia 2–4 dự án
INSERT WorksOn (Essn,Pno,Hours) VALUES
('100000001',101,40),('100000001',109,18),('100000001',103,12),
('100000002',101,22),('100000002',102,24),
('100000003',102,30),('100000003',109,18),
('100000004',104,42),('100000004',103,16),
('100000005',104,28),('100000005',105,12),
('100000006',104,26),('100000006',103,14),

('100000007',103,36),('100000007',110,20),
('100000008',103,22),('100000008',110,26),
('100000009',103,18),('100000009',101,12),

('100000010',101,24),('100000010',105,20),
('100000011',101,18),('100000011',105,14),
('100000012',105,26),('100000012',108,10),

('100000013',105,20),('100000013',111,18),
('100000014',105,22),('100000014',111,16),
('100000015',105,24),('100000015',111,12),

('100000016',106,34),('100000016',112,20),
('100000017',106,26),('100000017',112,18),
('100000018',106,22),('100000018',112,14),

('100000019',107,30),('100000019',110,12),
('100000020',107,24),('100000020',110,16),
('100000021',107,20),('100000021',110,14),

('100000022',108,28),('100000022',111,18),
('100000023',108,22),('100000023',111,16),
('100000024',108,24),('100000024',111,12),

('100000025',101,16),('100000025',109,14),('100000025',102,10),
('100000026',104,20),('100000026',103,18),
('100000027',110,22),('100000027',103,10),
('100000028',112,26),('100000028',106,16),

-- rải thêm cho phong phú
('100000002',109,10),('100000003',101,8),('100000006',105,8),
('100000008',101,6),('100000009',109,8),('100000010',111,6),
('100000011',111,6),('100000012',101,6),('100000014',108,5),
('100000015',108,5),('100000017',107,6),('100000018',107,6),
('100000020',112,8),('100000021',112,6),('100000022',105,8),
('100000023',105,6),('100000024',105,4),('100000025',110,6),
('100000026',110,6),('100000027',112,6),('100000028',101,8);
GO

/* =======================
   3) QUICK CHECK QUERIES
   ======================= */

-- Birthdays in current month
SELECT Ssn, Fname, Lname, Bdate
FROM Employee
WHERE MONTH(Bdate) = MONTH(GETDATE());

-- Projects by Department
SELECT d.Dnumber, d.Dname, p.Pnumber, p.Pname, p.Plocation
FROM Department d
LEFT JOIN Project p ON p.Dnum = d.Dnumber
ORDER BY d.Dnumber, p.Pnumber;

-- Hours by Employee/Project
SELECT e.Ssn, e.Fname, e.Lname, p.Pnumber, p.Pname, w.Hours
FROM WorksOn w
JOIN Employee e ON e.Ssn = w.Essn
JOIN Project  p ON p.Pnumber = w.Pno
ORDER BY e.Ssn, p.Pnumber;
