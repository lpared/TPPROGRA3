-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 30-06-2026 a las 02:45:52
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `mi_banco_db`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `liquidaciones`
--

CREATE TABLE `liquidaciones` (
  `id_liquidacion` int(11) NOT NULL,
  `num_cuenta` int(11) NOT NULL,
  `periodo` varchar(7) NOT NULL,
  `fecha_vencimiento` date NOT NULL,
  `total_a_pagar` decimal(10,2) NOT NULL,
  `pago_minimo` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `liquidaciones`
--

INSERT INTO `liquidaciones` (`id_liquidacion`, `num_cuenta`, `periodo`, `fecha_vencimiento`, `total_a_pagar`, `pago_minimo`) VALUES
(1, 1001, '2026-04', '2026-05-10', 25000.00, 5000.00),
(2, 1001, '2026-05', '2026-06-10', 32000.50, 7000.00),
(4, 1001, '2026-06', '2026-07-10', 45000.00, 9000.00),
(5, 1004, '2026-06', '2026-07-10', 45000.00, 9000.00);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tarjetas`
--

CREATE TABLE `tarjetas` (
  `num_cuenta` int(11) NOT NULL,
  `numero_tarjeta` varchar(16) NOT NULL,
  `banco_emisor` enum('Banco Nación','Banco Provincia','Banco Galicia','Banco Santander','Banco BBVA','Banco Macro') NOT NULL,
  `estado` enum('Activa','Bloqueada') DEFAULT 'Activa',
  `saldo` decimal(10,2) DEFAULT 0.00,
  `dni_titular` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tarjetas`
--

INSERT INTO `tarjetas` (`num_cuenta`, `numero_tarjeta`, `banco_emisor`, `estado`, `saldo`, `dni_titular`) VALUES
(1001, '4512765432109876', 'Banco Galicia', 'Activa', 15250.75, '20123456'),
(1002, '4512112233445566', 'Banco Nación', 'Activa', 0.00, '30987654'),
(1003, '4512998877665544', 'Banco Santander', 'Activa', 4500.00, '40111222'),
(1004, '4512863015983403', 'Banco Galicia', 'Activa', 0.00, '45678912');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `documento` varchar(20) NOT NULL,
  `tipo_doc` enum('DNI','PASAPORTE') NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `fecha_nacimiento` date NOT NULL,
  `email` varchar(100) NOT NULL,
  `usuario` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `creado_el` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`documento`, `tipo_doc`, `nombre`, `apellido`, `fecha_nacimiento`, `email`, `usuario`, `password`, `creado_el`) VALUES
('20123456', 'DNI', 'Carlos', 'Gómez', '1985-04-12', 'carlos.gomez@example.com', 'carlos85', 'clave123', '2026-06-29 01:41:05'),
('30987654', 'DNI', 'Ana', 'Martínez', '1992-09-23', 'ana.mtz@example.com', 'anamar', 'clave123', '2026-06-29 01:41:05'),
('40111222', 'DNI', 'Lucía', 'Rodríguez', '1998-11-05', 'lucia.rod@example.com', NULL, NULL, '2026-06-29 01:41:05'),
('44262421', 'DNI', 'Lucia', 'Munoz', '2002-06-24', 'luayelen2417@gmail.com', 'lulalen', 'hola123', '2026-06-30 00:17:30'),
('45678912', 'DNI', 'Pedro', 'Lopez', '1995-06-10', 'Pedro@gmail.com', 'pedro95', 'hola123', '2026-06-29 22:09:03');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `liquidaciones`
--
ALTER TABLE `liquidaciones`
  ADD PRIMARY KEY (`id_liquidacion`),
  ADD KEY `num_cuenta` (`num_cuenta`);

--
-- Indices de la tabla `tarjetas`
--
ALTER TABLE `tarjetas`
  ADD PRIMARY KEY (`num_cuenta`),
  ADD UNIQUE KEY `numero_tarjeta` (`numero_tarjeta`),
  ADD UNIQUE KEY `dni_titular` (`dni_titular`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`documento`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `usuario` (`usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `liquidaciones`
--
ALTER TABLE `liquidaciones`
  MODIFY `id_liquidacion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `tarjetas`
--
ALTER TABLE `tarjetas`
  MODIFY `num_cuenta` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1006;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `liquidaciones`
--
ALTER TABLE `liquidaciones`
  ADD CONSTRAINT `liquidaciones_ibfk_1` FOREIGN KEY (`num_cuenta`) REFERENCES `tarjetas` (`num_cuenta`) ON DELETE CASCADE;

--
-- Filtros para la tabla `tarjetas`
--
ALTER TABLE `tarjetas`
  ADD CONSTRAINT `tarjetas_ibfk_1` FOREIGN KEY (`dni_titular`) REFERENCES `usuarios` (`documento`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
