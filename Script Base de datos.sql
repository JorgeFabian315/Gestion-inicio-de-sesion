-- MySQL dump 10.13  Distrib 8.0.25, for Win64 (x86_64)
--
-- Host: localhost    Database: sistema
-- ------------------------------------------------------
-- Server version	8.0.27
/* 1 login exitoso
   2 usuario no existe
   3 contraseña incorrecta
   shai
*/
CREATE DATABASE InicioSesion;
USE InicioSesion;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bitacoras`
--

DROP TABLE IF EXISTS `bitacoras`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bitacoras` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Correo` varchar(90) NOT NULL,
  `Observaciones` text NOT NULL,
  `Fecha` datetime DEFAULT CURRENT_TIMESTAMP,
  IdUsuario INT NULL,
  PRIMARY KEY (`Id`),
	CONSTRAINT fk_Bitacoras_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Id) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=158 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bitacoras`
--

-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(80) NOT NULL,
  `Correo` varchar(90) NOT NULL,
  `Contrasena` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Correo_UNIQUE` (`Correo`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--


INSERT INTO Usuarios(Nombre,Correo,Contrasena) VALUES('Pepe2','Pepe89@gmail.com','123');

SELECT * FROM Usuarios;
SELECT * FROM Bitacoras;




/*
TRIGGERS

CREATE DEFINER=`root`@`localhost` TRIGGER `usuarios_BEFORE_INSERT` BEFORE INSERT ON `usuarios` FOR EACH ROW BEGIN
	SET NEW.Contrasena = SHA1(NEW.Contraseña);
	INSERT INTO Bitacoras(Correo, Observaciones) VALUES(NEW.Correo, CONCAT('Usuario registrado con el nombre: ', NEW.Nombre)); 
END

CREATE DEFINER = CURRENT_USER TRIGGER `iniciosesion`.`usuarios_AFTER_DELETE` AFTER DELETE ON `usuarios` FOR EACH ROW
BEGIN
	INSERT INTO Bitacoras(Correo, Observaciones) VALUES(OLD.Correo, CONCAT('Usuario eliminado con el nombre: ', OLD.Nombre));
END

CREATE DEFINER=`root`@`localhost` TRIGGER `usuarios_BEFORE_INSERT` BEFORE INSERT ON `usuarios` FOR EACH ROW BEGIN
	SET NEW.Contrasena = SHA1(NEW.Contrasena);
END
CREATE DEFINER=`root`@`localhost` TRIGGER `usuarios_AFTER_INSERT` AFTER INSERT ON `usuarios` FOR EACH ROW BEGIN
INSERT INTO Bitacoras(Correo, Observaciones, IdUsuario) VALUES(NEW.Correo, CONCAT('Usuario registrado con el nombre: ', NEW.Nombre), NEW.Id); 
END
CREATE DEFINER=`root`@`localhost` TRIGGER `usuarios_AFTER_DELETE` BEFORE DELETE ON `usuarios` FOR EACH ROW BEGIN
	INSERT INTO Bitacoras(Correo, Observaciones) VALUES(OLD.Correo, CONCAT('Usuario eliminado con el nombre: ', OLD.Nombre));
END

FUNCTIONS

CREATE DEFINER=`root`@`localhost` FUNCTION `fnInicioSesion`(spCorreo varchar(90), spContrasena varchar(30)) RETURNS int
    DETERMINISTIC
BEGIN
	SELECT COUNT(*) INTO @exito FROM Usuarios WHERE Correo = spCorreo AND Contrasena = sha1(spContrasena);
	SELECT COUNT(*) INTO @usuarioexiste FROM Usuarios WHERE Correo = spCorreo;
    
    SET @vregreso = 0;
    IF (@exito=1) THEN -- SI EL CORREO Y LA CONTRASEÑA EXISTEN
    BEGIN
		SELECT Id INTO @IdUsu FROM Usuarios WHERE Correo = spCorreo;
		INSERT INTO bitacoras (correo, observaciones, IdUsuario) VALUES (spCorreo, CONCAT('Ingreso exitoso: ', spCorreo), @IdUsu);
		SET @vregreso = 1;
    END;
    ELSEIF(@usuarioexiste=0) THEN -- SI EL CORREO NO EXISTE
    BEGIN
		INSERT INTO bitacoras(observaciones,correo) VALUES (CONCAT('El usuario ingresado no existe: ',spCorreo), spCorreo);
		SET @vregreso = 2;
    END;
    ELSE
	BEGIN
    		SELECT Id INTO @IdUsu FROM Usuarios WHERE Correo = spCorreo;
		INSERT INTO bitacoras(correo,observaciones,IdUsuario) VALUES(spcorreo,CONCAT("Usuario con contraseña incorrecta: ", spCorreo),@IdUsu);
        SET @vregreso=3;
	END;
    END IF;
	RETURN @vregreso;
END

*/