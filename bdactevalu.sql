CREATE DATABASE bdactevalu CHARACTER SET UTF8;

USE bdactevalu;

CREATE TABLE empleados (
NIF varchar(9) NOT NULL PRIMARY KEY,
nombre varchar(50) NOT NULL,
apellido varchar(100) NOT NULL,
administrador tinyint(1) NOT NULL,
claveAdmin varchar(10) DEFAULT NULL
);


CREATE TABLE fichajes (
id int not null,fichajes
NIFempleado varchar(9) NOT NULL,
dia date NOT NULL,
horaEntrada datetime NOT NULL,
horaSalida datetime NOT NULL,
fichadoEntrada tinyint(1) NOT NULL,
fichadoSalida tinyint(1) NOT NULL,
foreign key(NIFempleado) references empleados(NIF)
);