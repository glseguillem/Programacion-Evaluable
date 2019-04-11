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
id int NOT NULL PRIMARY KEY,
NIFempleado varchar(9) NOT NULL,
dia date NOT NULL,
hora datetime not null,
esEntrada tinyint(1) NOT NULL,
foreign key(NIFempleado) references empleados(NIF)
);